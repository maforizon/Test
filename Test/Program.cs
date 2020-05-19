using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Data.Common;
using System.Data;


namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {

            // Получить объект Connection подключенный к БД.
            MySqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();
            try
            {
             
                QueryEmployee(conn);
                Querrytime_reports(conn);
                Printtop("Monday",conn);
                Printtop("Tuesday", conn);
                Printtop("Wednesday", conn);
                Printtop("Thursday", conn);
                Printtop("Friday", conn);
                Printtop("Saturday", conn);
                Printtop("Sunday", conn);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e);
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
               
                conn.Close();
              
                conn.Dispose();
            }
            Console.Read();
        }

        private static void QueryEmployee(MySqlConnection conn)
        {
            string sql = "Select id, name from employees";

            //  объект Command.
            MySqlCommand cmd = new MySqlCommand();

            
            cmd.Connection = conn;
            cmd.CommandText = sql;


            using (DbDataReader reader = cmd.ExecuteReader())
            {

                if (reader.HasRows)
                {
                    Console.WriteLine("employees");
                    Console.WriteLine("|{0}|\t|{1}|\t", reader.GetName(0), reader.GetName(1));

                    while (reader.Read())
                    {
                      

                        int empIdIndex = reader.GetOrdinal("id"); 


                        long empId = Convert.ToInt64(reader.GetValue(0));

                     
                        string empNo = reader.GetString(1);
                        int empNameIndex = reader.GetOrdinal("name");
                        string empName = reader.GetString(empNameIndex);

                        Console.WriteLine("|{0}|\t|{1}|", empId, empNo);





                    }
                }
            }



        }
        private static void Querrytime_reports(MySqlConnection conn)
        {
            string sql = "Select id, employee_id,hours,dates from time_reports";

            
            MySqlCommand cmd = new MySqlCommand();

            
            cmd.Connection = conn;
            cmd.CommandText = sql;


            using (DbDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    Console.WriteLine("time_reports");
                    Console.WriteLine("|{0}|\t|{1}|\t|{2}|\t|{3}|\t", reader.GetName(0), reader.GetName(1), reader.GetName(2), reader.GetName(3));
                    while (reader.Read())
                    {
                       
                        int reportidindex = reader.GetOrdinal("id"); 

                        long reportid = Convert.ToInt64(reader.GetValue(0));

                        
                        int reportemployindex = reader.GetOrdinal("employee_id");
                        long emploid = Convert.ToInt64(reader.GetValue(1));

                        int hoursindex = reader.GetOrdinal("hours");
                        object hours = Convert.ToDouble(reader.GetValue(2));

                        int dateindex = reader.GetOrdinal("dates");
                        DateTime date = Convert.ToDateTime(reader.GetValue(3));


                       
                        Console.WriteLine("|{0}|\t|{1}|\t        |{2}|\t|{3}|\t", reportid, emploid, hours, +date.Month + "." + date.Day + "." + date.Year);


                        


                    }







                }
            }



        }
        public static void Printtop( string day, MySqlConnection conn)
        {
            string sql = "select employees.name, employees.id, time_reports.id, time_reports.employee_id, time_reports.hours ,time_reports.dates from employees, time_reports";

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = sql;
            using (DbDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int empIdIndex = reader.GetOrdinal("id"); 
                        long empId = Convert.ToInt64(reader.GetValue(1));

                        
                        string empNo = reader.GetString(0);
                        int empNameIndex = reader.GetOrdinal("name");
                        string empName = reader.GetString(empNameIndex);

                        int reportemployindex = reader.GetOrdinal("employee_id");
                        long emploid = Convert.ToInt64(reader.GetValue(3));

                        int hoursindex = reader.GetOrdinal("hours");
                        object hours = Convert.ToDouble(reader.GetValue(4));

                        int dateindex = reader.GetOrdinal("dates");
                        DateTime date = Convert.ToDateTime(reader.GetValue(5));


                        if (Convert.ToString(date.DayOfWeek) == day)
                        {


                            

                            MySqlConnection connect = DBUtils.GetDBConnection();
                            connect.Open(); //сам запрос находит топ 3 по всем данным, в sql не получилось искать даты по дню недели ,date.DayOfWeek) == day (как тут ) 
                            string commnd = "select employees.name, time_reports.employee_id,avg(time_reports.hours) from  time_reports, employees where  employees.id = time_reports.employee_id group by employee_id   order by avg(hours)  desc limit 3  ";
                            MySqlCommand cmmnd = new MySqlCommand(commnd, connect);



                            object count = cmmnd.ExecuteScalar();

                           
                            //С выводом лучших сотрудников неполадки, выводит их много ,хотя в запросе  устновлен вывод только первых трех значений, скорее всего проблема из-за того , что ввсе находится в while
                            Console.WriteLine("|{0}| {1} ({2} hours) \r",day, count,Math.Round(Convert.ToDouble( hours),2,MidpointRounding.AwayFromZero));//округление


                           

                        }
                    }


                }
           
                
            }


        }
       


    }
}