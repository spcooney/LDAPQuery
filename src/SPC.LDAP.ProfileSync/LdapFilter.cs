using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPC.LDAP.ProfileSync
{
    public class LdapFilter
    {
        private string _filter;
        public const string AllObjects = "(objectClass=*)";
        public const string PersonAndUser = "(&(objectClass=person)(objectClass=user))";

        public LdapFilter(string filter)
        {

        }

        public string Filter
        {
            get { return _filter; }
            set { _filter = value; }
        }
    }
}