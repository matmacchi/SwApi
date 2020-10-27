using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SwApi.Models
{
    public class PartToggleTarget
    {
        [Key]
        public int PartToggleTargetId { get; set; }
        public string Label { get; set; }

        public int Target { get; set; }

        public string Reference { get; set; }

    }
}
