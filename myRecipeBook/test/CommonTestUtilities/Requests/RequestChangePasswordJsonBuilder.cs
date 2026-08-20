using Bogus;
using myRecipeBook.Communication.Requests;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommonTestUtilities.Requests
{

    public class RequestChangePasswordJsonBuilder
    {
        public static RequestChangePasswordJson Build(int newPasswordLength=10)
        {
            return new Faker<RequestChangePasswordJson>()
                 .RuleFor(request => request.NewPassword, f => f.Internet.Password(length:newPasswordLength))
                 .RuleFor(request => request.CurrentPassword, f => f.Internet.Password());  
        }
    }
}
