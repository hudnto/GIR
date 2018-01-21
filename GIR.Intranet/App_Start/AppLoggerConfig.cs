using AppLogger.Enums;
using AppLogger.Interfaces;
using AppLogger.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace GIR.Intranet
{
    public class AppLoggerConfig : IConfigLog
    {
        public TipoArmazenamentoLoggerEnum GetTipoArmazenamento()
        {
            return TipoArmazenamentoLoggerEnum.FILESYSTEM;
        }

        public DbConnection ConnectionDataBase()
        {
            return null;
        }

        public string GetLocalArmazenamentoLog()
        {
            return System.Configuration.ConfigurationManager.AppSettings["logsAppLogger"];
        }

        public int GetLimiteMaximoMegaBytesArquivoLog()
        {
            return 10;//dez mega de limite
        }

        public TipoPeriodoExpurgoEnum GetPeriodoExpurgoLog()
        {
            return TipoPeriodoExpurgoEnum.MENSAL;
        }

        public string GetLocalArmazenamentoExpurgoLog()
        {
            return "";
        }

        public TipoLoggerEnum GetTipoLogAtivo()
        {
            return TipoLoggerEnum.TODOS;
        }

        public int GetQtdMinimaNotificacao()
        {
            return 0;
        }

        public List<IEmailLog> GetEmailsNotificacaoLog()
        {
            return null;
        }

        public IConfigSMTP GetConfigServidorEmail()
        {
            return null;
        }

        public string GetHostServidorEmail()
        {
            return null;
        }

        public IEmailLog GetEmailRemetenteLog()
        {
            return null;
        }

        public bool RegraNotificacao(LogEntry log)
        {
            return false;
        }
    }
}