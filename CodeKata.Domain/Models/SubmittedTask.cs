using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public DateTime LastUpdatedDateTime { get; set; }

        // Ref
        public virtual User SubmittedBy { get; set; }
        public virtual User LastUpdatedBy { get; set; }
        public virtual Attachment Attachment { get; set; }

        // List
        //public virtual ICollection<TestModel2> TestModel2s { get; set; } 


        // WinService Methods
        public static List<SubmittedTask> GetTasksByStatus(TaskStatus taskStatus)
        {
            using (var context = new CodeKataContext())
            {
                return context.SubmittedTasks
                    .Include("LastUpdatedBy")
                    .Include("SubmittedBy")
                    .Where(tsk => tsk.Status == taskStatus)
                    .ToList();
            }
        }

        public static SubmittedTask GetTaskById(int id)
        {
            using (var context = new CodeKataContext())
            {
                return context.SubmittedTasks.Single(tsk => tsk.Id == id);
            }
        }

        public void UpdateExistingTask()
        {
            using (var context = new CodeKataContext())
            {
                // https://msdn.microsoft.com/en-us/data/jj592676
                context.Entry(this).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}