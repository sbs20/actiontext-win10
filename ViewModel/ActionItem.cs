using System;
using System.ComponentModel;
using Windows.UI.Xaml;
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

        public static string ToRawString(DateTime creation, string body)
        {
            return ToString(creation) + " " + body;
        }

        public ActionItem(string raw)
        {
            this.todoTask = new TodoTask(raw);
        }

        public ActionItem() : this(string.Empty)
        {
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
            get { return this.todoTask.Priority.ToUpperInvariant(); }
            set
            {
                this.todoTask.Priority = value.ToUpperInvariant();
                this.OnPropertyChanged("Priority");
                this.OnPropertyChanged("PriorityColour");
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
            set
            {
                this.todoTask.CreationDate = ToString(value);
                this.OnPropertyChanged("CreationDate");
            }
        }

        public bool IsComplete
        {
            get { return this.todoTask.Completed; }
            set
            {
                this.todoTask.Completed = value;
                this.OnPropertyChanged("IsComplete");
                this.OnPropertyChanged("BodyColour");
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
                string priority = this.Priority == null ? string.Empty : this.Priority.ToLower();
                switch (priority)
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

        public string BodyColour
        {
            get
            {
                var textColour = Application.Current.Resources["SystemBaseHighColor"];
                return this.IsComplete ? "#808080" : textColour.ToString();
            }
        }

        public string StrikethroughLineVisibility
        {
            get { return this.IsComplete ? "Visible" : "Collapsed"; }
        }

        public string Raw
        {
            get { return this.todoTask.Raw; }
            set
            {
                this.todoTask = new TodoTask(value);
                this.OnPropertyChanged("CompletionDate");
                this.OnPropertyChanged("DisplayDate");
                this.OnPropertyChanged("Body");
                this.OnPropertyChanged("BodyColour");
                this.OnPropertyChanged("Priority");
                this.OnPropertyChanged("PriorityColour");
                this.OnPropertyChanged("CreationDate");
                this.OnPropertyChanged("IsComplete");
            }
        }
    }
}
