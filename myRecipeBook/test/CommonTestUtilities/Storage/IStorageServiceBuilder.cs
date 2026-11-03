using Moq;
using myRecipeBook.Domain.Entities;
using myRecipeBook.Domain.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTestUtilities.Storage
{
    public class IStorageServiceBuilder
    {
        public const string FakeUrl = "https://storage.example.com/default.jpg";        
        public static IStorageService Build()
        {
            var mock = new Mock<IStorageService>();

            mock.Setup(storage => storage.GetProfilePictureUrl(It.IsAny<User>()))
                .Returns(FakeUrl);

            mock.Setup(storage => storage.GetRecipeIllustrationUrl(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(FakeUrl);

            return mock.Object;
        }
    }
}
