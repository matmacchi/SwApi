using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwApi.Models
{
    public class PreAssemblyViewModel
    {
        public int PreAssemblyID { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public List<IFormFile> AssemblyFiles { get; set; }



    }
}
