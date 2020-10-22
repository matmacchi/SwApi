using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SwApi.Data;
using System.Text.Json;
using System.Diagnostics;
using SwApi.Models;

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
        public JsonResult AddPartToggle(object form)
        {
            Debugger.Break();

            string data = "";
            return new JsonResult(data);
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
