using JWTAuthWithDapper.Models;

namespace JWTAuthWithDapper.Interface
{
    public interface IEmployeeRepository
    {
        public List<Employee> GetAll();
    }
}
