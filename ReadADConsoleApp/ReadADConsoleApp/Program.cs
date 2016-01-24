using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices.AccountManagement;

namespace ReadADConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            readUsersFromAD();
            //createUser("egava", "Ezio", "Gava", "abcde@@12345!~");
            //resetPassword("egava", "abcde@@123456!~");
        }

        private static void resetPassword(String userLogonName, String newPwd)
        {
            PrincipalContext ctx = new PrincipalContext(ContextType.ApplicationDirectory, "localhost:50000", "CN=Users,CN=LDAP,DC=LGMA2,DC=COM");
            UserPrincipal usr = UserPrincipal.FindByIdentity(ctx, userLogonName);
            if (usr != null)
            {
                try
                {
                    usr.SetPassword(newPwd);
                    usr.Save();
                }
                catch(Exception ex){
                    System.Console.WriteLine(ex.Message);
                }
                
            }
        }

        private static void createUser(String userLogonName, String firstname, String surname, String pwd)
        {
            PrincipalContext ctx = new PrincipalContext(ContextType.ApplicationDirectory, "localhost:50000", "CN=Users,CN=LDAP,DC=LGMA2,DC=COM");
            UserPrincipal usr = UserPrincipal.FindByIdentity(ctx, userLogonName);
            if (usr == null)
            {
                //Create new user
                UserPrincipal userPrincipal = new UserPrincipal(ctx);
                userPrincipal.Surname = surname;
                userPrincipal.GivenName = firstname;
                userPrincipal.SetPassword(pwd);
                userPrincipal.Enabled = true;
                userPrincipal.Name = userLogonName;

                try
                {
                    userPrincipal.Save();
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                }
            }
        }

        private static void readUsersFromAD()
        {
            PrincipalContext ctx = new PrincipalContext(ContextType.ApplicationDirectory, "localhost:50000", "CN=LDAP,DC=LGMA2,DC=COM");
            try
            {
                // set up a "QBE" user principal
                UserPrincipal user = new UserPrincipal(ctx);

                // create your principal searcher passing in the QBE principal    
                PrincipalSearcher srch = new PrincipalSearcher(user);

                // find all matches
                foreach (var found in srch.FindAll())
                {
                    Principal p = (Principal)found;
                    System.Console.WriteLine(p.DistinguishedName);

                    foreach (var group in p.GetGroups())
                    {
                        GroupPrincipal g = (GroupPrincipal)group;
                        System.Console.WriteLine(g.DistinguishedName);
                    }
                }

            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }
    }
}
