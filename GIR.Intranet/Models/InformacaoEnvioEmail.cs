using System;

namespace GIR.Intranet.Models
{
    public class InformacaoEnvioEmail
    {
        public string Erro { get; set; }
        public DateTime Data { get; set; }
        public bool Enviado { get; set; }
    }
}