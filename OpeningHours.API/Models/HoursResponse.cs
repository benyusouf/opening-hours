using System.Collections.Generic;

namespace OpeningHours.API.Models
{
    public class HoursResponse
    {
        public string Day { get; set; }
        public IList<string> OpeningHours { get; set; }
    }
}
