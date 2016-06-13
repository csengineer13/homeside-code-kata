using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeKata.ViewModel.Common
{
    public class Notification
    {
        public Notification(int statusCode, string status)
        {
            this.StatusCode = statusCode;
            this.Status = status;
        }
        public int StatusCode { get; set; }
        public string Status { get; set; }
        public string Message => this.Status;
    }
}