namespace TicketingSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixUserCountryColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "Country", c => c.Int(nullable: false));
            DropColumn("dbo.User", "City");
        }
        
        public override void Down()
        {
            AddColumn("dbo.User", "City", c => c.String(maxLength: 250));
            DropColumn("dbo.User", "Country");
        }
    }
}
