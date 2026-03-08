using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using pingword.src.Data;

namespace pingword.Tests.Data
{
    public abstract class DatabaseTest : IDisposable
    {
        protected readonly AppDbContext _context;
        private readonly SqliteConnection _connection;

        public DatabaseTest()
        {
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new AppDbContext(options);
            _context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _context.Dispose();
            _connection.Close();
        }
    }
}
