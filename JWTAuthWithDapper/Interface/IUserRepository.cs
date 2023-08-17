using JWTAuthWithDapper.Models;

namespace JWTAuthWithDapper.Interface
{
    public interface IUserRepository
    {
        public List<UserInfo> SelectUser(string email, string password);
    }
}
