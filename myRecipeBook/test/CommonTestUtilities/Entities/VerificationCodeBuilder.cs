using Bogus;
using myRecipeBook.Domain.Entities;
using myRecipeBook.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTestUtilities.Entities
{
  public  class VerificationCodeBuilder
    {
        public static VerificationCode Build(User user)
        {
            return new Faker<VerificationCode>()
                .RuleFor(vc => vc.Code, f => f.Random.Int(1, 999_999).ToString("D6"))
                .RuleFor(vc => vc.Type, _ => VerificationCodeType.PasswordRecovery)
                .RuleFor(vc => vc.CreatedAt, _ => DateTime.UtcNow)
                .RuleFor(vc => vc.UserId, f => user.Id);
        }
    }
}
