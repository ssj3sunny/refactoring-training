using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refactoring
{
    class Login
    {
        private List<User> user;
        private User currentUser;

        public Login(List<User> user)
        {
            this.user = user;
            this.currentUser = null;
        }

        public bool validateUsername(string usernameInput) {
            bool validUser = false;
            foreach(User user in this.user) {
                if(user.Name == usernameInput) {
                    validUser = true;
                    this.currentUser = user;
                }
            }
            return validUser;
        }

        public bool validatePassword(string passwordInput)
        {
            if (this.currentUser != null)
            {
                return this.currentUser.Pwd == passwordInput;
            }
            return false;
        }

        public User getValidUser()
        {
            return this.currentUser;
        }
    }
}
