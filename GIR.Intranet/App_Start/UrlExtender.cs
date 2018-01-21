using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GIR.Intranet
{
    public static class UrlExtender
    {
        /// <summary>
        /// Para tratar a identificação o ambiente onde está rodando a aplicação, 
        /// criamos um extensão do @Url.ContentScript para facilitar a implementação
        /// dos script (javascript) nas páginas.
        /// </summary>
        /// <param name="instance">Instância do UrlHelper.</param>
        /// <param name="scriptLocalFile">Local de armazenamento do arquivo de Script: exemplo /bootstrap/js/bootstrap.3.1.1.js</param>
        /// <param name="loadJsIsDebugging">Indica a necessidade de verificar se a aplicação está em modo Debug </param>
        /// <returns></returns>
        public static string ContentScript(this UrlHelper instance, string scriptLocalFile, bool loadJsIsDebugging = true)
        {
            var urlServerScripts = System.Configuration.ConfigurationManager.AppSettings["urlServerScripts"];
            if (string.IsNullOrEmpty(urlServerScripts))
            {//Adicionar url padrão... se tudo der errado;
                urlServerScripts = "https://servicosonline.cassi.com.br/commonweb";
            }
            if (!scriptLocalFile.StartsWith("/"))
            {//Adicionar '/' para separação do servidor e arquivo JS.
                scriptLocalFile = "/" + scriptLocalFile;
            }
            var pathFileScript = urlServerScripts + scriptLocalFile;
            if (loadJsIsDebugging && instance.RequestContext.HttpContext.IsDebuggingEnabled)
            {//Verificar se o sistema está em modo Debug e recuperar o JS sem a compactação dos espaços (min.js);
                if (!pathFileScript.EndsWith(".js"))
                {//Adicionar a extenção do JS;
                    pathFileScript += ".js";
                }
            }
            else
            {
                if (!pathFileScript.EndsWith(".js") && !pathFileScript.EndsWith(".min.js"))
                {//Adicionar a extenção do JS com compactação dos espaços (mim.js);
                    pathFileScript += ".min.js";
                }
            }
            return instance.Content(pathFileScript);
        }

        /// <summary>
        /// Para tratar a identificação o ambiente onde está rodando a aplicação, 
        /// criamos um extensão do UrlHelper (@Url.ContentStyle) para facilitar a implementação
        /// dos styles (css) nas páginas.
        /// </summary>
        /// <param name="instance">Instância do UrlHelper.</param>
        /// <param name="styleLocalFile">Local de armazenamento do arquivo de Style (css): exemplo /bootstrap/css/bootstrap.3.1.1.css</param>
        /// <param name="loadCssIsDebugging">Indica a necessidade de verificar se a aplicação está em modo Debug </param>
        /// <returns></returns>
        public static string ContentStyle(this UrlHelper instance, string styleLocalFile, bool loadCssIsDebugging = true)
        {
            var urlServerStyles = System.Configuration.ConfigurationManager.AppSettings["urlServerStyles"];
            if (string.IsNullOrEmpty(urlServerStyles))
            {//Adicionar url padrão... se tudo der errado;
                urlServerStyles = "https://servicosonline.cassi.com.br/commonweb";
            }
            if (!styleLocalFile.StartsWith("/"))
            {//Adicionar '/' para separação do servidor e arquivo Css.
                styleLocalFile = "/" + styleLocalFile;
            }
            var pathFileStyle = urlServerStyles + styleLocalFile;
            if (loadCssIsDebugging && instance.RequestContext.HttpContext.IsDebuggingEnabled)
            {//Verificar se o sistema está em modo Debug e recuperar o Css sem a compactação dos espaços (min.css);
                if (!pathFileStyle.EndsWith(".css"))
                {//Adicionar a extenção do CSS;
                    pathFileStyle += ".css";
                }
            }
            else
            {
                if (!pathFileStyle.EndsWith(".css") && !pathFileStyle.EndsWith(".min.css"))
                {//Adicionar a extenção do CSS com compactação dos espaços (mim.css);
                    pathFileStyle += ".min.css";
                }
            }
            return instance.Content(pathFileStyle);
        }

        /// <summary>
        /// Para tratar a identificação o ambiente onde está rodando a aplicação, 
        /// criamos um extensão do UrlHelper (@Url.ContentImagem) para facilitar a implementação
        /// dos imagens padrão da CASSI nas páginas.
        /// </summary>
        /// <param name="instance">Instância do UrlHelper.</param>
        /// <param name="imagemLocalFile">Local de armazenamento do arquivo de Imagem: exemplo /cassi/imgs/favicon.ico</param>
        /// <returns></returns>
        public static string ContentImagem(this UrlHelper instance, string imagemLocalFile)
        {
            var urlServerImagens = System.Configuration.ConfigurationManager.AppSettings["urlServerImagens"];
            if (string.IsNullOrEmpty(urlServerImagens))
            {//Adicionar url padrão... se tudo der errado;
                urlServerImagens = "https://servicosonline.cassi.com.br/commonweb";
            }
            if (!imagemLocalFile.StartsWith("/"))
            {//Adicionar '/' para separação do servidor e arquivo de imagem.
                imagemLocalFile = "/" + imagemLocalFile;
            }
            var pathFileImagem = urlServerImagens + imagemLocalFile;
            return instance.Content(pathFileImagem);
        }

        /// <summary>
        /// Metodo para retornar a url do javascript com a versão do  Assembly  ex: ~/Content/scripts/AppCassiPadrao.js?v=1.0.0.0
        /// com isso a aplicação não guarda cache de script se a versão for alterada do produto. 
        /// <param name="instance">Instância do UrlHelper.</param>
        /// <param name="url"> destino a ser retornado </param>
        /// <returns></returns>
        public static string ContentVersion(this UrlHelper instance, string url)
        {
            return string.Format("{0}?v={1}", instance.Content(url), System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }

    }
}