using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Eins.TransportEntities.Interfaces
{
    public interface IBaseCard
    {
        //I really dont know what to put here, could be anything™

        public Task<bool> IsPlayable(IBaseCard card2);
    }
}
