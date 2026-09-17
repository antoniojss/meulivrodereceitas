using Bogus;
using myRecipeBook.Communication.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTestUtilities.Requests
{
    public class RequestPasswordRecoveryJsonBuilder
    {
        public static RequestPasswordRecoveryJson Build()
        {
            return new Faker<RequestPasswordRecoveryJson>()
                .RuleFor(request => request.Email, f => f.Internet.Email());
        }
    }
}
