using Cw3.Helpers;
using Cw3.Models;
using Cw3.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Cw3.DAL
{
    public class StudentsDb : IStudentsDb
    {
        string SqlCon = "Data Source=db-mssql;Initial Catalog=s18637;Integrated Security=True";
        readonly s18637Context context;
       public StudentsDb(s18637Context _context)
        {
            context = _context;
        }
        /*public MyHelper AddStudent(Student student)
{
using (var client = new SqlConnection(SqlCon))
{
using (var command = new SqlCommand())
{
  command.Connection = client;
  if (student.FirstName != null && student.IndexNumber != null && student.LastName != null && student.Bdate != null && student.Studies != null)
  {
      *//*Console.WriteLine(student.Studies);*//*
      command.CommandText = "Select * from studies where name=@studies";
      command.Parameters.AddWithValue("studies", student.Studies);
      client.Open();
      using (SqlTransaction transaction = client.BeginTransaction())
      {
          command.Transaction = transaction;
          string idStud="";
          try
          {
              idStud = "";
              var dr = command.ExecuteReader();
              if (!dr.Read())
              {
                  transaction.Rollback();
                  return new MyHelper("nie zlaeziono podanych studiow", -1);
              }
              else
              {
                  idStud = dr[0].ToString();
              }
          dr.Close();
          }
          catch (SqlException exe)
          {
              transaction.Rollback();
              return new MyHelper("wystapil blad podczas przeszukiwania studiow", -1);
          }
          try
          {
              command.CommandText = "Select * from student s join enrollment e on s.idenrollment = e.idenrollment join studies st on st.idstudy=e.idstudy where s.IndexNumber=@index and s.FirstName=@FirstName and s.LastName=@LastName and s.BirthDate=@BirthDate and st.name=@studies";
              command.Parameters.AddWithValue("index", student.IndexNumber);
              command.Parameters.AddWithValue("FirstName", student.FirstName);
              command.Parameters.AddWithValue("LastName", student.LastName);
              command.Parameters.AddWithValue("BirthDate", student.Bdate);
          var dr = command.ExecuteReader();
              if (dr.Read())
              {
                  dr.Close();
                  transaction.Rollback();
                  return new MyHelper("podany student juz istnieje", -1);
              }
          dr.Close();
      }
          catch (SqlException exe)
          {
          Console.WriteLine(exe);
              transaction.Rollback();
              return new MyHelper("wystapil blad podczas przeszukiwania studentow", -1);
          }
          string idE = "";
          try
          {
              command.CommandText = "select e.idenrollment from enrollment e join studies st on st.idstudy = e.idstudy where st.name=@studies and e.semester=1";
              var dr = command.ExecuteReader();
              idE = "";
              if (dr.Read())
              {
                  idE = dr[0].ToString();
              }
              else
              {
                  command.CommandText = "insert into enrollment values((select max(idenrollment)+1 from enrollment),1, @idStudy, getDate())";
                  command.Parameters.AddWithValue("idStudy", idStud);
                  command.ExecuteNonQuery();
                  command.CommandText = "select max(idenrollment) from enrollment";
                  dr = command.ExecuteReader();
                  dr.Read();
                  idE = dr[0].ToString();

              }
          dr.Close();
      }
          catch (Exception exe)
          {
              transaction.Rollback();
              return new MyHelper("wystapil blad podczas przeszukiwania lub dodawania enrollment", -1);
          }
          try
          {
              command.CommandText = "INSERT INTO STUDENT VALUES(@index,@FirstName,@LastName,@BirthDate,@idE);";
              command.Parameters.AddWithValue("idE", idE);
              command.ExecuteNonQuery();
              command.CommandText = "select * from enrollment where idenrollment = (select idenrollment from Student where IndexNumber=@index and FirstName=@FirstName and LastName=@LastName and BirthDate=@BirthDate)";
              var dr2 = command.ExecuteReader();
              dr2.Read();
              EnrollmentResponse enrollment = new EnrollmentResponse();
              enrollment.Semester = int.Parse(dr2[1].ToString());
              enrollment.IdStudies = int.Parse(dr2[2].ToString());
              enrollment.DataRozpoczecia = DateTime.Parse(dr2[3].ToString());
              MyHelper response = new MyHelper("dodano studenta", 0);
              response.enrollment = enrollment;
              dr2.Close();
              transaction.Commit();
              return response;
          }
          catch (Exception exe)
          {
              transaction.Rollback();
              return new MyHelper("wystapil blad podczas dodawania studenta", -1);
          }
      }
  }
  else
      return new MyHelper("podano bledne dane",-1);

  *//*command.Connection = client;
  command.CommandText = "INSERT INTO STUDENT VALUES(@index,@FirstName,@lastName,@BirthDate,@IdE);";
  command.Parameters.AddWithValue("index", student.IndexNumber);
  command.Parameters.AddWithValue("FirstName", student.FirstName);
  command.Parameters.AddWithValue("LastName", student.LastName);
  command.Parameters.AddWithValue("BirthDate", student.Bdate);
  command.Parameters.AddWithValue("IdE", 1);

  client.Open();
  command.ExecuteNonQuery();*//*
}

}*/

    /*}*/
       
        public MyHelper Promote(Enrollment se)
        {
            /* using (var client = new SqlConnection(SqlCon))
             {
                 using (var command = new SqlCommand())
                 {
                     command.Connection = client;
                     client.Open();
                     if (se.Semester != null && se.Studies != null)
                     {
                         command.CommandText = "select*from enrollment e join Studies st on e.IdStudy = st.IdStudy where e.Semester=@Sem and st.Name=@Stud";
                         command.Parameters.AddWithValue("Sem", se.Semester);
                         command.Parameters.AddWithValue("Stud", se.Studies);
                         var dr = command.ExecuteReader();
                         if (!dr.Read())
                         {
                             return new MyHelper("nie ma takiego enrollment", -1);
                         }
                         dr.Close();
                         using (var com = new SqlCommand(SqlCon))
                         {
                             com.Connection = client;
                             com.CommandText = "pormote";
                             com.CommandType = CommandType.StoredProcedure;
                             com.Parameters.AddWithValue("Studies", se.Studies);
                             com.Parameters.AddWithValue("Semester", se.Semester);
                             com.ExecuteNonQuery();
                             command.CommandText = "select * from enrollment e join Studies st on e.IdStudy = st.IdStudy where e.Semester = (@Sem+1) and st.Name = @Stud";
                             dr = command.ExecuteReader();
                             dr.Read();
                             EnrollmentResponse enrollment = new EnrollmentResponse();
                             enrollment.Semester = int.Parse(dr[1].ToString());
                             enrollment.IdStudies = int.Parse(dr[2].ToString());
                             enrollment.DataRozpoczecia = DateTime.Parse(dr[3].ToString());
                             MyHelper response = new MyHelper("udzielono promocji", 0);
                             response.enrollment = enrollment;
                             return response;
                         }
                     }
                     else
                         return new MyHelper("podano bledne dane", -1);
                 }
             }*/
            try
            {
                var res = context.Student.Where(x => x.IdEnrollment == se.IdEnrollment);
                var count = context.Enrollment.Where(x => x.IdEnrollment == (se.IdEnrollment + 1)).Count();
                if (count > 0)
                {
                    var enroll = context.Enrollment.Where(x => x.IdEnrollment == (se.IdEnrollment + 1)).First();
                    foreach (var s in res)
                    {
                        var idold = s.IdEnrollment;
                        s.IdEnrollment = enroll.IdEnrollment;
                        enroll.Student.Add(s);
                        var oldEnroll = context.Enrollment.Where(x => x.IdEnrollment == idold).First();
                        oldEnroll.Student.Remove(s);
                    }
                    context.SaveChanges();
                    MyHelper helper = new MyHelper("promoted", 0);
                    helper.enrollment = enroll;
                    return helper;
                }
                else
                {
                    HashSet<Student> set = new HashSet<Student>();
                    var enrollment = new Enrollment
                    {
                        Semester = se.Semester + 1,
                        IdStudy = se.IdStudy,
                        StartDate = DateTime.Now,
                        Student = set
                    };
                    context.Enrollment.Add(enrollment);
                    context.SaveChanges();
                    foreach (var s in res)
                    {
                        var idold = s.IdEnrollment;
                        var oldEnroll = context.Enrollment.Where(x => x.IdEnrollment == idold).First();
                        s.IdEnrollment = enrollment.IdEnrollment;
                        enrollment.Student.Add(s);
                        oldEnroll.Student.Remove(s);
                    }
                    context.SaveChanges();
                    MyHelper helper = new MyHelper("promoted", 0);
                    helper.enrollment = enrollment;
                    return helper;

                }
            }catch (Exception ex)
            {
                return new MyHelper(ex.ToString(), -1);
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
                    st.BirthDate = DateTime.Parse(dr["BirthDate"].ToString());
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
                        st.BirthDate = DateTime.Parse(dr["BirthDate"].ToString());
                        list.Add(st);
                    }
                }
            }
            return list;
        }

        public Enrollment GetStudyInfo(string id)
        {
            Enrollment se = new Enrollment();
            /*using (var client = new SqlConnection(SqlCon))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = client;
                    command.CommandText = "SELECT e.Semester, e.StartDate, st.Name FROM STUDENT s join Enrollment e on s.IdEnrollment=e.IdEnrollment join Studies st on e.IdStudy=st.IdStudy where s.indexNumber = @id";
                    command.Parameters.AddWithValue("id", id);
                    client.Open();
                    var dr = command.ExecuteReader();
                    dr.Read();
                    se.Semester = int.Parse(dr["Semester"].ToString());
                    se.DataRozpoczecia = DateTime.Parse(dr["StartDate"].ToString());
                    se.Studies = dr["Name"].ToString();
                }
            }*/
            return se;
        }

        public MyHelper AddStudent(Student student, string studia)
        {
            try
            {
                var count = context.Enrollment.Join(context.Studies, enroll => enroll.IdStudy, stud => stud.IdStudy, (enroll, stud) => new { enroll, stud }).Where(stud => stud.stud.Name == studia).Count();
                if (count > 0)
                {
                    var enroll = context.Enrollment.Join(context.Studies, enroll => enroll.IdStudy, stud => stud.IdStudy, (enroll, stud) => new { enroll, stud }).Where(stud => stud.stud.Name == studia && stud.enroll.Semester == 1).Select(x => x.enroll.IdEnrollment).First();
                    student.IdEnrollment = enroll;
                    context.Student.Add(student);
                    var enrollment = context.Enrollment.Where(x => x.IdEnrollment == enroll).First();
                    context.SaveChanges();
                    enrollment.Student.Add(student);
                    context.SaveChanges();
                    MyHelper helper = new MyHelper("added", 0);
                    helper.enrollment = enrollment;
                    return helper;
                }
                else
                {
                    var idStudy = context.Studies.Where(x => x.Name == studia).Select(x => x.IdStudy).First();
                    HashSet<Student> set = new HashSet<Student>();
                    var enrollment = new Enrollment
                    {
                        Semester = 1,
                        IdStudy = idStudy,
                        StartDate = DateTime.Now,
                        Student = set
                    };
                    context.Enrollment.Add(enrollment);
                    context.SaveChanges();
                    student.IdEnrollment = enrollment.IdEnrollment;
                    context.Student.Add(student);
                    context.SaveChanges();
                    enrollment.Student.Add(student);
                    context.SaveChanges();
                    MyHelper helper = new MyHelper("added", 0);
                    helper.enrollment = enrollment;
                    return helper;
                }
            }catch (Exception ex)
            {
                return new MyHelper(ex.ToString(), 0);
            }
        }

        public IEnumerable<Student> GetList()
        {
            var res = context.Student.ToList();
            return res;
        }

        public MyHelper UpdateStudent(Student student)
        {
            try
            {
                context.Student.Attach(student);
                context.Entry(student).State = EntityState.Modified;
                context.SaveChanges();
            }catch(Exception ex)
            {
                return new MyHelper(ex.ToString(), -1);
            }
            return new MyHelper("updated", 0);
        }

        public MyHelper DeleteStudent(string id)
        {
            try
            {
                var student = context.Student.Where(x => x.IndexNumber == id).First();
                context.Student.Remove(student);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                return new MyHelper(ex.ToString(), -1);
            }
            return new MyHelper("removed", 0);
        }
    }
}
