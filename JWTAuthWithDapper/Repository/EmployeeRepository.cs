using Dapper;
using JWTAuthWithDapper.Interface;
using JWTAuthWithDapper.Models;
using Microsoft.Data.SqlClient;

namespace JWTAuthWithDapper.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string connectionString = "Data Source=(localdb)\\local;Initial Catalog=JWTAuthentication;Integrated Security=True;";
        public List<Employee> GetAll()
        {
            try
            {
                using var con = new SqlConnection(connectionString);
                con.Open();
                var query = "SELECT * FROM dbo.Employee";
                var employees = con.Query<Employee>(query).ToList();
                return employees;
            } 
            catch
            {
                throw;
            }
        }
    }
}
