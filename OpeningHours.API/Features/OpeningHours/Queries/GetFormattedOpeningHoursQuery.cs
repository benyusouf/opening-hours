using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OpeningHours.API.Models;
using OpeningHours.API.Services;

namespace OpeningHours.API.Features.OpeningHours.Queries
{

    public class GetFormattedOpeningHoursQuery : HourRanges, IRequest<BaseResponse<string>>
    {
        
    }

    public class GetFormattedOpeningHoursQueryValidator : AbstractValidator<GetFormattedOpeningHoursQuery>
    {
        public GetFormattedOpeningHoursQueryValidator()
        {
            RuleFor(x => x).NotNull();
        }
    }

    public class GetFormattedOpeningHoursQueryHandler : IRequestHandler<GetFormattedOpeningHoursQuery, BaseResponse<string>>
    {
        private readonly ILogger<GetFormattedOpeningHoursQueryHandler> _logger;
        private readonly IOpeningHoursFormatter _openingHoursFormatter;

        public GetFormattedOpeningHoursQueryHandler(
            ILogger<GetFormattedOpeningHoursQueryHandler> logger,
            IOpeningHoursFormatter openingHoursFormatter
            )
        {
            _logger = logger;
            _openingHoursFormatter = openingHoursFormatter;
        }

        public Task<BaseResponse<string>> Handle(GetFormattedOpeningHoursQuery request, CancellationToken cancellationToken)
        {
            var jsonData = JsonConvert.SerializeObject(request);

            var openingHours = JsonConvert.DeserializeObject<Dictionary<string, IList<Entry>>>(jsonData);

            var formattedOpeningHours = _openingHoursFormatter.GetOpeningHoursHumanReadableFormat(openingHours);

            _logger.LogInformation("Data formatted successfully");
            return Task.FromResult(new BaseResponse<string>(true, "Operation Successful", formattedOpeningHours));
        }
    }
}
