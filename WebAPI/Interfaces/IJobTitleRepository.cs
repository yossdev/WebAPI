using WebAPI.Models;

namespace WebAPI.Interfaces
{
    public interface IJobTitleRepository
    {
        ICollection<JobTitle> GetAll();
        JobTitle? GetById(int id);
        void Create(JobTitle jobTitle);
        int Update(JobTitle jobTitle, int id);
        int Delete(int id);
    }
}
