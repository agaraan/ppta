using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using GenericParsing;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;

namespace PPTASchedulerForm
{
    public partial class SchedulerForm : Form
    {
        DataTable table = new DataTable();
        System.Text.RegularExpressions.Regex rg = new Regex(@"[a-zA-Z]");

        const string STUDENT_PERFORMANCE_QUERY = @"select studentperformance.student_id as student_id,theory, student.first_name as first_name, student.last_name as last_name, performance, preferred_start, preferred_end,teacher.teacher_id as teacher_id, code 
                                                from studentperformance, teacher, student
                                                where(student.student_id = studentperformance.student_id) and (student.teacher_id = teacher.teacher_id) and (studentperformance.theory is not null or studentperformance.performance is not null)"; // and (student.last_name='QUIRK' || student.last_name='AGARA' || student.last_name='DIYANNI' || student.last_name='ASTI')";

        public SchedulerForm()
        {
            InitializeComponent();
        }

        private bool isBreakTime(Teacher judge, Student student, TimeSpan startTime, TimeSpan endTime)
        {
            bool retValue = true;

          
            StringBuilder sb = new StringBuilder("SELECT judge_id, break_time_start, break_time_end from judge_break ");
            sb.AppendFormat("WHERE judge_id={0}", judge.ID);

            DataTable dtJudgeAvailability = DBMySQLUtils.ExecuteStatement(sb.ToString());

            //for judges who do not have lunch break;
            if (dtJudgeAvailability.Rows.Count == 0)
            {
                retValue = false;
            }
            else
            {

                foreach (DataRow row in dtJudgeAvailability.Rows)
                {

                    TimeSpan judge_break_start = TimeSpan.Parse(row["break_time_start"].ToString());
                    TimeSpan judge_break_end = TimeSpan.Parse(row["break_time_end"].ToString());

                    bool overlap = startTime < judge_break_end && judge_break_start < endTime;

                    if(overlap) { 
                        retValue = true;
                        break;
                    }
                    else
                    {
                        retValue = false;
                    }

                }
            }
            
            return retValue;
        }

        private bool isAvailable(Student student, Teacher currentJudge, TimeSpan startTime, TimeSpan endTime)
        {
            bool retValue = true;

            int student_id = student.ID;
            int teacher_id = currentJudge.ID;
           

            StringBuilder sb = new StringBuilder("SELECT teacher_id, start_time, end_time from performance_schedule ");
            sb.AppendFormat("WHERE (teacher_id={0})", teacher_id);

            DataTable dtAvailable = DBMySQLUtils.ExecuteStatement(sb.ToString());

            foreach (DataRow row in dtAvailable.Rows)
            {

                TimeSpan judging_start_time = TimeSpan.Parse(row["start_time"].ToString());
                TimeSpan judging_end_time = TimeSpan.Parse(row["end_time"].ToString());

                bool overlap = startTime < judging_end_time && judging_start_time < endTime;

                if (overlap)
                {
                    retValue = false;
                    break;
                }
                else
                {
                    retValue = true;
                }

            }

            return retValue;
        }

        private DataTable getDistinctLastNames()
        {
            string sql = @"select distinct student.last_name
                           from student, studentperformance
                           where student.student_id = studentperformance.student_id";

            DataTable dtLastName = DBMySQLUtils.ExecuteStatement(sql);

            return dtLastName;
        }

        private void ExctractStudentPerformanceLevel(string theory, string performance)
        {
            
            int perf_level = 0;
            int theory_level = 0;

            if (!string.IsNullOrEmpty(performance))
            {
                if(rg.IsMatch(performance))
                {
                    perf_level = Convert.ToInt32(performance[0]);
                } 
                else
                {
                    perf_level = int.Parse(performance);
                }
            }

            if (!string.IsNullOrEmpty(theory))
            {
                theory_level = int.Parse(theory);
            }
        }

        private List<Student> GetPerformanceStudentList()
        {
            //Create Student and Teacher objects
            List<Student> performanceStudents = new List<Student>();

            DataTable dtStudentperformance = DBMySQLUtils.ExecuteStatement(STUDENT_PERFORMANCE_QUERY);

            foreach (DataRow row in dtStudentperformance.Rows)
            {
                string teacherID = row["teacher_id"].ToString().Trim().ToUpper();
                string studentID = row["student_id"].ToString();
                string startTime = row["preferred_start"].ToString();
                string endTime = row["preferred_end"].ToString();
                string theory = row["theory"].ToString().Trim();
                string performance = row["performance"].ToString().Trim();
                string teacherCode = row["code"].ToString().Trim();
                string studentLastName = row["last_name"].ToString().Trim();
                string studentFirstName = row["first_name"].ToString().Trim();

                Student student = new Student(studentFirstName, studentLastName, int.Parse(studentID), startTime, endTime);

                if (!string.IsNullOrEmpty(theory))
                {
                    if (!rg.IsMatch(theory))
                    {
                        student.TheoryLevel = int.Parse(theory);
                    }
                }

                if (!string.IsNullOrEmpty(performance))
                    student.PerformanceCode = performance;

                int perfLevel = student.PerformanceLevel;

                Teacher teacher = new Teacher(int.Parse(teacherID), teacherCode);
                student.Teacher = teacher;

                performanceStudents.Add(student);
            }

            return performanceStudents;
        }

        private List<Teacher> GetJudgeList()
        {
            List<Teacher> judges = new List<Teacher>();
            string judge_availability_query = "select judge_avail_id, teacher_id, judging_level, availability_start, availability_end from judge_availability";

            DataTable dtJudgeAvailability = DBMySQLUtils.ExecuteStatement(judge_availability_query);


            foreach (DataRow row in dtJudgeAvailability.Rows)
            {
                int teacherID = int.Parse(row["teacher_id"].ToString()); 
                int judgingLevel = int.Parse(row["judging_level"].ToString());
                string judgeStartTime = row["availability_start"].ToString();
                string judgeEndTime =   row["availability_end"].ToString();

                Teacher judge = new Teacher(teacherID);
                judge.JudgingLevel = judgingLevel;
                judge.JudgeStartTime = judgeStartTime;
                judge.JudgeEndTime = judgeEndTime;
                judges.Add(judge);
            }

            return judges;
        }

        private TimeSpan computeEndTime(Student student)
        {
            TimeSpan endTime = TimeSpan.Zero;

            int performanceLevel = student.HighestPerformanceLevel;

           switch (performanceLevel)
            {
                case 1:
                case 2:
                    endTime = student.ScheduledStartTime.Add(new TimeSpan(0, 15, 0));
                break;
                case 3:
                case 4:
                case 5:
                case 6:
                    if (student.TheoryLevel > 0 && student.PerformanceLevel > 0)
                        endTime = student.ScheduledStartTime.Add(new TimeSpan(0, 30, 0));
                    else
                        endTime = student.ScheduledStartTime.Add(new TimeSpan(0, 15, 0));
                    break;
                case 7:
                case 8:
                case 9:
                case 10:
                    if (student.TheoryLevel > 0 && student.PerformanceLevel > 0)
                        endTime = student.ScheduledStartTime.Add(new TimeSpan(0, 60, 0));
                    else
                        endTime = student.ScheduledStartTime.Add(new TimeSpan(0, 30, 0));
                    break;
            }

            return endTime;
        }

        private bool AssignJudge(Student student, IEnumerator<Teacher> judgeEnumerator, List<Student> siblings)
        {
            bool isScheduled = false;

            while (judgeEnumerator.MoveNext())
            {
                Teacher currentJudge = judgeEnumerator.Current;

                student.ScheduledStartTime = student.StudentPreferredStartTime;

                while (!isScheduled)
                {
                    
                    //Check whether the judge is available to judge based on students preferred start time
                    if (TimeSpan.Compare(currentJudge.JudgeAvailabilityStartTime, student.ScheduledStartTime) > 0)
                    {
                        break;
                    }

                    //Check whether the judge is able to judge the current student
                    if (student.HighestPerformanceLevel > currentJudge.JudgingLevel)
                    {
                        break;
                    }

                    TimeSpan endTime = computeEndTime(student);
                    TimeSpan startTime = student.ScheduledStartTime;

                    bool available = isAvailable(student, currentJudge, startTime, endTime);
                    bool breakTime = true;

                    if (available) {
                        breakTime = isBreakTime(currentJudge, student, startTime, endTime);
                    }

                    if (available && !breakTime)
                    {
                        isScheduled = true;

                        StringBuilder sb = new StringBuilder("INSERT INTO performance_schedule(teacher_id,student_id,start_time,end_time) values(");
                        sb.AppendFormat("{0}, {1}, '{2}', '{3}')", currentJudge.ID, student.ID, startTime, endTime);
                        string sql = sb.ToString();

                        student.ScheduledStartTime = startTime;
                        student.ScheduledEndTime = endTime;
                        int rowsAffected = DBMySQLUtils.ExecuteQuery(sql);

                        break;
                    }
                    else
                    {
                        TimeSpan originalTime = student.ScheduledStartTime;
                        startTime = originalTime.Add(new TimeSpan(0, 15, 0));

                        student.ScheduledStartTime = startTime;
                    }
                }

                if(isScheduled) { break;  }

            }

            return isScheduled;
        }

        private List<Student> GetSiblingsList(IEnumerable<Student> students)
        {
            IEnumerator<Student> studentsEnumerator = students.GetEnumerator();
            List<Student> siblings = new List<Student>();

            while(studentsEnumerator.MoveNext())
            {
                Student student = studentsEnumerator.Current;
                siblings.Add(student);
            }

            return siblings;
        }

        private void btnSchedule_Click(object sender, EventArgs e)
        {
   
             List<Student> performanceStudents = this.GetPerformanceStudentList();

             List<Teacher> judges = this.GetJudgeList();

             IEnumerable<String> lastNames = performanceStudents.Select(student => student.LastName).Distinct();

             int lastNameCount = lastNames.OfType<string>().Count();

             IEnumerator<string> enumerator = lastNames.GetEnumerator();

             while (enumerator.MoveNext())
             {
                 string last_name = enumerator.Current;
                
                IEnumerable<Student> currentStudents = performanceStudents.Where(student => student.LastName.ToUpper().Equals(last_name.ToUpper()));
                IEnumerator<Student> studentsEnumerator = currentStudents.GetEnumerator();
                int lastNameCount1 = currentStudents.OfType<Student>().Count();

                //  List<Student> siblings = GetSiblingsList(currentStudents);
                while (studentsEnumerator.MoveNext())
                 {

                     Student currentStudent = studentsEnumerator.Current;

                    //Filter the judge based on the current student
                    IEnumerable<Teacher> judgeList = judges.Where(j => j.ID != currentStudent.Teacher.ID);                    
                    IEnumerator<Teacher> judgeEnumerator = judgeList.GetEnumerator();

                    this.AssignJudge(currentStudent, judgeEnumerator, null);

                 }
             } 

        }

        private void SchedulerForm_Load(object sender, EventArgs e)
        {
            table = FileParser.Parse(txtDataFile.Text);
            DBMySQLUtils.Init(ConfigurationManager.AppSettings["ConnectionString"]);
        }

        private void btnCreateTeacher_Click(object sender, EventArgs e)
        {
            string teacher;
            List<string> teachers = new List<string>();

            foreach (DataRow dr in table.Rows)
            {
                teacher = dr["Teacher"].ToString();

                if (!teachers.Contains(teacher))
                {
                    teachers.Add(teacher);
                }
            }

            StringBuilder set = new StringBuilder("");

            foreach (string s in teachers)
            {
                set.Append("('");
                set.Append(s);
                set.Append("'),");
            }

            string values = set.ToString().TrimEnd(',');
            string sql = @"insert into Teacher(code) values " + values;

            DBMySQLUtils.ExecuteQuery(sql);
        }

        private void btnAssignRoom_Click(object sender, EventArgs e)
        {
            string sql = "SELECT room_id FROM ROOM";

            DataTable dtRoom = DBMySQLUtils.ExecuteStatement(sql);

            List<string> rooms = new List<string>();

            foreach (DataRow row in dtRoom.Rows)
            {
                rooms.Add(row["room_id"].ToString());
            }

            sql = "SELECT teacher_id, code from Teacher";
            DataTable dtTeacher = new DataTable();

            dtTeacher = DBMySQLUtils.ExecuteStatement(sql);
            List<string> judges = new List<string>();


            foreach (DataRow row in dtTeacher.Rows)
            {
                judges.Add(row["teacher_id"].ToString());
            }

            int index = 0;

            foreach (String judge in judges)
            {
                sql = @"insert into judge_room(teacher_id, room_id) values (";
                sql = sql + (judge + ",'" + rooms[index] + "')");

                DBMySQLUtils.ExecuteQuery(sql);
                index++;
            }
        }

        private void btnStudent_Click(object sender, EventArgs e)
        {
            String sql = "SELECT teacher_id, code from Teacher";
            DataTable dtTeacher = new DataTable();

            dtTeacher = DBMySQLUtils.ExecuteStatement(sql);
            
            foreach (DataRow row in dtTeacher.Rows)
            {
                DataView dv = new DataView(table);
                string query = "Teacher='" + row["code"].ToString() + "'";
                dv.RowFilter = query;
                
                foreach (DataRowView rowView in dv)
                {
                    DataRow currentRow = rowView.Row;
                    string student = currentRow["StudentName"].ToString();
                    string[] name = student.Split(',');
                    StringBuilder sb = new StringBuilder("");

                    sb.Append("INSERT INTO student(last_name,first_name,teacher_id) values ('");
                    sb.Append(name[0].ToUpper().Trim());
                    sb.Append("','");
                    sb.Append(name[1].ToUpper().Trim());
                    sb.Append("',");
                    sb.Append(row["teacher_id"]);
                    sb.Append(")");

                    DBMySQLUtils.ExecuteQuery(sb.ToString());
                }

            }
        }

        private void btnPerformance_Click(object sender, EventArgs e)
        {
           
            string student, lastName, firstName, theory, performance,
                   preferredTiming, begin, end, studentID;
            string[] name;
            StringBuilder sb = new StringBuilder("");
            foreach (DataRow row in table.Rows)
            {
                student = row["StudentName"].ToString();
                name = student.Split(',');

                lastName = name[0].ToUpper().Trim();
                firstName = name[1].ToUpper().Trim();
                theory = row["Theory2018"].ToString().ToUpper().Trim();
                performance = row["Performance2018"].ToString().ToUpper().Trim();

                preferredTiming = row["StudentPreferredtiming"].ToString().ToUpper().Trim();

                begin = null;
                end = null;

                if (preferredTiming.IndexOf("ANYTIME") > -1)
                {
                    begin = "8:30:00";
                    end = "4:30:00";
                }
                else if (preferredTiming.IndexOf("AFTER") > -1) {
                    string[] temp = preferredTiming.Split(' ');
                    begin = temp[1].Trim();
                    end = "4:30:00";
                }
                else if (preferredTiming.IndexOf("BEFORE") > -1)
                {
                    string[] temp = preferredTiming.Split(' ');
                    begin = "8:30:00";
                    end = temp[1].Trim();
                }
                else if (preferredTiming.IndexOf("-") > -1)
                {
                    string[] temp = preferredTiming.Split('-');
                    begin = temp[0].Trim();
                    end = temp[1].Trim();
                }


                sb.Clear();

                sb.Append("SELECT student_id, last_name, first_name from student where last_name = '");
                sb.Append(lastName);
                sb.Append("' and first_name='");
                sb.Append(firstName);
                sb.Append("'");

                if(begin == null && end == null)
                {
                    begin = "8:30:00";
                    end = "4:30:00";
                }
                DataTable currentStudent = DBMySQLUtils.ExecuteStatement(sb.ToString());
                sb.Clear();

                if(currentStudent != null && currentStudent.Rows.Count == 1)
                {
                    studentID = currentStudent.Rows[0]["student_id"].ToString();
                    sb.Append("INSERT INTO STUDENTPERFORMANCE (student_id, preferred_start, preferred_end, theory, performance) VALUES (");
                    sb.Append(studentID);
                    sb.Append(",'");
                    sb.Append(begin);
                    sb.Append("','");
                    sb.Append(end);
                    sb.Append("'");
                    if (!string.IsNullOrEmpty(theory))
                    {
                        sb.Append(",'");
                        sb.Append(theory);
                        sb.Append("'");
                    }
                    else
                    {
                        sb.Append(",NULL");
                    }

                    if (!string.IsNullOrEmpty(performance))
                    {
                        sb.Append(",'");
                        sb.Append(performance);
                        sb.Append("'");
                    }
                    else
                    {
                        sb.Append(",NULL");
                    }

                    sb.Append(")");

                    string sql = sb.ToString();

                    int rowsAffected = DBMySQLUtils.ExecuteQuery(sql);

                }

            }
        }

        private void btnJudgeAvail_Click(object sender, EventArgs e)
        {
            String sql = "SELECT teacher_id, code from Teacher";
            DataTable dtTeacher = new DataTable();
            DataTable dtJudgeAvailability = new DataTable();

            dtJudgeAvailability.Columns.Add("TeacherID");
            dtJudgeAvailability.Columns.Add("Teacher");
            dtJudgeAvailability.Columns.Add("JudgingLevel");
            dtJudgeAvailability.Columns.Add("Availability");

            dtTeacher = DBMySQLUtils.ExecuteStatement(sql);

            foreach (DataRow row in dtTeacher.Rows)
            {
                DataView dv = new DataView(table);
                string query = "Teacher='" + row["code"].ToString() + "'";
                dv.RowFilter = query;

                foreach (DataRowView rowView in dv)
                {
                    DataRow newRow = dtJudgeAvailability.NewRow();
                    DataRow currentRow = rowView.Row;

                    newRow["TeacherID"] = row["teacher_id"].ToString();
                    newRow["Teacher"] = currentRow["Teacher"];
                    newRow["JudgingLevel"] = currentRow["JudgingLevel"];
                    newRow["Availability"] = currentRow["Availability"];
                    

                    dtJudgeAvailability.Rows.Add(newRow);
                    break;
                }

            }

            StringBuilder sb = new StringBuilder("");

            foreach(DataRow dr in dtJudgeAvailability.Rows)
            {
                sb.Clear();

                string availability = dr["Availability"].ToString().ToUpper().Trim();

                sb.Append("INSERT INTO JUDGE_AVAILABILITY(teacher_id,judging_level,availability_start, availability_end) VALUES(");
                sb.Append(dr["TeacherID"].ToString());
                sb.Append(",'");
                sb.Append(dr["JudgingLevel"].ToString());
                sb.Append("',");
                
                switch(availability)
                {
                    case "MORNING":
                        sb.Append("'8:30:00','12:00:00')");
                    break;
                    case "AFTERNOON":
                        sb.Append("'12:00:00','4:30:00')");
                        break;
                    case "BOTH":
                        sb.Append("'8:30:00','4:30:00')");
                    break;
                }

                DBMySQLUtils.ExecuteQuery(sb.ToString());

            }  
        }

        private void btnAssignLunch_Click(object sender, EventArgs e)
        {
            string[] break_time = {"11:30:00","12:00:00","12:30:00","13:00:00" };

            String sql = "select judge_avail_id, teacher_id, judging_level, availability_start, availability_end from judge_availability where availability_start = '8:30:00' and availability_end = '04:30:00'";
            DataTable dtJudgeAvailability = new DataTable();

            dtJudgeAvailability = DBMySQLUtils.ExecuteStatement(sql);

            int index = 0;

            StringBuilder sb = new StringBuilder("");
            
            foreach(DataRow row in dtJudgeAvailability.Rows)
            {
                sb.Clear();
                if(index > 3)
                {
                    index = 0;
                }

                TimeSpan breakTimeEnd = TimeSpan.Parse(break_time[index]).Add(new TimeSpan(0,30,0));
                                
                sb.Append("INSERT into judge_break(judge_id, break_time_start, break_time_end) values(");
                sb.Append(row["teacher_id"].ToString());
                sb.Append(",'");
                sb.Append(break_time[index]);
                sb.Append("','");
                sb.Append(breakTimeEnd);
                sb.Append("')");

                DBMySQLUtils.ExecuteQuery(sb.ToString());
                index++;
            }
                       
         }
    }
}
