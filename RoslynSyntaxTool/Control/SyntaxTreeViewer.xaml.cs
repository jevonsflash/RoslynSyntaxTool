using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Classification;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.CodeAnalysis.CSharp;
using CommunityToolkit.Mvvm.Messaging;
using Workshop.Common;
using CommunityToolkit.Mvvm.DependencyInjection;
using Workshop.ViewModel;
using Workshop.Model;

namespace Workshop.Control
{
    /// <summary>
    /// SyntaxTreeViewer.xaml 的交互逻辑
    /// </summary>
    public partial class SyntaxTreeViewer : UserControl
    {
        public SyntaxTreeViewer()
        {
            InitializeComponent();


        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            (this.DataContext as SyntaxTreeViewerViewModel).CurrentItem=e.NewValue as SyntaxTreeItemInfo;

        }
    }

}
