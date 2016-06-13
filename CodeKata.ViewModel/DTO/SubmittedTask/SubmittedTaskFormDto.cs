using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CodeKata.Domain.Models;

namespace CodeKata.ViewModel.DTO.SubmittedTask
{
    public class SubmittedTaskFormDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public TaskType Type { get; set; }
        public int SubmittedById { get; set; }
    }
}