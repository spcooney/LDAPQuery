using Microsoft.Office.Server.UserProfiles;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using System;
using SPC.LDAP.ProfileSync.Configuration;

namespace SPC.LDAP.ProfileSync
{
    class ProfileManager
    {
        public const string UUID_PROPERTY = "SPCID";

        SPFarm _farm;
        SPServiceContext _context;
        UserProfileManager _manager;
        ProfilePropertyManager _propertyManager;

        SyncConfiguration _config = null;

        public SyncConfiguration Configuration
        {
            get
            {
                if (_config == null)
                {
                    _config = _farm.GetChild<SyncConfiguration>(SyncConfiguration.StorageName);

                    if (_config == null)
                    {
                        Logger.WriteInfo("Adding default sync configuration");

                        _config = new SyncConfiguration(SyncConfiguration.StorageName, _farm);
                        _config.AddDefaultPropertyMappings();
                        _config.AddDefaultOUs();
                        _config.Update();
                    }
                }

                return _config;
            }
        }

        public ProfileManager()
        {
            var farm = SPFarm.Local;

            if (farm == null)
            {
                throw new Exception("Provided SPFarm is null");
            }

            _farm = farm;
            var config = Configuration;

            if (config == null)
            {
                throw new Exception("Configuration is null");
            }
            
            var proxyGroup = farm.ServiceApplicationProxyGroups[config.ProxyGroup];

            if (proxyGroup == null)
            {
                var msg = "Failed to get Service Application Proxy Group with name '" + config.ProxyGroup + "'";
                throw new Exception(msg);
            }

            _context = SPServiceContext.GetContext(proxyGroup, SPSiteSubscriptionIdentifier.Default);

            if (_context == null)
            {
                var msg = "Failed to get Service Context";
                throw new Exception(msg);
            }

            _manager = new UserProfileManager(_context);

            if (_manager == null)
            {
                var msg = "Failed to get UserProfileManager";
                throw new Exception(msg);
            }

            _propertyManager = new UserProfileConfigManager(_context).ProfilePropertyManager;

            if (_propertyManager == null)
            {
                var msg = "Failed to get UserProfileConfigManager";
                throw new Exception(msg);
            }
        }

        public void CreateUIDProperty()
        {
            var propManager = _propertyManager.GetCoreProperties();

            var property = propManager.GetPropertyByName(UUID_PROPERTY);

            if (property != null)
            {
                return;
            }

            Logger.WriteInfo("Creating property '" + UUID_PROPERTY + "' in profile service");
            var profilePropMgr = new UserProfileConfigManager(_context).ProfilePropertyManager;
            var corePropManager = profilePropMgr.GetCoreProperties();

            // Create the property.
            var coreProp = corePropManager.Create(false);
            coreProp.Name = UUID_PROPERTY;
            coreProp.DisplayName = "SPC UID";
            coreProp.Type = PropertyDataType.String;
            coreProp.Length = 36;
            coreProp.IsMultivalued = false;
            coreProp.IsSearchable = true;
            corePropManager.Add(coreProp);

            // Create a profile type property and make the core property visible in the Details section page.
            var typePropManager = profilePropMgr.GetProfileTypeProperties(ProfileType.User);
            var typeProp = typePropManager.Create(coreProp);
            typeProp.IsVisibleOnViewer = true;
            typePropManager.Add(typeProp);

            // Create a profile subtype property.
            var subtypeManager = ProfileSubtypeManager.Get(_context);
            var subtype = subtypeManager.GetProfileSubtype(ProfileSubtypeManager.GetDefaultProfileName(ProfileType.User));
            var subtypePropMgr = profilePropMgr.GetProfileSubtypeProperties(subtype.Name);
            var subtypeProp = subtypePropMgr.Create(typeProp);
            subtypeProp.IsUserEditable = false;
            subtypeProp.DefaultPrivacy = Privacy.Public;
            subtypeProp.UserOverridePrivacy = true;
            subtypePropMgr.Add(subtypeProp);

            Logger.WriteInfo("Created SPC UID property in profile service");

        }

        public UserProfile GetByName(string name)
        {
            try
            {
                var profile = _manager.GetUserProfile(name);
                return profile;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public UserProfile GetById(string id)
        {
            var profiles = _manager.GetEnumerator();

            while (profiles.MoveNext())
            {
                var profile = (UserProfile)profiles.Current;

                if ((string)profile[UUID_PROPERTY].Value == id)
                {
                    return profile;
                }
            }

            return null;
        }


        public UserProfile CreateProfile(string email)
        {
            return _manager.CreateUserProfile(email);
        }
    }
}
