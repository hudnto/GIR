using System.Collections.Generic;
using System.Linq;

namespace GIR.Integracao.Model
{
    public class PerfilUsuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }

        #region Conversores
        internal static ICollection<PerfilUsuario> Converter(ICollection<ServicoGASC.PerfilUsuario> perfilsUsuarioServico)
        {
            ICollection<PerfilUsuario> perfilsUsuario = perfilsUsuarioServico.Select(Converter).ToList();
            return perfilsUsuario;
        }

        private static PerfilUsuario Converter(ServicoGASC.PerfilUsuario origem)
        {
            PerfilUsuario destino = new PerfilUsuario
            {
                Id = origem.Id,
                Nome = origem.Nome,
                Descricao = origem.Descricao
            };

            return destino;
        }
        #endregion
    }
}
