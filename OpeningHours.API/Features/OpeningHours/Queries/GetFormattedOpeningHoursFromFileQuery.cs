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

{public class GetFormattedOpeningHoursFromFileQuery : IRequest<BaseResponse<string>>
    {
        /// <summary>
        /// IFormFile in json format
        /// </summary>
        public IFormFile File { get; set; }
    }

    public class GetFormattedOpeningHoursFromFileQueryValidator : AbstractValidator<GetFormattedOpeningHoursFromFileQuery>
    {
        public GetFormattedOpeningHoursFromFileQueryValidator()
        {
            RuleFor(x => x.File).SetValidator(new JsonFileValidator());
        }
    }

    public class GetFormattedOpeningHoursFromFileQueryHandler : IRequestHandler<GetFormattedOpeningHoursFromFileQuery, BaseResponse<string>>
    {
        private readonly ILogger<GetFormattedOpeningHoursFromFileQueryHandler> _logger;
        private readonly IOpeningHoursFormatter _openingHoursFormatter;

        public GetFormattedOpeningHoursFromFileQueryHandler(
            ILogger<GetFormattedOpeningHoursFromFileQueryHandler> logger,
            IOpeningHoursFormatter openingHoursFormatter
            )
        {
            _logger = logger;
            _openingHoursFormatter = openingHoursFormatter;
        }

        public async Task<BaseResponse<string>> Handle(GetFormattedOpeningHoursFromFileQuery request, CancellationToken cancellationToken)
        {
            string jsonContent;
            using (var stream = request.File.OpenReadStream())
            using (var reader = new StreamReader(stream))
            {
                jsonContent = await reader.ReadToEndAsync();
            }

            var openingHours = JsonConvert.DeserializeObject<Dictionary<string, IList<Entry>>>(jsonContent);

            var formattedOpeningHours = _openingHoursFormatter.GetOpeningHoursHumanReadableFormat(openingHours);

            _logger.LogInformation("Data formatted successfully");
            return new BaseResponse<string>(true, "Operation Successful", formattedOpeningHours);
        }
    }
}
