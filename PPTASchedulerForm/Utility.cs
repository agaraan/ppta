using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPTASchedulerForm
{
    public static class Utility
    {
        public static TimeSpan ConvertTo24TimeFormat(string time)
        {
            string[] s = time.Split(':');
            int h = int.Parse(s[0]);
            TimeSpan timeSpan = TimeSpan.Zero;
            StringBuilder sb;

            switch (h)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                    int hour = 12 + h;
                    sb = new StringBuilder(hour.ToString());
                    sb.AppendFormat(":{0}:{1}", s[1], s[2]);
                    timeSpan = TimeSpan.Parse(sb.ToString());
                break;
                default:
                    timeSpan = TimeSpan.Parse(time);
                break;
            }

            return timeSpan;

        }
    }
}
