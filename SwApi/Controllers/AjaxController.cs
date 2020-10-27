using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SwApi.Data;
using System.Text.Json;
using System.Diagnostics;
using SwApi.Models;
using System.Collections;

namespace SwApi.Controllers
{
    public class AjaxController : Controller
    {

        private readonly SwApiContext _context;
        private readonly SwApiManager _swApiMng;

        public AjaxController(SwApiContext context, SwApiManager swApiMng)
        {
            _context = context;
            _swApiMng = swApiMng;
        }

        [HttpPost]
        public JsonResult AddEquationSetter(string Label,int EquationTarget,int MinValue,int MaxValue,int StepValue, string ReferenceTag)
        {
            EquationSetter equation = new EquationSetter()
            {
                Label = Label,
                EquationTarget = EquationTarget,
                MinValue = MinValue,
                MaxValue = MaxValue,
                StepValue = StepValue,
                ReferenceTag = ReferenceTag,
                SldAssemblyID = null,
            };

            _context.EquationSetter.Add(equation);
            _context.SaveChanges();

            return new JsonResult(new { id = equation.ID, label = equation.Label });
        }

        [HttpPost]
        public JsonResult AddPartTarget(string Label, int PartTarget, string Reference)
        {
            PartToggleTarget target = new PartToggleTarget()
            {
                Label = Label,
                Target = PartTarget,
                Reference = Reference
            };

            _context.PartToggleTarget.Add(target);
            _context.SaveChanges();

            return new JsonResult(new { id = target.PartToggleTargetId, label = target.Label });
        }

        [HttpPost]
        public JsonResult AddPartToggle(string Label,List<int> PartToggleTargets,string ReferenceTag)
        {

            List<PartToggleTarget> partToggleTargets = new List<PartToggleTarget>();

            foreach(var partToggleTarget in PartToggleTargets)
            {
                partToggleTargets.Add(_context.PartToggleTarget.Find(partToggleTarget));
            }

            PartToggle partToggle = new PartToggle()
            {
                Label = Label,
                PartTargets = partToggleTargets,
                ReferenceTag = ReferenceTag
            };

            _context.PartToggle.Add(partToggle);
            _context.SaveChanges();


            return new JsonResult(new { id = partToggle.PartToggleId , label = partToggle.Label });
        }

        [HttpPost]
        public JsonResult GetEquationTargets(int? PreAssemblyId = null)
        {

            if (PreAssemblyId == null)
            {
                return new JsonResult(new { });
            }

            var preAssembly = _context.PreAssemblyModel.Find(PreAssemblyId);

            return new JsonResult(preAssembly.EquationLabels);
        }

        [HttpPost]
        public JsonResult GetPartTargets(int? PreAssemblyId = null)
        {
            if(PreAssemblyId == null)
            {
                return new JsonResult(new { });
            }


            var preAssembly = _context.PreAssemblyModel.Find(PreAssemblyId);

            

            return new JsonResult(preAssembly.PartLabels);
        }

        [HttpPost]
        public JsonResult GetAvailableMainFileOptions(int preAssemblyId)
        {
            
            List<string> availableEquations;
            string jsonData ="";
            var preAssembly = _context.PreAssemblyModel.Find(preAssemblyId);


            Debug.WriteLine(preAssemblyId);
            //Debugger.Break();



            if (preAssembly != null)
            {
                availableEquations = _swApiMng.GetAvailableEquation(preAssembly.GetMainFilePath());
                jsonData = JsonSerializer.Serialize(availableEquations);
            }



            //Debugger.Break(); 


            return Json(new { data = jsonData } );
        }

        
    }
}
