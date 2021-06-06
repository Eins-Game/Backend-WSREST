using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Eins.TransportEntities.Interfaces
{
    public interface IBaseCard
    {
        public Task<bool> IsPlayable(IBaseCard card2);
    }
}
