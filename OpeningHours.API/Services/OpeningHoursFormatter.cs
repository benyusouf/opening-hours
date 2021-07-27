using OpeningHours.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpeningHours.API.Services
{
    public class OpeningHoursFormatter: IOpeningHoursFormatter
    {
        public string GetOpeningHoursHumanReadableFormat(IDictionary<string, IList<Entry>> weeklyOpeningTime)
        {
            if (weeklyOpeningTime is null || !weeklyOpeningTime.Any()) return string.Empty;

            var formattedOpeningHours = string.Empty;

            for(int i = 0; i < weeklyOpeningTime.Count; i++)
            {
                var day = weeklyOpeningTime.ElementAt(i);

                if (i != weeklyOpeningTime.Count -1)
                {
                    var firstEntryNextDay = weeklyOpeningTime.ElementAt(i + 1).Value.OrderBy(x => x.Value).FirstOrDefault();
                    if(firstEntryNextDay.Type.Equals("close", StringComparison.CurrentCultureIgnoreCase))
                    {
                        day.Value.Add(firstEntryNextDay);
                        weeklyOpeningTime.ElementAt(i+1).Value.RemoveAt(0);
                    }
                    
                }
                    

                formattedOpeningHours += $"{day.Key}: {GetFormattedTimes(day.Value)}";

                if (i != weeklyOpeningTime.Count - 1)
                    formattedOpeningHours += Environment.NewLine;
            }

            return formattedOpeningHours;
        }

        private string GetFormattedTimes(IList<Entry> entries)
        {
            var openingTimes = string.Empty;

            for(int i = 0; i < entries.Count; i++)
            {
                var entry = entries[i];

                var time = DateTimeOffset.FromUnixTimeSeconds(entry.Value).ToString("h:mm tt");

                if (entry.Type.Equals("open", StringComparison.InvariantCultureIgnoreCase) && entries.Count > 1)
                    openingTimes += $"{time} - ";
                if (entry.Type.Equals("open", StringComparison.InvariantCultureIgnoreCase) && entries.Count == 1)
                    openingTimes += time;
                else if (entry.Type.Equals("close", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (i != entries.Count - 1)
                        openingTimes += $"{time}, ";
                    else
                        openingTimes += $"{time}";
                }
                else continue;
                    
            }

            return openingTimes;
        }
    }

    public interface IOpeningHoursFormatter
    {
        string GetOpeningHoursHumanReadableFormat(IDictionary<string, IList<Entry>> weeklyOpeningHours);
    }
}
