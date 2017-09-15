using Microsoft.SharePoint.Administration;
using System;
using SPC.LDAP.ProfileSync.Configuration;

namespace SPC.LDAP.ProfileSync
{
    public class ProfileSyncJob : SPJobDefinition
    {
        public static string JobName = "SPC Synchronization Job";

        public ProfileSyncJob()
            : base()
        {
        }

        public ProfileSyncJob(SPService service, SPServer server, SPJobLockType lockType)
            : base(JobName, service, server, lockType)
        {
            this.Title = JobName;
        }

        public override void Execute(Guid targetInstanceId)
        {
            Logger.WriteInfo("SPC Sync starting");
            try
            {
                var profileManager = new ProfileManager();
                var syncManager = new SyncManager(profileManager);
                syncManager.Sync();
                Logger.WriteInfo("SPC Sync completed");
                base.Execute(targetInstanceId);
            }
            catch (Exception ex)
            {
                Logger.WriteError("Error in execute: ", ex.ToString());
            }
        }
    }
}