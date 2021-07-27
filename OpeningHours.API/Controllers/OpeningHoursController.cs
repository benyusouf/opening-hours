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
        /// Verifies OTP sent to customer during password reset
        /// </summary>
        /// <param name="commad">VerifyResetPasswordOtpCommand containing customerId and Otp</param>
        /// <returns>Unique id of the customer that performs the verification</returns>
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> VerifyResetPasswordOtp(IFormFile file)
        {
            var query = new GetFormattedOpeningHoursQuery { File = file };
            if (!ModelState.IsValid)
                return BadRequest(ModelState.ValidationState);

            var result = await _mediatr.Send(query);
            return result.Status ? Ok(result) : BadRequest(result);
        }
    }
}
