using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SwApi.Models
{
    public class EquationSetter
    {
        public int ID { get; set; }
        public string Label { get; set; }

        public bool IsActivated { get; set; }

        public int EquationTarget { get; set; }

        public int DefaultValue { get; set; }

        public int MinValue { get; set; }

        public int MaxValue { get; set; }

        public int StepValue { get; set; }

        public string ReferenceTag { get; set; }

        [ForeignKey("SldAssembly")]
        public int? SldAssemblyID { get; set; }

        public SldAssembly SldAssembly { get; set; }



    }


}
