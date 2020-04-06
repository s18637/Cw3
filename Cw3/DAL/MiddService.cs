using Cw3.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Cw3.DAL
{
    public class MiddService : IMiddService
    {
        string SqlCon = "Data Source=db-mssql;Initial Catalog=s18637;Integrated Security=True";

        public MyHelper checkIndex(string index)
        {
            MyHelper response;
            using (var client = new SqlConnection(SqlCon))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = client;
                    client.Open();
                    command.CommandText = "Select * from student where IndexNumber=@index";
                    command.Parameters.AddWithValue("index", index);
                    var d = command.ExecuteReader();
                    if (!d.Read())
                    {
                        response = new MyHelper("nie znaleziono takiego studenta", -1);
                    }
                    else
                    {
                        response = new MyHelper("znaleziono studenta", 0);
                    }
                    return response;
                }
            }
        }
    }
}
