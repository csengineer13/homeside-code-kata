using System;
using System.Collections.Generic;

namespace CodeKata.Domain.Models
{
    public class TestModel
    {
        public int Id { get; set; }
        public string Name { get; set; }


        // Val
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDateTime { get; set; }

        // Ref
        public virtual ICollection<TestModel2> TestModel2s { get; set; } 
    }
}