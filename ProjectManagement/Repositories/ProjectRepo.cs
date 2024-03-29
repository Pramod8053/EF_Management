using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.Repositories
{
    public class ProjectRepo :IProjectRepo
    {
        private readonly DBContextManagessssaar _dbcontext;
        public ProjectRepo() {
            _dbcontext = new DBContextManagessssaar();
        }

        public string AddResource(Resource _obj)
        {
            try
            {
                if (_dbcontext.resources.ToList().FindAll(x => x.Name == _obj.Name).Count > 0)
                {
                    return "Resource exists";
                }
                else
                {
                    _dbcontext.resources.Add(_obj);
                    _dbcontext.SaveChanges();
                    return "Resource Added";
                }
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public string AddTask(ProjectTask _task)
        {
            try
            {
                if(_dbcontext.projectTasks.ToList().FindAll(x=>x.TaskName == _task.TaskName && x.ProjectID==_task.ProjectID).Count > 0)
                {
                    return "Task exists";
                }
                else
                {
                    _dbcontext.projectTasks.Add(_task);
                    _dbcontext.SaveChanges();
                    return "Record Added";
                }
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public List<Project> GetProjectlst()
        {
            List<Project> _prolst = new List<Project>();
            var project = from pro in _dbcontext.project.ToList()
                                    join 
                                    typ in _dbcontext.projectTypes.ToList() on pro.ProjectTypeID equals typ.ProjectTypeID
                                    select new Project { 
                                    ProjectID=pro.ProjectID,
                                    Name=pro.Name,
                                    TypeName = typ.TypeName,
                                    ProjectTypeID=pro.ProjectTypeID,
                                    Client=pro.Client,
                                    StartDate = pro.StartDate,
                                    EndDate=pro.EndDate
                                    };
            
                _prolst = project.ToList();
            

            return _prolst;
        }

        public List<ProjectTask> GetProjectTask()
        {
            var projectT = from pt in _dbcontext.projectTasks.ToList()
                           join tar in _dbcontext.taskAssignResources.ToList() on pt.ProjectTaskID equals tar.ProjectTaskID into tarGroup
                           from ta in tarGroup.DefaultIfEmpty()
                            select new 
                           {
                               ProjectID = pt.ProjectID,
                               ProjectTaskID = pt.ProjectTaskID,
                               TaskName = pt.TaskName,
                               StartDate = pt.StartDate,
                               EndDate = pt.EndDate,
                               TaskTime = pt.TaskTime,
                               ActualTime = pt.ActualTime,
                               status = pt.status,
                               TaskAssignResourceID = (ta != null) ? ta.TaskAssignResourceID : 0,

                           };
            var ActualTaskTime = _dbcontext.actualTaskWorks.ToList();
            var projectTasklist = from prot in projectT.ToList()
                                  join atw in _dbcontext.actualTaskWorks.ToList() on prot.TaskAssignResourceID equals atw.TaskAssignResourceID into atwGroup
                                  from at in atwGroup.DefaultIfEmpty()
                                  group new { prot, at }
                                  by new { prot.ProjectID, prot.ProjectTaskID, prot.TaskName, prot.StartDate, prot.EndDate, prot.TaskTime } into grouped
                                  select new ProjectTask
                                  {
                                      ProjectID = grouped.Key.ProjectID,
                                      ProjectTaskID = grouped.Key.ProjectTaskID,
                                      TaskName = grouped.Key.TaskName,
                                      StartDate = grouped.Key.StartDate,
                                      EndDate = grouped.Key.EndDate,
                                      TaskTime = grouped.Key.TaskTime,
                                      ActualTime = grouped.Sum(x => x.at != null ? x.at.TimeTaken : 0), // Calculate sum of ActualTime each group,
                                status = (_dbcontext.taskAssignResources.ToList().FindAll(x=> x.ProjectTaskID == grouped.Key.ProjectTaskID).Count==
                                ActualTaskTime.FindAll(y => y.ProjectTaskID == grouped.Key.ProjectTaskID && y.status==2).Count
                                && _dbcontext.taskAssignResources.ToList().FindAll(x => x.ProjectTaskID == grouped.Key.ProjectTaskID).Count>0 ? 2 :1) //grouped.Max(x=>x.at!=null?x.at.status:0),

                                  };

          //  _dbcontext.taskAssignResources.ToList().FindAll(x => x.ProjectTaskID == grouped.Key.ProjectTaskID).Count == 0 ? 0 : 1
            return projectTasklist.ToList();
        }

        public List<ProjectType> GetProjectTypelst()
        {
            return _dbcontext.projectTypes.ToList();
        }

        public List<Resource> GetResource()
        {
            return _dbcontext.resources.ToList();
        }

   
        public List<Resource> GetResourceNotAssigns(int ProjectID)
        {
            var _resource = from reso in _dbcontext.resources.ToList()
                            join ra in _dbcontext.resourceAssigns.ToList().FindAll(x => x.ProjectID == ProjectID) 
                                    on reso.ResourceID equals ra.ResourceID
                            into resoGroup
                            from td in resoGroup.DefaultIfEmpty()
                            where  td == null
                            select reso;
            return _resource.ToList();
        }

        public string SaveProject(Project _obj)
        {
            try
            {
                if(_dbcontext.project.ToList().FindAll(x=>x.Name == _obj.Name).Count > 0)
                {
                    return "Project Name is already exists.";
                }
                else
                {
                    _dbcontext.project.Add(_obj);
                    _dbcontext.SaveChanges();
                    return "Record added";
                }
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public string SaveProjectType(ProjectType _obj)
        {
            try
            {
                if (_dbcontext.projectTypes.ToList().FindAll(x => x.TypeName == _obj.TypeName).Count > 0)
                {
                    return "Already exists this type name";
                }
                else
                {
                    _dbcontext.projectTypes.Add(_obj);
                    _dbcontext.SaveChanges();
                    return "Record added";
                }
            }
           catch(Exception ex)
            {
                return ex.Message;
            }
         
            
        }

       

        public string AddResourceToProject(ResourceAssign _reso)
        {
            try
            {
                _dbcontext.resourceAssigns.Add(_reso);
                _dbcontext.SaveChanges();
                return "Resource Assign";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public List<Resource> GetResourceAssigns(int ProjectID)
        {
            var _resource = from ra in _dbcontext.resourceAssigns.ToList().FindAll(x => x.ProjectID == ProjectID)
                            join reso in _dbcontext.resources.ToList() on ra.ResourceID equals reso.ResourceID
                            select new Resource
                            { 
                            ResourceAssignID = ra.ResourceAssignID,
                            ResourceID = reso.ResourceID,
                            Name =reso.Name,
                            Email = reso.Email,
                            Mobile=reso.Mobile,
                            Designation=reso.Designation 
                            };

            return _resource.ToList();
        }

        public string SaveTaskAssignToResource(TaskAssignResource _obj)
        {
            try
            {
                _dbcontext.taskAssignResources.Add(_obj);
                _dbcontext.SaveChanges();
                return "Assign the task...";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public List<TaskAssignResource> GetTaskAssignResources(int ProjectID)
        {
            var _taskAssign = from tar in _dbcontext.taskAssignResources.ToList().FindAll(x => x.ProjectID == ProjectID)
                              join task in _dbcontext.projectTasks.ToList().FindAll(y => y.ProjectID == ProjectID)
                               on tar.ProjectTaskID equals task.ProjectTaskID
                              join resAss in _dbcontext.resourceAssigns.ToList().FindAll(z => z.ProjectID == ProjectID) on tar.ResourceAssignID equals resAss.ResourceAssignID
                              join reso in _dbcontext.resources.ToList() on resAss.ResourceID equals reso.ResourceID
                              select new TaskAssignResource
                              {
                                  TaskAssignResourceID = tar.TaskAssignResourceID,
                                  ProjectTaskID = tar.ProjectTaskID,
                                  AssignDate = tar.AssignDate,
                                  AssignTime = tar.AssignTime,
                                  ActualTime = _dbcontext.actualTaskWorks.ToList().FindAll(x=>x.TaskAssignResourceID== tar.TaskAssignResourceID).Sum(k=>k.TimeTaken),
                                  ResourceName = reso.Name,
                                  TaskName = task.TaskName,
                                  status= (_dbcontext.actualTaskWorks.ToList().FindAll(x => x.TaskAssignResourceID == tar.TaskAssignResourceID).Count>0? _dbcontext.actualTaskWorks.ToList().FindAll(x => x.TaskAssignResourceID == tar.TaskAssignResourceID).OrderByDescending(d=>d.ActualTaskWorkID).FirstOrDefault().status:0)
                              };
            return _taskAssign.ToList();
        }

        public string SaveActualTaskWork(ActualTaskWork _obj)
        {
            try
            {
                _dbcontext.actualTaskWorks.Add(_obj);
                _dbcontext.SaveChanges();
                return "Updated Task time";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
