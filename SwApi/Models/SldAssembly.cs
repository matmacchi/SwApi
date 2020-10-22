using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SwApi.Models
{
    public class SldAssembly
    {
        [ForeignKey("PreAssembly")]
        public int ID { get; set; }

        public string Path { get; set; }

        public string Name { get; set; }

        public virtual PreAssemblyModel PreAssembly { get; set; }

        public ICollection<EquationSetter> Equations { get; set; }

        public ICollection<PartToggle> PartToggles { get; set; }

        public SldAssembly()
        {
            this.PartToggles = new HashSet<PartToggle>();
            this.Equations = new HashSet<EquationSetter>();
        }
    }
}
