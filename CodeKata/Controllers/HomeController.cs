using System.Collections.Generic;
using System.Linq;
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

        //
        // GET: /SubmittedTasks
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