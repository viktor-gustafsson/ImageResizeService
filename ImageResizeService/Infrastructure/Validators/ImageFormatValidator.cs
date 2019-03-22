using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ImageResizeService.Infrastructure.Validators
{
    public class ImageFormatValidator : Attribute, IModelValidator
    {
        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {
            if (!(context.Model is string input))
                return GetError();

            var lower = input.ToLower();
            switch (lower)
            {
                case "jpeg":
                    return Enumerable.Empty<ModelValidationResult>();
                case "png":
                    return Enumerable.Empty<ModelValidationResult>();
                default:
                    return GetError();
            }
        }

        private IEnumerable<ModelValidationResult> GetError()
        {
            return new List<ModelValidationResult>
            {
                new ModelValidationResult(string.Empty,
                    "Invalid image format, supported formats are jpeg or png")
            };
            ;
        }
    }
}