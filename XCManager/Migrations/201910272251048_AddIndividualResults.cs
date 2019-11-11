namespace XCManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIndividualResults : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.IndividualResults",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        finishingTime = c.Time(nullable: false, precision: 7),
                        Place = c.Int(nullable: false),
                        runner_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Runners", t => t.runner_Id)
                .Index(t => t.runner_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IndividualResults", "runner_Id", "dbo.Runners");
            DropIndex("dbo.IndividualResults", new[] { "runner_Id" });
            DropTable("dbo.IndividualResults");
        }
    }
}
