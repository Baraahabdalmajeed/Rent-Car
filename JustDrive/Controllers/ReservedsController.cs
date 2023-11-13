using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JustDrive.Data;
using JustDrive.Models;

namespace JustDrive.Controllers
{
    public class ReservedsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservedsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reserveds
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Reserved.Include(r => r.Car).Include(r => r.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Reserveds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserved = await _context.Reserved
                .Include(r => r.Car)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.ReservedId == id);
            if (reserved == null)
            {
                return NotFound();
            }

            return View(reserved);
        }

        // GET: Reserveds/Create
        public IActionResult Create()
        {
            ViewData["CarId"] = new SelectList(_context.Car, "CarId", "Name");
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Email");
            return View();
        }

        // POST: Reserveds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReservedId,StartDate,EndtDate,Price,IsArrive,Status,ReserveType,CarId,UserId")] Reserved reserved)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reserved);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarId"] = new SelectList(_context.Car, "CarId", "Name", reserved.CarId);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Email", reserved.UserId);
            return View(reserved);
        }

        // GET: Reserveds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserved = await _context.Reserved.FindAsync(id);
            if (reserved == null)
            {
                return NotFound();
            }
            ViewData["CarId"] = new SelectList(_context.Car, "CarId", "Name", reserved.CarId);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Email", reserved.UserId);
            return View(reserved);
        }

        // POST: Reserveds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReservedId,StartDate,EndtDate,Price,IsArrive,Status,ReserveType,CarId,UserId")] Reserved reserved)
        {
            if (id != reserved.ReservedId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reserved);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservedExists(reserved.ReservedId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarId"] = new SelectList(_context.Car, "CarId", "Name", reserved.CarId);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Email", reserved.UserId);
            return View(reserved);
        }

        // GET: Reserveds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserved = await _context.Reserved
                .Include(r => r.Car)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.ReservedId == id);
            if (reserved == null)
            {
                return NotFound();
            }

            return View(reserved);
        }

        // POST: Reserveds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reserved = await _context.Reserved.FindAsync(id);
            _context.Reserved.Remove(reserved);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservedExists(int id)
        {
            return _context.Reserved.Any(e => e.ReservedId == id);
        }
    }
}
