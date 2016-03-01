using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using TodoTask = ToDoLib.Task;

namespace sbs20.Tasktxt.Model
{
    public class TaskItemManager
    {
        public TaskCollection Tasks
        {
            get; private set;
        }

        public TaskItemManager()
        {
            this.Tasks = TaskCollection.Instance;
        }

        public async void Load()
        {
            var file = await StorageProvider.LoadFileAsync();
            var lines = await FileIO.ReadLinesAsync(file);
            foreach (string line in lines)
            {
                this.Tasks.Add(new TodoTask(line));
            }
        }

        //protected string GetPreferredFileLineEndingFromFile()
        //{
        //    try
        //    {
        //        using (StreamReader fileStream = new StreamReader(_filePath))
        //        {
        //            char previousChar = '\0';

        //            // Read the first 4000 characters to try and find a newline
        //            for (int i = 0; i < 4000; i++)
        //            {
        //                int b = fileStream.Read();
        //                if (b == -1) break;

        //                char currentChar = (char)b;
        //                if (currentChar == '\n')
        //                {
        //                    return (previousChar == '\r') ? "\r\n" : "\n";
        //                }

        //                previousChar = currentChar;
        //            }

        //            // if no newline found, use the default newline character for the environment
        //            return Environment.NewLine;
        //        }
        //    }
        //    catch (IOException ex)
        //    {
        //        var msg = "An error occurred while trying to read the task list file";
        //        Log.Error(msg, ex);
        //        throw new TaskException(msg, ex);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex.ToString());
        //        throw;
        //    }
        //}

        //public async Task WriteTasksToFile(IEnumerable<TodoTask> tasks)
        //{
        //    await FileIO.WriteLinesAsync(this.storageFile, tasks.Select(t => t.ToString()));
        //}

        //private async Task<IEnumerable<TodoTask>> ReadTasksFromFile()
        //{
        //    var lines = await FileIO.ReadLinesAsync(this.storageFile);
        //    return lines.Select(s => new TodoTask(s));
        //}

        private void Merge(ToDoLib.TaskList tasklist, IEnumerable<TodoTask> tasks)
        {

        }
    }
}
