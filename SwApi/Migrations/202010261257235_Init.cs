namespace SwApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SldAssembly",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        Path = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PreAssemblyModel", t => t.ID)
                .Index(t => t.ID);
            
            CreateTable(
                "dbo.EquationSetter",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Label = c.String(),
                        IsActivated = c.Boolean(nullable: false),
                        EquationTarget = c.Int(nullable: false),
                        DefaultValue = c.Int(nullable: false),
                        MinValue = c.Int(nullable: false),
                        MaxValue = c.Int(nullable: false),
                        StepValue = c.Int(nullable: false),
                        ReferenceTag = c.String(),
                        SldAssemblyID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.SldAssembly", t => t.SldAssemblyID)
                .Index(t => t.SldAssemblyID);
            
            CreateTable(
                "dbo.PartToggle",
                c => new
                    {
                        PartToggleId = c.Int(nullable: false, identity: true),
                        Label = c.String(),
                        SldAssembly_ID = c.Int(),
                    })
                .PrimaryKey(t => t.PartToggleId)
                .ForeignKey("dbo.SldAssembly", t => t.SldAssembly_ID)
                .Index(t => t.SldAssembly_ID);
            
            CreateTable(
                "dbo.PartToggleTarget",
                c => new
                    {
                        PartToggleTargetId = c.Int(nullable: false, identity: true),
                        Label = c.String(),
                        Target = c.Int(nullable: false),
                        Reference = c.String(),
                        PartToggleId = c.Int(),
                    })
                .PrimaryKey(t => t.PartToggleTargetId)
                .ForeignKey("dbo.PartToggle", t => t.PartToggleId)
                .Index(t => t.PartToggleId);
            
            CreateTable(
                "dbo.PreAssemblyModel",
                c => new
                    {
                        PreAssemblyModelID = c.Int(nullable: false, identity: true),
                        Path = c.String(),
                        Name = c.String(),
                        CreationDate = c.DateTime(nullable: false),
                        MainFileName = c.String(),
                        _equationLabels = c.String(),
                        _partsLabels = c.String(),
                    })
                .PrimaryKey(t => t.PreAssemblyModelID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SldAssembly", "ID", "dbo.PreAssemblyModel");
            DropForeignKey("dbo.PartToggle", "SldAssembly_ID", "dbo.SldAssembly");
            DropForeignKey("dbo.PartToggleTarget", "PartToggle_PartToggleId", "dbo.PartToggle");
            DropForeignKey("dbo.EquationSetter", "SldAssemblyID", "dbo.SldAssembly");
            DropIndex("dbo.PartToggleTarget", new[] { "PartToggle_PartToggleId" });
            DropIndex("dbo.PartToggle", new[] { "SldAssembly_ID" });
            DropIndex("dbo.EquationSetter", new[] { "SldAssemblyID" });
            DropIndex("dbo.SldAssembly", new[] { "ID" });
            DropTable("dbo.PreAssemblyModel");
            DropTable("dbo.PartToggleTarget");
            DropTable("dbo.PartToggle");
            DropTable("dbo.EquationSetter");
            DropTable("dbo.SldAssembly");
        }
    }
}
