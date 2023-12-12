using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harmony.Persistence.Domain.Base
{
    public abstract class BaseClassifier
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty!;
    }
}
