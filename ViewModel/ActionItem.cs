using System;
using System.ComponentModel;
using TodoTask = ToDoLib.Task;

namespace Sbs20.Actiontext.ViewModel
{
    public class ActionItem : INotifyPropertyChanged
    {
        private TodoTask todoTask;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string property)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        private static DateTime ToDateTime(string s)
        {
            DateTime dt = DateTime.MinValue;
            DateTime.TryParse(s, out dt);
            return dt;
        }

        private static string ToString(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd");
        }

        public ActionItem()
        {
            this.todoTask = new TodoTask(null, null, null, null);
        }

        public static ActionItem Parse(string raw)
        {
            return new ActionItem
            {
                todoTask = new TodoTask(raw)
            };
        }

        public string Body
        {
            get { return this.todoTask.Body; }
            set
            {
                this.todoTask.Body = value;
                this.OnPropertyChanged("Body");
            }
        }

        public string Priority
        {
            get { return this.todoTask.Priority; }
            set
            {
                this.todoTask.Priority = value;
                this.OnPropertyChanged("Priority");
            }
        }

        public DateTime CompletionDate
        {
            get { return ToDateTime(this.todoTask.CompletedDate); }
            set
            {
                this.todoTask.CompletedDate = ToString(value);
                this.OnPropertyChanged("CompletionDate");
                this.OnPropertyChanged("DisplayDate");
            }
        }

        public DateTime CreationDate
        {
            get { return ToDateTime(this.todoTask.CreationDate); }
        }

        public bool IsComplete
        {
            get { return this.todoTask.Completed; }
            set
            {
                this.todoTask.Completed = value;
                this.OnPropertyChanged("IsComplete");
                this.CompletionDate = DateTime.Now;
            }
        }

        public DateTime DisplayDate
        {
            get { return this.IsComplete ? this.CompletionDate : this.CreationDate; }
        }

        public string DisplayDateString
        {
            get { return ToString(this.DisplayDate); }
        }

        public string PriorityColour
        {
            get
            {
                switch (this.Priority.ToLower())
                {
                    case "(a)":
                        return "Red";

                    case "(b)":
                        return "Orange";

                    case "(c)":
                        return "Green";

                    case "(d)":
                        return "SkyBlue";

                    default:
                        return "#808080";
                }
            }
        }

        public string StrikethroughLineVisibility
        {
            get { return this.IsComplete ? "Visible" : "Collapsed"; }
        }
    }
}
