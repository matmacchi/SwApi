using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace SwApi.Models
{
    public class PreAssemblyModel
    {

        public int PreAssemblyModelID { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }

        public string MainFileName { get; set; }

        public string _equationLabels { get; set; }

        public string _partsLabels { get; set; }

        [NotMapped]
        public List<string> EquationLabels { 
            get{
                return _equationLabels.Split(';').ToList();
            }
            set { _equationLabels = string.Join(";", value); }
            }

        [NotMapped]
        public List<string> PartLabels
        {
            get
            {
                return _partsLabels.Split(';').ToList();
            }
            set
            {
                _partsLabels = string.Join(";", value);
            }
        }



        public virtual SldAssembly Assembly { get; set; }



        public string GetStatus()
        {
            string status = "empty";



            return status;
        }

        public string GetMainFilePath()
        {
            string mainFilePath = System.IO.Path.Combine(Path, MainFileName);

            return mainFilePath;
        }



    }
}



