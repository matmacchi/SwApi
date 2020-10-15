using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SwApi.Data;
using SwApi.Models;

namespace SwApi.Controllers
{
    public class AssembliesController : Controller
    {
        private readonly SwApiContext _context;

        public AssembliesController(SwApiContext context)
        {
            _context = context;
        }

        // GET: Assemblies
        public async Task<IActionResult> Index()
        {
            return View(await _context.Assembly.ToListAsync());
        }

        // GET: Assemblies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @assembly = await _context.Assembly
                .FirstOrDefaultAsync(m => m.ID == id);
            if (@assembly == null)
            {
                return NotFound();
            }

            return View(@assembly);
        }

        // GET: Assemblies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Assemblies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Path,Name")] Assembly @assembly)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@assembly);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@assembly);
        }

        // GET: Assemblies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @assembly = await _context.Assembly.FindAsync(id);
            if (@assembly == null)
            {
                return NotFound();
            }
            return View(@assembly);
        }

        // POST: Assemblies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Path,Name")] Assembly @assembly)
        {
            if (id != @assembly.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@assembly);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssemblyExists(@assembly.ID))
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
            return View(@assembly);
        }

        // GET: Assemblies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @assembly = await _context.Assembly
                .FirstOrDefaultAsync(m => m.ID == id);
            if (@assembly == null)
            {
                return NotFound();
            }

            return View(@assembly);
        }

        // POST: Assemblies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @assembly = await _context.Assembly.FindAsync(id);
            _context.Assembly.Remove(@assembly);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssemblyExists(int id)
        {
            return _context.Assembly.Any(e => e.ID == id);
        }
    }
}
