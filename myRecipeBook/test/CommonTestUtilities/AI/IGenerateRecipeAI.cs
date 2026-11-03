using Moq;
using myRecipeBook.Domain.AI;
using myRecipeBook.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTestUtilities.AI
{
    public class IGenerateRecipeAIBuilder
    {

        public static IGenerateRecipeAI Build()
        {
          var mock = new Mock<IGenerateRecipeAI>();

            mock.Setup(service  => service.GenerateRecipe(It.IsAny<string>()))
                .ReturnsAsync((string prompt) =>  
                {
                    return prompt.IsEmpty() ? null : GeneratedRecipeDtoBuilder.Build(); 
                }); 
            return mock.Object;     
        }
    }
}
