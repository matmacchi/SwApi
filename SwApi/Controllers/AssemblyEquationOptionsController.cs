using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SwApi.Data;
using SwApi.Models;

namespace SwApi.Controllers
{
    public class AssemblyEquationOptionsController : Controller
    {
        private readonly SwApiContext _context;

        public AssemblyEquationOptionsController(SwApiContext context)
        {
            _context = context;
        }

        // GET: AssemblyEquationOptions
        public async Task<IActionResult> Index()
        {
            return View(await _context.AssemblyEquationOption.ToListAsync());
        }

        // GET: AssemblyEquationOptions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assemblyEquationOption = await _context.AssemblyEquationOption
                .FirstOrDefaultAsync(m => m.ID == id);
            if (assemblyEquationOption == null)
            {
                return NotFound();
            }

            return View(assemblyEquationOption);
        }

        // GET: AssemblyEquationOptions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AssemblyEquationOptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Type,EquationTarget,DefaultValue,MinValue,MaxValue")] AssemblyEquationOption assemblyEquationOption)
        {
            if (ModelState.IsValid)
            {
                _context.Add(assemblyEquationOption);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(assemblyEquationOption);
        }

        // GET: AssemblyEquationOptions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assemblyEquationOption = await _context.AssemblyEquationOption.FindAsync(id);
            if (assemblyEquationOption == null)
            {
                return NotFound();
            }
            return View(assemblyEquationOption);
        }

        // POST: AssemblyEquationOptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Type,EquationTarget,DefaultValue,MinValue,MaxValue")] AssemblyEquationOption assemblyEquationOption)
        {
            if (id != assemblyEquationOption.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(assemblyEquationOption);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssemblyEquationOptionExists(assemblyEquationOption.ID))
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
            return View(assemblyEquationOption);
        }

        // GET: AssemblyEquationOptions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assemblyEquationOption = await _context.AssemblyEquationOption
                .FirstOrDefaultAsync(m => m.ID == id);
            if (assemblyEquationOption == null)
            {
                return NotFound();
            }

            return View(assemblyEquationOption);
        }

        // POST: AssemblyEquationOptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var assemblyEquationOption = await _context.AssemblyEquationOption.FindAsync(id);
            _context.AssemblyEquationOption.Remove(assemblyEquationOption);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssemblyEquationOptionExists(int id)
        {
            return _context.AssemblyEquationOption.Any(e => e.ID == id);
        }
    }
}
