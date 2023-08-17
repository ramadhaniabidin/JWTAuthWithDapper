using Dapper;
using JWTAuthWithDapper.Interface;
using JWTAuthWithDapper.Models;
using Microsoft.Data.SqlClient;

namespace JWTAuthWithDapper.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly string connectionString = "Data Source=(localdb)\\local;Initial Catalog=JWTAuthentication;Integrated Security=True;";
        public UserInfo SelectUser(string email, string password)
        {
            try
            {
                using var con = new SqlConnection(connectionString);
                con.Open();
                var query = @"SELECT * FROM dbo.UserInfo WHERE Email = @email AND Password = @password";
                var user = con.QueryFirstOrDefault<UserInfo>(query, new {email, password});
                return user;
            }
            catch
            {
                throw;
            }
        }
    }
}
