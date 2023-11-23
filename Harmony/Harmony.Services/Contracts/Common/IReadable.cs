using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harmony.Bussiness.Services.Contracts.Common
{
    public interface IReadable<RType> where RType : class
    {
        Task<IEnumerable<RType>> Get();
        Task<RType>? GetById(int id);
        
        
    }
}
