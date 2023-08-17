using JWTAuthWithDapper.Models;

namespace JWTAuthWithDapper.Interface
{
    public interface IUserRepository
    {
        public UserInfo SelectUser(string email, string password);
    }
}
