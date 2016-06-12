namespace CodeKata.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nullabledatetimes : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SubmittedTask", "QueuedDateTime", c => c.DateTime());
            AlterColumn("dbo.SubmittedTask", "StartDateTime", c => c.DateTime());
            AlterColumn("dbo.SubmittedTask", "EndDateTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SubmittedTask", "EndDateTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.SubmittedTask", "StartDateTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.SubmittedTask", "QueuedDateTime", c => c.DateTime(nullable: false));
        }
    }
}
