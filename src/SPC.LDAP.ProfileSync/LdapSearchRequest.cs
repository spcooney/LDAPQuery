namespace SPC.LDAP.ProfileSync
{
    using System.DirectoryServices.Protocols;

    public class LdapSearchRequest
    {
        private string _distName;
        private LdapFilter _filter;
        private SearchScope _scope;
        private SearchRequest _currentSearchRequest;
        public static readonly string[] UserAttributes = new string[15]
        {
            "givenname",
            "sn",
            "ou",
            "title",
            "cn",
            "st",
            "l",
            "uid",
            "telephonenumber",
            "postalcode",
            "initials",
            "mail",
            "departmentnumber",
            "physicalDeliveryOfficeName",
            "objectclass"
        };

        public LdapSearchRequest(string distinquishedName, LdapFilter filter, SearchScope scope) : this(distinquishedName, filter, scope, null)
        {
        }

        public LdapSearchRequest(string distinquishedName, LdapFilter filter, SearchScope scope, params string[] attributes)
        {
            _distName = distinquishedName;
            _filter = filter;
            _scope = scope;
            CurrentSearchRequest = new SearchRequest(_distName, _filter.Filter, _scope, attributes);
        }

        public string DistinguisedName
        {
            get { return _distName; }
            set { _distName = value; }
        }

        public LdapFilter Filter
        {
            get { return _filter; }
            set { _filter = value; }
        }

        public SearchScope Scope
        {
            get { return _scope; }
            set { _scope = value; }
        }

        public SearchRequest CurrentSearchRequest
        {
            get { return _currentSearchRequest; }
            set { _currentSearchRequest = value; }
        }

    }
}