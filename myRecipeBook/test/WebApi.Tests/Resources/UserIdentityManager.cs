using System;
using System.Collections.Generic;
using System.Text;

namespace WebApi.Tests.Resources
{
    public  class UserIdentityManager
    {
        private readonly myRecipeBook.Domain.Entities.User _user;
        private readonly string _password;
        private readonly string _accessToken;

        public UserIdentityManager(myRecipeBook.Domain.Entities.User user, string password, string accessToken)
        {
            _user = user;
            _password = password;
            _accessToken = accessToken;
        }

        public Guid GetId() => _user.Id;
        
        public string GetName() => _user.Name;  
        
        public string GetEmail() => _user.Email;
        
        public string GetPassword() => _password;

        public string GetAccessToken() => _accessToken;
    }

}
