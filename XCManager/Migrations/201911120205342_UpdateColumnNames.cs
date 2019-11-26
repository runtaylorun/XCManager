namespace XCManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateColumnNames : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.IndividualResults", new[] { "runner_Id" });
            CreateIndex("dbo.IndividualResults", "Runner_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.IndividualResults", new[] { "Runner_Id" });
            CreateIndex("dbo.IndividualResults", "runner_Id");
        }
    }
}
