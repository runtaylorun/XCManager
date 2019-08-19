namespace XCManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRunnerDbset : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Runners",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Grade = c.Byte(nullable: false),
                        Email = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Runners");
        }
    }
}
