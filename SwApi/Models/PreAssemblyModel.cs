using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SwApi.Models
{
    public class PreAssemblyModel
    {

        public int PreAssemblyModelID { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }

        public string MainFileName {get;set;}



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



