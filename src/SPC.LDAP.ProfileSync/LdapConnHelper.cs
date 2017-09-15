namespace SPC.LDAP.ProfileSync
{
    using System;
    using System.DirectoryServices.Protocols;
    using System.Net;
    using SPC.LDAP.ProfileSync.Configuration;

    /// <summary>
    /// Author: Shawn P. Cooney
    /// Wrapper class around the LdapConnection class.  Makes it easier to query with LDAP.
    /// </summary>
    public class LdapConnHelper : IDisposable
    {
        private bool _isDisposed;
        private bool _isConnected = false;
        public const int DefaultLdapPortNum = 389;
        public const int DefaultProtocolVer = 3;
        private LdapConnection _ldapConn;
        private LdapDirectoryIdentifier _ldapDirectoryID;

        public LdapConnHelper(string server)
            : this(server, DefaultLdapPortNum, true, false)
        {
        }

        public LdapConnHelper(string server, int port)
            : this(server, port, true, false)
        {
        }

        public LdapConnHelper(string server, int port, bool fullyQualifiedHost)
            : this(server, port, fullyQualifiedHost, false)
        {
        }

        public LdapConnHelper(string server, int port, bool fullyQualifiedHost, bool isConnectionless)
        {
            if (LdapDirectoryID == null)
            {
                LdapDirectoryID = new LdapDirectoryIdentifier(server, port, fullyQualifiedHost, isConnectionless);
            }
            Connection = new LdapConnection(LdapDirectoryID);
        }

        public bool TryConnection(TimeSpan timeout, NetworkCredential credentials, int protocolVer, AuthType authenticationType)
        {
            if (Connection == null)
            {
                throw new InvalidOperationException("LDAP connection cannot be null");
            }
            Connection.Timeout = timeout;
            Connection.Credential = credentials;
            Connection.SessionOptions.ProtocolVersion = protocolVer;
            Connection.AuthType = authenticationType;
            try
            {
                Connection.Bind();
                IsConnected = true;
            }
            catch (Exception ex)
            {
                IsConnected = false;
                Logger.WriteError(ex.Message);
            }
            return IsConnected;
        }

        public SearchResponse PerformSearch(LdapSearchRequest request)
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("You must connect before you can perform a search");
            }
            SearchResponse sr = null;
            try
            {
                SearchRequest searchRequest = request.CurrentSearchRequest;
                sr = (SearchResponse)Connection.SendRequest(request.CurrentSearchRequest);
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex.Message);
            }
            return sr;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    if (_ldapConn != null)
                    {
                        _ldapConn.Dispose();
                        _ldapConn = null;
                        _isConnected = false;
                    }
                }
            }
            IsDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool IsConnected
        {
            get { return _isConnected; }
            set { _isConnected = value; }
        }

        public LdapConnection Connection
        {
            get { return _ldapConn; }
            set { _ldapConn = value; }
        }

        public LdapDirectoryIdentifier LdapDirectoryID
        {
            get { return _ldapDirectoryID; }
            set { _ldapDirectoryID = value; }
        }

        public bool IsDisposed
        {
            get { return _isDisposed; }
            set { _isDisposed = value; }
        }
    }
}