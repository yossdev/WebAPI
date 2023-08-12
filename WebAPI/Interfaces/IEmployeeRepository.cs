using WebAPI.Models;

namespace WebAPI.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<ICollection<Employee>> GetAll();
        Employee? GetByNIK(string nik);
        void Create(Employee employee);
        int Update(Employee employee, string nik);
        int Delete(string nik);
    }
}
