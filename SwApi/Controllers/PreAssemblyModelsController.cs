using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using SwApi.Data;
using SwApi.Models;

using Microsoft.EntityFrameworkCore.Metadata.Internal;

using System.IO;
using System.IO.Compression;
using Microsoft.AspNetCore.Routing.Constraints;
using System.Diagnostics;
//using SwApi.Migrations;
using PreAssemblyModel = SwApi.Models.PreAssemblyModel;
using Microsoft.VisualStudio.Web.CodeGeneration;
using Microsoft.Extensions.Hosting;
using System.Data;

namespace SwApi.Controllers
{
    public class PreAssemblyModelsController : Controller
    {
        private readonly SwApiContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly SwApiManager _swApiManager;

        public PreAssemblyModelsController(SwApiContext context,IWebHostEnvironment environment, SwApiManager swApiManager)
        {
            _context = context;
            _environment = environment;
            _swApiManager = swApiManager;

        }

        // GET: PreAssemblyModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.PreAssemblyModel.ToListAsync());
        }

        // GET: PreAssemblyModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var preAssemblyModel = await _context.PreAssemblyModel
                .FirstOrDefaultAsync(m => m.PreAssemblyModelID == id);
            if (preAssemblyModel == null)
            {
                return NotFound();
            }

            return View(preAssemblyModel);
        }

        // GET: PreAssemblyModels/Create
        public IActionResult Create(PreAssemblyViewModel modelView)
        {


           //modelView.MainFileName = modelView.MainFileName.Select(m => new SelectListItem());
           //modelView.MainFileName = new SelectList(Enumerable.Empty<SelectListItem>());


            return View(modelView);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PreAssemblyViewModel modelView,List<IFormFile> assemblyFiles)
        {
            string rootFilePath = "";


            if (modelView.AssemblyFiles != null)
            {
               string sldasmFile = CheckAssemblyFiles(assemblyFiles);

                if (sldasmFile == null)
                {
                    ModelState.AddModelError("AssemblyFiles", "Assembly files should contain only 1 sldasm file");
                    return View(modelView);
                }


                var uploads = Path.Combine(_environment.WebRootPath, "uploads");
                rootFilePath = Path.Combine(uploads, modelView.Name);
                Directory.CreateDirectory(rootFilePath);
                foreach(IFormFile file in assemblyFiles)
                {
                    var filePath = Path.Combine(rootFilePath, file.FileName);
                    file.CopyTo(new FileStream(filePath, FileMode.Create));
                }

            }

            var model = new PreAssemblyModel
            {
                Name = modelView.Name,
                Path = rootFilePath,
                CreationDate = DateTime.Now,
                Assembly = null,
                MainFileName = modelView.MainFileName
            };



            // to do  : Return something
            if (ModelState.IsValid)
            {
                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        private bool CheckMissingFiles(string assemblyFile)
        {

            _swApiManager.CheckMissingFiles(assemblyFile);
            Debugger.Break();

            return true;
        }

        private string CheckAssemblyFiles(List<IFormFile> assemblyFiles)
        {

            //string[] assemblyFileNames = Directory.GetFiles(preAssembly.Path.ToString());
            int numberOfSldasm = 0;
            string sldasmFile = "";

            foreach(var file in assemblyFiles)
            {

                if (Path.GetExtension(file.FileName).ToLower() == ".sldasm")
                {
                    numberOfSldasm++;
                    sldasmFile = file.FileName;
                }
            }



            if(numberOfSldasm == 1)
            {
                return sldasmFile;
            }

            else
            {
                return null;
            }

        }
 




        // GET: PreAssemblyModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var preAssemblyModel = await _context.PreAssemblyModel.FindAsync(id);
            if (preAssemblyModel == null)
            {
                return NotFound();
            }
            return View(preAssemblyModel);
        }

        // POST: PreAssemblyModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Path,Name,CreationDate")] PreAssemblyModel preAssemblyModel)
        {
            if (id != preAssemblyModel.PreAssemblyModelID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(preAssemblyModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PreAssemblyModelExists(preAssemblyModel.PreAssemblyModelID))
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
            return View(preAssemblyModel);
        }

        // GET: PreAssemblyModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var preAssemblyModel = await _context.PreAssemblyModel
                .FirstOrDefaultAsync(m => m.PreAssemblyModelID == id);
            if (preAssemblyModel == null)
            {
                return NotFound();
            }

            return View(preAssemblyModel);
        }

        // POST: PreAssemblyModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var preAssemblyModel = await _context.PreAssemblyModel.FindAsync(id);
            //_context.Assembly.Remove(preAssemblyModel.Assembly);
            _context.PreAssemblyModel.Remove(preAssemblyModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PreAssemblyModelExists(int id)
        {
            return _context.PreAssemblyModel.Any(e => e.PreAssemblyModelID == id);
        }
    }
}
