using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


using SwApi.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace SwApi.DAL
{
    public class SwApiContext: DbContext
    {
        public SwApiContext(): base("SwApiContext")
        {

        }

        public DbSet<PreAssemblyModel> PreAssemblies { get; set; }
        public DbSet<AssemblyEquationOption> AssemblyEquationOptions { get; set; }

        public DbSet<Assembly> Assemblies { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
