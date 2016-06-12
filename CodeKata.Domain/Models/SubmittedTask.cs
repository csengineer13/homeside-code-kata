using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeKata.Domain.Models
{
    public enum TaskStatus
    {
        Queued,
        Processing,
        Finished,
        Error
    }
    public enum TaskType
    {
        PayMIP, 
        PostTransactions,
        CreateWarehouseLineFile
    }

    public class SubmittedTask
    {
        // Val
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TaskStatus Status { get; set; }
        public TaskType Type { get; set; }
        public DateTime SubmitDateTime { get; set; }
        public DateTime? QueuedDateTime { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public DateTime LastUpdatedDateTime { get; set; }

        // Ref
        public virtual User SubmittedBy { get; set; }
        public virtual User LastUpdatedBy { get; set; }
        public virtual Attachment Attachment { get; set; }

        // List
        //public virtual ICollection<TestModel2> TestModel2s { get; set; } 
    }
}