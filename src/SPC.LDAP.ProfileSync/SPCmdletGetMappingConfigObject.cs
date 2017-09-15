using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.PowerShell;
using System.Management.Automation;
using SPC.LDAP.ProfileSync.Configuration;

namespace SPC.LDAP.ProfileSync
{
    [Cmdlet("Get", "SPCConfiguration", DefaultParameterSetName = "DefaultSet")]
    public class SPCmdletGetMappingConfigObject : SPCmdlet
    {
        protected override void InternalProcessRecord()
        {
            var farm = SPFarm.Local;

            SyncConfiguration sc = farm.GetChild<SyncConfiguration>(SyncConfiguration.StorageName);

            if (sc == null)
            {
                sc = new SyncConfiguration(SyncConfiguration.StorageName, farm);
                sc.AddDefaultPropertyMappings();
                sc.AddDefaultOUs();
                sc.Update();
            }

            base.WriteObject(sc);
        }
    }
}
