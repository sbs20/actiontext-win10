using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommonExtensions;

namespace ToDoLib
{
    /// <summary>
    /// A thin data access abstraction over the actual todo.txt file
    /// </summary>
    public class TaskList
    {
        // It may look like an overly simple approach has been taken here, but it's well considered. This class
        // represents *the file itself* - when you call a method it should be as though you directly edited the file.
        // This reduces the likelihood of concurrent update conflicts by making each action as autonomous as possible.
        // Although this does lead to some extra IO, it's a small price for maintaining the integrity of the file.

        // NB, this is not the place for higher-level functions like searching, task manipulation etc. It's simply 
        // for CRUDing the todo.txt file. 

        public List<Task> Tasks { get; private set; }

        // Task List MetaData
        public List<string> Projects { get; private set; }
        public List<string> Contexts { get; private set; }
        public List<string> Priorities { get; private set; }
        public bool PreserveWhiteSpace { get; set; }

        public TaskList(bool preserveWhitespace = false)
        {
            PreserveWhiteSpace = preserveWhitespace;
        }

        public void UpdateTaskListMetaData()
        {
            var UniqueProjects = new SortedSet<string>();
            var UniqueContexts = new SortedSet<string>();
            var UniquePriorities = new SortedSet<string>();

            foreach (Task t in Tasks)
            {
                foreach (string p in t.Projects)
                {
                    UniqueProjects.Add(p);
                }
                foreach (string c in t.Contexts)
                {
                    UniqueContexts.Add(c);
                }
                UniquePriorities.Add(t.Priority);
            }

            this.Projects = UniqueProjects.ToList<string>();
            this.Contexts = UniqueContexts.ToList<string>();
            this.Priorities = UniquePriorities.ToList<string>();
        }

        public void Add(Task task)
        {
            Tasks.Add(task);
            UpdateTaskListMetaData();
        }

        public void Delete(Task task)
        {
            Tasks.Remove(Tasks.First(t => t.Raw == task.Raw));
            UpdateTaskListMetaData();
        }

        /// <summary>
        /// This method updates one task in the file. It works by replacing the "current task" with the "new task".
        /// </summary>
        /// <param name="currentTask">The task to replace.</param>
        /// <param name="newTask">The replacement task.</param>
        /// <param name="reloadTasksPriorToUpdate">Optionally reload task file prior to the update. Default is TRUE.</param>
        /// <param name="writeTasks">Optionally write task file after the update. Default is TRUE.</param>
        /// <param name="reloadTasksAfterUpdate">Optionally reload task file after the update. Default is TRUE.</param>
        public void Update(Task currentTask, Task newTask)
        {
            Log.Debug("Updating task '{0}' to '{1}'", currentTask.ToString(), newTask.ToString());

            try
            {
                // ensure that the task list still contains the current task...
                if (!Tasks.Any(t => t.Raw == currentTask.Raw))
                {
                    throw new Exception("That task no longer exists in to todo.txt file.");
                }

                var currentIndex = Tasks.IndexOf(Tasks.First(t => t.Raw == currentTask.Raw));
                Tasks[currentIndex] = newTask;

                Log.Debug("Task '{0}' updated", currentTask.ToString());
            }
            catch (IOException ex)
            {
                var msg = "An error occurred while trying to update your task in the task list file.";
                Log.Error(msg, ex);
                throw new TaskException(msg, ex);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                throw;
            }
            finally
            {
                UpdateTaskListMetaData();
            }
        }

    }
}
