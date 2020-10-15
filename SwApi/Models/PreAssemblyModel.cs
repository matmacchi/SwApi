using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SwApi.Models
{
    public class PreAssemblyModel
    {
        [ForeignKey("Assembly")]
        public int ID { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual Assembly Assembly { get; set; }



        public string getStatus()
        {
            string status = "empty";



            return status;
        }



    }
}



