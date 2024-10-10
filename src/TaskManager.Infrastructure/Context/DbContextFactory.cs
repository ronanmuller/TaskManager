using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using TaskManager.Infrastructure.Context;

namespace GE.SpreadSheet.Data
{
    public class DbContextFactory : IDesignTimeDbContextFactory<ReadContext>
    {
        public ReadContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ReadContext>();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Definir o caminho base
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // Carregar o arquivo appsettings.json
                .Build();

           string connectionString = configuration.GetConnectionString("DefaultConnection");
            //string connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("A string de conexão não está definida.");
            }

            optionsBuilder.UseSqlServer(connectionString);

            return new ReadContext(optionsBuilder.Options);
        }
    }
}