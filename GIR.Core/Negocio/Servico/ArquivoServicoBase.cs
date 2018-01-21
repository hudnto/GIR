using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using Cassi.Utilitarios.Util;
using GIR.Core.Infraestrutura.Helpers;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace GIR.Core.Negocio.Servico
{
    public class ArquivosServicoBase
    {
        public readonly String CaminhoDiretorio;
        private String UsuarioRede { get; set; }
        private String SenhaRede { get; set; }
        public NetworkCredential Usuario { get; set; }
        public static readonly String PastaTempUpload = String.Format("TEMP{0}", new Random().Next(1, 5000));

        public ArquivosServicoBase()
        {
            var nomeServer = ConfigurationManager.AppSettings["NomeServer"].RemoveContraBarra();
            var diretorio = ConfigurationManager.AppSettings["DiretorioServer"].RemoveContraBarra();

            UsuarioRede = ConfigurationManager.AppSettings["UsuarioRede"];
            SenhaRede = ConfigurationManager.AppSettings["SenhaRede"];
            CaminhoDiretorio = String.Format(@"\\{0}\{1}", nomeServer, diretorio);
            Usuario = new NetworkCredential(UsuarioRede, SenhaRede);
        }

        public bool ArquivoExiste(string path)
        {
            using (RedeUtil.RecuperarConexaoRede(CaminhoDiretorio, Usuario))
            {
                return File.Exists(path);
            }
        }

        public void MoverArquivo(string pathOrigem, string pathDestino)
        {
            using (RedeUtil.RecuperarConexaoRede(CaminhoDiretorio, Usuario))
            {
                File.Move(pathOrigem, pathDestino);
            }
        }

        public void DeletarArquivo(string path)
        {
            using (RedeUtil.RecuperarConexaoRede(CaminhoDiretorio, Usuario))
            {
                File.Delete(path);
            }
        }

        public String BuscaExtensaoArquivo(string path)
        {
            using (RedeUtil.RecuperarConexaoRede(CaminhoDiretorio, Usuario))
            {
                return Path.GetExtension(path).ToLower();
            }
        }

        public byte[] GetBytesArquivo(string path)
        {
            using (RedeUtil.RecuperarConexaoRede(CaminhoDiretorio, Usuario))
            {
                return File.ReadAllBytes(path);
            }
        }

        public PdfReader InstanciarPdfReader(string path)
        {
            using (RedeUtil.RecuperarConexaoRede(CaminhoDiretorio, Usuario))
            {
                return new PdfReader(path);
            }
        }

        public PdfCopy InstanciarPdfCopy(Document doc, string path)
        {
            using (RedeUtil.RecuperarConexaoRede(CaminhoDiretorio, Usuario))
            {
                return new PdfCopy(doc, new FileStream(path, FileMode.Create));
            }
        }

        public bool DiretorioExiste(string path)
        {
            using (RedeUtil.RecuperarConexaoRede(CaminhoDiretorio, Usuario))
            {
                return Directory.Exists(path);
            }
        }

        public void RemoverDiretorio(string path)
        {
            using (RedeUtil.RecuperarConexaoRede(CaminhoDiretorio, Usuario))
            {
                if (Directory.Exists(path))
                    Directory.Delete(path, true);
            }
        }

        public void CriarDiretorios(IEnumerable<string> caminhos)
        {
            using (RedeUtil.RecuperarConexaoRede(CaminhoDiretorio, Usuario))
            {
                foreach (var caminho in caminhos)
                {
                    Directory.CreateDirectory(caminho);
                }
            }
        }

        public void CriarDiretorio(string caminho)
        {
            using (RedeUtil.RecuperarConexaoRede(CaminhoDiretorio, Usuario))
            {
                Directory.CreateDirectory(caminho);
            }
        }
    }
}