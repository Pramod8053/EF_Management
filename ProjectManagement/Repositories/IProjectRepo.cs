using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.Repositories
{
   public interface IProjectRepo
    {
        List<ProjectType> GetProjectTypelst();
        string SaveProjectType(ProjectType _obj);
        List<Project> GetProjectlst();
        string SaveProject(Project _obj);
        List<Resource> GetResource();
        string AddResource(Resource _obj);
        List<ProjectTask> GetProjectTask();
        string AddTask(ProjectTask _task);
        List<Resource> GetResourceAssigns(int ProjectID);
        List<Resource> GetResourceNotAssigns(int ProjectID);
        string AddResourceToProject(ResourceAssign _reso);
        string SaveTaskAssignToResource(TaskAssignResource _obj);
        List<TaskAssignResource> GetTaskAssignResources(int ProjectID);
        string SaveActualTaskWork(ActualTaskWork _obj);
        List<ActualTaskWork> GetActualTaskWorksReport(int ProjectID);
    }
}
