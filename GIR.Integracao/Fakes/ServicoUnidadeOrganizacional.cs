using System;
using System.Collections.Generic;
using System.Linq;
using GIR.Integracao.ServicoUnidadeOrganizacional;
using GIR.Integracao.Fakes.Interace;
using IServicoUnidadeOrganizacional = GIR.Integracao.Fakes.Interace.IServicoUnidadeOrganizacional;
using UnidadeOrganizacional = GIR.Integracao.Model.UnidadeOrganizacional;

namespace GIR.Integracao.Fakes
{
    public class ServicoUnidadeOrganizacional : IServicoUnidadeOrganizacional
    {
        private readonly ServicoUnidadeOrganizacionalClient _servico;
        private readonly List<EstruturaOrganizacional> _listaEstruturas;
        private const int UnidadeSuperior = 0;

        public ServicoUnidadeOrganizacional()
        {
            _servico = new ServicoUnidadeOrganizacionalClient();
            _listaEstruturas = new List<EstruturaOrganizacional>();
        }

      
        public virtual List<UnidadeOrganizacional> ListarUnidades()
        {
            var unidades = new List<UnidadeOrganizacional>();

            try
            {
                GerarEstruturaOrganizacional(UnidadeSuperior, unidades, true);
            }
            catch (Exception exception)
            {
                throw new Exception("Erro ao recuperar os dados das unidades organizacionais do ServicoUnidadeOrganizacional", exception);
            }

            return unidades;
        }

        public List<UnidadeOrganizacional> RecuperarUnidadesSubordinadasPorId(int unidadeId, bool includeSelf = false)
        {
            var unidades = new List<UnidadeOrganizacional>();

            try
            {
                GerarEstruturaOrganizacional(unidadeId, unidades, includeSelf);
            }
            catch (Exception exception)
            {
                throw new Exception("Erro ao recuperar os dados das unidades organizacionais do ServicoUnidadeOrganizacional", exception);
            }

            return unidades;
        }

        public virtual RetornoEstruturaOrganizacional ObterEstruturaOrganizacional(int unidadeSuperior)
        {
            var estrutura = new RetornoEstruturaOrganizacional();
            try
            {
                estrutura = _servico.ObterEstruturaOrganizacional(unidadeSuperior);
            }
            catch (Exception exception)
            {
                throw new Exception("Erro ao recuperar os dados da estrutura organizacional do ObterEstruturaOrganizacional", exception);
            }

            return estrutura;
        }

        private void GerarEstruturaOrganizacional(int unidadeSuperior, List<UnidadeOrganizacional> unidades, bool includeSelf)
        {
            var retornoServico = ObterEstruturaOrganizacional(unidadeSuperior);
            var unidadesSubordinadas = retornoServico.EstruturaOrganizacional.UnidadesOrganizacionaisSubordinadas != null
                ? retornoServico.EstruturaOrganizacional.UnidadesOrganizacionaisSubordinadas.ToList()
                : new List<EstruturaOrganizacional>();
            
            if(includeSelf)
            _listaEstruturas.Add(retornoServico.EstruturaOrganizacional);

            foreach (var unidade in unidadesSubordinadas)
            {
                _listaEstruturas.Add(unidade);
                ExtrairSubordinadas(unidade);
            }

            _listaEstruturas.ForEach(u => unidades.Add(new UnidadeOrganizacional
            {
                Codigo = u.CodigoUnidadeOrganizacional,
                CodigoPai = u.UnidadeOrganizacionalSuperior != null ? u.UnidadeOrganizacionalSuperior.CodigoUnidadeOrganizacional : new int?(),
                Email = u.EmailUnidadeOrganizacional,
                Nome = u.NomeUnidadeOrganizacional
            }));

            _listaEstruturas.Clear();
        }

        private void ExtrairSubordinadas(EstruturaOrganizacional estrutura)
        {
            if (estrutura == null)
                return;

            foreach (var unidade in estrutura.UnidadesOrganizacionaisSubordinadas
                ?? Enumerable.Empty<EstruturaOrganizacional>())
            {
                _listaEstruturas.Add(unidade);
                ExtrairSubordinadas(unidade);
            }
        }

        public Integracao.ServicoUnidadeOrganizacional.UnidadeOrganizacional ObterUnidadeOrganizacional(int codigoUnidade)
        {
            throw new NotImplementedException();
        }
    }
}
