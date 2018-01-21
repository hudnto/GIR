using System;
using GIR.Integracao.ServicoColaborador;
using Colaborador = GIR.Integracao.Model.Colaborador;
using UnidadeOrganizacional = GIR.Integracao.Model.UnidadeOrganizacional;

namespace GIR.Integracao.Fakes
{
    public class ServicoColaborador
    {
        private readonly ServicoColaboradorClient _servico;

        public ServicoColaborador()
        {
            _servico = new ServicoColaboradorClient();
        }

        public Colaborador ObterColaborador(string loginUsuario)
        {
            Colaborador colaborador;

            try
            {
                RetornoColaborador retorno = _servico.ObterColaborador(loginUsuario);
                var colaboradorServico = retorno.Colaborador;

                colaborador = new Colaborador
                {
                    Nome = colaboradorServico.NomeCompletoColaborador,
                    Cargo = colaboradorServico.NomeCargoColaborador,
                    Email = colaboradorServico.EmailColaborador,
                    UnidadeOrganizacional = new UnidadeOrganizacional
                    {
                        Codigo= colaboradorServico.UnidadeOrganizacionalColaborador.CodigoUnidadeOrganizacional,
                        Nome = colaboradorServico.UnidadeOrganizacionalColaborador.NomeUnidadeOrganizacional,
                        Email = colaboradorServico.UnidadeOrganizacionalColaborador.NomeUnidadeOrganizacional
                    }
                };
            }
            catch (Exception exception)
            {
                throw new Exception("Erro ao recuperar os dados do colaborador " + loginUsuario + " do IServicoColaborador", exception);
            }

            return colaborador;
        }
    }
}
