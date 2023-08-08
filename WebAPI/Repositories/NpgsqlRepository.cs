using Npgsql;

namespace WebAPI.Repositories
{
    public class NpgsqlDBSource
    {
        public NpgsqlDataSource db_src { get; set; }
        public NpgsqlDBSource(IConfiguration config)
        {
            string conn_str = config["ConnectionStrings:PG"];
            db_src = NpgsqlDataSource.Create(conn_str);
        }
    }

    // Learn more about Npgsql https://www.npgsql.org/
    public class NpgsqlRepository : IStartupFilter
    {
        private readonly NpgsqlDataSource _db_src;

        public NpgsqlRepository(NpgsqlDBSource db)
        {
            _db_src = db.db_src;
        }

        public void Migrate()
        {
            string script = File.ReadAllText("./Migrations/Setup.sql");
            using (var cmd = _db_src.CreateCommand(script))
            {
                cmd.ExecuteNonQuery();
            };
        }

        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            // Call your initialization method
            Migrate();

            return next;
        }
    }
}
