using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Sbs20.Actiontext.Model
{
    public class ActionItemAdapter
    {
        private const string CompletedPattern = @"^X\s((\d{4})-(\d{2})-(\d{2}))?";
        private const string PriorityPattern = @"^(?<priority>\([A-Z]\)\s)";
        private const string CreatedDatePattern = @"(?<date>(\d{4})-(\d{2})-(\d{2}))";

        private const string DueRelativePattern = @"due:(?<dateRelative>today|tomorrow|monday|tuesday|wednesday|thursday|friday|saturday|sunday)";

        private const string DueDatePattern = @"due:(?<date>(\d{4})-(\d{2})-(\d{2}))";
        private const string ProjectPattern = @"(?<proj>(?<=^|\s)\+[^\s]+)";
        private const string ContextPattern = @"(^|\s)(?<context>\@[^\s]+)";

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

        public static void Parse(string raw, IActionItem action)
        {
            raw = raw.Replace(Environment.NewLine, ""); //make sure it's just on one line

            //Replace relative days with hard date
            //Supports english: 'today', 'tomorrow', and full weekdays ('monday', 'tuesday', etc)
            //If today is the specified weekday, due date will be in one week
            //TODO implement short weekdays ('mon', 'tue', etc) and other languages
            var reg = new Regex(DueRelativePattern, RegexOptions.IgnoreCase);
            var dueDateRelative = reg.Match(raw).Groups["dateRelative"].Value.Trim();
            if (!string.IsNullOrEmpty(dueDateRelative))
            {
                var isValid = false;

                var due = new DateTime();
                dueDateRelative = dueDateRelative.ToLower();
                if (dueDateRelative == "today")
                {
                    due = DateTime.Now;
                    isValid = true;
                }
                else if (dueDateRelative == "tomorrow")
                {
                    due = DateTime.Now.AddDays(1);
                    isValid = true;
                }
                else if (dueDateRelative == "monday" | dueDateRelative == "tuesday" | dueDateRelative == "wednesday" |
                        dueDateRelative == "thursday" | dueDateRelative == "friday" | dueDateRelative == "saturday" |
                        dueDateRelative == "sunday")
                {
                    due = DateTime.Now;
                    var count = 0;

                    //if day of week, add days to today until weekday matches input
                    //if today is the specified weekday, due date will be in one week
                    do
                    {
                        count++;
                        due = due.AddDays(1);
                        isValid = string.Equals(due.ToString("dddd", new CultureInfo("en-US")),
                                                dueDateRelative,
                                                StringComparison.CurrentCultureIgnoreCase);
                    } while (!isValid && (count < 7));
                    // The count check is to prevent an endless loop in case of other culture.
                }

                if (isValid)
                    raw = reg.Replace(raw, "due:" + due.ToString("yyyy-MM-dd"));
            }

            //Set Raw string after replacing relative date but before removing matches
            action.Raw = raw;

            // because we are removing matches as we go, the order we process is important. It must be:
            // - completed
            // - priority
            // - due date
            // - created date
            // - projects | contexts
            // What we have left is the body

            reg = new Regex(CompletedPattern, RegexOptions.IgnoreCase);
            var s = reg.Match(raw).Value.Trim();

            if (string.IsNullOrEmpty(s))
            {
                action.IsComplete = false;
                action.CompletionDate = DateTime.MaxValue;
            }
            else
            {
                action.IsComplete = true;
                if (s.Length > 1)
                    action.CompletionDate = ToDateTime(s.Substring(2));
            }

            raw = reg.Replace(raw, string.Empty);

            reg = new Regex(PriorityPattern, RegexOptions.IgnoreCase);
            action.Priority = reg.Match(raw).Groups["priority"].Value.Trim();
            raw = reg.Replace(raw, string.Empty);

            reg = new Regex(DueDatePattern);
            action.DueDate = ToDateTime(reg.Match(raw).Groups["date"].Value.Trim());
            raw = reg.Replace(raw, string.Empty);

            reg = new Regex(CreatedDatePattern);
            action.CreationDate = ToDateTime(reg.Match(raw).Groups["date"].Value.Trim());
            raw = reg.Replace(raw, string.Empty);

            var ProjectSet = new SortedSet<string>();
            reg = new Regex(ProjectPattern);
            var projects = reg.Matches(raw);
            action.PrimaryProject = null;
            int i = 0;
            foreach (Match project in projects)
            {
                var p = project.Groups["proj"].Value.Trim();
                ProjectSet.Add(p);
                if (i == 0)
                {
                    action.PrimaryProject = p;
                }
                i++;
            }
            action.Projects.Concat(ProjectSet);
            raw = reg.Replace(raw, "");

            var ContextsSet = new SortedSet<string>();
            reg = new Regex(ContextPattern);
            var contexts = reg.Matches(raw);
            action.PrimaryContext = null;
            i = 0;
            foreach (Match context in contexts)
            {
                var c = context.Groups["context"].Value.Trim();
                ContextsSet.Add(c);
                if (i == 0)
                {
                    action.PrimaryContext = c;
                }
                i++;
            }
            action.Contexts.Concat(ContextsSet);
            raw = reg.Replace(raw, string.Empty);

            action.Body = raw.Trim();
        }

        public static string ToString(IActionItem actionItem)
        {
            var str = "";
            if (!string.IsNullOrEmpty(actionItem.Raw))
            // always use Raw if possible as it will preserve placement of projects and contexts
            {
                var reg = new Regex(CompletedPattern, RegexOptions.IgnoreCase);
                var rawCompleted = reg.IsMatch(actionItem.Raw);

                str = actionItem.Raw;

                // we only need to mess with the raw string if its completed status has changed
                if (rawCompleted != actionItem.IsComplete)
                {
                    if (actionItem.IsComplete)
                    {
                        if (!Settings.PreservePriorityOnComplete)
                        {
                            str = Regex.Replace(actionItem.Raw, PriorityPattern, "");
                        }

                        str = "x " + ToString(actionItem.CompletionDate) + " " + str;
                    }
                    else
                    {
                        str = reg.Replace(actionItem.Raw, "").Trim();
                    }
                }
            }
            else
            {
                str = string.Format("{0}{1}{2} {3} {4}",
                    actionItem.IsComplete ? "x " + ToString(actionItem.CompletionDate) + " " : "",
                    actionItem.Priority == null ? "N/A" : actionItem.Priority + " ",
                    actionItem.Body,
                    string.Join(" ", actionItem.Projects),
                    string.Join(" ", actionItem.Contexts));
            }

            return str;
        }

        public static string ToString(DateTime creation, string body)
        {
            return ToString(creation) + " " + body;
        }
    }
}
