using Bogus;
using Moq;
using myRecipeBook.Domain.Entities;
using myRecipeBook.Domain.Security.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTestUtilities.Security
{
    public class IAccessTokenGeneratorBuilder
    {
        public static IAccessTokenGenerator Build()
        {   
            var mock = new Mock<IAccessTokenGenerator>();   
         
            var fakeToken = new Faker().Random.String2(32, "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789");

            mock.Setup(generator => generator.Generate(It.IsAny<User>()))
                .Returns(fakeToken);    

            return mock.Object;
        }
    }
}
