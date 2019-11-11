namespace XCManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateRaces : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Races (RaceName, Date, Distance, Location) VALUES (\'Tiger Invitational\', 8/27/2019, \'5K\', \'Warsaw IN\')");
            Sql("INSERT INTO Races (RaceName, Date, Distance, Location) VALUES (\'Marion Invitational\', 8/31/2019, \'5K\', \'Marion IN\')");
            Sql("INSERT INTO Races (RaceName, Date, Distance, Location) VALUES (\'Wawasee Dual Meet\', 9/5/2019, \'5K\', \'Wawasee IN\')");
            Sql("INSERT INTO Races (RaceName, Date, Distance, Location) VALUES (\'New Prarie Invitational\', 9/15/2019, \'5K\', \'New Prarie IN\')");
            Sql("INSERT INTO Races (RaceName, Date, Distance, Location) VALUES (\'Culver Academy Invitational\', 9/23/2019, \'5K\', \'Culver IN\')");
        }
        
        public override void Down()
        {
        }
    }
}
