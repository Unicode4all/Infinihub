using System.Collections.Generic;

namespace Infinity.so.Models
{
    public interface IBanRepository
    {
        void Add(Ban ban);
        IEnumerable<Ban> GetAll();
        Ban Find(string key);
        void Remove(string key);
        void Update(Ban ban);
    }
}