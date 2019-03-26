using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ImageResizeService.Infrastructure.Validators
{
    public class SizeValidator : ValidationAttribute, IModelValidator
    {
        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {
            var input = (int) context.Model;
            return input > 0 ? Enumerable.Empty<ModelValidationResult>() : GetError();
        }

        private static IEnumerable<ModelValidationResult> GetError()
        {
            return new List<ModelValidationResult>
            {
                new ModelValidationResult(string.Empty,
                    "Input value can not be less than 0")
            };
        }
    }
}