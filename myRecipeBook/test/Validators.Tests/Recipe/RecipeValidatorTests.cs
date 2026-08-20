using CommonTestUtilities.Requests;
using myRecipeBook.Application.UseCases.Recipe;
using myRecipeBook.Communication.Enums;
using myRecipeBook.Communication.Requests;
using myRecipeBook.Exception;
using Shouldly;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Validators.Tests.Recipe
{
    public class RecipeValidatorTests
    {
        [Fact]
        public void Success()
        {
            var validator = new RecipeValidator();

            var request = RequestRecipeJsonBuilder.Build();

            var result = validator.Validate(request);

            result.IsValid.ShouldBeTrue();
        }


        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("             ")]
        [SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters", Justification = "use test nullable")]
        public void Validate_ShouldHaveError_WhenTitleIsEmpty(string title)
        {
            var validator = new RecipeValidator();

            var request = RequestRecipeJsonBuilder.Build();
            request.Title = title;

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_RECIPE_TITLE_REQUIRED));
        }

        [Fact]
        public void Validate_ShouldHaveError_WhenTitleExceedsMaxLength()
        {
            var validator = new RecipeValidator();

            var request = RequestRecipeJsonBuilder.Build();

            request.Title = new string('a', 501);

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_NAME_EXCEED_QTY_CHARACTERS));
        }

        [Fact]
        public void Validate_ShouldHaveError_WhenCookTimeIsInvalid()
        {
            var validator = new RecipeValidator();

            var request = RequestRecipeJsonBuilder.Build();

            request.CookTime = (CookTime)999;

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldSatisfyAllConditions(errors =>
            {
                errors.Count.ShouldBe(1);
                errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_COOKTIME_INVALID));
            });
        }

        [Fact]
        public void Validate_ShouldHaveError_WhenDishTypesContainsInvalidValue()
        {
            var validator = new RecipeValidator();
            
            var request = RequestRecipeJsonBuilder.Build();

            request.DishTypes = new List<DishType> { (DishType)999 };

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();

            result.Errors.ShouldSatisfyAllConditions(errors =>
            {
                errors.Count.ShouldBe(1);
                errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_DISHTYPE_INVALID));
            });
        }

        [Fact]
        public void Validate_ShouldHaveError_WhenIngredientsIsEmpty()
        {
            var validator = new RecipeValidator();

            var request = RequestRecipeJsonBuilder.Build();

            request.Ingredients = new List<string>();

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();

            result.Errors.ShouldSatisfyAllConditions(errors =>
            {
                errors.Count.ShouldBe(1);
                errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_AT_LAST_ONE_INGREDIENT));
            });
        }


        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("             ")]
        [SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters", Justification = "use test nullable")]
        public void Validate_ShouldHaveError_WhenIngredientItemIsEmpty(string ingredient)
        {
            var validator = new RecipeValidator();

            var request = RequestRecipeJsonBuilder.Build();

            request.Ingredients = [ ingredient ];

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();

            result.Errors.ShouldSatisfyAllConditions(errors =>
            {
                errors.Count.ShouldBe(1);
                errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_INGREDIENT_EMPTY));
            });
        }

        [Fact]
        public void Validate_ShouldHaveError_WhenIngredientItemTooLong()
        {
            var validator = new RecipeValidator();

            var request = RequestRecipeJsonBuilder.Build();

            request.Ingredients = new List<string> { new string('a', 501) };

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();

            result.Errors.ShouldSatisfyAllConditions(errors =>
            {
                errors.Count.ShouldBe(1);
                errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_DESCRIPTION_EXCEED_QTYMAX_CHARACTERS));
            });
        }

        [Fact]
        public void Validate_ShouldHaveError_WhenInstructionsIsEmpty()
        {
            var validator = new RecipeValidator();

            var request = RequestRecipeJsonBuilder.Build();

            request.Instructions = [];

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();

            result.Errors.ShouldSatisfyAllConditions(errors =>
            {
                errors.Count.ShouldBe(1);
                errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_AT_LAST_ONE_INSTRUCTION));
            });
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-5)]
        [SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters", Justification = "use test nullable")]
        public void Validate_ShouldHaveError_WhenInstructionOrderInvalid(int order)
        {
            var validator = new RecipeValidator();

            var request = RequestRecipeJsonBuilder.Build();

            request.Instructions = [new RequestRecipeInstructionJson 
            { Order = order, Description = "Valid Description" }];

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();

            result.Errors.ShouldSatisfyAllConditions(errors =>
            {
                errors.Count.ShouldBe(1);
                errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_INSTRUCTION_ORDER_INVALID));
            });
        }


        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("             ")]
        [SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters", Justification = "use test nullable")]
        public void Validate_ShouldHaveError_WhenInstructionDescriptionIsEmpty(string description)
        {
            var validator = new RecipeValidator();

            var request = RequestRecipeJsonBuilder.Build();

            request.Instructions = new List<RequestRecipeInstructionJson>
            {
                new RequestRecipeInstructionJson { Order = 1, Description = description}
            };

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldSatisfyAllConditions(errors =>
            {
                errors.Count.ShouldBe(1);
                errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_INSTRUCTION_DESCRIPTION_REQUIRED));
            });
        }

        [Fact]
        public void Validate_ShouldHaveError_WhenInstructionDescriptionTooLong()
        {
            var validator = new RecipeValidator();

            var request = RequestRecipeJsonBuilder.Build();

            request.Instructions = new List<RequestRecipeInstructionJson>
            {
                new RequestRecipeInstructionJson { Order = 1, Description = new string('a', 2001) }
            };

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldSatisfyAllConditions(errors =>
            {
                errors.Count.ShouldBe(1);
                errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_DESCRIPTION_EXCEED_QTYMAX_CHARACTERS));
            });
        }

        [Fact]
        public void Validate_ShouldHaveError_WhenDishTypesIsEmpty()
        {
            var validator = new RecipeValidator();

            var request = RequestRecipeJsonBuilder.Build();

            request.DishTypes = new List<DishType>();


            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldSatisfyAllConditions(errors =>
            {
                errors.Count.ShouldBe(1);
                errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_RECIPE_DISHTYPES_REQUIRED));
            });
        }

        [Fact]
        public void Validate_ShouldHaveError_WhenInstructionsHaveDuplicatedOrder()
        {
            var validator = new RecipeValidator();
            var request = RequestRecipeJsonBuilder.Build();
            request.Instructions = new List<RequestRecipeInstructionJson>
            {
                new RequestRecipeInstructionJson { Order = 1, Description = "One" },
                new RequestRecipeInstructionJson { Order = 1, Description = "Two" }
            };

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldSatisfyAllConditions(errors =>
            {
                errors.Count.ShouldBe(1);
                errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_RECIPE_INSTRUCTION_ORDER_DUPLICATED));
            });
        }
    }
}
