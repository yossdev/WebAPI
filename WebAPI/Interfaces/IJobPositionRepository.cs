using WebAPI.Models;

namespace WebAPI.Interfaces
{
    public interface IJobPositionRepository
    {
        ICollection<JobPosition> GetAll();
        JobPosition? GetById(int id);
        void Create(JobPosition jobPosition);
        int Update(JobPosition jobPosition, int id);
        int Delete(int id);
    }
}
