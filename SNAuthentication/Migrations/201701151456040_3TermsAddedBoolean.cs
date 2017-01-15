namespace SNAuthentication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3TermsAddedBoolean : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "SuryaTermsAccept", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "GoldenLotusTermsAccept", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "EarthPeceTermsAccept", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "EarthPeceTermsAccept");
            DropColumn("dbo.AspNetUsers", "GoldenLotusTermsAccept");
            DropColumn("dbo.AspNetUsers", "SuryaTermsAccept");
        }
    }
}
