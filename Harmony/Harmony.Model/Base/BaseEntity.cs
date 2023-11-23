using Harmony.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harmony.Persistence.Domain.Base
{
    public abstract class BaseEntity : IDeleteFlagEntity
    {
        public int Id { get; set; }
        public bool IsActive { get; set; } = true!;
        public DateTimeOffset CreatedDate { get; set; }
        public int CreatedById { get; set; }
        public DateTimeOffset? LastUpdatedDate { get; set; }
        public int LastUpdatedById { get; set; }
        public bool IsDelete { get; set; }
    }
}
