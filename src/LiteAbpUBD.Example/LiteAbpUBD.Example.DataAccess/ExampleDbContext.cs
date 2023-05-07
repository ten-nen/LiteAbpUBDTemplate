using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using LiteAbpUBD.Example.DataAccess.Entities;
using LiteAbpUBD.Example.DataAccess.Extensions;
using System.Data.Common;
using System.Data;

namespace LiteAbpUBD.Example.DataAccess
{
    [ConnectionStringName("Example")]
    public class ExampleDbContext :
        AbpDbContext<ExampleDbContext>
    {
        public DbSet<Customer> Customers { get; set; }
        public ExampleDbContext(DbContextOptions<ExampleDbContext> options)
           : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            /* Include modules to your migration db context */

            /* Configure your own tables/entities inside here */

            builder.ConfigureDataPlatform();
        }
    }

    public class EfCoreRepository<TEntity> : EfCoreRepository<ExampleDbContext, TEntity>
    where TEntity : class, IEntity
    {
        public ExampleDbContext CurrentDbContext { get; }
        public EfCoreRepository(IDbContextProvider<ExampleDbContext> dbContextProvider) : base(dbContextProvider)
        {
            CurrentDbContext = dbContextProvider.GetDbContextAsync().GetAwaiter().GetResult();
        }
    }

    public static class DbContextExtensions
    {
        public static DataTable QueryDataTable(this DbContext context,
               string sqlQuery, params DbParameter[] parameters)
        {
            var dataTable = new DataTable();
            var connection = context.Database.GetDbConnection();
            var dbFactory = DbProviderFactories.GetFactory(connection);
            using (var cmd = dbFactory.CreateCommand())
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlQuery;
                if (parameters != null)
                {
                    foreach (var item in parameters)
                    {
                        cmd.Parameters.Add(item);
                    }
                }
                using (var adapter = dbFactory.CreateDataAdapter())
                {
                    adapter.SelectCommand = cmd;
                    adapter.Fill(dataTable);
                }
            }
            return dataTable;
        }
    }
}
