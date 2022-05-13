using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workshop.Control;

namespace Workshop.Model
{
    public class SyntaxTreeItemInfo
    {
        public SyntaxTreeItemInfo()
        {
            this.Items=new ObservableCollection<SyntaxTreeItemInfo>();
        }

        public SyntaxTag Tag { get; set; }

        public bool IsExpanded { get; set; }

        public string Background { get; set; }
        public string ToolTip { get; internal set; }
        public Action<object, object> Expanded { get; internal set; }
        public string Header { get; internal set; }
        public IList<SyntaxTreeItemInfo> Items { get; internal set; }
        public Action<object, object> Selected { get; internal set; }
    }
}
