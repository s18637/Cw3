using Cw3.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Cw3.DAL
{
    public class StudentsDb : IStudentsDb
    {
        string SqlCon = "Data Source=db-mssql;Initial Catalog=s18637;Integrated Security=True";
        public void AddStudent(Student student)
        {
            using (var client = new SqlConnection(SqlCon))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = client;
                    command.CommandText = "INSERT INTO STUDENT VALUES(@index,@FirstName,@lastName,@BirthDate,@IdE);";
                    command.Parameters.AddWithValue("index", student.IndexNumber);
                    command.Parameters.AddWithValue("FirstName", student.FirstName);
                    command.Parameters.AddWithValue("LastName", student.LastName);
                    command.Parameters.AddWithValue("BirthDate", student.Bdate);
                    command.Parameters.AddWithValue("IdE", 1);

                    client.Open();
                    command.ExecuteNonQuery();
                }

            }

        }

        public Student GetStudent(string id)
        {
            var st = new Student();
            using (var client = new SqlConnection(SqlCon))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = client;
                    command.CommandText = "SELECT * FROM STUDENT WHERE IndexNumber=@id";
                    command.Parameters.AddWithValue("id", id);
                    client.Open();
                    var dr = command.ExecuteReader();
                    dr.Read();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.IndexNumber = dr["IndexNumber"].ToString();
                    st.Bdate = DateTime.Parse(dr["BirthDate"].ToString());
                }
            }
            return st;
        }

        public IEnumerable<Student> GetStudents()
        {
            ICollection<Student> list = new List<Student>();
            using (var client = new SqlConnection(SqlCon))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = client;
                    command.CommandText = "SELECT * FROM STUDENT";
                    client.Open();
                    var dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        var st = new Student();
                        st.FirstName = dr["FirstName"].ToString();
                        st.LastName = dr["LastName"].ToString();
                        st.IndexNumber = dr["IndexNumber"].ToString();
                        st.Bdate = DateTime.Parse(dr["BirthDate"].ToString());
                        list.Add(st);
                    }
                }
            }
            return list;
        }

        public StundetEnrollment GetStudyInfo(string id)
        {
            StundetEnrollment se = new StundetEnrollment();
            using (var client = new SqlConnection(SqlCon))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = client;
                    command.CommandText = "SELECT e.Semester, e.StartDate, st.Name FROM STUDENT s join Enrollment e on s.IdEnrollment=e.IdEnrollment join Studies st on e.IdStudy=st.IdStudy where s.indexNumber = @id";
                    command.Parameters.AddWithValue("id", id);
                    client.Open();
                    var dr = command.ExecuteReader();
                    dr.Read();
                    se.Semestr = int.Parse(dr["Semester"].ToString());
                    se.DataRozpoczecia = DateTime.Parse(dr["StartDate"].ToString());
                    se.NazwaStudiow = dr["Name"].ToString();
                }
            }
            return se;
        }
    }
}
