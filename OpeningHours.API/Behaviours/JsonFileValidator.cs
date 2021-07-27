using System.IO;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace OpeningHours.API.Behaviours
{
    public class JsonFileValidator : AbstractValidator<IFormFile>
    {
        public JsonFileValidator()
        {
            RuleFor(x => x.Length).NotNull().LessThanOrEqualTo(1024000)
                .WithMessage("File size must be 1mmb or less");

            RuleFor(x => x.FileName).NotNull().Must(x => Path.GetExtension(x).Equals(".json"))
                .WithMessage("Only JSON file is allowed");
        }
    }
}
