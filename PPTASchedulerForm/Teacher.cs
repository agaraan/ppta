using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPTASchedulerForm
{
    public class Teacher : Person
    {
        private string teacherCode;
        private string judgeStartTime;
        private string judgeEndTime;
        private int judgingLevel;

        public Teacher(string firstName, string middleName, string lastName, int Id) : base(firstName, middleName, lastName, Id) { }

        public Teacher(string firstName, string lastName, int Id) : base(firstName, lastName, Id) { }

        public Teacher(string firstName, string lastName, int Id, string judgeStartTime, string judgeEndTime) : base(firstName, lastName, Id)
        {
            this.judgeStartTime = judgeStartTime;
            this.judgeEndTime = judgeEndTime;
        }

        public Teacher(int Id, string teacherCode) : base(null, null, Id)
        {
            this.teacherCode = teacherCode;
        }

        public Teacher(int Id) : base(null, null, Id)
        {

        }

        public string JudgeStartTime
        {
            get
            {
                return judgeStartTime;
            }
            set
            {
                judgeStartTime = value;
            }

        }

        public string JudgeEndTime
        {
            get
            {
                return judgeEndTime;
            }
            set
            {
                judgeEndTime = value;
            }
        }

        public int JudgingLevel
        {
            get
            {
                return judgingLevel;
            }
            set
            {
                judgingLevel = value;
            }
        }

        public TimeSpan JudgeAvailabilityStartTime
        {
            get
            {
                if (!string.IsNullOrEmpty(judgeStartTime))
                {
                    return (Utility.ConvertTo24TimeFormat(judgeStartTime));
                    //return (TimeSpan.Parse(judgeStartTime));
                }

                return TimeSpan.Zero;
            }
        }

        public TimeSpan JudgeAvailabilityEndTime
        {
            get
            {
                if (!string.IsNullOrEmpty(judgeEndTime))
                {
                    return (Utility.ConvertTo24TimeFormat(judgeEndTime));
                    //return (TimeSpan.Parse(judgeEndTime));
                }

                return TimeSpan.Zero;
            }
        }



        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Teacher ID: {0} Code: {1}  ID: {2}", ID, teacherCode);
          
            return sb.ToString();
        }

    }
}
