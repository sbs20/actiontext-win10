using Sbs20.Actiontext.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Xaml;

namespace Sbs20.Actiontext.ViewModel
{
    public class ActionItem : IActionItem, INotifyPropertyChanged
    {
        private string raw;
        private string body;
        private string priority;
        private DateTime completionDate;
        private DateTime creationDate;
        private bool isComplete;
        
        public event PropertyChangedEventHandler PropertyChanged;

        public IList<string> Projects { get; private set; }
        public string PrimaryProject { get; set; }
        public IList<string> Contexts { get; private set; }
        public string PrimaryContext { get; set; }
        public DateTime DueDate { get; set; }
        public int Index { get; set; }

        protected void OnPropertyChanged(string property)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public ActionItem(string raw, int index)
        {
            this.raw = string.Empty;
            this.body = string.Empty;
            this.priority = string.Empty;
            this.completionDate = DateTime.MaxValue;
            this.creationDate = DateTime.Today;
            this.isComplete = false;
            this.Index = index;

            this.Contexts = new List<string>();
            this.PrimaryContext = string.Empty;
            this.Projects = new List<string>();
            this.PrimaryProject = string.Empty;

            this.DueDate = DateTime.MaxValue;

            ActionItemAdapter.Parse(raw, this);
        }

        public ActionItem() : this(string.Empty, 0)
        {
        }

        public string Body
        {
            get { return this.body; }
            set
            {
                this.body = value;
                this.OnPropertyChanged("Body");
            }
        }

        public string Priority
        {
            get { return this.priority ?? string.Empty; }
            set
            {
                this.priority = value == null ? string.Empty : value.ToUpperInvariant();
                this.OnPropertyChanged("Priority");
                this.OnPropertyChanged("PriorityColour");
            }
        }

        public DateTime CompletionDate
        {
            get { return this.completionDate; }
            set
            {
                this.completionDate = value;
                this.OnPropertyChanged("CompletionDate");
                this.OnPropertyChanged("DisplayDate");
            }
        }

        public DateTime CreationDate
        {
            get { return this.creationDate; }
            set
            {
                this.creationDate = value;
                this.OnPropertyChanged("CreationDate");
            }
        }

        public bool IsComplete
        {
            get { return this.isComplete; }
            set
            {
                if (this.isComplete != value)
                {
                    if (Settings.PreservePriorityOnComplete)
                    {
                        if (value && this.Priority.Length > 0)
                        {
                            this.Body = this.Priority + " " + this.Body;
                            this.Priority = null;
                        }
                        // TODO
                        else if (!value && this.Body.StartsWith("("))
                        {
                            this.Priority = this.Body.Substring(0, 3);
                            this.Body = this.Body.Substring(3).Trim();
                        }
                    }

                    this.isComplete = value;
                    this.OnPropertyChanged("IsComplete");
                    this.OnPropertyChanged("BodyColour");
                    this.CompletionDate = DateTime.Now;
                }
            }
        }

        public DateTime DisplayDate
        {
            get { return this.IsComplete ? this.CompletionDate : this.CreationDate; }
        }

        public string DisplayDateString
        {
            get { return this.DisplayDate.ToString("D"); }
        }

        public string PriorityColour
        {
            get
            {
                switch (this.Priority)
                {
                    case "(A)":
                        return "Red";

                    case "(B)":
                        return "Orange";

                    case "(C)":
                        return "Green";

                    case "(D)":
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
            get { return this.raw; }
            set { this.raw = value; }
        }

        public void Reparse()
        {
            ActionItemAdapter.Parse(this.Raw, this);
        }
    }
}
