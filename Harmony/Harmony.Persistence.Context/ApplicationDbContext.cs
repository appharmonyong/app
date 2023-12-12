using Harmony.Persistence.Context.Common;
using Harmony.Persistence.Domain;
using Microsoft.EntityFrameworkCore;

namespace Harmony.Persistence.Context
{
    public class ApplicationDbContext : BaseContext<ApplicationDbContext>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        public DbSet<UserEntity> User { get; set; }

        #region Fluent API

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.FluentApiConfig<ApplicationDbContext>();
            base.OnModelCreating(modelBuilder);
        }

        #endregion

    }
}