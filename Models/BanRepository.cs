using Infinity.so.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infinity.so.Models
{
    public class BanRepository : IBanRepository
    {
        private readonly ApplicationDbContext _context;

        public BanRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Ban> GetAll()
        {
            return  _context.Bans.ToList();
        }

        public async void Add(Ban ban)
        {
            ban.BanDate = DateTime.Now;
            _context.Add(ban);
            await _context.SaveChangesAsync();
        }

        public Ban Find(string key)
        {
            return _context.Bans.Find(key);
        }

        public async Task<IEnumerable<Ban>> FindByCkey(string query)
        {
            var bans = from b in _context.Bans select b;

            if (!String.IsNullOrEmpty(query)) 
            {
                bans = bans.Where(b => b.SubjectCkey.Contains(query));
            }
            

            return bans.ToList();
        }

        public async void Remove(string key)
        {
            var ban = await _context.Bans.FindAsync(key);
            _context.Remove(ban);
            await _context.SaveChangesAsync();
        }

        public async void Update(Ban ban)
        {
            _context.Add(ban);
            await _context.SaveChangesAsync();
        }
    }
}