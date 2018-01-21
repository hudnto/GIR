using System;
using System.Collections.Generic;
using GIR.Integracao.Model;

namespace GIR.Integracao.Fakes.Interace
{
    public abstract class IServicoEmail
    {
        private readonly List<string> _destinatarios;

        protected IServicoEmail()
        {
            _destinatarios = new List<string>();
        }

        public void AdicionarDestinatarios(List<String> destinatarios)
        {
            _destinatarios.AddRange(destinatarios);
        }

        public void AdicionarDestinatario(String destinatario)
        {
            _destinatarios.Add(destinatario);
        }

        public abstract void EnviarEmail(EstruturaEmail estruturaEmail);
    }
}
