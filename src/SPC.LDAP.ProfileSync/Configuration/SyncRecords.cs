using System;
using System.Runtime.InteropServices;

namespace SPC.LDAP.ProfileSync.Configuration
{
    [Guid("e158742e-6369-4559-9763-cbf44553dce9")]
    public class SyncRecord 
    {
        public string OUName { get; set; }
        public string SyncTime { get; set; }

        public bool IsValid
        {
            get { return (String.IsNullOrWhiteSpace(OUName) == false && String.IsNullOrWhiteSpace(SyncTime) == false); }
        }

        public DateTime LastSync
        {
            get
            {
                var date = new DateTime(2000, 1, 1);

                if (!String.IsNullOrWhiteSpace(SyncTime))
                {
                    DateTime.TryParse(SyncTime, out date);
                }

                return date;
            }

            set
            {
                SyncTime = value.ToString();
            }
        }

        public SyncRecord(string ouName, string syncTime)
        {
            this.OUName = ouName;
            this.SyncTime = syncTime;
        }

    }
}
