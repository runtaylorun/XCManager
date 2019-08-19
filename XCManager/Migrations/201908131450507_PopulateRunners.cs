namespace XCManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateRunners : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Runners (Name, Grade, Email) VALUES (\'Jacob Denzer\', 12, \'jdenzer@warsawschools.org\')");
            Sql("INSERT INTO Runners (Name, Grade, Email) VALUES (\'Austin Fleming\', 12, \'AFleming@warsawschools.org\')");
            Sql("INSERT INTO Runners (Name, Grade, Email) VALUES (\'Konnor Fleming\', 10, \'KFleming@warsawschools.org\')");
            Sql("INSERT INTO Runners (Name, Grade, Email) VALUES (\'Sam Lechlightner\', 12, \'SLechlightner@warsawschools.org\')");
            Sql("INSERT INTO Runners (Name, Grade, Email) VALUES (\'Tanner Stiver\', 11, \'TStiver@warsawschools.org\')");
            Sql("INSERT INTO Runners (Name, Grade, Email) VALUES (\'Harrison Phipps\', 10, \'HPhipps@warsawschools.org\')");
        }
        
        public override void Down()
        {
        }
    }
}
