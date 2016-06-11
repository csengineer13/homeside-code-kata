using System;
using System.Collections.Generic;

namespace CodeKata.Domain.Models
{

    public enum TestEnum
    {
        Foo, Bar, Lorem, Ipsum
    }

    public class TestModel2
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TestEnum? TestEnum { get; set; }


        // Val
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDateTime { get; set; }
    }
}