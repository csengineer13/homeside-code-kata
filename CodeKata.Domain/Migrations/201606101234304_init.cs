namespace CodeKata.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TestModel",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CreatedBy = c.String(),
                        CreatedDateTime = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
                        TestModel_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TestModel", t => t.TestModel_Id)
                .Index(t => t.TestModel_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TestModel2", "TestModel_Id", "dbo.TestModel");
            DropIndex("dbo.TestModel2", new[] { "TestModel_Id" });
            DropTable("dbo.TestModel2");
            DropTable("dbo.TestModel");
        }
    }
}
