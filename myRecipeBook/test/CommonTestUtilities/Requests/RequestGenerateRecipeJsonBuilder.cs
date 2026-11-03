using Bogus;
using myRecipeBook.Communication.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTestUtilities.Requests
{
    public  class RequestGenerateRecipeJsonBuilder
    {
        public static  RequestGenerateRecipeJson Build()
        {
            return new Faker<RequestGenerateRecipeJson>()
                 .RuleFor(request => request.Prompt, faker => faker.Lorem.Sentence());            
        }

    }
} 
