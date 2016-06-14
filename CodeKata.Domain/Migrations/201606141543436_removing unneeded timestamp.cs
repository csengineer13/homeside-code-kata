namespace CodeKata.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removingunneededtimestamp : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.SubmittedTask", "QueuedDateTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SubmittedTask", "QueuedDateTime", c => c.DateTime());
        }
    }
}
