using System.Web.Http;
using ZK.TaskManager.Core.Services;
using ZK.TaskManager.Core.Task;

namespace ZK.TaskManager.Node.API
{
    public class JobController : ApiController
    {
       

        [HttpPost]
        public bool StartJob(string jobid)
        {
            if (string.IsNullOrEmpty(jobid))
                return false;
            var job = JobService.Query(jobid);
            if (job == null)
                return false;
            job.Task = TaskService.Query(job.TaskId);
            if (job.TaskId == null)
                return false;
            //启动任务
            if (!JobHelper.StartJob(job))
                return false;
            return true;
        }

        [HttpPost]
        public bool DeleteJob(string jobid)
        {
            if (string.IsNullOrEmpty(jobid))
                return false;
            var job = JobService.Query(jobid);
            if (job == null)
                return false;
            //删除任务
            if (!JobHelper.DeleteJob(job))
                return false;
            return true;
        }

        [HttpPost]
        public bool PauseJob(string jobid)
        {
            if (string.IsNullOrEmpty(jobid))
                return false;
            var job = JobService.Query(jobid);
            if (job == null)
                return false;
            //暂停任务
            if (!JobHelper.PauseJob(job))
                return false;
            return true;
        }

        [HttpPost]
        public bool ResumeJob(string jobid)
        {
            if (string.IsNullOrEmpty(jobid))
                return false;
            var job = JobService.Query(jobid);
            if (job == null)
                return false;
            //恢复任务
            if (!JobHelper.ResumeJob(job))
                return false;
            return true;
        }

        [HttpPost]
        public int GetCount()
        {
            return JobHelper.GetJobCount();
        }
    }
}
