using AppLogger;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GIR.Intranet
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            Log.InitLog(GetKeyAppLogger(), new AppLoggerConfig());

            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        /// <summary>
        /// Recupera o 'keylog' de identificação dos registros de log,
        /// para facilitar será recuperado no nome do projeto.
        /// </summary>
        /// <returns></returns>
        private string GetKeyAppLogger()
        {
            string appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            if (appName.Contains('.'))
            {//Pegar somente o primeiro nome da aplicação (AppCassiPadrao), sem a camada: ex.: AppCassiPadrao.Apresentacao;
                appName = appName.Substring(0, appName.IndexOf('.'));
            }
            return "Log" + appName;
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            System.Web.HttpContext context = HttpContext.Current;
            var pag = context.Request.RawUrl;
            System.Exception erro = Context.Server.GetLastError();
            var pathErro = System.Configuration.ConfigurationManager.AppSettings["urlPaginaErro"];
            var logEntry = Log.Get().LogError(erro, pag);
            Response.Redirect(string.Format("{0}?Id={1}&dataHora={2}", pathErro, logEntry.Id, logEntry.DataHora.ToString("s")));
        }
    }
}