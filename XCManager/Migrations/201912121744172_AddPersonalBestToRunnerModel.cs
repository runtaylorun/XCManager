namespace XCManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPersonalBestToRunnerModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Runners", "PersonalBest", c => c.Time(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Runners", "PersonalBest");
        }
    }
}
