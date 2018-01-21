using System.Collections.Generic;
using System.Linq;
using GIR.Integracao.Model;
using GIR.Integracao.WsParticipanteConsulta;

namespace GIR.Integracao.Fachadas
{
    public class ServicoParticipanteFachada : IServicoParticipante
    {
        private readonly ParticipanteConsultaClient _client;

        public ServicoParticipanteFachada()
        {
            _client = new ParticipanteConsultaClient();
        }

        public IEnumerable<ListarParticipante> ListarParticipantes(int usuarioId)
        {
            var participante = Enumerable.Empty<ListarParticipante>();

            var consulta = _client.listarParticipante(usuarioId, null, 0, null, null);

            if (consulta != null)
            {
                participante = consulta.Select(p => new ListarParticipante()
                {
                    participante = p.participante 
                });
            }

            return participante;
        }

        public ParticipanteModel ObterParticipante(int participanteId)
        {
            return GetFake();
        }

        private ParticipanteModel GetFake()
        {
            return new ParticipanteModel()
            {
                Email = "emailfachada@cassi.com.br",
                Cpf = "12345678900",
                Handle = 123
            };
        }
    }
}
