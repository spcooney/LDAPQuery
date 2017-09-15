using Microsoft.SharePoint.Administration;
using System.Collections.Generic;

namespace SPC.LDAP.ProfileSync.Configuration
{
    public class Logger : SPDiagnosticsService
    {
        public static string DiagnosticAreaName = "SPC Sync";
        private static Logger _logger;
        public static Logger Current
        {
            get
            {
                if (_logger == null)
                {
                    _logger = new Logger();
                }

                return _logger;
            }
        }

        public Logger()
            : base("SPC Sync Logging Service", SPFarm.Local)
        {

        }

        public static class Category
        {
            public const string Error = "SPC Sync Error";
            public const string Info = "SPC Sync Info";
        }

        protected override IEnumerable<SPDiagnosticsArea> ProvideAreas()
        {
            List<SPDiagnosticsArea> areas = new List<SPDiagnosticsArea>
            {
                new SPDiagnosticsArea(DiagnosticAreaName, new List<SPDiagnosticsCategory>
                {
                    new SPDiagnosticsCategory(Category.Error, TraceSeverity.Unexpected, EventSeverity.Error),
                    new SPDiagnosticsCategory(Category.Info, TraceSeverity.Verbose, EventSeverity.Information)
                })
            };

            return areas;
        }


        public static void WriteInfo(string message)
        {
            WriteLog(Category.Info, DiagnosticAreaName, message);
        }

        public static void WriteInfo(string source, string message)
        {
            WriteLog(Category.Info, source, message);
        }

        public static void WriteError(string message)
        {
            WriteLog(Category.Error, DiagnosticAreaName, message);
        }

        public static void WriteError(string source, string message)
        {
            WriteLog(Category.Error, source, message);
        }

        public static void WriteLog(string categoryName, string source, string message)
        {
            SPDiagnosticsCategory category = Logger.Current.Areas[DiagnosticAreaName].Categories[categoryName];
            Logger.Current.WriteTrace(0, category, category.TraceSeverity, string.Concat(message));
        }
    }
}
