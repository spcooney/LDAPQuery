using Microsoft.SharePoint.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Web.Script.Serialization;

namespace SPC.LDAP.ProfileSync.Configuration
{
    [Guid("670C34D6-A4EF-40B7-9054-C7C4EDB9DF6D")]
    public class SyncConfiguration : SPPersistedObject
    {
        public static string StorageName = "ProfileSyncConfig";

        [Persisted]
        private Dictionary<string, string> _propertytMappings = new Dictionary<string, string>();

        [Persisted]
        private Dictionary<string, string> _syncRecords = new Dictionary<string, string>();

        [Persisted]
        private Dictionary<string, string> _syncTargets = new Dictionary<string, string>();

        [Persisted]
        private string _idProperty = "";

        public string IdProperty
        {
            get { return !String.IsNullOrWhiteSpace(_idProperty) ? _idProperty : "UID"; }
            set { _idProperty = value; }
        }

        [Persisted]
        private string _emailProperty = "";

        public string EmailProperty
        {
            get { return !String.IsNullOrWhiteSpace(_emailProperty) ? _emailProperty : "mail"; }
            set { _emailProperty = value; }
        }

        [Persisted]
        private string _modifiedProperty = "";

        public string ModifiedProperty
        {
            get { return !String.IsNullOrWhiteSpace(_modifiedProperty) ? _modifiedProperty : "whenChanged"; }
            set { _modifiedProperty = value; }
        }

        [Persisted]
        private string _ldapUri = "";

        public string LdapURI
        {
            get { return !String.IsNullOrWhiteSpace(_ldapUri) ? _ldapUri : "LDAP://ldap.spcury.com"; }
            set { _ldapUri = value; }
        }

        [Persisted]
        private string _rootOU = "";

        public string RootOU
        {
            get { return _rootOU; }
            set { _rootOU = value; }
        }

        [Persisted]
        private bool _debugPrint = false;

        public bool DebugPrint
        {
            get { return _debugPrint; }
            set { _debugPrint = value; }
        }

        [Persisted]
        private string _proxyGroup = "";

        public string ProxyGroup
        {
            get { return !String.IsNullOrWhiteSpace(_proxyGroup) ? _proxyGroup : "SPCProfileService"; }
            set { _proxyGroup = value; }
        }

        public SyncConfiguration()
            :base()
        {
        }

        public SyncConfiguration(string name, SPPersistedObject parent)
            :base(name, parent)
        {
        }

        public SyncConfiguration(string name, SPPersistedObject parent, Guid id)
            :base(name, parent, id)
        {
        }

        public static Guid ObjectId
        {
            get { return new Guid("670C34D6-A4EF-40B7-9054-C7C4EDB9DF6D"); }
        }

        protected override bool HasAdditionalUpdateAccess()
        {
            return true;
        }

        public void AddDefaultPropertyMappings()
        {
            Logger.WriteInfo("Adding default property mappings");
            AddPropertyMapping("mail", "WorkEmail");
            AddPropertyMapping("sn", "LastName");
            AddPropertyMapping("givenName", "FirstName");
            AddPropertyMapping("cn", "PreferredName");
            AddPropertyMapping("title", "Title");
            AddPropertyMapping("telephoneNumber", "WorkPhone");
        }

        public void AddDefaultOUs()
        {
            AddSyncTarget("Example 1", "Department 1, People");
            AddSyncTarget("Example 2", "Department 2, People");
        }

        public void AddPropertyMapping(string source, string destination, string name = "")
        {
            var mapping = destination + "==" + name;

            if (_propertytMappings.ContainsKey(source))
            {
                _propertytMappings[source] = mapping;
            }
            else
            {
                _propertytMappings.Add(source, mapping);
            }
        }

        public void RemovePropertyMapping(string source)
        {
            if (_propertytMappings.ContainsKey(source))
            {
                _propertytMappings.Remove(source);
            }
        }

        public void AddSyncRecord(string ou, DateTime syncTime)
        {
            if (_syncRecords.ContainsKey(ou))
            {
                _syncRecords[ou] = syncTime.ToString();
            }
            else
            {
                _syncRecords.Add(ou, syncTime.ToString());
            }
        }

        public void RemoveSyncRecord(string ou)
        {
            if (_syncRecords.ContainsKey(ou))
            {
                _syncRecords.Remove(ou);
            }
        }

        public void AddSyncTarget(string name, string targetOU)
        {
            if (_syncTargets.ContainsKey(name))
            {
                _syncTargets[name] = targetOU;
            }
            else
            {
                _syncTargets.Add(name, targetOU);
            }
        }

        public void RemoveSyncTarget(string name)
        {
            if (_syncTargets.ContainsKey(name))
            {
                _syncTargets.Remove(name);
            }
        }

        public string Export()
        {
            try
            {
                var buffer = new StringBuilder();

                if (_propertytMappings == null)
                {
                    _propertytMappings = new Dictionary<string, string>();
                }

                buffer.Append("{\r\n");
                buffer.Append(String.Format("\t\"DebugPrint\": \"{0}\",\r\n", DebugPrint.ToString()));
                buffer.Append(String.Format("\t\"ProxyGroup\": \"{0}\",\r\n", ProxyGroup));
                buffer.Append(String.Format("\t\"LDAPURI\": \"{0}\",\r\n", LdapURI));
                buffer.Append(String.Format("\t\"RootOU\": \"{0}\",\r\n", RootOU));
                buffer.Append(String.Format("\t\"IdProperty\": \"{0}\",\r\n", IdProperty));
                buffer.Append(String.Format("\t\"EmailProperty\": \"{0}\",\r\n", EmailProperty));
                buffer.Append(String.Format("\t\"ModifiedProperty\": \"{0}\",\r\n", ModifiedProperty));

                buffer.Append("\t\"Properties\":\r\n\t[\r\n");

                var count = PropertyMappings.Count;
                var i = 0;

                foreach (var mapping in PropertyMappings)
                {
                    buffer.Append(String.Format("\t\t{{\r\n\t\t\t\"Name\": \"{0}\",\r\n\t\t\t\"Source\": \"{1}\",\r\n\t\t\t\"Destination\": \"{2}\"\r\n\t\t}}", mapping.Name, mapping.Source, mapping.Destination));

                    if (++i < count)
                    {
                        buffer.Append(",");
                    }

                    buffer.Append("\r\n");
                }

                buffer.Append("\t],\r\n");

                //sync targets
                buffer.Append("\t\"SyncTargets\":\r\n\t[\r\n");

                count = SyncTargets.Count;
                i = 0;

                foreach (var target in SyncTargets)
                {
                    buffer.Append(String.Format("\t\t{{\r\n\t\t\t\"Name\": \"{0}\",\r\n\t\t\t\"OU\": \"{1}\"\r\n\t\t}}", target.Name, target.TargetOU));

                    if (++i < count)
                    {
                        buffer.Append(",");
                    }

                    buffer.Append("\r\n");
                }

                buffer.Append("\t],\r\n");

                //sync records

                buffer.Append("\t\"SyncRecords\":\r\n\t[\r\n");

                count = SyncRecords.Count;
                i = 0;

                foreach (var mapping in SyncRecords)
                {
                    buffer.Append(String.Format("\t\t{{\r\n\t\t\t\"OU\": \"{0}\",\r\n\t\t\t\"LastSync\": \"{1}\"\r\n\t\t}}", mapping.OUName, mapping.SyncTime));

                    if (++i < count)
                    {
                        buffer.Append(",");
                    }

                    buffer.Append("\r\n");
                }

                buffer.Append("\t]\r\n");

                buffer.Append("}");

                return buffer.ToString();
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.Category.Error, Logger.DiagnosticAreaName, ex.ToString());
                throw;
            }
        }

        public string Import(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException("Input is required", "input");
            }

            if (_propertytMappings == null)
            {
                _propertytMappings = new Dictionary<string, string>();
            }

            try
            {
                var result = "OK";

                var j = new JavaScriptSerializer();
                Dictionary<string, object> config = j.Deserialize<Dictionary<string, object>>(input);

                if (config.ContainsKey("RootOU"))
                {
                    RootOU = (string)config["RootOU"];
                }

                if (config.ContainsKey("DebugPrint"))
                {
                    var debug = false;
                    Boolean.TryParse((string)config["DebugPrint"], out debug);
                    DebugPrint = debug;
                }

                if (config.ContainsKey("ProxyGroup"))
                {
                    ProxyGroup = (string)config["ProxyGroup"];
                }

                if (config.ContainsKey("LDAPURI"))
                {
                    LdapURI = (string)config["LDAPURI"];
                }

                if (config.ContainsKey("IdProperty"))
                {
                    IdProperty = (string)config["IdProperty"];
                }

                if (config.ContainsKey("EmailProperty"))
                {
                    EmailProperty = (string)config["EmailProperty"];
                }

                if (config.ContainsKey("ModifiedProperty"))
                {
                    ModifiedProperty = (string)config["ModifiedProperty"];
                }

                if (config.ContainsKey("Properties"))
                {
                    _propertytMappings.Clear();
                    var mappings = (System.Collections.ArrayList)config["Properties"];

                    foreach (Dictionary<string, object> pair in mappings)
                    {
                        var source = "";
                        var destination = "";
                        var name = "";

                        foreach (var item in pair)
                        {
                            if (item.Key.ToLower() == "source")
                            {
                                source = (string)item.Value;
                            }
                            else if (item.Key.ToLower() == "destination")
                            {
                                destination = (string)item.Value;
                            }
                            else if (item.Key.ToLower() == "name")
                            {
                                name = (string)item.Value;
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(source) && !string.IsNullOrWhiteSpace(destination))
                        {
                            var mapping = new PropertyMapping(source, destination, name);
                            _propertytMappings.Add(mapping.Source, mapping.RawValue);
                        }
                    }

                    result = String.Format("Imported {0} property mappings", PropertyMappings.Count);
                    Logger.WriteLog(Logger.Category.Info, Logger.DiagnosticAreaName, result);
                }

                if (config.ContainsKey("SyncTargets"))
                {
                    _syncTargets.Clear();
                    var records = (System.Collections.ArrayList)config["SyncTargets"];

                    foreach (Dictionary<string, object> pair in records)
                    {
                        var name = "";
                        var ou = "";

                        foreach (var item in pair)
                        {
                            if (item.Key.ToLower() == "name")
                            {
                                name = (string)item.Value;
                            }
                            else if (item.Key.ToLower() == "ou")
                            {
                                ou = (string)item.Value;
                            }

                        }

                        if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(ou))
                        {
                            _syncTargets.Add(name, ou);
                        }
                    }

                    result = String.Format("Imported {0} sync targets", SyncTargets.Count);
                    Logger.WriteLog(Logger.Category.Info, Logger.DiagnosticAreaName, result);
                }

                if (config.ContainsKey("SyncRecords"))
                {
                    _syncRecords.Clear();
                    var records = (System.Collections.ArrayList)config["SyncRecords"];

                    foreach (Dictionary<string, object> pair in records)
                    {
                        var ouName = "";
                        var lastSync = "";

                        foreach (var item in pair)
                        {
                            if (item.Key.ToLower() == "ou")
                            {
                                ouName = (string)item.Value;
                            }
                            else if (item.Key.ToLower() == "lastsync")
                            {
                                lastSync = (string)item.Value;
                            }

                        }

                        if (!string.IsNullOrWhiteSpace(ouName) && !string.IsNullOrWhiteSpace(lastSync))
                        {
                            _syncRecords.Add(ouName, lastSync);
                        }
                    }

                    result = String.Format("Imported {0} sync records", SyncRecords.Count);
                    Logger.WriteLog(Logger.Category.Info, Logger.DiagnosticAreaName, result);
                }

                Update();

                result = "Configuration import complete";
                Logger.WriteLog(Logger.Category.Info, Logger.DiagnosticAreaName, result);

                return result;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.Category.Error, Logger.DiagnosticAreaName, ex.ToString());
                throw;
            }

        }

        public IList<PropertyMapping> PropertyMappings
        {
            get
            {
                var list = new List<PropertyMapping>();

                foreach (var pair in _propertytMappings)
                {
                    list.Add(new PropertyMapping(pair.Key, pair.Value));
                }

                return list;
            }
        }

        public IList<SyncRecord> SyncRecords
        {
            get
            {
                var list = new List<SyncRecord>();

                foreach (var pair in _syncRecords)
                {
                    list.Add(new SyncRecord(pair.Key, pair.Value));
                }

                return list;
            }
        }

        public IList<SyncTarget> SyncTargets
        {
            get
            {
                var list = new List<SyncTarget>();

                foreach (var pair in _syncTargets)
                {
                    list.Add(new SyncTarget(pair.Key, pair.Value));
                }

                return list;
            }
        }
    }
}
