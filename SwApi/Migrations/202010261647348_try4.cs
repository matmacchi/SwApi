namespace SwApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class try4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SldAssembly", "Reference", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SldAssembly", "Reference");
        }
    }
}
