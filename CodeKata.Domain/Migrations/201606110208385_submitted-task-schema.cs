namespace CodeKata.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class submittedtaskschema : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TestModel2", "TestModel_Id", "dbo.TestModel");
            DropIndex("dbo.TestModel2", new[] { "TestModel_Id" });
            CreateTable(
                "dbo.SubmittedTask",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        FilePathURL = c.String(),
                        Status = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        SubmitDateTime = c.DateTime(nullable: false),
                        QueuedDateTime = c.DateTime(nullable: false),
                        StartDateTime = c.DateTime(nullable: false),
                        EndDateTime = c.DateTime(nullable: false),
                        LastUpdatedDateTime = c.DateTime(nullable: false),
                        LastUpdatedBy_Id = c.Int(),
                        SubmittedBy_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.LastUpdatedBy_Id)
                .ForeignKey("dbo.User", t => t.SubmittedBy_Id)
                .Index(t => t.LastUpdatedBy_Id)
                .Index(t => t.SubmittedBy_Id);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmployeeId = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        Phone = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.TestModel2", "TestModel_Id");
            DropTable("dbo.TestModel");
        }
        
        public override void Down()
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
            
            AddColumn("dbo.TestModel2", "TestModel_Id", c => c.Int());
            DropForeignKey("dbo.SubmittedTask", "SubmittedBy_Id", "dbo.User");
            DropForeignKey("dbo.SubmittedTask", "LastUpdatedBy_Id", "dbo.User");
            DropIndex("dbo.SubmittedTask", new[] { "SubmittedBy_Id" });
            DropIndex("dbo.SubmittedTask", new[] { "LastUpdatedBy_Id" });
            DropTable("dbo.User");
            DropTable("dbo.SubmittedTask");
            CreateIndex("dbo.TestModel2", "TestModel_Id");
            AddForeignKey("dbo.TestModel2", "TestModel_Id", "dbo.TestModel", "Id");
        }
    }
}
