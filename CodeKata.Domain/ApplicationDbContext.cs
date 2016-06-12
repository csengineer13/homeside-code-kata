using CodeKata.Domain.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace CodeKata.Domain
{
    public class CodeKataContext : DbContext
    {

        public CodeKataContext() : base("CodeKataContext")
        {
            // Stop EF from creating "proxy" objects when queried (breaks automapper)
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = true;
        }

        public DbSet<SubmittedTask> SubmittedTasks { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}