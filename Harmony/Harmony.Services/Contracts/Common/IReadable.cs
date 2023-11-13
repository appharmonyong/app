using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harmony.Bussiness.Services.Contracts.Common
{
    public interface IReadable<RType> where RType : class
    {
        public Task<IEnumerable<RType>> Get();
        public Task<RType>? GetById(int id);
    }
}
