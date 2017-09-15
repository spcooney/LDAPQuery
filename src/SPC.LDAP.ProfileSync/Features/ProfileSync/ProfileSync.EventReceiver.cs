using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using System;
using System.Runtime.InteropServices;
using SPC.LDAP.ProfileSync.Configuration;

namespace SPC.LDAP.ProfileSync.Features.ProfileSync
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>
    [Guid("1db7b636-8549-407d-a373-b0daf8103e04")]
    public class ProfileSyncEventReceiver : SPFeatureReceiver
    {
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            try
            {
                Logger.WriteInfo("Activating SPC Synchronization feature");
                var profileManager = new ProfileManager();
                var config = profileManager.Configuration;
                profileManager.CreateUIDProperty();
                var farm = SPFarm.Local;
                SyncService syncService = null;
                foreach (var service in farm.Services)
                {
                    if (String.Compare(service.Name, SyncService.ServiceName, true) == 0)
                    {
                        syncService = service as SyncService;
                        break;
                    }
                }
                if (syncService == null)
                {
                    Logger.WriteInfo(String.Format("Creating service '{0}' on SPFarm", SyncService.ServiceName));
                    syncService = new SyncService(farm);
                    syncService.Update();
                }
                foreach (var server in farm.Servers)
                {
                    SyncServiceInstance syncInstance = null;
                    foreach (var instance in server.ServiceInstances)
                    {
                        if (String.Compare(instance.Name, SyncServiceInstance.ServiceInstanceName, true) == 0)
                        {
                            syncInstance = (SyncServiceInstance)instance;
                            break;
                        }
                    }
                    if (syncInstance == null)
                    {
                        Logger.WriteInfo(String.Format("Adding service instance '{0}' to server '{1}'", SyncServiceInstance.ServiceInstanceName, server.Name));
                        syncInstance = new SyncServiceInstance(server, syncService);
                        syncInstance.Update();
                    }
                }
                var schedule = new SPDailySchedule();
                schedule.BeginHour = 1;
                schedule.EndHour = 4;
                ProfileSyncJob job = null;
                foreach (var definition in syncService.JobDefinitions)
                {
                    if (String.Compare(definition.Name, ProfileSyncJob.JobName, true) == 0)
                    {
                        job = (ProfileSyncJob)definition;
                    }
                }
                if (job == null)
                {
                    Logger.WriteInfo("Adding job definition");
                    job = new ProfileSyncJob(syncService, null, SPJobLockType.Job);
                    job.Schedule = schedule;
                    job.Update();
                    syncService.JobDefinitions.Add(job);
                }
                syncService.Update();
                Logger.WriteInfo("Feature activating complete");
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex.ToString());
                throw;
            }
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            Logger.WriteInfo("Feature deactivating");
            lock (this)
            {
                try
                {
                    var farm = SPFarm.Local;
                    SyncService syncService = null;
                    foreach (var service in farm.Services)
                    {
                        if (String.Compare(service.Name, SyncService.ServiceName, true) == 0)
                        {
                            syncService = service as SyncService;
                            break;
                        }
                    }
                    if (syncService != null)
                    {
                        foreach (var server in farm.Servers)
                        {
                            foreach (var instance in server.ServiceInstances)
                            {
                                if (String.Compare(instance.Name, SyncServiceInstance.ServiceInstanceName, true) == 0)
                                {
                                    Logger.WriteInfo(String.Format("Removing service instance '{0}' on server '{1}'", SyncServiceInstance.ServiceInstanceName, server.Name));
                                    instance.Delete();
                                }
                            }
                        }
                        foreach (var definition in syncService.JobDefinitions)
                        {
                            if (String.Compare(definition.Name, ProfileSyncJob.JobName, true) == 0)
                            {
                                Logger.WriteInfo("Removing job definition");
                                definition.Delete();
                            }
                        }
                        Logger.WriteInfo(String.Format("Removing service '{0}' from SPFarm", SyncService.ServiceName));
                        syncService.Delete();
                    }
                    Logger.WriteInfo("Feature deactivation complete");
                }
                catch (Exception ex)
                {
                    Logger.WriteError(ex.ToString());
                    throw;
                }
            }
        }
    }
}