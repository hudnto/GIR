using System;
using GIR.Core.Dados;
using GIR.Core.Dados.Entidade;
using GIR.Core.Dados.Infraestrutura;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using GIR.Core.Infraestrutura.Extensoes;
using GIR.Core.Negocio.DTO;
using GIR.Core.Negocio.Enum;

namespace GIR.Core.Negocio.Servico
{
    public class GIRServico
    {
        private readonly GIRContexto _contexto;
        private readonly RepositorioAtualizavel<Processamento> _repositorioProcessamento;
        private readonly RepositorioAtualizavel<Arquivo> _repositorioArquivo;
        private readonly RepositorioAtualizavel<Contribuinte> _repositorioContribuinte;
        private readonly RepositorioAtualizavel<Log> _repositorioLog;
        private readonly RepositorioAtualizavel<Ocorrencia> _repositorioOcorrencia;

        public GIRServico()
            : this(new GIRContexto())
        {
            _repositorioProcessamento = new RepositorioAtualizavel<Processamento>(_contexto);
            _repositorioContribuinte = new RepositorioAtualizavel<Contribuinte>(_contexto);
            _repositorioArquivo = new RepositorioAtualizavel<Arquivo>(_contexto);
            _repositorioLog = new RepositorioAtualizavel<Log>(_contexto);
            _repositorioOcorrencia = new RepositorioAtualizavel<Ocorrencia>(_contexto);
        }

        public GIRServico(GIRContexto contexto)
        {
            _contexto = contexto;
        }

        public ICollection<int> BuscarAnosExercicio()
        {
            var anos = _repositorioProcessamento.Buscar(p => p.AnoExercicio != default(int))
                                                .GroupBy(p => p.AnoExercicio)
                                                .Select(p => p.Key);

            return anos.ToList();
        }

        public List<Contribuinte> BuscarTodosRegistrosProcessados()
        {
            return _repositorioContribuinte.Todos().ToList();
        }

        public void GravarRotina(LoteDTO lote)
        {
            using (var transaction = _contexto.Database.BeginTransaction())
            {
                _contexto.Configuration.AutoDetectChangesEnabled = false;
                _contexto.Configuration.ValidateOnSaveEnabled = false;

                try
                {
                    var contribuintes = lote.ContribuintesArquivoDirf.Where(p => p.Status == StatusContribuinte.Sucesso).ToList();
                    var ocorrencias = lote.ContribuintesArquivoDirf.Where(p => p.Status != StatusContribuinte.Sucesso).ToList();
                    lote.TotalArquivosGerados = contribuintes.Count;
                    lote.TotalArquivosImportados = lote.ArquivosImportados.Count();

                    var processamentoId = GravarProcessamento(lote, contribuintes);
                    GravarContribuintes(contribuintes, processamentoId);
                    GravarArquivos(lote.ArquivosImportados, contribuintes, processamentoId);
                    LimparOcorrencias(lote);
                    GravarOcorrencias(ocorrencias, lote, processamentoId);
                    GravarLog(lote, processamentoId);

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();

                    throw new Exception(e.Message);
                }
            }
        }

        public int GravarProcessamento(LoteDTO lote, IEnumerable<ContribuinteDTO> contribuintes)
        {
            var processamento = new Processamento();
            processamento.Id = lote.Codigo;
            processamento.SituacaoProcessamento = (short)lote.TipoSituacao;
            processamento.AnoExercicio = lote.AnoExercicio;
            processamento.AnoCalendario = lote.AnoCalendario;
            processamento.Descricao = lote.Descricao;
            processamento.Individual = lote.Individual;
            processamento.DataRegistro = DateTime.Now;

            if (processamento.Id > 0)
            {
                _repositorioProcessamento.Atualizar(processamento);
            }
            else
            {
                _repositorioProcessamento.Adicionar(processamento);
            }

            _repositorioProcessamento.SalvarAlteracoes();

            return processamento.Id;
        }

        public void GravarContribuintes(IEnumerable<ContribuinteDTO> contribuintesDto, int processamentoId)
        {
            var processamento = _contexto.Processamentos.FirstOrDefault(p => p.Id == processamentoId);
            var cpfsCnpjsContribuintes = contribuintesDto.Select(c => c.Arquivo.NomeArquivo);
            var contribuintes = contribuintesDto.Where(c => (_contexto.Contribuintes.FirstOrDefault(ct => ct.CPFCNPJ == c.Arquivo.NomeArquivo) == null)).ToList();
            var contribuintesAtualizar = _contexto.Contribuintes.Where(c => cpfsCnpjsContribuintes.Contains(c.CPFCNPJ)).Include(c => c.Processamentos).ToList();
            var contribuintesAInserir = new List<Contribuinte>();

            contribuintes.ForEach(c => contribuintesAInserir.Add(new Contribuinte()
            {
                CPFCNPJ = c.Arquivo.NomeArquivo,
                TipoContribuinteId = (short)c.TipoContribuinte,
                Email = String.Empty,
                Processamentos = new Collection<Processamento>() { processamento }
            }));

            contribuintesAtualizar.ForEach(c =>
            {
                if (c.Processamentos.Any(m => m.Id == processamentoId))
                    contribuintesAtualizar.Remove(c);
                else
                {
                    c.Processamentos.Add(processamento);
                    //_contexto.Entry(c).State = EntityState.Modified;
                    /* Tratamento do seguinte bug 
                     * Ao atualizar collection de processamentos utilizando EntityState.Modified,
                     * quando esta desabilitado do contexto AutoDetectChangesEnabled ValidateOnSaveEnabled,
                     * Não esta sendo armazenado as referencias entre contribuinte e processamento na tabela associativa
                     */
                    _contexto.ChangeTracker.DetectChanges();
                }
            });

            _repositorioContribuinte.AdicionarRange(contribuintesAInserir);
            _repositorioContribuinte.SalvarAlteracoes();
        }

        public void GravarArquivos(IEnumerable<ArquivoDTO> arquivosImportadosDto, ICollection<ContribuinteDTO> contribuintesDto, int processamentoId)
        {
            if (arquivosImportadosDto.Any())
            {
                var arquivosImportadosAInserir = new List<Arquivo>();
                var caminhoArquivoBancoImportado = arquivosImportadosDto.FirstOrDefault().CaminhoArquivoBanco;
                var nomesArquivosImportados = arquivosImportadosDto.Select(a => a.NomeArquivo);

                var arquivosImportados = arquivosImportadosDto.Where(c => _contexto.Arquivos.FirstOrDefault(a =>
                                         a.CaminhoDiretorio == c.CaminhoArquivoBanco && a.NomeArquivo == c.NomeArquivo) == null).ToList();

                arquivosImportados.ForEach(c => arquivosImportadosAInserir.Add(new Arquivo()
                {
                    ProcessamentoId = processamentoId,
                    SituacaoId = 0,
                    CaminhoDiretorio = c.CaminhoArquivoBanco,
                    ExtensaoArquivo = c.ExtensaoArquivo,
                    DataUltimaAtualizacao = DateTime.Now,
                    NomeArquivo = c.NomeArquivo
                }));

                var arquivosImportadosAtualizar = _contexto.Arquivos.Where(a => a.CaminhoDiretorio == caminhoArquivoBancoImportado &&
                                                  nomesArquivosImportados.Contains(a.NomeArquivo)).ToList();

                arquivosImportadosAtualizar.ForEach(a =>
                {
                    a.SituacaoId = 0; a.DataUltimaAtualizacao = DateTime.Now;
                    _contexto.Entry(a).State = EntityState.Modified;
                });

                _repositorioArquivo.AdicionarRange(arquivosImportadosAInserir);
            }

            if (contribuintesDto.Any())
            {
                var arquivosAInserir = new List<Arquivo>();
                var cpfsCnpjs = contribuintesDto.Select(c => c.Arquivo.NomeArquivo);
                var caminhoArquivoBanco = contribuintesDto.FirstOrDefault().Arquivo.CaminhoArquivoBanco;

                var arquivos = contribuintesDto.Where(c => _contexto.Arquivos.FirstOrDefault(a =>
                                a.CaminhoDiretorio == c.Arquivo.CaminhoArquivoBanco && a.NomeArquivo == c.Arquivo.NomeArquivo) == null).ToList();

                arquivos.ForEach(c => arquivosAInserir.Add(new Arquivo()
                {
                    ProcessamentoId = processamentoId,
                    SituacaoId = (short)c.TipoSituacao,
                    CaminhoDiretorio = c.Arquivo.CaminhoArquivoBanco,
                    ExtensaoArquivo = c.Arquivo.ExtensaoArquivo,
                    DataUltimaAtualizacao = DateTime.Now,
                    NomeArquivo = c.Arquivo.NomeArquivo,
                }));

                var arquivosAtualizar = _contexto.Arquivos.Where(a => a.CaminhoDiretorio == caminhoArquivoBanco &&
                                                  cpfsCnpjs.Contains(a.NomeArquivo)).ToList();

                arquivosAtualizar.ForEach(a =>
                {
                    a.SituacaoId = 0; a.DataUltimaAtualizacao = DateTime.Now;
                    _contexto.Entry(a).State = EntityState.Modified;
                });

                _repositorioArquivo.AdicionarRange(arquivosAInserir);
            }

            _repositorioArquivo.SalvarAlteracoes();
        }

        public void LimparOcorrencias(LoteDTO lote)
        {
            if (lote.Codigo > 0)
            {
                var ocorrencias = _contexto.Ocorrencias.Where(o => o.ProcessamentoId == lote.Codigo).ToList();
                ocorrencias.ForEach(o => _repositorioOcorrencia.Excluir(o));
                _repositorioOcorrencia.SalvarAlteracoes();
            } 
        }

        public void GravarOcorrencias(IEnumerable<ContribuinteDTO> ocorrenciasDto, LoteDTO lote, int processamentoId)
        {
            var resultado = new List<Ocorrencia>();

            foreach (var item in ocorrenciasDto)
            {
                resultado.Add(new Ocorrencia()
                {
                    ProcessamentoId = processamentoId,
                    SituacaoId = (short)item.Status,
                    LoginUsuario = lote.LoginUsuario,
                    DescricaoOcorrencia = String.Format("CPF/CNPJ {0} não foi possível separar arquivo.", item.CpfCnpj)
                });
            }

            //Caso ocorra erro
            if (lote.TipoSituacao == TipoSituacao.Erro)
            {
                resultado.Add(new Ocorrencia()
                {
                    ProcessamentoId = processamentoId,
                    SituacaoId = (short)StatusContribuinte.FalhaArquivo,
                    LoginUsuario = lote.LoginUsuario,
                    DescricaoOcorrencia = String.Format("Erro no processamento: {0} {1}", lote.CodigoErro, lote.MensagemErro)
                });
            }

            //Data hora processamento
            resultado.Add(new Ocorrencia()
            {
                ProcessamentoId = processamentoId,
                SituacaoId = (short)(lote.TipoSituacao == TipoSituacao.Erro ? StatusContribuinte.FalhaArquivo : StatusContribuinte.Sucesso),
                LoginUsuario = lote.LoginUsuario,
                DescricaoOcorrencia = String.Format("Data/Hora início do processamento: {0} - Data/Hora final do processamento: {1}", lote.InicioProcessamento.ToString("g"), DateTime.Now.ToString("g"))
            });

            //Totais arquivos importados
            resultado.Add(new Ocorrencia()
            {
                ProcessamentoId = processamentoId,
                SituacaoId = (short)(lote.TipoSituacao == TipoSituacao.Erro ? StatusContribuinte.FalhaArquivo : StatusContribuinte.Sucesso),
                LoginUsuario = lote.LoginUsuario,
                DescricaoOcorrencia = String.Format("Quantidade de Arquivos PDF importados: {0}", lote.TotalArquivosImportados)
            });

            //Totais arquivos gerados
            resultado.Add(new Ocorrencia()
            {
                ProcessamentoId = processamentoId,
                SituacaoId = (short)(lote.TipoSituacao == TipoSituacao.Erro ? StatusContribuinte.FalhaArquivo : StatusContribuinte.Sucesso),
                LoginUsuario = lote.LoginUsuario,
                DescricaoOcorrencia = String.Format("Arquivos gerados com sucesso: {0}", lote.TotalArquivosGerados)
            });

            _repositorioOcorrencia.AdicionarRange(resultado);
            _repositorioOcorrencia.SalvarAlteracoes();

        }

        public void GravarLog(LoteDTO lote, int processamentoId)
        {
            var log = new Log()
            {
                DataRegistroAcao = DateTime.Now,
                LoginUsuario = lote.LoginUsuario,
                DescricaoUsuario = String.Format("{0} - Rotina {1} {2}", lote.Operacao.GetDescription(), processamentoId, lote.TipoSituacao.GetDescription())
            };
            _repositorioLog.Adicionar(log);
            _repositorioLog.SalvarAlteracoes();
        }
    }
}