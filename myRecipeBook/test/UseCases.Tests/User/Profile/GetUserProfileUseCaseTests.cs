using CommonTestUtilities.Entities;
using CommonTestUtilities.Identity;
using CommonTestUtilities.Storage;
using myRecipeBook.Application.UseCases.User.Profile;
using myRecipeBook.Domain.Identity;
using myRecipeBook.Domain.Storage;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using ZstdSharp.Unsafe;

namespace UseCases.Tests.User.Profile
{
    public class GetUserProfileUseCaseTests   
    {
     
        [Fact]
        public async Task Success()
        {

          (var user, var _) = UserBuilder.Build();

           var useCase = CreateUseCase(user);

           var result = await useCase.Execute();

           result.ShouldNotBeNull();  

           result.Name.ShouldBe(user.Name);  
            
           result.Email.ShouldBe(user.Email);  
        }
     private static GetUserProfileUseCase CreateUseCase(myRecipeBook.Domain.Entities.User user
         )
        {
            var loggedUser = ILoggedUserBuilder.Build(user);
            var storageService = IStorageServiceBuilder.Build();
            return new GetUserProfileUseCase(loggedUser, storageService);
        }

    }
}
