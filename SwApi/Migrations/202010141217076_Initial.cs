namespace SwApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Assembly",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Path = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.AssemblyEquationOption",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Type = c.String(),
                        EquationTarget = c.String(),
                        DefaultValue = c.Int(nullable: false),
                        MinValue = c.Int(nullable: false),
                        MaxValue = c.Int(nullable: false),
                        Assembly_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Assembly", t => t.Assembly_ID)
                .Index(t => t.Assembly_ID);
            
            CreateTable(
                "dbo.PreAssemblyModel",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        Path = c.String(),
                        Name = c.String(),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Assembly", t => t.ID)
                .Index(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PreAssemblyModel", "ID", "dbo.Assembly");
            DropForeignKey("dbo.AssemblyEquationOption", "Assembly_ID", "dbo.Assembly");
            DropIndex("dbo.PreAssemblyModel", new[] { "ID" });
            DropIndex("dbo.AssemblyEquationOption", new[] { "Assembly_ID" });
            DropTable("dbo.PreAssemblyModel");
            DropTable("dbo.AssemblyEquationOption");
            DropTable("dbo.Assembly");
        }
    }
}
