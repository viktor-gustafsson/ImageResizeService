using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ImageResizeService.Infrastructure.Validators
{
    public class ImageFormatValidator : ValidationAttribute, IModelValidator
    {
        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {
            var input = (string) context.Model;
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
        }
    }
}