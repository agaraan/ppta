using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PPTASchedulerForm
{
    public class Student : Person
    {
        private int theoryLevel;
        private int performanceLevel;
        private string performanceCode;
        private string studentPreferredStartTime;
        private string studentPreferredEndTime;
        private TimeSpan scheduledStartTime;
        private TimeSpan scheduledEndTime;
        private TimeSpan convertedStartTime;
        private TimeSpan convertedEndTime;
        private Teacher teacher;

        System.Text.RegularExpressions.Regex rg = new Regex(@"[a-zA-Z]");


        public Student(string firstName, string middleName, string lastName, int Id) : base(firstName, middleName, lastName, Id) { }

        public Student(string firstName, string lastName, int Id) : base(firstName, lastName, Id) { }

        public Student(string firstName, string lastName, int Id, string preferredStartTime, string preferredEndTime) : base(firstName, lastName, Id)
        {
            studentPreferredStartTime = preferredStartTime;
            studentPreferredEndTime = preferredEndTime;
            convertedStartTime = Utility.ConvertTo24TimeFormat(studentPreferredStartTime);
            convertedEndTime = Utility.ConvertTo24TimeFormat(studentPreferredEndTime);
            this.ScheduledStartTime = convertedStartTime;
        }

        public int TheoryLevel
        {
            get
            {
                return theoryLevel;
            }
            set
            {
                theoryLevel = value;
            }
        }

        public Teacher Teacher
        {
            get
            {
                return teacher;
            }
            set
            {
                this.teacher = value;
            }
        }

        public int PerformanceLevel
        {
            get
            {
                performanceLevel = 0;
              
                if (!string.IsNullOrEmpty(performanceCode))
                {
                    if (rg.IsMatch(performanceCode))
                    {
                        performanceLevel = (int)char.GetNumericValue(performanceCode[0]);
                    }
                    else
                    {
                        performanceLevel = int.Parse(performanceCode);
                    }
                }

                return performanceLevel;
            }
        }

        public string PerformanceCode
        {
            get
            {
                return performanceCode;
            }
            set
            {
                performanceCode = value;
            }
        }

        public string PreferredStartTime
        {
            get
            {
                return studentPreferredStartTime;
            }

        }

        public string PreferredEndTime
        {
            get
            {
                return studentPreferredEndTime;
            }
        }


        public TimeSpan StudentPreferredStartTime
        {
            get
            {
                return convertedStartTime;
            }
        }

        public TimeSpan ScheduledStartTime
        {
            get
            {
                return scheduledStartTime;
            }
            set
            {
                scheduledStartTime = value;
            }

        }

        public TimeSpan ScheduledEndTime
        {
            get
            {
                return scheduledEndTime;
            }
            set
            {
                scheduledEndTime = value;
            }

        }

        public TimeSpan StudentPreferredEndTime
        {
            get
            {
                return convertedEndTime;
            }
        }

        public int HighestPerformanceLevel
        {
            get
            {
                int highestLevel;

                if (!string.IsNullOrEmpty(PerformanceCode))
                {
                    if (rg.IsMatch(PerformanceCode))
                    {
                        highestLevel = performanceLevel;
                    }
                    else
                    {
                        highestLevel = (this.PerformanceLevel > this.TheoryLevel) ? PerformanceLevel : TheoryLevel;
                    }
                }
                else 
                {
                    highestLevel = TheoryLevel;

                }

                return highestLevel;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("First Name: {0} Last Name: {1}  ID: {2}", FirstName, LastName, ID);
            sb.AppendFormat("Preferred Start Time: {1}", StudentPreferredStartTime);

            return sb.ToString();
        }
    }
}
