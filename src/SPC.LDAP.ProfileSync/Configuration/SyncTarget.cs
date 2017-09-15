using System;
using System.Runtime.InteropServices;

namespace SPC.LDAP.ProfileSync.Configuration
{
    [Guid("864bd1e8-9eb6-45d8-9cc2-4ada6e9211fe")]
    public class SyncTarget
    {
        public string Name { get; set; }
        public string TargetOU { get; set; }

        public SyncTarget(string targetOU)
        {
            this.Name = "";
            this.TargetOU = targetOU;
        }

        public SyncTarget(string name, string targetOU)
        {
            this.Name = name;
            this.TargetOU = targetOU;
        }
    }
}
