using System;
using System.Collections.Generic;
using System.Linq;
using GIR.Core.Infraestrutura.Cache;
using GIR.Integracao;
using GIR.Integracao.Fakes;
using GIR.Integracao.Model;
using GIR.Integracao.ServicoUnidadeOrganizacional;
using ServicoException = GIR.Core.Dados.Infraestrutura.ServicoException;
using UnidadeOrganizacional = GIR.Integracao.Model.UnidadeOrganizacional;

namespace GIR.Core.Negocio.Servico
{
    public class FachadaServico
    {
        private readonly IUtilitarioConsulta _utilitarioServico;
        private readonly ServicoUnidadeOrganizacionalClient _unidadeOrganizacionalServico;
        private readonly ServicoEmailCassi _emailServico;
        private readonly CacheServico _servicoCache;

        public FachadaServico()
        {
            _utilitarioServico = new UtilitarioConsulta();
            _emailServico = new ServicoEmailCassi();
            _servicoCache = new CacheServico();
            _unidadeOrganizacionalServico = new ServicoUnidadeOrganizacionalClient();
        }

        public void EnviarEmail(EstruturaEmail estrutura)
        {
            try
            {
                _emailServico.EnviarEmail(estrutura);
            }
            catch (Exception e)
            {
                throw new ServicoException(string.Format("Erro ao enviar o Email utilizando o Serviço {0}", _emailServico.GetType().Name), e);
            }
        }

        public IEnumerable<Uf> ListarEstados()
        {
            try
            {
                IEnumerable<Uf> estadosModelo = _servicoCache.ObterOuAtribuir("FachadaServico.ListarUfs",
                    () => _utilitarioServico.ListarUfs());

                var estados = estadosModelo
                    .Select(e => new Uf()
                    {
                        Id = e.Id,
                        Nome = e.Nome,
                        Sigla = e.Sigla
                    }).ToList();

                return estados;
            }
            catch (Exception e)
            {
                throw new ServicoException(string.Format("Erro ao buscar os Estados no Serviço {0}", _utilitarioServico.GetType().Name), e);
            }
        }

        public Uf ObterUf(int uf)
        {
            try
            {
                return _utilitarioServico.ObterUfPorId(uf);
            }
            catch (Exception e)
            {
                throw new ServicoException(string.Format("Erro ao buscar os Estados no Serviço {0}", _utilitarioServico.GetType().Name), e);
            }
        }

      
    }
}
