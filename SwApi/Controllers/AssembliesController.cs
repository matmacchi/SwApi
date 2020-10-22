using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient.DataClassification;
using Microsoft.EntityFrameworkCore;
using SwApi.Data;
using SwApi.Models;

namespace SwApi.Controllers
{
    public class AssembliesController : Controller
    {
        private readonly SwApiContext _context;
        private readonly SwApiManager _swApiMng;

        public AssembliesController(SwApiContext context,SwApiManager swApiManager)
        {
            _context = context;
            _swApiMng = swApiManager;
        }

        /*

        private List<string> getAvailableEquationFromSwFile(PreAssemblyModel preAssembly)
        {
            var availableEquation = _swApiMng.GetAvailableEquation(preAssembly.Path);
            Debugger.Break();
            return availableEquation;
        }*/

        private AssemblyView PopulateAssemblyView(AssemblyView model)
        {
            var preAssemblies = new List<SelectListItem>();

            var preAssemblyModels = _context.PreAssemblyModel.Where(pa => pa.Assembly == null).ToList();

            foreach(var preAssemblyModel in preAssemblyModels)
            {
                preAssemblies.Add(new SelectListItem()
                {
                    Value = preAssemblyModel.PreAssemblyModelID.ToString(),
                    Text = preAssemblyModel.Name,
                });
            }


            model.PreAssemblyChoices = new SelectList(preAssemblies, "Value","Text");

            return model;
        }

        // GET: Assemblies
        public async Task<IActionResult> Index()
        {
            var swApiContext = _context.SldAssembly.Include(@a => @a.PreAssembly);

            //_swApiMng.GetAvailableEquation("");


            return View(await swApiContext.ToListAsync());
        }

        // GET: Assemblies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @assembly = await _context.SldAssembly
                .Include(@a => @a.PreAssembly)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (@assembly == null)
            {
                return NotFound();
            }

            return View(@assembly);
        }

        public IActionResult CreatePage1()
        {
            var model = new AssemblyView();
            model = PopulateAssemblyView(model);

            return View(model);
        }

        private List<EquationSetter> PopulateEquations(PreAssemblyModel preAssembly)
        {
            var rawEquations = _swApiMng.GetAvailableEquation(preAssembly.GetMainFilePath());
            List<EquationSetter> equationsList = new List<EquationSetter>();

            for(int i =0;i < rawEquations.Count; i++)
            {
                EquationSetter equation = new EquationSetter()
                {
                    EquationTarget = i,
                    Label = rawEquations[i],
                    MaxValue = 0,
                    MinValue = 0,
                    DefaultValue = 0,
                    ReferenceTag = "%tag%",
                    StepValue = 0,
                    IsActivated = false,
                };
                equationsList.Add(equation);
            }

            return equationsList;
        }


        public IActionResult CreatePage2(int PreAssemblyChoices)
        {
            var model = new AssemblyView();
            model.selectedPreAssemblyId = PreAssemblyChoices;
            var preAssembly = _context.PreAssemblyModel.Find(PreAssemblyChoices);
            model.Equations = PopulateEquations(preAssembly); 
            Debugger.Break();
            return View(model);
        }

        // GET: Assemblies/Create
        public IActionResult Create()
        {
            var model = new AssemblyView();
            model = PopulateAssemblyView(model);
            //ViewData["PreAssemblyChoices"] = model.PreAssemblyChoices;

            return View(model);
        }

        // POST: Assemblies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AssemblyView modelView)
        {

            var @assembly = new SldAssembly();

            if (ModelState.IsValid)
            {
                _context.Add(@assembly);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AssemblyID"] = new SelectList(_context.PreAssemblyModel, "PreAssemblyModelID", "PreAssemblyModelID", @assembly.ID);
            return View(modelView);
        }

        // GET: Assemblies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @assembly = await _context.SldAssembly.FindAsync(id);
            if (@assembly == null)
            {
                return NotFound();
            }
            ViewData["AssemblyID"] = new SelectList(_context.PreAssemblyModel, "PreAssemblyModelID", "PreAssemblyModelID", @assembly.ID);
            return View(@assembly);
        }

        // POST: Assemblies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AssemblyID,Path,Name")] SldAssembly @assembly)
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
            ViewData["AssemblyID"] = new SelectList(_context.PreAssemblyModel, "PreAssemblyModelID", "PreAssemblyModelID", @assembly.ID);
            return View(@assembly);
        }

        // GET: Assemblies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @assembly = await _context.SldAssembly
                .Include(@a => @a.PreAssembly)
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
            var @assembly = await _context.SldAssembly.FindAsync(id);
            _context.SldAssembly.Remove(@assembly);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssemblyExists(int id)
        {
            return _context.SldAssembly.Any(e => e.ID == id);
        }
    }
}
