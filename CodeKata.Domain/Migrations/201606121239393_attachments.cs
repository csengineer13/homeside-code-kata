namespace CodeKata.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class attachments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attachment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        FileType = c.String(),
                        FileContent = c.Binary(),
                        CreatedDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.SubmittedTask", "Attachment_Id", c => c.Int());
            CreateIndex("dbo.SubmittedTask", "Attachment_Id");
            AddForeignKey("dbo.SubmittedTask", "Attachment_Id", "dbo.Attachment", "Id");
            DropColumn("dbo.SubmittedTask", "FilePathURL");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SubmittedTask", "FilePathURL", c => c.String());
            DropForeignKey("dbo.SubmittedTask", "Attachment_Id", "dbo.Attachment");
            DropIndex("dbo.SubmittedTask", new[] { "Attachment_Id" });
            DropColumn("dbo.SubmittedTask", "Attachment_Id");
            DropTable("dbo.Attachment");
        }
    }
}
