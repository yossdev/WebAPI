using Npgsql;
using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.Repositories
{
    public class JobTitleRepository : IJobTitleRepository
    {
        private readonly NpgsqlDataSource _db_src;
        private const string JobTitle_TABLE = "\"JobTitle\"";

        public JobTitleRepository(NpgsqlDBSource db)
        { 
            _db_src = db.db_src;
        }

        public void Create(JobTitle jobTitle)
        {
            string script = $"INSERT into {JobTitle_TABLE} (code, name) VALUES (UPPER(@code), @name)";
            try
            {
                using (var cmd = _db_src.CreateCommand(script))
                {
                    cmd.Parameters.AddWithValue("code", jobTitle.Code);
                    cmd.Parameters.AddWithValue("name", jobTitle.Name);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (NpgsqlException)
            {
                throw;
            }
        }

        public int Delete(int id)
        {
            string script = $"DELETE from {JobTitle_TABLE} WHERE id = @id";
            try
            {
                using (var cmd = _db_src.CreateCommand(script))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    var rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected;
                }
            }
            catch (NpgsqlException)
            {
                throw;
            }
        }

        public ICollection<JobTitle> GetAll()
        {
            string script = $"SELECT * from {JobTitle_TABLE}";
            using (var cmd = _db_src.CreateCommand(script))
            using (var reader = cmd.ExecuteReader())
            {
                List<JobTitle> jobTitles = new List<JobTitle>();
                while (reader.Read())
                {
                    JobTitle job_title = new JobTitle()
                    {
                        Id = reader.GetInt32(0),
                        Code = reader.GetString(1),
                        Name = reader.GetString(2),
                        CreatedAt = reader.GetDateTime(3),
                        UpdatedAt = reader.GetDateTime(4)
                    };
                    jobTitles.Add(job_title);
                }
                return jobTitles;
            }
        }

        public JobTitle? GetById(int id)
        {
            string script = $"SELECT * from {JobTitle_TABLE} WHERE ID = @id";
            try
            {
                using (var cmd = _db_src.CreateCommand(script))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            JobTitle job_title = new JobTitle()
                            {
                                Id = reader.GetInt32(0),
                                Code = reader.GetString(1),
                                Name = reader.GetString(2),
                                CreatedAt = reader.GetDateTime(3),
                                UpdatedAt = reader.GetDateTime(4)
                            };
                            return job_title;
                        }
                    };
                };
            }
            catch (NpgsqlException)
            {
                throw;
            }

            return null;
        }

        public int Update(JobTitle jobTitle, int id)
        {
            string script = $"UPDATE {JobTitle_TABLE} SET code = UPPER(@code), name = @name, updated_at = now() WHERE id = @id";
            try
            {
                using (var cmd = _db_src.CreateCommand(script))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.Parameters.AddWithValue("code", jobTitle.Code);
                    cmd.Parameters.AddWithValue("name", jobTitle.Name);
                    var rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected;
                }
            }
            catch (NpgsqlException)
            {
                throw;
            }
        }
    }
}
