namespace SNAuthentication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReferredBy : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ReferredBy", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "ReferredBy");
        }
    }
}
