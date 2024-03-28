using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjectManagement.Models;
using ProjectManagement.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProjectRepo _repo;
        public HomeController(ILogger<HomeController> logger,IProjectRepo repo)
        {
            _logger = logger;
            _repo = repo;
        }

        public IActionResult Index()
        {

            return View(_repo.GetProjectlst());
        }
        public IActionResult Create()
        {
            Project _pro = new Project();
            _pro.List_ProjectType = _repo.GetProjectTypelst();
            return View(_pro);
        }
        [HttpPost]
        public IActionResult Create(Project pro)
        {
            if (ModelState.IsValid)
            {
                TempData["msg"] = _repo.SaveProject(pro);
            }
            else
            {
                TempData["msg"] = ModelState.ErrorCount;
                pro.List_ProjectType = _repo.GetProjectTypelst();
                return View(pro);
            }
            return RedirectToAction("Index");
        }
            public IActionResult Type()
        {
            return View(_repo.GetProjectTypelst());
        }
        [HttpPost]
        public IActionResult TypeAdd(ProjectType _type)
        {
            if (ModelState.IsValid)
            {
                TempData["msg"] = _repo.SaveProjectType(_type);
            }
            else
            {
                TempData["msg"] = ModelState.ErrorCount;
            }
            return RedirectToAction("Type");
        }
        [HttpPost]
        public IActionResult AddTask(ProjectTask _task)
        {
            TempData["msg"] = _repo.AddTask(_task);
            return RedirectToAction("Details",new { id= _task.ProjectID });
        }
        public IActionResult Resource()
        {
            
            return View(_repo.GetResource());
        }
        [HttpPost]
        public IActionResult Resource(Resource _obj)
        {
            TempData["msg"] = _repo.AddResource(_obj);
            return View(_repo.GetResource());
        }
        public IActionResult Details(int id)
        {
            Project _obj = GetProjectByID(id);
            return View(_obj);
        }
        public Project GetProjectByID(int id)
        {
            Project _obj = _repo.GetProjectlst().FirstOrDefault(x => x.ProjectID == id);
            _obj.List_ProjectTask = _repo.GetProjectTask().FindAll(x => x.ProjectID == id);
            _obj.List_ResourceAssign = _repo.GetResourceAssigns(id);
            _obj.List_ResourceNotAssign = _repo.GetResourceNotAssigns(id);
            _obj.List_TaskAssignResource = _repo.GetTaskAssignResources(id);
            return _obj;
        }
        [HttpPost]
        public IActionResult AssignResource(Project _project)
        {
            foreach(var item in _project.List_ResourceNotAssign.FindAll(x=>x.Status==true))
            {
                ResourceAssign ra = new ResourceAssign();
                ra.ProjectID = _project.ProjectID;
                ra.ResourceID = item.ResourceID;
                _repo.AddResourceToProject(ra);
            }
            return RedirectToAction("Details", new { id = _project.ProjectID });
        }
        public IActionResult Assign(int id)
        {
            Project _obj = GetProjectByID(id);
            return View(_obj);
        }
        [HttpPost]
        public IActionResult Assign(TaskAssignResource _assign)
        {
            TempData["msg"] = _repo.SaveTaskAssignToResource(_assign);
            Project _obj = GetProjectByID(_assign.ProjectID);
            return View(_obj);
        }
        [HttpPost]
        public IActionResult SaveActualTaskWork(ActualTaskWork _atw)
        {
            TempData["msg"] = _repo.SaveActualTaskWork(_atw);
          
            return RedirectToAction("Assign",new {id=_atw.ProjectID });
        }
            public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
