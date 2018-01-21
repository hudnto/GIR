using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Cassi.Utilitarios.Util;
using GIR.Core.Infraestrutura.Extensoes;
using GIR.Core.Negocio.DTO;
using GIR.Core.Negocio.Enum;
using GIR.Core.Negocio.Erro;
using iTextSharp.text;
using iTextSharp.text.exceptions;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace GIR.Core.Negocio.Servico
{
    public class ArquivoServico : ArquivosServicoBase
    {
        public const String Processados = "PROCESSADOS";
        public const String Web = "WEB";
        public const String Importados = "IMPORTADOS";
        public const String Funcionarios = "FUNCIONARIOS";
        public const String Prestadores = "PRESTADORES";
        private const String BPFDEC = "BPFDEC";
        private const String BPJDEC = "BPJDEC";
        private readonly GIRServico _gir;
        public ConexaoRede Conexao { get; set; }
        public ContribuinteDTO Contribuinte { get; set; }

        public ArquivoServico()
        {
            _gir = new GIRServico();
        }

        public void GerarProcessamento(LoteDTO lote)
        {
            try
            {
                using (Conexao = RedeUtil.RecuperarConexaoRede(CaminhoDiretorio, Usuario))
                {
                    CriarEstruturaDiretorios(lote.AnoExercicio);

                    MigrarArquivosUpload(lote);

                    if (!lote.Individual)
                        lote.ContribuintesArquivoTxt = LerDirfTxt(lote);

                    SplitLote(lote);
                }
            }
            catch (NegocioException e)
            {
                foreach (var arquivo in lote.ArquivosImportados)
                {
                    File.Delete(arquivo.CaminhoArquivo);
                }

                Conexao.Dispose();

                throw new NegocioException(e.Message, e.InnerException);
            }
            catch (Exception e)
            {
                Conexao.Dispose();

                lote.TipoSituacao = TipoSituacao.Erro;
                lote.CodigoErro = Marshal.GetExceptionCode();
                lote.MensagemErro = e.Message;

                if (Contribuinte != null)
                {
                    Contribuinte.TipoSituacao = TipoSituacao.Erro;
                    Contribuinte.Status = StatusContribuinte.FalhaArquivo;
                }

                _gir.GravarRotina(lote);

                throw new Exception(e.Message, e.InnerException);
            }
        }

        public List<ContribuinteDTO> LerDirfTxt(LoteDTO lote)
        {
            List<ContribuinteDTO> contribuintes = new List<ContribuinteDTO>();
            var txt = lote.ArquivosImportados.FirstOrDefault(c => c.ExtensaoArquivo == ".txt");

            var file = new StreamReader(txt.CaminhoArquivo);
            string linha;

            while ((linha = file.ReadLine()) != null)
            {
                if (lote.TipoContribuinte == TipoContribuinte.FuncionarioCassi && linha.Contains(BPFDEC))
                {
                    var dados = linha.Split('|');

                    if (String.IsNullOrEmpty(dados[1]))
                        continue;

                    contribuintes.Add(new ContribuinteDTO()
                    {
                        CpfCnpj = (dados[1].Length > 11 ? FormatacaoUtil.FormatarCNPJ(dados[1]) : FormatacaoUtil.FormatarCPF(dados[1])),
                        TipoContribuinte = lote.TipoContribuinte,
                        Status = (dados[1].Length > 11 ? StatusContribuinte.FalhaCnpj : StatusContribuinte.FalhaCpf)
                    });
                }

                if (lote.TipoContribuinte == TipoContribuinte.PrestadorFornecedor && (linha.Contains(BPFDEC) || linha.Contains(BPJDEC)))
                {
                    var dados = linha.Split('|');

                    if (String.IsNullOrEmpty(dados[1]))
                        continue;

                    contribuintes.Add(new ContribuinteDTO()
                    {
                        CpfCnpj = (dados[1].Length > 11 ? FormatacaoUtil.FormatarCNPJ(dados[1]) : FormatacaoUtil.FormatarCPF(dados[1])),
                        TipoContribuinte = lote.TipoContribuinte,
                        Status = (dados[1].Length > 11 ? StatusContribuinte.FalhaCnpj : StatusContribuinte.FalhaCpf)
                    });
                }
            }

            return contribuintes.GroupBy(x => x.CpfCnpj).Select(y => y.FirstOrDefault()).ToList();
        }

        public void SplitLote(LoteDTO lote)
        {
            var arquivosPdfs = lote.ArquivosImportados.Where(a => a.ExtensaoArquivo == ".pdf").ToList();

            foreach (var arquivoImportado in arquivosPdfs)
            {
                if (!File.Exists(arquivoImportado.CaminhoArquivo))
                    continue;

                if (lote.Individual)
                    SplitPdfIndividual(arquivoImportado, lote);
                else
                    SplitPdf(arquivoImportado, lote);
            }

            //lote.TipoSituacao = TipoSituacao.Processado;

            //_gir.GravarRotina(lote);
        }

        public void SplitPdf(ArquivoDTO arquivoImportado, LoteDTO lote)
        {
            var parametro = (lote.TipoContribuinte == TipoContribuinte.FuncionarioCassi ? Funcionarios : Prestadores);
            string caminho = string.Format(@"{0}\{1}\{2}\{3}", CaminhoDiretorio, lote.AnoExercicio, Processados, parametro);

            using (var reader = new PdfReader(arquivoImportado.CaminhoArquivo))
            {
                int total = reader.NumberOfPages;

                for (int pagina = 1; pagina <= total; pagina++)
                {
                    var textoPag = PdfTextExtractor.GetTextFromPage(reader, pagina, new SimpleTextExtractionStrategy());

                    var cpfsCnpjs = IdentificarCpfCnpjPdf(textoPag);

                    if (cpfsCnpjs.Count() < 0)
                        continue;

                    ContribuinteDTO contribuinte = null;

                    foreach (var cpfCnpj in cpfsCnpjs)
                    {
                        contribuinte = lote.ContribuintesArquivoTxt.FirstOrDefault(c => c.CpfCnpj == cpfCnpj);
                        if (contribuinte != null)
                            break;
                    }

                    if (contribuinte != null)
                    {
                        //Contribuinte encontrado no txt e pdf! 
                        contribuinte.Arquivo.CaminhoArquivo = caminho;
                        contribuinte.Arquivo.CaminhoArquivoBanco = String.Format(@"\{0}\{1}\{2}", lote.AnoExercicio, Processados, parametro);

                        if (GerarArquivo(reader, contribuinte, pagina))
                        {
                            contribuinte.Status = StatusContribuinte.Sucesso;
                            contribuinte.TipoSituacao = TipoSituacao.Processado;
                        }
                    }
                }
            }
        }

        public void SplitPdfIndividual(ArquivoDTO arquivoImportado, LoteDTO lote)
        {
            var parametro = (lote.TipoContribuinte == TipoContribuinte.FuncionarioCassi ? Funcionarios : Prestadores);
            string caminho = string.Format(@"{0}\{1}\{2}\{3}", CaminhoDiretorio, lote.AnoExercicio, Processados, parametro);

            using (var reader = new PdfReader(arquivoImportado.CaminhoArquivo))
            {
                int total = reader.NumberOfPages;

                for (int pagina = 1; pagina <= total; pagina++)
                {
                    var textoPag = PdfTextExtractor.GetTextFromPage(reader, pagina,
                        new SimpleTextExtractionStrategy());

                    var cpfsCnpjs = IdentificarCpfCnpjPdf(textoPag);

                    if (cpfsCnpjs.Count() < 0)
                        continue;

                    lock (lote.ContribuinteIndividual)
                    {
                        foreach (var cpfCnpj in cpfsCnpjs)
                        {
                            if (string.IsNullOrEmpty(lote.ContribuinteIndividual.CpfCnpj))
                            {
                                lote.ContribuinteIndividual.CpfCnpj = cpfCnpj;
                                lote.ContribuinteIndividual.Arquivo.CaminhoArquivo = caminho;
                                lote.ContribuinteIndividual.Arquivo.CaminhoArquivoBanco = String.Format(@"\{0}\{1}\{2}", lote.AnoExercicio, Processados, parametro);

                                if (GerarArquivo(reader, lote.ContribuinteIndividual, pagina))
                                {
                                    lote.ContribuinteIndividual.Status = StatusContribuinte.Sucesso;
                                    lote.ContribuinteIndividual.TipoSituacao = TipoSituacao.Processado;
                                }
                            }
                            else if(lote.ContribuinteIndividual.CpfCnpj == cpfCnpj)
                            {
                                if (GerarArquivo(reader, lote.ContribuinteIndividual, pagina))
                                {
                                    lote.ContribuinteIndividual.Status = StatusContribuinte.Sucesso;
                                    lote.ContribuinteIndividual.TipoSituacao = TipoSituacao.Processado;
                                }

                            }
                            else
                            {
                                throw new NegocioException("Rotina indicada para geração individual deve possuir arquivo que contenha apenas um CPF/CNPJ.");
                            }
                        }
                    }
                }
            }
        }

        private IEnumerable<String> IdentificarCpfCnpjPdf(string conteudo)
        {
            const string cnpjCassi = "33.719.485/0001-27";
            string pattern = @"([0-9]{2}[\.]?[0-9]{3}[\.]?[0-9]{3}[\/]?[0-9]{4}[-]?[0-9]{2})|([0-9]{3}[\.]?[0-9]{3}[\.]?[0-9]{3}[-]?[0-9]{2})";
            var matches = Regex.Matches(conteudo, pattern).Cast<Match>().ToList();

            if (matches.Count > 0)
                return matches.Where(m => m.Value != cnpjCassi).Select(m => m.Value).ToList();

            return new List<String>();
        }

        private String IdentificarTipoArquivoPdf(string conteudo)
        {
            string CSLL = @"CSLL";
            string IR = @"([I][\s|M][\s|M|P][\s|M|P|O][\s|M|P|O|S][\s|M|P|O|S|T][\s|M|P|O|S|T|O]{1,7})";

            var matcheCSLL = Regex.Match(conteudo, CSLL);
            var matcheIR = Regex.Match(conteudo, IR);

            if (!String.IsNullOrEmpty(matcheCSLL.Value))
                return matcheCSLL.Value;

            if (!String.IsNullOrEmpty(matcheIR.Value))
                return "IR";

            return String.Empty;
        }

        public bool VerificaSeArquivoFoiDuplicado(ArquivoDTO arquivo, IEnumerable<ArquivoDTO> arquivos)
        {
            var files = arquivos.Where(a => a.Bytes != null).ToList();

            if (files.Count > 0)
            {
                var bytes = File.ReadAllBytes(arquivo.CaminhoArquivo);

                if (files.Any(f => f.Bytes.SequenceEqual(bytes)))
                {
                    return true;
                }

                arquivo.Bytes = bytes;
            }
            else
            {
                arquivo.Bytes = File.ReadAllBytes(arquivo.CaminhoArquivo);

                return false;
            }

            return false;
        }

        private bool GerarArquivo(PdfReader reader, ContribuinteDTO contribuinte, int pagina)
        {
            var cpfCnpj = contribuinte.CpfCnpj.Replace(".", String.Empty).
                            Replace("-", String.Empty).Replace("/", String.Empty);

            contribuinte.Arquivo.NomeArquivo = cpfCnpj;
            contribuinte.Arquivo.ExtensaoArquivo = ".pdf";

            var pathArquivo = String.Format(@"{0}\{1}_TASK{2}{3}", contribuinte.Arquivo.CaminhoArquivo, cpfCnpj, Task.CurrentId, ".pdf");

            if (File.Exists(pathArquivo))
            {
                int i = 1;
                pathArquivo = String.Format(@"{0}\{1}_TASK{2}_{3}{4}", contribuinte.Arquivo.CaminhoArquivo, cpfCnpj, Task.CurrentId, i, ".pdf");

                while (File.Exists(pathArquivo))
                {
                    i++;
                    pathArquivo = String.Format(@"{0}\{1}_TASK{2}_{3}{4}", contribuinte.Arquivo.CaminhoArquivo, cpfCnpj, Task.CurrentId, i, ".pdf");
                }
            }

            var document = new Document(reader.GetPageSizeWithRotation(pagina));

            var provider = new PdfCopy(document, new FileStream(pathArquivo, FileMode.Create));
            var importedPage = provider.GetImportedPage(reader, pagina);

            document.Open();

            provider.AddPage(importedPage);

            document.Close();

            provider.Close();

            return true;
            
        }

        private void MergeArquivo(PdfReader reader, string pathRaiz, string pathArquivo, string cpfCnpj, int pagina)
        {
            var pathArquivoTemp = String.Format(@"{0}\{1}_TEMP{2}", pathRaiz, cpfCnpj, ".pdf");

            using (var readerArquivoExistente = new PdfReader(pathArquivo))
            {
                var document = new Document(reader.GetPageSizeWithRotation(pagina));
                var provider = new PdfCopy(document, new FileStream(pathArquivoTemp, FileMode.Create));

                document.Open();

                var conteudoPaginas = new List<string>();
                string textoPag;

                //Adiciona páginas do arquivo existente
                for (int i = 1; i <= readerArquivoExistente.NumberOfPages; i++)
                {
                    textoPag = PdfTextExtractor.GetTextFromPage(readerArquivoExistente, i, new SimpleTextExtractionStrategy());
                    var imported = provider.GetImportedPage(readerArquivoExistente, i);

                    if (!string.IsNullOrWhiteSpace(textoPag))
                    {
                        conteudoPaginas.Add(textoPag);

                        provider.AddPage(imported);
                    }

                }

                textoPag = PdfTextExtractor.GetTextFromPage(reader, pagina, new SimpleTextExtractionStrategy());

                if (!conteudoPaginas.Contains(textoPag))
                {
                    var importedPage = provider.GetImportedPage(reader, pagina);

                    provider.AddPage(importedPage);
                }

                document.Close();

                provider.Close();
            }

            File.Delete(pathArquivo);

            File.Move(pathArquivoTemp, pathArquivo);
        }

        public void MergeArquivo(string pathRaizLote, string pathArquivo, string pathArquivoParaMerge, string cpfCnpj, LoteDTO lote)
        {
            var pathArquivoTemp = String.Format(@"{0}\{1}_TEMP{2}", pathRaizLote, cpfCnpj, ".pdf");

            //Arquivo formatado -> Ex..: 00081876000100.pdf
            //Arquivo para merger -> Ex..: 00081876000100_TASK1_1.pdf

            using (var readerArquivoFormatado = new PdfReader(pathArquivo))
            using (var readerArquivoParaMerge = new PdfReader(pathArquivoParaMerge))
            {
                int totalArqFormatado = readerArquivoFormatado.NumberOfPages;
                int totalArqMerge = readerArquivoParaMerge.NumberOfPages;

                var document = new Document(readerArquivoFormatado.GetPageSizeWithRotation(1));
                var provider = new PdfCopy(document, new FileStream(pathArquivoTemp, FileMode.Create));

                document.Open();

                var conteudoPaginas = new List<string>();
                string textoPag;

                //Adiciona paginas ao arquivo temporario. 
                for (int pagina = 1; pagina <= totalArqFormatado; pagina++)
                {
                    textoPag = PdfTextExtractor.GetTextFromPage(readerArquivoFormatado, pagina, new SimpleTextExtractionStrategy());

                    var imported = provider.GetImportedPage(readerArquivoFormatado, pagina);

                    provider.AddPage(imported);

                    conteudoPaginas.Add(textoPag);
                }

                //Adiciona paginas ao arquivo temporario se não houver duplicacao.
                for (int pagina = 1; pagina <= totalArqMerge; pagina++)
                {
                    textoPag = PdfTextExtractor.GetTextFromPage(readerArquivoParaMerge, pagina, new SimpleTextExtractionStrategy());

                    if (!string.IsNullOrWhiteSpace(textoPag) && !conteudoPaginas.Contains(textoPag))
                    {
                        var imported = provider.GetImportedPage(readerArquivoParaMerge, pagina);

                        provider.AddPage(imported);
                    }
                }

                document.Close();

                provider.Close();
            }

            File.Delete(pathArquivo);

            File.Delete(pathArquivoParaMerge);

            File.Move(pathArquivoTemp, pathArquivo);

            //throw new InvalidPdfException("Teste");
        }

        private void MigrarArquivosUpload(LoteDTO lote)
        {
            var origem = String.Format(@"{0}\{1}", CaminhoDiretorio, PastaTempUpload);
            var destino = String.Format(@"{0}\{1}\{2}", CaminhoDiretorio, lote.AnoExercicio, Importados);

            var dir = new DirectoryInfo(origem);

            if (dir.Exists)
            {
                var fileTxt = dir.GetFiles("*.txt");
                var filesPdf = dir.GetFiles("*.pdf");

                var files = filesPdf.Concat(fileTxt).ToList();

                foreach (var file in files)
                {
                    if (!File.Exists(string.Format(@"{0}\{1}", destino, file.Name)))
                        File.Move(string.Format(@"{0}\{1}", file.DirectoryName, file.Name), string.Format(@"{0}\{1}", destino, file.Name));
                    var arquivo = lote.ArquivosImportados.FirstOrDefault(a => a.NomeArquivo == file.Name);

                    if (arquivo != null)
                    {
                        arquivo.CaminhoArquivo = String.Format(@"{0}\{1}", destino, arquivo.NomeArquivo);
                        arquivo.CaminhoArquivoBanco = String.Format(@"\{0}\{1}", lote.AnoExercicio, Importados);
                    }

                }

                dir.Delete(true);
            }
        }

        public int BuscarTotaisArquivosImportados(int anoExercicio)
        {
            var caminho = String.Format(@"{0}\{1}\{2}", CaminhoDiretorio, anoExercicio, Importados);

            if (Directory.Exists(caminho))
                return Directory.GetFiles(caminho).Length;

            return 0;
        }

        public void CriarEstruturaDiretorios(int anoExercicio)
        {
            var diretorio = String.Format(@"{0}\{1}", CaminhoDiretorio, anoExercicio);

            if (Directory.Exists(diretorio)) return;

            //Cria estrutura para pasta processados
            Directory.CreateDirectory(String.Format(@"{0}\{1}\{2}\{3}", CaminhoDiretorio, anoExercicio, Processados, Funcionarios));
            Directory.CreateDirectory(String.Format(@"{0}\{1}\{2}\{3}", CaminhoDiretorio, anoExercicio, Processados, Prestadores));

            //Cria estrutura para pasta web
            Directory.CreateDirectory(String.Format(@"{0}\{1}\{2}\{3}", CaminhoDiretorio, anoExercicio, Web, Funcionarios));
            Directory.CreateDirectory(String.Format(@"{0}\{1}\{2}\{3}", CaminhoDiretorio, anoExercicio, Web, Prestadores));

            //Cria pasta para armazenar arquivos importados
            Directory.CreateDirectory(String.Format(@"{0}\{1}\{2}", CaminhoDiretorio, anoExercicio, Importados));
        }
    }
}