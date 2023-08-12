using Npgsql;
using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly NpgsqlDataSource _db_src;
        private const string Employee_TABLE = "\"Employee\"";
        private const string JobPosition_TABLE = "\"JobPosition\"";
        private const string JobTitle_TABLE = "\"JobTitle\"";

        public EmployeeRepository(NpgsqlDBSource db)
        {
            _db_src = db.db_src;
        }

        public void Create(Employee employee)
        {
            string script = $"INSERT into {Employee_TABLE} (name, address, job_position_id, job_title_id) VALUES (@name, @address, @job_position_id, @job_title_id)";
            try
            {
                using (var cmd = _db_src.CreateCommand(script))
                {
                    cmd.Parameters.AddWithValue("name", employee.Name);
                    cmd.Parameters.AddWithValue("address", employee.Address);
                    cmd.Parameters.AddWithValue("job_position_id", employee.JobPosition.Id);
                    cmd.Parameters.AddWithValue("job_title_id", employee.JobTitle.Id);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (NpgsqlException)
            {
                throw;
            }
        }

        public int Delete(string nik)
        {
            string script = $"DELETE from {Employee_TABLE} WHERE nik = @nik";
            try
            {
                using (var cmd = _db_src.CreateCommand(script))
                {
                    cmd.Parameters.AddWithValue("nik", nik);
                    var rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected;
                }
            }
            catch (NpgsqlException)
            {
                throw;
            }
            throw new NotImplementedException();
        }

        public Employee? GetByNIK(string nik)
        {
            string script = $"SELECT * from {Employee_TABLE} inner join {JobPosition_TABLE} on {Employee_TABLE}.\"job_position_id\" = {JobPosition_TABLE}.\"id\" inner join {JobTitle_TABLE} on {Employee_TABLE}.\"job_title_id\" = {JobTitle_TABLE}.\"id\" WHERE {Employee_TABLE}.\"nik\" = @nik";

            try
            {
                using (var cmd = _db_src.CreateCommand(script))
                {
                    cmd.Parameters.AddWithValue("nik", nik);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            JobTitle jobTitle = new JobTitle()
                            {
                                Id = reader.GetInt32(14),
                                Code = reader.GetString(15),
                                Name = reader.GetString(16),
                                CreatedAt = reader.GetDateTime(17),
                                UpdatedAt = reader.GetDateTime(18)
                            };

                            JobPosition jobPosition = new JobPosition()
                            {
                                Id = reader.GetInt32(8),
                                Code = reader.GetString(9),
                                Name = reader.GetString(10),
                                CreatedAt = reader.GetDateTime(11),
                                UpdatedAt = reader.GetDateTime(12)
                            };
        
                            Employee employee = new Employee()
                            {
                                Id = reader.GetGuid(0),
                                NIK = reader.GetString(1),
                                Name = reader.GetString(2),
                                Address = reader.GetString(3),
                                CreatedAt = reader.GetDateTime(4),
                                UpdatedAt = reader.GetDateTime(5),
                                JobPosition = jobPosition,
                                JobTitle = jobTitle
                            };
                            return employee;
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

        public async Task<ICollection<Employee>> GetAll()
        {
            string script = @$"
                SELECT * from {Employee_TABLE} 
                inner join {JobPosition_TABLE} on {Employee_TABLE}.""job_position_id"" = {JobPosition_TABLE}.""id"" 
                inner join {JobTitle_TABLE} on {Employee_TABLE}.""job_title_id"" = {JobTitle_TABLE}.""id""";
            using var cmd = _db_src.CreateCommand(script);
            using var reader = await cmd.ExecuteReaderAsync();
            List<Employee> employees = new();
            while (await reader.ReadAsync())
            {
                JobTitle jobTitle = new JobTitle()
                {
                    Id = reader.GetInt32(14),
                    Code = reader.GetString(15),
                    Name = reader.GetString(16),
                    CreatedAt = reader.GetDateTime(17),
                    UpdatedAt = reader.GetDateTime(18)
                };

                JobPosition jobPosition = new JobPosition()
                {
                    Id = reader.GetInt32(8),
                    Code = reader.GetString(9),
                    Name = reader.GetString(10),
                    CreatedAt = reader.GetDateTime(11),
                    UpdatedAt = reader.GetDateTime(12)
                };

                Employee employee = new Employee()
                {
                    Id = reader.GetGuid(0),
                    NIK = reader.GetString(1),
                    Name = reader.GetString(2),
                    Address = reader.GetString(3),
                    CreatedAt = reader.GetDateTime(4),
                    UpdatedAt = reader.GetDateTime(5),
                    JobPosition = jobPosition,
                    JobTitle = jobTitle
                };
                employees.Add(employee);
            }
            return employees;
        }

        public int Update(Employee employee, string nik)
        {
            string script = @$"
                UPDATE {Employee_TABLE} 
                SET name = @name, address = @address, job_position_id = @job_position_id, job_title_id = @job_title_id,
                updated_at = now()
                WHERE nik = @nik";
            try
            {
                using (var cmd = _db_src.CreateCommand(script))
                {
                    cmd.Parameters.AddWithValue("name", employee.Name);
                    cmd.Parameters.AddWithValue("address", employee.Address);
                    cmd.Parameters.AddWithValue("job_position_id", employee.JobPosition.Id);
                    cmd.Parameters.AddWithValue("job_title_id", employee.JobTitle.Id);
                    cmd.Parameters.AddWithValue("nik", nik);
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
