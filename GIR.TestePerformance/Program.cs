using GIR.Core.Infraestrutura.Helpers;
using GIR.Core.Negocio.DTO;
using GIR.Core.Negocio.Enum;
using GIR.Core.Negocio.Servico;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Cassi.Utilitarios.Util;
using GIR.Core.Dados.Entidade;
using GIR.Core.Negocio.Erro;

namespace GIR.TestePerformance
{
    internal class Program
    {
        private static ArquivoServico _arquivoServico { get; set; }
        private static GIRServico _gir { get; set; }
        private static int Ano { get; set; }

        public static void Main(string[] args)
        {
            //Variáveis de configurações
            _arquivoServico = new ArquivoServico();
            _gir = new GIRServico();
            Ano = 2018;

            LoteDTO lote;

            using (_arquivoServico.Conexao = RedeUtil.RecuperarConexaoRede(_arquivoServico.CaminhoDiretorio, _arquivoServico.Usuario))
            {
                Console.WriteLine("==========Teste de performance em arquivos==========");
                Console.WriteLine("Digite SIM para processamento individual");
                Console.WriteLine("Digite NÂO para processamento em lote");
                lote = GerarLote();
                var frase = Console.ReadLine();

                if (!string.IsNullOrEmpty(frase) && frase.ToLower() == "sim")
                    lote.Individual = true;
                else
                    lote.ContribuintesArquivoTxt = _arquivoServico.LerDirfTxt(lote);

                var tarefas = new List<Task>();

                //Para cada arquivo importado (upload) cria-se task para fazer a divisão desses arquivos
                foreach (var arquivo in lote.ArquivosImportados.Where(a => a.ExtensaoArquivo == ".pdf"))
                {
                    var tarefa = new Task(() => { Rotina(arquivo, lote); });
                    tarefas.Add(tarefa);
                    tarefa.Start();
                }

                //Depois que as tarefas terminarem de ser executadas. Verifica-se por ultimo, se houver erro ou não.
                var continuation = Task.Factory.ContinueWhenAll(tarefas.ToArray(), (antecedents) =>
                {
                    if (antecedents.All(a => a.Exception == null))
                    {
                        try
                        {
                            RenomearArquivos(lote);
                            lote.TipoSituacao = TipoSituacao.Processado;
                            _gir.GravarRotina(lote);
                        }
                        catch (DbEntityValidationException)
                        {
                            throw;
                        }
                        catch (Exception e)
                        {
                            //Caso falha a renomeacao dos arquivos ou merge
                            lote.ContribuintesArquivoTxt.ForEach(c =>
                            {
                                if (c.Arquivo.CaminhoArquivo != null)
                                {
                                    c.Status = StatusContribuinte.FalhaArquivo;
                                    c.TipoSituacao = TipoSituacao.Erro;
                                }
                            });
                            lote.ContribuinteIndividual.Status = StatusContribuinte.FalhaArquivo;
                            lote.ContribuinteIndividual.TipoSituacao = TipoSituacao.Erro;
                            
                            var exception = e.InnerException ?? e;
                            var w32ex = exception as Win32Exception;

                            lote.TipoSituacao = TipoSituacao.Erro;
                            lote.CodigoErro = (w32ex != null ? w32ex.ErrorCode : 0);
                            lote.MensagemErro = exception.Message;

                            _gir.GravarRotina(lote);

                            throw e;
                        }

                        Console.WriteLine("Todas as tarefas foram concluídas com sucesso!");
                        Debug.WriteLine("Todas as tarefas foram concluídas com sucesso!");
                    }
                    //Se houver qualquer exceção lançada em uma task, trataremos aqui esta exceção, neste bloco de codigo
                    else
                    {
                        var task = antecedents.FirstOrDefault(a => a.Exception.GetType() == typeof (NegocioException));
                        if (task != null)
                        {
                            var ex = task.Exception;
                            if (ex != null)
                                throw new NegocioException(ex.Message, ex.InnerException);
                        }

                        //Seta falha no arquivo pois não foi feito a renomeacao e nem o merge
                        lote.ContribuintesArquivoTxt.ForEach(c =>
                        {
                            if (c.Arquivo.CaminhoArquivo != null)
                            {
                                c.Status = StatusContribuinte.FalhaArquivo;
                                c.TipoSituacao = TipoSituacao.Erro;
                            }
                        });
                        lote.ContribuinteIndividual.Status = StatusContribuinte.FalhaArquivo;
                        lote.ContribuinteIndividual.TipoSituacao = TipoSituacao.Erro;

                        task = (antecedents.FirstOrDefault(a => a.Exception != null));

                        var exception = task.Exception.InnerException ?? task.Exception;
                        var w32ex = exception as Win32Exception;

                        lote.TipoSituacao = TipoSituacao.Erro;
                        lote.CodigoErro = (w32ex != null ? w32ex.ErrorCode : 0);
                        lote.MensagemErro = exception.Message;
                        
                        _gir.GravarRotina(lote);

                        var exceptions = antecedents.Where(a => a.Exception != null).Select(a => a.Exception).ToList();
                        var excecoes = new List<Exception>();

                        foreach (var excep in exceptions)
                        {
                            excecoes.Add(new Exception(excep.InnerException.Message, excep.InnerException));
                        }

                        AggregateException e = new AggregateException(excecoes);

                        Console.WriteLine("Ocorreu falhas no processamento - Split pdf falhou!");
                        Debug.WriteLine("Ocorreu falhas no processamento - Split pdf falhou!");

                        throw e;
                    }
                });

                continuation.Wait();
                Console.Read();
            }
        }

        public static void Rotina(ArquivoDTO arquivoImportado, LoteDTO lote)
        {
            if (File.Exists(arquivoImportado.CaminhoArquivo))
            {
                try
                {
                    if (lote.Individual)
                        _arquivoServico.SplitPdfIndividual(arquivoImportado, lote);
                    else
                        _arquivoServico.SplitPdf(arquivoImportado, lote);

                    Console.WriteLine("Sucesso na TASK SPLIT PDF ID:" + Task.CurrentId);
                    Debug.WriteLine("Sucesso na TASK SPLIT PDF ID:" + Task.CurrentId);
                }
                catch (NegocioException e)
                {
                    throw new NegocioException(e.Message, e.InnerException);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Houve falha na TASK SPLIT PDF ID:" + Task.CurrentId);
                    Debug.WriteLine("Houve falha na TASK SPLIT PDF ID:" + Task.CurrentId);

                    var msg = string.Format("Falha no processamento - TASK ID:{0} SPLIT PDF - {1}", Task.CurrentId, e.Message);
                    throw new Exception(msg, e.InnerException);
                }
            }
            else
            {
                var msg = string.Format("Arquivo {0} NÃO EXISTI!", arquivoImportado.CaminhoArquivo);
                Console.WriteLine(msg);
                Debug.WriteLine(msg);                
            }
        }

        public static LoteDTO GerarLote()
        {
            var lote = new LoteDTO();
            lote.Codigo = 0;
            lote.AnoExercicio = Ano;
            lote.AnoCalendario = Ano - 1;
            lote.Descricao = "Teste de perfomance";
            lote.LoginUsuario = "capetinha@123";
            lote.Operacao = OperacaoGIR.IncluirRotina;
            lote.TipoContribuinte = TipoContribuinte.PrestadorFornecedor;
            lote.TipoSituacao = TipoSituacao.Processado;
            lote.InicioProcessamento = DateTime.Now;
            lote.ArquivosImportados = BuscarArquivosImportados(Ano);

            return lote;
        }

        public static IEnumerable<ArquivoDTO> BuscarArquivosImportados(int ano)
        {
            var origem = String.Format(@"{0}\{1}\IMPORTADOS", _arquivoServico.CaminhoDiretorio, ano);
            var dir = new DirectoryInfo(origem);
            var arquivos = new List<ArquivoDTO>();

            if (dir.Exists)
            {
                var fileTxt = dir.GetFiles("*.txt");
                var filesPdf = dir.GetFiles("*.pdf");

                var files = filesPdf.Concat(fileTxt).ToList();

                foreach (var file in files)
                {
                    var arquivo = new ArquivoDTO();
                    arquivo.CaminhoArquivoBanco = String.Format(@"\{0}\IMPORTADOS", ano);
                    arquivo.CaminhoArquivo = file.DirectoryName + @"\" + file.Name;
                    arquivo.NomeArquivo = file.Name;
                    arquivo.ExtensaoArquivo = file.Extension;

                    arquivos.Add(arquivo);
                }
            }

            return arquivos;
        }

        private static void RenomearArquivos(LoteDTO lote)
        {
            var parametro = (lote.TipoContribuinte == TipoContribuinte.FuncionarioCassi ? ArquivoServico.Funcionarios : ArquivoServico.Prestadores);
            string origem = string.Format(@"{0}\{1}\{2}\{3}", _arquivoServico.CaminhoDiretorio, Ano, ArquivoServico.Processados, parametro);
            
            var tarefas = new List<Task>();
            Task tarefa;
            var contribuintes = (lote.Individual ? new List<ContribuinteDTO>{ lote.ContribuinteIndividual } : 
                                 lote.ContribuintesArquivoTxt.Where(p => p.Status == StatusContribuinte.Sucesso).ToList());

            var dir = new DirectoryInfo(origem);

            if (dir.Exists)
            {
                /*Criacao de mais task para realizar a renomeacao e merge de arquivo.
                 * Sera criado uma task para cada range de 250 contribuintes 
                */

                const int rangeParaCadaTask = 250;
                var total = contribuintes.Count;

                for (int i = 0; i < total; i = i+rangeParaCadaTask)
                {
                    var listaContribuintes = contribuintes.Skip(i).Take(rangeParaCadaTask).ToList();
                    tarefa = new Task(() => { RotinaRenomearArquivos(dir, origem, listaContribuintes, lote); });
                    tarefas.Add(tarefa);
                    tarefa.Start();                        
                }                    
                
                var continuation = Task.Factory.ContinueWhenAll(tarefas.ToArray(), (antecedents) =>
                {
                    if (antecedents.Any(a => a.Exception != null))
                    {
                        var exceptions = antecedents.Where(a => a.Exception != null).Select(a => a.Exception).ToList();
                        var excecoes = new List<Exception>();

                        foreach (var exception in exceptions)
                        {
                            excecoes.Add(new Exception(exception.InnerException.Message, exception.InnerException));
                        }
                        
                        AggregateException e = new AggregateException(excecoes);

                        Console.WriteLine("Ocorreu falhas no processamento - Renomear arquivo e merge falhou!");
                        Debug.WriteLine("Ocorreu falhas no processamento - Renomear arquivo e merge falhou!");

                        throw e;
                    }
                });
                continuation.Wait();
            }
        }

        private static void RotinaRenomearArquivos(DirectoryInfo diretorio, string origem, List<ContribuinteDTO> contribuintes, LoteDTO lote)
        {
            foreach (var contribuinte in contribuintes)
            {
                //A propriedade CpfCnpj do ContribuinteDTO armazena cpf ou cnpj com mascara!
                var cpfCnpj = contribuinte.Arquivo.NomeArquivo;
                var pattern = string.Format(@"*{0}*", cpfCnpj);
                var arquivos = diretorio.GetFiles(pattern);
                var caminhoRenomeado = string.Format(@"{0}\{1}.pdf", origem, cpfCnpj);

                foreach (var arquivo in arquivos)
                {
                    if (!File.Exists(caminhoRenomeado))
                    {
                        File.Move(arquivo.FullName, caminhoRenomeado);
                    }
                    else
                    {
                        _arquivoServico.MergeArquivo(origem, caminhoRenomeado, arquivo.FullName, cpfCnpj, lote);
                    }
                }
            }

            var msg = string.Format("Sucesso na TASK ID:{0} RENOMEAR ARQUIVOS", Task.CurrentId);
            Console.WriteLine(msg);
            Debug.WriteLine(msg);
        }
    }
}
