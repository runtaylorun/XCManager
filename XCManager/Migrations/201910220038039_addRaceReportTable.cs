namespace XCManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addRaceReportTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RaceReportBinaries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        Data = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.RaceReportBinaries");
        }
    }
}
