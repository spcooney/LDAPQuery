using System.ComponentModel;
using System.Management.Automation;

namespace SPC.LDAP.ProfileSync
{
    [RunInstaller(true)]
    public class CmdletInstaller : PSSnapIn
    {
        public CmdletInstaller()
            : base()
        { }

        public override string Name
        {
            get { return "SPCConfiguration"; }
        }

        public override string Vendor
        {
            get { return "SPC"; }
        }

        public override string Description
        {
            get { return "Registers cmdlets to administer the SPC Profile Sync configuration"; }
        }
    }
}
