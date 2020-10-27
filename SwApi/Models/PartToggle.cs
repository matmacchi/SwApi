using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SwApi.Models
{
    public class PartToggle
    {

        private static readonly char delimiter = ';';

        [Key]
        public int PartToggleId { get; set; }

        //public ICollection<Assembly> Assemblies { get; set; }

        private string _parts;

        private string _partLabels;

        private string _partReferences;

        [NotMapped]
        public string[] Parts { 
            get { return _parts.Split(delimiter); }
            set { _parts = string.Join($"{delimiter}", value); }
        }

        [NotMapped]
        public string[] PartLabels {
            get { return _partLabels.Split(delimiter); }
            set { _parts = string.Join($"{delimiter}", value); }
        }

        [NotMapped]
        public string[] PartReference {
            get { return _partReferences.Split(delimiter); }
            set { _parts = string.Join($"{delimiter}", value); }
        }


        public string Label { get; set; }


        public string ReferenceTag;

        public ICollection<PartToggleTarget> PartTargets { get; set; }

        public PartToggle()
        {
            PartTargets = new HashSet<PartToggleTarget>();
        }
    }
}
