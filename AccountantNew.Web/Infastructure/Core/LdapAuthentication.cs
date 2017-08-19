using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Net;
using System.Web;

namespace AccountantNew.Web.Infastructure.Core
{
    public class LdapAuthentication
    {
        private string _path;
        private string _domain;
        public LdapAuthentication(string domain)
        {
            _path = "LDAP://" + domain;
            _domain = domain;
        }

        private LdapConnection _conn;
        private LdapConnection Conn
        {
            get
            {
                if (_conn == null)
                {
                    _conn = new LdapConnection(_domain);
                }
                return _conn;
            }
        }

        public bool ValidateUser(string user, string pass)
        {
            try
            {
                user = user.ToLower().Trim();
                user = user.Contains(_domain) ? user : user + "@" + _domain;
                string domainAndUsername = _domain + @"\" + user;

                NetworkCredential credential = new NetworkCredential(user, pass);
                Conn.Credential = credential;
                Conn.Bind();
                return true;
            }
            catch (LdapException e)
            {
                return false;
            }
        }

        //public bool IsUserInGroup(string groupName, string user)
        //{
        //    var groups = GetGroup(user);
        //    bool result = groups.Contains(groupName);
        //    return result;
        //}

        //public List<string> GetGroup(string user)
        //{
        //    PrincipalContext context = new PrincipalContext(ContextType.Domain, "cp.com.vn");
        //    UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(context, user);
        //    var groups = userPrincipal.GetGroups(context).ToList();
        //    var result = new List<string>();
        //    groups.ForEach(group => result.Add(group.SamAccountName));
        //    return result;
        //}
    }
}