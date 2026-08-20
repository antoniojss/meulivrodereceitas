using System;
using System.Linq;
using FluentValidation;
using myRecipeBook.Communication.Requests;
using myRecipeBook.Communication.Enums;
using myRecipeBook.Exception;

namespace myRecipeBook.Application.UseCases.Recipe
{
    public class RecipeValidator : AbstractValidator<RequestRecipeJson>
    {
        public RecipeValidator()
        {
            RuleFor(r => r.Title)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.VALIDATION_RECIPE_TITLE_REQUIRED)
                .MaximumLength(500)
                .WithMessage(ResourceMessagesException.VALIDATION_NAME_EXCEED_QTY_CHARACTERS);

            RuleFor(r => r.CookTime)
                .IsInEnum()
                .WithMessage(ResourceMessagesException.VALIDATION_COOKTIME_INVALID);

            RuleForEach(r => r.DishTypes)
                .IsInEnum()
                .WithMessage(ResourceMessagesException.VALIDATION_DISHTYPE_INVALID);

            // DishTypes is required (at least one)
            RuleFor(r => r.DishTypes)
                .NotNull()
                .WithMessage(ResourceMessagesException.VALIDATION_RECIPE_DISHTYPES_REQUIRED)
                .Must(list => list != null && list.Any())
                .WithMessage(ResourceMessagesException.VALIDATION_RECIPE_DISHTYPES_REQUIRED);

            RuleFor(r => r.Ingredients)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.VALIDATION_AT_LAST_ONE_INGREDIENT);

            RuleForEach(r => r.Ingredients)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.VALIDATION_INGREDIENT_EMPTY)
                .MaximumLength(500)
                .WithMessage(ResourceMessagesException.VALIDATION_DESCRIPTION_EXCEED_QTYMAX_CHARACTERS);

            RuleFor(r => r.Instructions)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.VALIDATION_AT_LAST_ONE_INSTRUCTION);

            // Instructions must not contain duplicated Order values
            When(r => r.Instructions != null, () =>
            {
                RuleFor(r => r.Instructions)
                    .Must(list => list.Select(i => i.Order).Distinct().Count() == list.Count)
                    .WithMessage(ResourceMessagesException.VALIDATION_RECIPE_INSTRUCTION_ORDER_DUPLICATED)
                    .When(r => r.Instructions.Count > 1); 
            });

            RuleForEach(r => r.Instructions).ChildRules(instruction =>
            {
                instruction.RuleFor(item => item.Order)
                        .GreaterThan(0)
                        .WithMessage(ResourceMessagesException.VALIDATION_INSTRUCTION_ORDER_INVALID);

                instruction.RuleFor(item => item.Description)
               .Cascade(CascadeMode.Stop)
               .NotEmpty()
               .WithMessage(ResourceMessagesException.VALIDATION_INSTRUCTION_DESCRIPTION_REQUIRED)
               .MaximumLength(2000)
               .WithMessage(ResourceMessagesException.VALIDATION_DESCRIPTION_EXCEED_QTYMAX_CHARACTERS);
            });
        }
    }
}