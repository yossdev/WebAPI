using Npgsql;

namespace WebAPI.Repositories
{
    // Learn more about Npgsql https://www.npgsql.org/
    public class NpgsqlRepository
    {
        private readonly NpgsqlDataSource _db_src;

        public NpgsqlRepository(IConfiguration config)
        {
            string conn_str = config["ConnectionStrings:PG"];
            _db_src = NpgsqlDataSource.Create(conn_str);
        }

        public async Task Migrate()
        {
            string script = File.ReadAllText("./Migrations/Setup.sql");
            await using var cmd = _db_src.CreateCommand(script);
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
