using System.Data.Entity;
using System.Data.SQLite;
using RateApplication.Backend.Skills;
using SQLite.CodeFirst;

namespace RateApplication.Backend.Sqlite
{
    public class SkillsDbContext : DbContext
    {
        public SkillsDbContext(string path) : base(new SQLiteConnection(path), contextOwnsConnection: true)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var sqliteConnectionInitializer = new SqliteCreateDatabaseIfNotExists<SkillsDbContext>(modelBuilder);
            Database.SetInitializer(sqliteConnectionInitializer);
        }

        public DbSet<Skill> Skills { get; set; }
    }
}