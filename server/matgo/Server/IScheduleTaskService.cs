﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.ScheduleTask
{
    public partial interface IScheduleTaskService
    {
        void DeleteTask(ScheduleTask task);
        ScheduleTask GetTaskById(int taskId);
        ScheduleTask GetTaskByType(string type);
        IList<ScheduleTask> GetAllTasks(bool showHidden = false);
        void InsertTask(ScheduleTask task);
        void UpdateTask(ScheduleTask task);
    }
}
