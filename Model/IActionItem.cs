using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sbs20.Actiontext.Model
{
    public interface IActionItem
    {
        IList<string> Projects { get; }
        string PrimaryProject { get; set; }
        IList<string> Contexts { get; }
        string PrimaryContext { get; set; }
        DateTime DueDate { get; set; }
        bool IsComplete { get; set; }
        DateTime CompletionDate { get; set; }
        DateTime CreationDate { get; set; }
        string Priority { get; set; }
        string Body { get; set; }
        string Raw { get; set; }
        int Index { get; set; }
    }
}
