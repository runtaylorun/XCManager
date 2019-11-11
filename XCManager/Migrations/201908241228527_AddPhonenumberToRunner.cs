namespace XCManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPhonenumberToRunner : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Runners", "PhoneNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Runners", "PhoneNumber");
        }
    }
}
