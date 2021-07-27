using System.Collections.Generic;

namespace OpeningHours.API.Models
{
    public class ErrorResponse
    {
        public bool Status { get; set; } = false;
        public string Message { get; set; }
        public IList<FieldValidationError> Errors { get; set; } = new List<FieldValidationError>();
        public string TraceId { get; set; }

    }

    public class FieldValidationError
    {
        public string FieldName { get; set; }
        public List<string> FieldErrors { get; set; } = new List<string>();

    }
}
