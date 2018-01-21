namespace GIR.Integracao.Model
{
    public class UnidadeOrganizacional
    {
        public int Codigo { get; set; }   
        public int? CodigoPai { get; set; }  
        public string Nome { get; set; }  
        public string Email { get; set; }
    }
}
