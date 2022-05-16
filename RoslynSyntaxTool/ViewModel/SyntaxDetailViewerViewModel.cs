using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.CodeAnalysis;
using Workshop.Model;

namespace Workshop.ViewModel
{

    public class SyntaxDetailViewerViewModel : ObservableObject
    {

        public SyntaxDetailViewerViewModel()
        {


        }

        public void Init(SyntaxNode syntaxTreeItemInfo)
        {
            this.CurrentItem=syntaxTreeItemInfo;
        }


        private SyntaxNode _currentItem;

        public SyntaxNode CurrentItem
        {
            get { return _currentItem; }
            set
            {
                _currentItem = value;

                OnPropertyChanged();
            }
        }

    }
}
