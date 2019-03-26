using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ImageResizeService.Infrastructure.Validators
{
    public class QualityValidator : ValidationAttribute, IModelValidator
    {
        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {
            var input = (int) context.Model;
            if (input >= 0 && input <= 100)
                return Enumerable.Empty<ModelValidationResult>();

            return GetError();
        }

        private static List<ModelValidationResult> GetError()
        {
            return new List<ModelValidationResult>
            {
                new ModelValidationResult(string.Empty,
                    "Jpeg quality can only be between 0 and 100, default is 100")
            };
        }
    }
}