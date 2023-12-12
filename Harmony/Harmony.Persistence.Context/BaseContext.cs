using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony.Common;
using Harmony.Persistence.Domain.Base;

namespace Harmony.Persistence.Context
{
    public class BaseContext<Context> : DbContext where Context : DbContext
    {
        public BaseContext(DbContextOptions<Context> options) : base(options)
        {
        }

        //private readonly UserContext _userContext;
        Object _userContext;
        public BaseContext(DbContextOptions<Context> options, Object userContext) : base(options)
        {
            //_userContext = userContext;
        }

        private void CustomSave()
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedById = 0;
                        entry.Entity.CreatedDate = Server.GetDate();

                        break;
                    case EntityState.Modified:
                        entry.Entity.LastUpdatedById =0;
                        entry.Entity.LastUpdatedDate = Server.GetDate();
                        break;
                }
            }

        }

        public override int SaveChanges()
        {
            CustomSave();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            CustomSave();
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
