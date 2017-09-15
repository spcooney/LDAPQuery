using System;
using System.DirectoryServices;
using System.Linq;
using SPC.LDAP.ProfileSync.Configuration;

namespace SPC.LDAP.ProfileSync
{
    class SyncManager
    {
        public const string DATE_FORMAT = "yyyyMMddHHmmss";
        private ProfileManager _profileManager;
        private SyncConfiguration _config;

        public SyncManager(ProfileManager profileManager)
        {
            _profileManager = profileManager;
            _config = profileManager.Configuration;
        }

        public void Sync()
        {
            if (_config.DebugPrint)
            {
                Logger.WriteInfo("Sync Manager starting");
            }
            if (_config.SyncTargets.Count == 0)
            {
                Logger.WriteError("No Sync Targets found in configuration, skipping sync");
            }
            foreach (var ou in _config.SyncTargets)
            {
                try
                {
                    SyncOU(ou);
                }
                catch (Exception ex)
                {
                    Logger.WriteError(String.Format("Failed to sync OU '{0}:{1}'. Error: {2}", ou.Name, ou.TargetOU, ex.ToString()));
                }
            }
            _config.Update();
        }

        private void SyncOU(SyncTarget ou)
        {
            var uri = BuildUrl(_config.LdapURI, ou.TargetOU);
            if (_config.DebugPrint)
            {
                Logger.WriteInfo(String.Format("Syncing OU '{0}:{1}' with URI: {2}", ou.Name, ou.TargetOU, uri));
            }
            using (var root = new DirectoryEntry(uri))
            {
                root.AuthenticationType = AuthenticationTypes.None;
                using (var searcher = new DirectorySearcher(root))
                {
                    var query = "(&(objectClass=user)(objectCategory=person)";
                    var syncRecord = _config.SyncRecords.FirstOrDefault(a => a.OUName == ou.Name);
                    if (syncRecord != null)
                    {
                        if (_config.DebugPrint)
                        {
                            Logger.WriteInfo("Adding last sync time to OU: " + ou + " Time: " + syncRecord.LastSync.ToString());
                        }
                        query += String.Format("({0}>={1}.0Z)", _config.ModifiedProperty, syncRecord.LastSync.ToString(DATE_FORMAT));
                    }
                    query += ")";
                    searcher.Filter = query;
                    searcher.PropertiesToLoad.Add(_config.IdProperty);
                    searcher.PropertiesToLoad.Add(_config.EmailProperty);
                    foreach (var mapping in _config.PropertyMappings)
                    {
                        searcher.PropertiesToLoad.Add(mapping.Source);
                    }
                    var results = searcher.FindAll();
                    if (_config.DebugPrint)
                    {
                        Logger.WriteInfo(String.Format("Found {0} entries for OU={1}", results.Count.ToString(), ou.Name));
                    }
                    foreach (SearchResult result in results)
                    {
                        var entry = result.GetDirectoryEntry();
                        var uid = GetProperty(entry, _config.IdProperty);
                        if (!string.IsNullOrWhiteSpace(uid))
                        {
                            var profile = _profileManager.GetById(uid.ToString());
                            var email = GetProperty(entry, _config.EmailProperty);
                            if (profile == null)
                            {
                                if (!string.IsNullOrWhiteSpace(email))
                                {
                                    profile = _profileManager.GetByName(email);
                                }
                            }
                            if (profile == null && !string.IsNullOrWhiteSpace(email))
                            {
                                if (_config.DebugPrint)
                                {
                                    Logger.WriteInfo(String.Format("Creating profile for {0}", email));
                                }
                                profile = _profileManager.CreateProfile(email);
                            }
                            if (profile != null)
                            {
                                profile[ProfileManager.UUID_PROPERTY].Value = uid;
                                foreach (var mapping in _config.PropertyMappings)
                                {
                                    var value = GetProperty(entry, mapping.Source);
                                    if (!string.IsNullOrWhiteSpace(value))
                                    {
                                        try
                                        {
                                            profile[mapping.Destination].Value = value;
                                        }
                                        catch (Exception ex)
                                        {
                                            Logger.WriteError(String.Format("Failed to set property '{0}'. Error: {1}", mapping.Destination, ex.Message));
                                        }
                                    }
                                }
                                profile.Commit();
                            }
                            else
                            {
                                Logger.WriteError(String.Format("Failed to find/create profile for ldap entry {0}", result.Path));
                            }
                        }
                        else
                        {
                            Logger.WriteError(String.Format("Failed to get UID for ldap entry {0}", result.Path));
                        }
                    }
                }
            }
            _config.AddSyncRecord(ou.Name, DateTime.UtcNow);
            if (_config.DebugPrint)
            {
                Logger.WriteInfo(String.Format("Sync completed for OU={0}", ou.Name));
            }
        }

        string BuildUrl(string server, string ouString)
        {
            var url = server.Trim();
            if (url.EndsWith("/"))
            {
                url = url.TrimEnd('/');
            }
            var start = url.IndexOf("//") + 2;
            var stop = url.LastIndexOf(":");
            if (stop < start)
            {
                stop = url.Length;
            }
            var dcString = url.Substring(start, stop - start);
            var dcs = dcString.Split('.');
            var ous = ouString.Split(',').Reverse().ToArray();
            for (var i = 0; i < ous.Length; i++)
            {
                url += i == 0 ? "/" : ", ";
                url += "OU=" + ous[i];
            }
            if (!string.IsNullOrWhiteSpace(_config.RootOU))
            {
                url += ", OU=" + _config.RootOU;
            }
            foreach (var dc in dcs)
            {
                url += ",DC=" + dc;
            }
            return url;
        }

        private static string GetProperty(DirectoryEntry item, string name)
        {
            try
            {
                var value = item.Properties[name];
                return value.Value.ToString();
            }
            catch
            {
            }
            return String.Empty;
        }
    }
}