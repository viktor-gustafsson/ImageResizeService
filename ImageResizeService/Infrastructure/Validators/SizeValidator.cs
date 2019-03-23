using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ImageResizeService.Infrastructure.Validators
{
    public class SizeValidator : Attribute, IModelValidator
    {
        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {
            var input = (int) context.Model;
            if (input < 0)
                return Enumerable.Empty<ModelValidationResult>();

            return new List<ModelValidationResult>
            {
                new ModelValidationResult(string.Empty,
                    "Input value can not be less than 0")
            };
        }
    }
}