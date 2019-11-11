namespace XCManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRaceIdToReportBinary : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RaceReportBinaries", "RaceId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RaceReportBinaries", "RaceId");
        }
    }
}
