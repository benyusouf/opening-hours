using System.Collections.Generic;

namespace OpeningHours.API.Models
{
    public class HourRanges
    {
        public IList<Entry> Monday { get; set; }
        public IList<Entry> Tuesday { get; set; }
        public IList<Entry> Wednesday { get; set; }
        public IList<Entry> Thursday { get; set; }
        public IList<Entry> Friday { get; set; }
        public IList<Entry> Sarturday { get; set; }
        public IList<Entry> Sunday { get; set; }
    }
}
