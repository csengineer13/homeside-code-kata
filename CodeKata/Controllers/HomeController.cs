using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using CodeKata.Common;
using CodeKata.Domain;
using CodeKata.Domain.Models;
using CodeKata.ViewModel;
using CodeKata.ViewModel.DTO.SubmittedTask;

namespace CodeKata.Controllers
{
    public class HomeController : Controller
    {
        private static IMapper _mapper;
        public HomeController()
        {
            _mapper = AutoMapperConfig.MapperConfiguration.CreateMapper();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SubmitTask(HttpPostedFileBase fileToUpload, SubmittedTaskFormDto submittedTaskForm)
        {
            var fileExists = fileToUpload != null && fileToUpload.ContentLength > 0;
            if (!fileExists)
            {
                return Json(new
                {
                    statusCode = 400,
                    status = "Bad Request! Upload Failed",
                    file = string.Empty
                }, JsonRequestBehavior.AllowGet);
            }

            // todo: combine these into a single map? Or..
            // todo: Easier to follow logic when separate?
            var newAttachment = _mapper.Map<Attachment>(fileToUpload);
            var newTask = _mapper.Map<SubmittedTask>(submittedTaskForm);
            newTask.Attachment = newAttachment;

            using (var context = new CodeKataContext())
            {
                context.SubmittedTasks.Add(newTask);
                context.SaveChanges();
            }

            if (fileToUpload.FileName.Length > 0)
            {
                return Json(new
                {
                    statusCode = 200,
                    status = fileToUpload.FileName + " was uploaded!"
                }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(new
                {
                    statusCode = 400,
                    status = "error encountered",
                    file = fileToUpload.FileName
                }, JsonRequestBehavior.AllowGet);

            }
        }


        //
        // GET: /Home/SubmittedTasks
        [HttpGet]
        public ActionResult SubmittedTasks()
        {
            List<SubmittedTask> allSubmittedTasks;
            using (var context = new CodeKataContext())
            {
                allSubmittedTasks = context.SubmittedTasks
                    .Include("LastUpdatedBy")
                    .Include("SubmittedBy")
                    .ToList();
            }

            var returnDictionary = new Dictionary<string, List<SubmittedTaskDto>>
            {
                {"data", _mapper.Map<List<SubmittedTask>, List<SubmittedTaskDto>>(allSubmittedTasks)}
            };

            return new JsonNetResult { Data = returnDictionary };
        }
    }
}