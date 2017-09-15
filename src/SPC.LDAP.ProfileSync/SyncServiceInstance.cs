using Microsoft.SharePoint.Administration;
using System.Collections.Generic;

namespace SPC.LDAP.ProfileSync
{
    public class SyncServiceInstance : SPServiceInstance
    {
        public static string ServiceInstanceName = "SPC Synchronization Service Instance";
        private SPActionLink _manageLink;
        private SPActionLink _provisionLink;
        private SPActionLink _unprovisionLink;

        public override SPActionLink ManageLink
        {
            get
            {
                if (_manageLink == null)
                {
                    _manageLink = new SPActionLink(SPActionLinkType.None);
                }
                return _manageLink;
            }
        }

        public override SPActionLink ProvisionLink
        {
            get
            {
                if (_provisionLink == null)
                {
                    _provisionLink = new SPActionLink(SPActionLinkType.ObjectModel);
                }
                return _provisionLink;
            }
        }

        public override SPActionLink UnprovisionLink
        {
            get
            {
                if (_unprovisionLink == null)
                {
                    _unprovisionLink = new SPActionLink(SPActionLinkType.ObjectModel);
                }
                return _unprovisionLink;
            }
        }

        public SyncServiceInstance() : base() { }

        public SyncServiceInstance(SPServer server, SyncService service)
            : base(ServiceInstanceName, server, service)
        {
        }
    }
}