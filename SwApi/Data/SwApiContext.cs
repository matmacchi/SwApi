using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SwApi.Models;

namespace SwApi.Data
{
    public class SwApiContext : DbContext
    {
        public SwApiContext (DbContextOptions<SwApiContext> options)
            : base(options)
        {
        }

        public DbSet<SwApi.Models.PreAssemblyModel> PreAssemblyModel { get; set; }

        public DbSet<SwApi.Models.EquationSetter> EquationSetter { get; set; }

        public DbSet<SwApi.Models.SldAssembly> SldAssembly { get; set; }

        public DbSet<SwApi.Models.PartToggle> PartToggle { get; set; }

        public DbSet<SwApi.Models.PartToggleTarget> PartToggleTarget { get; set; }
    }
}
