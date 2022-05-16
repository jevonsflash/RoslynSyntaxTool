using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workshop.Control;

namespace Workshop.Model
{
    public class SyntaxTreeItemInfo : ObservableObject
    {

        public SyntaxTreeItemInfo()
        {
            this.Items=new ObservableCollection<SyntaxTreeItemInfo>();
            this.PropertyChanged+=SyntaxTreeItemInfo_PropertyChanged;
        }

        private void SyntaxTreeItemInfo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IsExpanded))
            {
                this.Expanded?.Invoke(this);
            }
        }

        public SyntaxTag Tag { get; set; }

        private bool _isExpanded;

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                _isExpanded = value;
                OnPropertyChanged();
            }
        }


        public string Background { get; set; }
        public string ToolTip { get; internal set; }
        public Action<SyntaxTreeItemInfo> Expanded { get; internal set; }
        public string Header { get; internal set; }
        public IList<SyntaxTreeItemInfo> Items { get; internal set; }
        public Action<SyntaxTreeItemInfo> Selected { get; internal set; }
    }
}
