using DataAccess.SQL;
using Microsoft.EntityFrameworkCore;

namespace Migrations
{
    public static class Program
    {
        public static void Main()
        {
            var builder = DbContextHelper.GetDefaultDbContextOptionsBuilder();

            using var context = new WeatherForecastDbContext(builder.Options);

            context.Database.Migrate();
        }
    }
}
