using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeKata.ViewModel.DTO.SubmittedTask
{
    public class SubmittedTaskDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string FileURL { get; set; }
    }
}