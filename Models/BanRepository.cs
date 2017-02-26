//   Copyright 2017 Solaris 13 Foundation
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
using Infinihub.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infinihub.Models
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

        public async Task Add(Ban ban)
        {
            ban.BanDate = DateTime.Now;
            ban.BanExpirationDate = DateTime.Now.AddMinutes(ban.BanExpiration);
            _context.Add(ban);
            await _context.SaveChangesAsync();
        }
            
        public Ban Find(string ckey, string cid, string ip)
        {
            var ban = _context.Bans.Where(b => b.SubjectCkey == ckey.ToLower() || b.SubjectCid == cid || b.SubjectIPAddress == ip
                        && (b.BanType == BanType.PERMABAN || (b.BanType == BanType.TEMP && (b.BanExpirationDate > DateTime.Now)))).FirstOrDefault();
            return ban;
        }

        public async Task<Ban> FindByIdAsync(int id)
        {
            return await _context.Bans.FindAsync(id);
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

        public async void Remove(int id)
        {
            var ban = await _context.Bans.FindAsync(id);
            _context.Bans.Remove(ban);
            await _context.SaveChangesAsync();
        }

        public async void Update(Ban newban)
        {
            var ban = await _context.Bans.FindAsync(newban.Id);
            if (ban == null)
            {
                return;
            }

            _context.Entry(ban).CurrentValues.SetValues(newban);
            await _context.SaveChangesAsync();
        }
    }
}