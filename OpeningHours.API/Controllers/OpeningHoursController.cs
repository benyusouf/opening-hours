using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpeningHours.API.Features.OpeningHours.Queries;
using OpeningHours.API.Models;

namespace OpeningHours.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OpeningHoursController : ControllerBase {

        private readonly ISender _mediatr;
        public OpeningHoursController(ISender sender)
        {
            _mediatr = sender;
        }

        /// <summary>
        /// Gets formatted restaurant opening weekly hours
        /// </summary>
        /// <param name="file">An IFormFile file in json format containg the data</param>
        /// <returns>Line separated string containg the formatted data</returns>
        [HttpPost("from-file")]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> VerifyResetPasswordOtp(IFormFile file)
        {
            var query = new GetFormattedOpeningHoursFromFileQuery { File = file };
            if (!ModelState.IsValid)
                return BadRequest(ModelState.ValidationState);

            var result = await _mediatr.Send(query);
            return result.Status ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Gets formatted restaurant opening weekly hours
        /// </summary>
        /// <param name="request">JSON formatted body payload</param>
        /// <returns>Line separated string containg the formatted data</returns>
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> VerifyResetPasswordOtp([FromBody] GetFormattedOpeningHoursQuery request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.ValidationState);

            var result = await _mediatr.Send(request);
            return result.Status ? Ok(result) : BadRequest(result);
        }
    }
}
