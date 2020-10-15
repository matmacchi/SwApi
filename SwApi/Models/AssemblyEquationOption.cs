using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwApi.Models
{
    public class AssemblyEquationOption
    {
        public int ID { get; set; }
        public string Name { get; set; }
        
        public string Type { get; set; }

        public string EquationTarget { get; set; }

        public int DefaultValue { get; set; }

        public int MinValue { get; set; }

        public int MaxValue { get; set; }

    }
}
