using Bogus;
using myRecipeBook.Communication.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTestUtilities.Requests
{
    public class RequestUpdateUserJsonBuilder
    {
        public static RequestUpdateUserJson Build()
        {
            return new Faker<RequestUpdateUserJson>()
                 .RuleFor(request => request.Name, f => f.Person.FullName)
                 .RuleFor(request => request.Email, (f, user) => f.Internet.Email(user.Name));
        }

    }
}
