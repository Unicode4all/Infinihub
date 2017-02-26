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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Infinihub.Data;
using Infinihub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;

namespace Infinihub.so.Controllers
{
    [Route("bans")]
    public class BansController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<BansController> _localizer;

        public BansController(ApplicationDbContext context, IStringLocalizer<BansController> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        // GET: Bans
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Bans.ToListAsync());
        }

        // GET: Bans/Details/5
        [Route("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ban = await _context.Bans.SingleOrDefaultAsync(m => m.Id == id);
            if (ban == null)
            {
                return NotFound();
            }

            return View(ban);
        }

        // GET: Bans/Create
        [Route("create")]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Bans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("create")]
        [Authorize]
        public async Task<IActionResult> Create(Ban ban)
        {
            if (ModelState.IsValid)
            {
                ban.BanDate = DateTime.Now;
                ban.BanExpirationDate = DateTime.Now.AddMinutes(ban.BanExpiration);
                _context.Bans.Add(ban);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(ban);
        }

        // GET: Bans/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ban = await _context.Bans.SingleOrDefaultAsync(m => m.Id == id);
            if (ban == null)
            {
                return NotFound();
            }
            return View(ban);
        }

        // POST: Bans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("BanId,AdminCkey,BanDate,BanExpiryTime,BanType,Job,SubjectCid,SubjectCkey,SubjectIPAddress")] Ban ban)
        {
            if (id != ban.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ban);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BanExists(ban.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(ban);
        }

        // GET: Bans/Delete/5
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ban = await _context.Bans.SingleOrDefaultAsync(m => m.Id == id);
            if (ban == null)
            {
                return NotFound();
            }

            return View(ban);
        }

        // POST: Bans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ban = await _context.Bans.SingleOrDefaultAsync(m => m.Id == id);
            _context.Bans.Remove(ban);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool BanExists(int id)
        {
            return _context.Bans.Any(e => e.Id == id);
        }
    }
}
