using System;

namespace CodeKata.Domain.Models
{
    public class Attachment
    {
        public Attachment()
        {
            
        }

        // Val
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        // EF, SQL, Storing Byte[]
        // http://stackoverflow.com/questions/25400555/save-and-retrieve-image-binary-from-sql-server-using-entity-framework-6
        public byte[] FileContent { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}