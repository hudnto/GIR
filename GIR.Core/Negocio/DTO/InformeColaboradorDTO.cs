namespace GIR.Core.Negocio.DTO
{
    /// <summary>
    /// Objeto para trafegar os dados de informe de rendimento do colbaorador Cassi pelas 
    /// camdas da aplicação.
    /// </summary>
    public class InformeColaboradorDTO
    {
        public int AnoExercicio { get; set; }
        public string CPFCNPJ { get; set; }
        public string CaminhoDiretorio { get; set; }
        public string ExtensaoArquivo { get; set; }
    }
}
