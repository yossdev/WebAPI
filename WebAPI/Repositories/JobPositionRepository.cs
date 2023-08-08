using Npgsql;
using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.Repositories
{
    public class JobPositionRepository : IJobPositionRepository
    {
        private readonly NpgsqlDataSource _db_src;
        private const string JobPosition_TABLE = "\"JobPosition\"";
        private const string JobTitle_TABLE = "\"JobTitle\"";

        public JobPositionRepository(NpgsqlDBSource db)
        {
            _db_src = db.db_src;
        }

        public void Create(JobPosition jobPosition)
        {
            string script = $"INSERT into {JobPosition_TABLE} (code, name, job_title_id) VALUES (UPPER(@code), @name, @job_title_id)";
            try
            {
                using (var cmd = _db_src.CreateCommand(script))
                {
                    cmd.Parameters.AddWithValue("code", jobPosition.Code);
                    cmd.Parameters.AddWithValue("name", jobPosition.Name);
                    cmd.Parameters.AddWithValue("job_title_id", jobPosition.JobTitle.Id);
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
            string script = $"DELETE from {JobPosition_TABLE} WHERE id = @id";
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

        public ICollection<JobPosition> GetAll()
        {
            string script = $"SELECT * from {JobPosition_TABLE} inner join {JobTitle_TABLE} on {JobPosition_TABLE}.\"job_title_id\" = {JobTitle_TABLE}.\"id\"";
            using (var cmd = _db_src.CreateCommand(script))
            using (var reader = cmd.ExecuteReader())
            {
                List<JobPosition> jobPositions = new List<JobPosition>();
                while (reader.Read())
                {
                    JobTitle jobTitle = new JobTitle()
                    {
                        Id = reader.GetInt32(6),
                        Code = reader.GetString(7),
                        Name = reader.GetString(8),
                        CreatedAt = reader.GetDateTime(9),
                        UpdatedAt = reader.GetDateTime(10)
                    };

                    JobPosition jobPosition = new JobPosition()
                    {
                        Id = reader.GetInt32(0),
                        Code = reader.GetString(1),
                        Name = reader.GetString(2),
                        CreatedAt = reader.GetDateTime(3),
                        UpdatedAt = reader.GetDateTime(4),
                        JobTitle = jobTitle
                    };
                    jobPositions.Add(jobPosition);
                }
                return jobPositions;
            }
        }

        public JobPosition? GetById(int id)
        {
            string script = $"SELECT * from {JobPosition_TABLE} inner join {JobTitle_TABLE} on {JobPosition_TABLE}.\"job_title_id\" = {JobTitle_TABLE}.\"id\" WHERE {JobPosition_TABLE}.\"id\" = @id";

            try
            {
                using (var cmd = _db_src.CreateCommand(script))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            JobTitle jobTitle = new JobTitle()
                            {
                                Id = reader.GetInt32(6),
                                Code = reader.GetString(7),
                                Name = reader.GetString(8),
                                CreatedAt= reader.GetDateTime(9),
                                UpdatedAt = reader.GetDateTime(10)
                            };

                            JobPosition jobPosition = new JobPosition()
                            {
                                Id = reader.GetInt32(0),
                                Code = reader.GetString(1),
                                Name = reader.GetString(2),
                                CreatedAt = reader.GetDateTime(3),
                                UpdatedAt = reader.GetDateTime(4),
                                JobTitle = jobTitle
                            };
                            return jobPosition;
                        }
                    }
                }
            }
            catch (NpgsqlException)
            {
                throw;
            }

            return null;
        }

        public int Update(JobPosition jobPosition, int id)
        {
            string script = $"UPDATE {JobPosition_TABLE} SET code = UPPER(@code), name = @name, job_title_id = @job_title_id, updated_at = now() WHERE id = @id";
            try
            {
                using (var cmd = _db_src.CreateCommand(script))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.Parameters.AddWithValue("code", jobPosition.Code);
                    cmd.Parameters.AddWithValue("name", jobPosition.Name);
                    cmd.Parameters.AddWithValue("job_title_id", jobPosition.JobTitle.Id);
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
