namespace SPC.LDAP.ProfileSync.WinForm
{
    using System;
    using System.Collections;
    using System.DirectoryServices;
    using System.DirectoryServices.ActiveDirectory;
    using System.DirectoryServices.Protocols;
    using System.Net;
    using System.Security.Cryptography.X509Certificates;
    using System.Security.Principal;
    using System.Windows.Forms;

    public partial class FrmSearchDomain : Form
    {
        private const string spcLdap = "ldap.spc.com";
        private const string FullspcLdap = "";
        private const string AllObjFilter = "(objectclass=*)";
        private const string AllEmailFilter = "(&(objectClass=user)(email=*))";
        private const string AllPeopleFilter = "(&(objectClass=person)(cn=*))";

        public FrmSearchDomain()
        {
            InitializeComponent();
        }

        private void FrmSearchDomain_Load(object sender, EventArgs e)
        {
            LoadBaseDNData();
        }

        private void BtnBaseDN_Click(object sender, EventArgs e)
        {
            LoadBaseDNData();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            using (LdapConnHelper ldapConn = new LdapConnHelper(DomainName, LdapConnHelper.DefaultLdapPortNum, true, false))
            {
                ldapConn.TryConnection(new TimeSpan(0, 1, 0), CredentialCache.DefaultNetworkCredentials, 3, AuthType.Basic);
                if (ldapConn.IsConnected)
                {
                    WriteTrace("Connected to: " + DomainName);
                }
                LdapFilter ldapFilter = new LdapFilter(TxtSearchFilter.Text.Trim());
                LdapSearchRequest searchReq = new LdapSearchRequest(DdBaseDN.Text, ldapFilter,
                    System.DirectoryServices.Protocols.SearchScope.Subtree, LdapSearchRequest.UserAttributes);
                SearchResponse searchResp = ldapConn.PerformSearch(searchReq);
                foreach (SearchResultEntry sre in searchResp.Entries)
                {
                    LdapUserRecord usrRecord = new LdapUserRecord(sre);
                    if (usrRecord.IsValidUser())
                    {
                        WriteTrace(usrRecord.ToString());
                    }
                }
            }
        }

        private void LoadBaseDNData()
        {
            using (LdapConnHelper ldapConn = new LdapConnHelper(DomainName, LdapConnHelper.DefaultLdapPortNum, true, false))
            {
                ldapConn.TryConnection(new TimeSpan(0, 1, 0), CredentialCache.DefaultNetworkCredentials, 3, AuthType.Basic);
                if (ldapConn.IsConnected)
                {
                    WriteTrace("Connected to: " + DomainName);
                }
                LdapFilter ldapFilter = new LdapFilter(LdapFilter.AllObjects);
                LdapSearchRequest searchReq = new LdapSearchRequest(RootOU, ldapFilter,
                    System.DirectoryServices.Protocols.SearchScope.OneLevel, null);
                SearchResponse searchResp = ldapConn.PerformSearch(searchReq);
                foreach (SearchResultEntry sre in searchResp.Entries)
                {
                    DdBaseDN.Items.Add(sre.DistinguishedName);
                }
            }
        }

        private void WriteTrace(string text)
        {
            richTextBox1.AppendText(DateTime.Now.ToString() + ": " + text + Environment.NewLine);
        }

        private string DomainName
        {
            get { return TxtDomainUrl.Text.Trim(); }
        }

        private string RootOU
        {
            get { return TxtRootOU.Text.Trim(); }
        }
    }
}
