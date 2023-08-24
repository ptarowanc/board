using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Xefier.Threading.Tasks;

public class TaskExample : MonoBehaviour
{
    #region Start
    private void Start()
    {
        TaskRun();
        TaskContinueWith();
        TaskResult();
        TaskException();
        TaskStatus();
        TaskWhenAll();
        TaskWhenAny();
        TaskWait();
        TaskWaitAll();
        TaskWaitAny();
    } 
    #endregion

    #region Examples
    private void TaskRun()
    {
        //Option1: Method
        Task.Run(RunMethod);

        //Option2a: Lambda
        Task.Run(() =>
        {
            //TODO: Replace with your code
            Debug.Log("Task.Run:Lambda");
        });

        //Option2b: Inline Lambda
        Task.Run(() => Debug.Log("Task.Run:InlineLambda"));
    }

    private void TaskContinueWith()
    {
        //Option1: Method
        Task.Run(RunMethod).ContinueWith(ContinueWithMethod);

        //Option2a: Lambda
        Task.Run(RunMethod).ContinueWith((t) =>
        {
            //TODO: Replace with your code
            Debug.Log("Task.ContinueWith:Lamda");
        });

        //Option2b: Inline Lambda
        Task.Run(RunMethod).ContinueWith((t) => Debug.Log("Task.ContinueWith:InlineLamda"));
    }

    private void TaskResult()
    {
        Task<float>.Run(() => new System.Random().Next()).ContinueWith((t) =>
        {
            //TODO: Replace with your code
            Debug.Log(string.Format("Task<float>.Result = {0}", t.Result));
        });
    }

    private void TaskException()
    {
        Task.Run(() =>
        {
            throw new Exception("(EXPECTED) Example exception handling");
        }).ContinueWith((t) =>
        {
            //Log exception that occurred in thread
            Debug.LogException(t.Exception);
        });
    }

    private void TaskStatus()
    {
        Task.Run(RunMethod).ContinueWith((t) =>
        {
            //Check status with Task.Status
            Debug.Log(string.Format("Task.Status = {0}", t.Status));
            //Check if an exception occurred with Task.IsFaulted
            Debug.Log(string.Format("Task.IsFaulted = {0}", t.IsFaulted));
            //Check if task was canceled
            Debug.Log(string.Format("Task.IsCanceled = {0}", t.IsCanceled));
            //Check if task has completed successfully
            Debug.Log(string.Format("Task.IsCompleted = {0}", t.IsCompleted));
        });
    }

    private void TaskWhenAll()
    {
        var tasks = RunMultipleTasks(4);
        Task.WhenAll(tasks).ContinueWith((t) =>
        {
            //TODO: Replace with your code
            Debug.Log("Task.WhenAll: All tasks completed");
        });
    }

    private void TaskWhenAny()
    {
        var tasks = RunMultipleTasks(4);
        Task.WhenAny(tasks).ContinueWith((t) =>
        {
            //TODO: Replace with your code
            Debug.Log(string.Format("Task.WhenAny: Task{0} completed", ((Task<int>)t.Result).Result));
        });
    }

    private void TaskWait()
    {
        var task = Task.Run(RunMethod);
        task.Wait();
        //TODO: Replace with your code
        Debug.Log("Task.Wait");
    }

    private void TaskWaitAll()
    {
        var tasks = RunMultipleTasks(4);
        Task.WaitAll(tasks);
        //TODO: Replace with your code
        Debug.Log("Task.WaitAll");
    }

    private void TaskWaitAny()
    {
        var tasks = RunMultipleTasks(4);
        int idx = Task.WaitAny(tasks);
        //TODO: Replace with your code
        Debug.Log(string.Format("Task.WaitAny: Task{0} completed", idx));
    } 
    #endregion

    #region Utility Methods
    private void RunMethod()
    {
        //TODO: Replace with your code
        Debug.Log("Task.Run:Method");
    }

    private void ContinueWithMethod(ITask t)
    {
        //TODO: Replace with your code
        Debug.Log("Task.ContinueWith:Method");
    }

    private List<ITask> RunMultipleTasks(int count)
    {
        List<ITask> tasks = new List<ITask>();
        for (int i = 0; i < count; i++)
        {
            int iCopy = i; //Make a copy of i (Important)
            tasks.Add(Task<int>.Run(() =>
            {
                Debug.Log(string.Format("Task{0} running", iCopy));
                Thread.Sleep(10); //Fake delay
                return iCopy;
            }));
        }
        return tasks;
    } 
    #endregion
}
