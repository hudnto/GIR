using GIR.Core.Infraestrutura.Helpers;
using GIR.Core.Negocio.Consultas.QueryHandler;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cassi.Utilitarios.Util;
using GASC.Seguranca;
using GIR.Core.Negocio.Consultas.Filtro;
using GIR.Core.Negocio.Enum;
using GIR.Core.Negocio.Servico;
using GIR.Intranet.Models;

namespace GIR.Intranet.Controllers
{
    //[CassiAuthorize(Sistema = GascFuncionalidade.Sistema)]
    public class ArquivoController : GenericController
    {
        public FileResult BuscaPdf(string fileName, int ano)
        {
            if (fileName == null && ano == 0)
                throw new Exception("Parâmetros inválidos");
            
            var consulta = new ConsultaInformeColaboradorQueryHandler();
            var filtro = new InformeColaboradorFiltro {AnoExercicio = ano, CPFCNPJ = fileName};
           
            var arquivo = consulta.Execute(filtro).FirstOrDefault();

            var caminhoDir = arquivo.CaminhoDiretorio;
            var nomeServer = ConfigurationManager.AppSettings["NomeServer"].RemoveContraBarra();
            var diretorioServer = ConfigurationManager.AppSettings["DiretorioServer"].RemoveContraBarra();
            var raizPathInformesColaboradores = String.Format(@"\\{0}\{1}", nomeServer, diretorioServer);

            while (Path.IsPathRooted(caminhoDir))
            {
                caminhoDir = caminhoDir.Remove(0, 1);
            }

            var path = Path.Combine(raizPathInformesColaboradores, caminhoDir, fileName +
                arquivo.ExtensaoArquivo);

            var servico = new ArquivoServico();
            try
            {
                using (servico.Conexao = RedeUtil.RecuperarConexaoRede(servico.CaminhoDiretorio, servico.Usuario))
                {
                    FileStream file = new FileStream(path, FileMode.Open);
                    return File(file, "application/pdf", fileName + ".pdf");
                }
            }
            catch (Exception e)
            {
                servico.Conexao.Dispose();

                throw new Exception("Erro ao tentar baixar arquivo " + arquivo.CPFCNPJ + " " + e.Message, e.InnerException);
            }
        }

        public static IEnumerable<ArquivoVM> ArmazenarArquivosParaProcessamento(IEnumerable<HttpPostedFileBase> arquivos, ArquivoServico servico)
        {
            var arquivosArmazenados = new List<ArquivoVM>();
            var caminho = String.Format(@"{0}\{1}", servico.CaminhoDiretorio, ArquivoServico.PastaTempUpload);
            var dto = new ArquivoVM();

            try
            {
                using (servico.Conexao = RedeUtil.RecuperarConexaoRede(servico.CaminhoDiretorio, servico.Usuario))
                {
                    Directory.CreateDirectory(caminho);

                    foreach (var item in arquivos)
                    {
                        var nomeArquivo = ValidaNomeArquivoParaUpload(item.FileName);
                        var path = String.Format(@"{0}\{1}", caminho, nomeArquivo);

                        dto = new ArquivoVM()
                        {
                            CaminhoArquivo = path,
                            NomeArquivo = nomeArquivo
                        };
                        /*
                         * Utilizando filestream ao invés do método SaveAs do HttpPostedFileBase
                         * devido ao um bug de envio de arquivo pdf corrompido
                         */
                        using (FileStream arquivo = new FileStream(path, FileMode.Create))
                        {
                            item.InputStream.CopyTo(arquivo);
                        }

                        dto.ExtensaoArquivo = Path.GetExtension(path).ToLower();

                        arquivosArmazenados.Add(dto);
                    }
                }

            }
            catch (Exception e)
            {
                servico.Conexao.Dispose();

                if (!String.IsNullOrEmpty(dto.CaminhoArquivo))
                {
                    dto.ArquivoFalhouAoImportar = true;
                    arquivosArmazenados.Add(dto);
                }
                else
                {
                    throw new Exception("Erro ao tentar importar arquivos" + e.Message, e.InnerException);
                }
            }

            return arquivosArmazenados;
        }

        public static void ValidaEnvioArquivosDuplicados(List<ArquivoVM> arquivosUpload, List<HttpPostedFileBase> arquivosEnviados, ArquivoServico arquivoServico)
        {
            //Verificar se o usuário mandou outro arquivo txt
            if (arquivosUpload.Any(a => a.ExtensaoArquivo == ".txt") &&
                arquivosEnviados.Any(a => a.ContentType == "text/plain"))
            {
                //Remoção do arquivo na lista e diretório para efetuar upload novamente 
                var txt = arquivosUpload.FirstOrDefault(a => a.ExtensaoArquivo == ".txt");
                arquivoServico.DeletarArquivo(txt.CaminhoArquivo);
                arquivosUpload.Remove(txt);
            }

            //Verifica com base no nome do arquivo se já foi feito upload
            for (int i = arquivosEnviados.Count - 1; i >= 0; i--)
            {
                var file = arquivosUpload.FirstOrDefault(a => a.NomeArquivo == arquivosEnviados[i].FileName);
                if (file != null)
                {
                    if (file.ArquivoFalhouAoImportar)
                    {
                        //Remoção da lista para efetuar upload novamente
                        arquivosUpload.Remove(file);
                    }
                    else
                    {
                        //Remoção para não duplicar e não processar arquivo novamente 
                        arquivosEnviados.RemoveAt(i);
                    }
                }
            }
        }

        private static string ValidaNomeArquivoParaUpload(string path)
        {
            var splitCaminhoArquivo = path.Split('\\');

            if (splitCaminhoArquivo.Any())
            {
                return splitCaminhoArquivo.LastOrDefault();
            }

            return path;
        }
    }
}