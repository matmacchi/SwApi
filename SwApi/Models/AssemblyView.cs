using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SwApi.Models
{
    public class AssemblyView
    {
        public int AssemblyID { get; set; }

        public IEnumerable<SelectListItem> PreAssemblyChoices { get; set; }

        public int selectedPreAssemblyId { get; set; }

        public string Path { get; set; }

        public string Name { get; set; }


        public List<int> AssemblyEquationOptionIds { get; set; }

        public List<string> EquationLabels { get; set; }

        public List<string> PartsLabels { get; set; }

    }

}
