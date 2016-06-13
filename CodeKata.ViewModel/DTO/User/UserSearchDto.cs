using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeKata.ViewModel.DTO
{
    public class UserSearchDto
    {
        public int id { get; set; } // Has to be lowercase for select2
        public string Name { get; set; }
        public string EmployeeId { get; set; }
    }
}