using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwApi.Models
{
    public class PreAssemblyViewModel
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public List<IFormFile> AssemblyFiles { get; set; }

        public string MainFileName { get; set; }




    }
}
