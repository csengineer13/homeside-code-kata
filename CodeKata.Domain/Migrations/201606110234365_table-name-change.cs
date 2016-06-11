namespace CodeKata.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tablenamechange : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.TestModel2");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TestModel2",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        TestEnum = c.Int(),
                        CreatedBy = c.String(),
                        CreatedDateTime = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
    }
}
