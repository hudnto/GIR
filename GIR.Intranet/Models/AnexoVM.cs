using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using GIR.Core.Negocio.Mensagem;
using iTextSharp.text.pdf;

namespace GIR.Intranet.Models
{
    public class AnexoVM : IValidatableObject
    {
        public List<HttpPostedFileBase> Arquivos { get; set; }

        public String TipoArquivo { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var resultado = new List<ValidationResult>();
            var contentType = TipoArquivo;
            var extensaoValida = (contentType == "application/pdf") ? ".pdf" : ".txt";
            var nomeCampo = (contentType == "application/pdf") ? "PDF" : "TXT";
            const int vinteMegaBytes = 20971520;

            if (String.IsNullOrEmpty(contentType))
            {
                var display = String.Format(Mensagem.MA001, nomeCampo);
                resultado.Add(new ValidationResult(display, new List<String> { nomeCampo }));
                return resultado;
            }

            if (Arquivos == null)
            {
                var display = String.Format(Mensagem.MA001, nomeCampo);
                resultado.Add(new ValidationResult(display, new List<String> { nomeCampo }));
                return resultado;
            }

            if (Arquivos.Any(a => a == null))
            {
                var display = String.Format(Mensagem.MA001, nomeCampo);
                resultado.Add(new ValidationResult(display, new List<String> { nomeCampo }));
                return resultado;
            }

            foreach (var arquivo in Arquivos)
            {
                var extensao = Path.GetExtension(arquivo.FileName);

                if (String.IsNullOrEmpty(extensao))
                {
                    resultado.Add(new ValidationResult(Mensagem.MA004, new List<String> { nomeCampo }));
                    return resultado;
                }

                if (arquivo.InputStream.Length > vinteMegaBytes)
                    resultado.Add(new ValidationResult(Mensagem.MA004, new List<String> { nomeCampo }));                    

                if (arquivo.ContentType != contentType)
                    resultado.Add(new ValidationResult(Mensagem.MA004, new List<String> { nomeCampo }));                    
                
                if (extensaoValida != extensao.ToLower())
                    resultado.Add(new ValidationResult(Mensagem.MA004, new List<String> { nomeCampo }));
            }

            return resultado;
        }

        private bool ValidoPdf(Stream stream, out string mensagem)
        {
            bool valido = true;

            PdfReader reader = null;

            try
            {
                reader = new PdfReader(stream);
                mensagem = String.Empty;
            }
            catch (Exception e)
            {
                mensagem = e.Message;
                valido = false;
            }

            reader.Close();

            return valido;
        }
    }
}