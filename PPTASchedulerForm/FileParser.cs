using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using GenericParsing;

namespace PPTASchedulerForm
{
    public static class FileParser
    {
        public static DataTable Parse(String filePath)
        {
            DataTable table = new DataTable();

            table.Columns.Add("Teacher", typeof(string));
            table.Columns.Add("JudgingLevel", typeof(string));
            table.Columns.Add("Availability", typeof(string));
            table.Columns.Add("StudentName", typeof(string));
            table.Columns.Add("Level2017", typeof(string));
            table.Columns.Add("Theory2018", typeof(string));
            table.Columns.Add("Performance2018", typeof(string));
            table.Columns.Add("StudentPreferredtiming", typeof(string));

            using (GenericParser parser = new GenericParser())
            {
                parser.SetDataSource(filePath);

                parser.ColumnDelimiter = ',';
                parser.FirstRowHasHeader = true;
                parser.MaxBufferSize = 4096;
                parser.MaxRows = 200;
                parser.TextQualifier = '\"';

                while (parser.Read())
                {
                    DataRow row = table.NewRow();

                    row["Teacher"] = parser["Teacher"];
                    row["JudgingLevel"] = parser["Highest Level"];
                    row["Availability"] = parser["Availability"];
                    row["StudentName"] = parser["Student Name"];
                    row["Level2017"] = parser["2017 Level"];
                    row["Theory2018"] = parser["2018 Theory"];
                    row["Performance2018"] = parser["2018 Performance"];
                    row["StudentPreferredtiming"] = parser["Student Preferred Time"];

                    table.Rows.Add(row);
                }

                parser.Close();
            }

            return table;
        }
            
    }
}
