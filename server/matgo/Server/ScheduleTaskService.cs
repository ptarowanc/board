using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.ScheduleTask
{
    public partial class ScheduleTaskService : IScheduleTaskService
    {

        void IScheduleTaskService.DeleteTask(ScheduleTask task)
        {
            throw new NotImplementedException();
        }

        IList<ScheduleTask> IScheduleTaskService.GetAllTasks(bool showHidden)
        {
            throw new NotImplementedException();
        }

        ScheduleTask IScheduleTaskService.GetTaskById(int taskId)
        {
            throw new NotImplementedException();
        }

        ScheduleTask IScheduleTaskService.GetTaskByType(string type)
        {
            throw new NotImplementedException();
        }

        void IScheduleTaskService.InsertTask(ScheduleTask task)
        {
            throw new NotImplementedException();
        }

        void IScheduleTaskService.UpdateTask(ScheduleTask task)
        {
            throw new NotImplementedException();
        }
    }
}
