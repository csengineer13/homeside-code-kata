using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using CodeKata.Common;
using CodeKata.Domain;
using CodeKata.Domain.Models;
using CodeKata.ViewModel;
using CodeKata.ViewModel.Common;
using CodeKata.ViewModel.DTO;
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
        public ActionResult SubmitTask(HttpPostedFileBase fileToUpload, SubmittedTaskFormDto submittedTaskForm)
        {
            var fileExists = fileToUpload != null && fileToUpload.ContentLength > 0;
            if (!fileExists)
            {
                return new JsonNetResult
                {
                    Data = new Notification(400, "Bad Request! Upload Failed")
                };
            }

            // todo: combine these into a single map? Or..
            // todo: Easier to follow logic when separate?
            var newAttachment = _mapper.Map<Attachment>(fileToUpload);
            var newTask = _mapper.Map<SubmittedTask>(submittedTaskForm);
            newTask.Attachment = newAttachment;

            using (var context = new CodeKataContext())
            {
                // Associate with user
                var matchedUser = context.Users.Single(usr => usr.Id == submittedTaskForm.SubmittedById);
                newTask.SubmittedBy = matchedUser;
                newTask.LastUpdatedBy = matchedUser;

                // Tell EF that this is an existing obj
                context.Users.Attach(newTask.SubmittedBy);
                context.Users.Attach(newTask.LastUpdatedBy);

                // Add & Update
                context.SubmittedTasks.Add(newTask);
                context.SaveChanges();
            }

            return new JsonNetResult
            {
                Data = new Notification(200, "Task submitted successfully!")
            };
        }


        //
        // GET: /Home/GetUsers
        [HttpGet]
        public ActionResult GetUsers(string searchTerm = "", int page = 1)
        {
            List<User> allUsers;
            using (var context = new CodeKataContext())
            {
                allUsers = context.Users
                    .Where(usr => (usr.FirstName + " " + usr.LastName + " " + usr.EmployeeId).Contains(searchTerm))
                    .OrderBy(usr => usr.FirstName)
                    .ToList();
            }

            var returnDictionary = new Dictionary<string, dynamic>
            {
                {"items", _mapper.Map<List<User>, List<UserSearchDto>>(allUsers) },
                {"total_count", allUsers.Count() }
            };

            return new JsonNetResult { Data = returnDictionary };
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
                    .Include("Attachment")
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