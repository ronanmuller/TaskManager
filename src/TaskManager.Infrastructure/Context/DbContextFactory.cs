using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TaskManager.Infrastructure.Context
{
    [ExcludeFromCodeCoverage]
    public class DbContextFactory : IDesignTimeDbContextFactory<ReadContext>
    {
        public ReadContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ReadContext>();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Definir o caminho base
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // Carregar o arquivo appsettings.json
                .Build();

           string connectionString = configuration.GetConnectionString("ConnectionStrings__DefaultConnection");
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