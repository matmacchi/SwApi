namespace SwApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.EquationSetter", name: "SldAssembly_ID", newName: "SldAssemblyID");
            RenameIndex(table: "dbo.EquationSetter", name: "IX_SldAssembly_ID", newName: "IX_SldAssemblyID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.EquationSetter", name: "IX_SldAssemblyID", newName: "IX_SldAssembly_ID");
            RenameColumn(table: "dbo.EquationSetter", name: "SldAssemblyID", newName: "SldAssembly_ID");
        }
    }
}
