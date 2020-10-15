using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwApi.Models
{
    public class Assembly
    {
        public int ID { get; set; }

        public string Path { get; set; }

        public string Name { get; set; }

        public virtual PreAssemblyModel PreAssembly { get; set; }

        public List<AssemblyEquationOption> AssemblyEquationOptions { get; set; }
    }
}
