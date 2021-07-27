using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OpeningHours.API.Behaviours;
using OpeningHours.API.Models;
using OpeningHours.API.Services;

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
            RuleFor(x => x.File).SetValidator(new JsonFileValidator());
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

        public async Task<BaseResponse<string>> Handle(GetFormattedOpeningHoursQuery request, CancellationToken cancellationToken)
        {
            string jsonContent;
            using (var stream = request.File.OpenReadStream())
            using (var reader = new StreamReader(stream))
            {
                jsonContent = await reader.ReadToEndAsync();
            }

            var openingHours = JsonConvert.DeserializeObject<Dictionary<string, IList<Entry>>>(jsonContent);

            var formattedOpeningHours = _openingHoursFormatter.GetOpeningHoursHumanReadableFormat(openingHours);

            return new BaseResponse<string>(true, "Operation Successful", formattedOpeningHours);
        }
    }
}
