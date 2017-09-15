namespace SPC.LDAP.ProfileSync
{
    using System;
    using System.DirectoryServices.Protocols;
    using System.Text;

    /// <summary>
    /// Represents an LDAP user.
    /// </summary>
    public class LdapUserRecord
    {
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public string CommonName { get; set; }
        public string Initials { get; set; }
        public string Title { get; set; }
        public string UniqueID { get; set; }
        public string DeliveryOfficeName { get; set; }
        public string OrganizationalUnit { get; set; }
        public string PostalCode { get; set; }
        public string Location { get; set; }
        public string State { get; set; }
        public string Telephone { get; set; }
        public string DepartmentNumber { get; set; }
        public string ObjectClass { get; set; }
        private const string ToStrFormat = "{0}: {1};";
        private SearchResultEntry _searchResultEntry;

        public LdapUserRecord()
        {

        }

        public LdapUserRecord(SearchResultEntry entry)
        {
            _searchResultEntry = entry;
            if (entry.Attributes == null)
            {
                return;
            }
            if (entry.Attributes["givenname"] != null)
            {
                GivenName = entry.Attributes["givenname"][0].ToString();
            }
            if (entry.Attributes["sn"] != null)
            {
                Surname = entry.Attributes["sn"][0].ToString();
            }
            if (entry.Attributes["cn"] != null)
            {
                CommonName = entry.Attributes["cn"][0].ToString();
            }
            if (entry.Attributes["initials"] != null)
            {
                Initials = entry.Attributes["initials"][0].ToString();
            }
            if (entry.Attributes["title"] != null)
            {
                Title = entry.Attributes["title"][0].ToString();
            }
            if (entry.Attributes["uid"] != null)
            {
                UniqueID = entry.Attributes["uid"][0].ToString();
            }
            if (entry.Attributes["physicalDeliveryOfficeName"] != null)
            {
                DeliveryOfficeName = entry.Attributes["physicalDeliveryOfficeName"][0].ToString();
            }
            if (entry.Attributes["ou"] != null)
            {
                OrganizationalUnit = entry.Attributes["ou"][0].ToString();
            }
            if (entry.Attributes["postalcode"] != null)
            {
                PostalCode = entry.Attributes["postalcode"][0].ToString();
            }
            if (entry.Attributes["l"] != null)
            {
                Location = entry.Attributes["l"][0].ToString();
            }
            if (entry.Attributes["telephonenumber"] != null)
            {
                Telephone = entry.Attributes["telephonenumber"][0].ToString();
            }
            if (entry.Attributes["departmentnumber"] != null)
            {
                DepartmentNumber = entry.Attributes["departmentnumber"][0].ToString();
            }
            if (entry.Attributes["st"] != null)
            {
                State = entry.Attributes["st"][0].ToString();
            }
        }

        /// <summary>
        /// Checks if the search result entry is a user.
        /// </summary>
        /// <returns>True, if the entry is a user.  Otherwise, false.</returns>
        public bool IsValidUser()
        {
            SearchResultEntry entry = _searchResultEntry;
            if (entry == null)
            {
                return false;
            }
            if (entry.Attributes["objectclass"] == null)
            {
                return false;
            }
            for (int i = 0; i < entry.Attributes["objectclass"].Count; i++)
            {
                string val = entry.Attributes["objectclass"][i].ToString().ToLower();
                if (val.Equals("person") ||
                    val.Equals("pivuser") ||
                    val.Equals("organizationalperson") ||
                    val.Equals("entrustuser") ||
                    val.Equals("govt-organizationalperson"))
                {
                    return true;
                }
            }
            return false;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (!String.IsNullOrEmpty(GivenName))
            {
                sb.AppendFormat(ToStrFormat, "Given Name", GivenName);
            }
            if (!String.IsNullOrEmpty(Surname))
            {
                sb.AppendFormat(ToStrFormat, "Surname", Surname);
            }
            if (!String.IsNullOrEmpty(CommonName))
            {
                sb.AppendFormat(ToStrFormat, "Common Name", CommonName);
            }
            if (!String.IsNullOrEmpty(Initials))
            {
                sb.AppendFormat(ToStrFormat, "Initials", Initials);
            }
            if (!String.IsNullOrEmpty(Title))
            {
                sb.AppendFormat(ToStrFormat, "Title", Title);
            }
            if (!String.IsNullOrEmpty(UniqueID))
            {
                sb.AppendFormat(ToStrFormat, "Unique ID", UniqueID);
            }
            if (!String.IsNullOrEmpty(DeliveryOfficeName))
            {
                sb.AppendFormat(ToStrFormat, "Delivery Office Name", DeliveryOfficeName);
            }
            if (!String.IsNullOrEmpty(OrganizationalUnit))
            {
                sb.AppendFormat(ToStrFormat, "Organizational Unit", OrganizationalUnit);
            }
            if (!String.IsNullOrEmpty(PostalCode))
            {
                sb.AppendFormat(ToStrFormat, "Postal Code", PostalCode);
            }
            if (!String.IsNullOrEmpty(Location))
            {
                sb.AppendFormat(ToStrFormat, "Location", Location);
            }
            if (!String.IsNullOrEmpty(State))
            {
                sb.AppendFormat(ToStrFormat, "State", State);
            }
            if (!String.IsNullOrEmpty(Telephone))
            {
                sb.AppendFormat(ToStrFormat, "Telephone", Telephone);
            }
            if (!String.IsNullOrEmpty(DepartmentNumber))
            {
                sb.AppendFormat(ToStrFormat, "Department Number", DepartmentNumber);
            }
            return sb.ToString();
        }
    }
}