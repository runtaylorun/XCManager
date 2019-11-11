namespace XCManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRaceToIndividualResult : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.IndividualResults", "Race_Id", c => c.Int());
            CreateIndex("dbo.IndividualResults", "Race_Id");
            AddForeignKey("dbo.IndividualResults", "Race_Id", "dbo.Races", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IndividualResults", "Race_Id", "dbo.Races");
            DropIndex("dbo.IndividualResults", new[] { "Race_Id" });
            DropColumn("dbo.IndividualResults", "Race_Id");
        }
    }
}
