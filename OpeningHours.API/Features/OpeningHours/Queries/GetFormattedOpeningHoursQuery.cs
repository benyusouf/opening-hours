using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OpeningHours.API.Models;

namespace OpeningHours.API.Features.OpeningHours.Queries
{

    public class GetFormattedOpeningHoursQuery : IRequest<BaseResponse<string>>
    {
        public IFormFile File { get; set; }
    }

    public class GetFormattedOpeningHoursQueryValidator : AbstractValidator<GetFormattedOpeningHoursQuery>
    {
        public GetFormattedOpeningHoursQueryValidator()
        {
            RuleFor(x => x.File).Must(x => x.GetType() == typeof(IFormFile)).WithMessage("Missing or invalid required parameter File");
        }
    }

    public class GetFormattedOpeningHoursQueryHandler : IRequestHandler<GetFormattedOpeningHoursQuery, BaseResponse<string>>
    {
        private readonly ILogger<GetFormattedOpeningHoursQueryHandler> _logger;

        public GetFormattedOpeningHoursQueryHandler(
            ILogger<GetFormattedOpeningHoursQueryHandler> logger
            )
        {
            _logger = logger;
        }

        public Task<BaseResponse<string>> Handle(GetFormattedOpeningHoursQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new BaseResponse<string>(true, "Operation Successful", "Abdullahi"));
        }
    }
}
