using Microsoft.SharePoint.Administration;

namespace SPC.LDAP.ProfileSync
{
    public class SyncService : SPService
    {
        public static string ServiceName = "SPC Synchronization Service";

        public override string DisplayName
        {
            get
            {
                return ServiceName;
            }
        }

        public override string TypeName
        {
            get
            {
                return ServiceName;
            }
        }

        public override string ToString()
        {
            return ServiceName;
        }

        public SyncService()
            :base()
        {
        }

        public SyncService(SPFarm farm) 
            : base(ServiceName, farm)
        {
        }
    }
}
