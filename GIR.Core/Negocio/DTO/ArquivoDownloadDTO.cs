namespace GIR.Core.Negocio.DTO
{
    /// <summary>
    /// Objeto para trafegar os dados necessários para fazer o download do arquivo.
    /// </summary>
    public class ArquivoDownloadDTO
    {
        public int AnoExercicio { get; set; }
        public string CaminhoDiretorio { get; set; }
        public string NomeArquivo { get; set; }
        public string ExtensaoArquivo { get; set; }
    }
}
