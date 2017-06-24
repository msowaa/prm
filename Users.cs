using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation
{
    static class Users
    {
        public class User
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public User(string email, string password)
            {
                this.Email = email;
                this.Password = password;
            }
        }

        public static User PrimarkUser001 = new User("seleniumtester001@gmail.com", "qwerty");
        public static User PrimarkUser002 = new User("seleniumtester002@gmail.com", "qwerty");
        public static User SocialServicesUser001 = new User("seleniumtester001@gmail.com", "qwer!234");
        public static User SocialServicesUser002 = new User("seleniumtester002@gmail.com", "qwer!234");
        public static User PasswordChangeUser = new User("passwordchange@gmail.com", "asdfgh");
        public static User NewUser = new User("newuseremail@gmail.com", "qwerty");
        public static User SitecoreAdmin = new User("sitecore\\pnaliwajko", "pn.123");        

    }
}
