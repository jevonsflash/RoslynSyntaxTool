using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Classification;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using RoslynQuoter;
using Workshop.Common;
using Workshop.Control;
using Workshop.Helper;
using Workshop.Manager;
using Workshop.Model;
using Workshop.Service;

namespace Workshop.ViewModel
{

    public class SyntaxTreeViewerViewModel : ObservableObject
    {


        private ImmutableArray<ClassifiedSpan> classifiedSpans;
        private bool _isNavigatingFromTreeToSource;
        private bool _isNavigatingFromSourceToTree;

        public SyntaxTree SyntaxTree { get; private set; }
        public SemanticModel SemanticModel { get; private set; }

        public SyntaxTreeViewerViewModel()
        {

            this.RootItems=new ObservableCollection<SyntaxTreeItemInfo>();
            this.PropertyChanged+=SyntaxTreeViewerViewModel_PropertyChanged;

        }

        private void SyntaxTreeViewerViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CurrentItem))
            {
                if (CurrentItem==null)
                {
                    return;
                }
                CurrentItem.Selected?.Invoke(CurrentItem);
            }
        }

        public async void Init(string csharpText)
        {
            // <Snippet1>

            var workspace = new AdhocWorkspace();
            var projectId = ProjectId.CreateNewId();
            var versionStamp = VersionStamp.Create();
            var projectInfo = ProjectInfo.Create(projectId, versionStamp, "NewProject", "projName", LanguageNames.CSharp);
            var newProject = workspace.AddProject(projectInfo);

            var sourceText = SourceText.From(csharpText);
            var document = workspace.AddDocument(newProject.Id, "NewFile.cs", sourceText);
            var syntaxTree = await document.GetSyntaxTreeAsync();
            //  var compilation = CSharpCompilation.Create("Dummy").AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location)).AddSyntaxTrees(syntaxTree);
            var semanticModel = await document.GetSemanticModelAsync();

            /*
                        var syntaxTree = CSharpSyntaxTree.ParseText(sourceText);
                        var compilation = CSharpCompilation.Create("Dummy").AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location)).AddSyntaxTrees(syntaxTree);
                        var semanticModel = compilation.GetSemanticModel(syntaxTree);
            */

            DisplaySyntaxTree(syntaxTree, semanticModel);
            Console.WriteLine(RootItems);
            this.OnPropertyChanged(nameof(RootItems));


        }

        public void DisplaySyntaxTree(SyntaxTree tree, SemanticModel model = null)
        {
            if (tree != null)
            {
                this.SyntaxTree = tree;
                this.SemanticModel = model;
                this.AddNode(null, this.SyntaxTree.GetRoot(default(CancellationToken)));

            }
        }

        private void AddNode(SyntaxTreeItemInfo parentItem, SyntaxNode node)
        {
            if (node == null)
            {
                return;
            }
            string kind = node.Kind().ToString();
            SyntaxTag syntaxTag = new SyntaxTag
            {
                SyntaxNode = node,
                Category = SyntaxCategory.SyntaxNode,
                Span = node.Span,
                FullSpan = node.FullSpan,
                Kind = kind,
                ParentItem = parentItem
            };
            SyntaxTreeItemInfo item = this.CreateSyntaxTreeItemInfo(syntaxTag, syntaxTag.Kind + " " + node.Span.ToString(), node.ContainsDiagnostics);
            // item.SetResourceReference(System.Windows.Controls.Control.ForegroundProperty, SyntaxVisualizerControl.SyntaxNodeTextBrushKey);
            if (this.SyntaxTree != null && node.ContainsDiagnostics)
            {
                item.ToolTip = string.Empty;
                foreach (Diagnostic diagnostic in this.SyntaxTree.GetDiagnostics(node))
                {
                    SyntaxTreeItemInfo item2 = item;
                    object toolTip = item2.ToolTip;
                    item2.ToolTip = ((toolTip != null) ? toolTip.ToString() : null) + diagnostic.ToString() + "\n";
                }
                item.ToolTip = item.ToolTip.ToString().Trim();
            }
            item.Background="blue";
            item.Selected += sender =>
            {
                this._isNavigatingFromTreeToSource = true;
                this.Type = node.GetType().Name;
                this.Kind = kind;

                var syntaxDetailViewerViewModel = Ioc.Default.GetService<SyntaxDetailViewerViewModel>();
                syntaxDetailViewerViewModel.Init(node);

                item.IsExpanded = true;
                if (!this._isNavigatingFromSourceToTree && this.SyntaxNodeNavigationToSourceRequested != null)
                {
                    this.SyntaxNodeNavigationToSourceRequested(node);
                }
                this._isNavigatingFromTreeToSource = false;
                //e.Handled = true;
            };
            item.Expanded +=  sender =>
            {
                if (item.Items.Count == 1 && item.Items[0] == null)
                {
                    item.Items.RemoveAt(0);
                    if (this.SemanticModel != null)
                    {
                        IOperation operation = this.SemanticModel.GetOperation(node, default(CancellationToken));
                        if (operation != null && operation.Parent == null)
                        {
                            this.AddOperation(item, operation);
                        }
                    }
                    foreach (SyntaxNodeOrToken nodeOrToken2 in node.ChildNodesAndTokens())
                    {
                        this.AddNodeOrToken(item, nodeOrToken2);
                    }
                }
            };
            if (parentItem == null)
            {
                this.RootItems.Clear();
                this.RootItems.Add(item);
            }
            else
            {
                parentItem.Items.Add(item);
            }
            if (node.ChildNodesAndTokens().Count > 0)
            {

                foreach (SyntaxNodeOrToken nodeOrToken in node.ChildNodesAndTokens())
                {
                    this.AddNodeOrToken(item, nodeOrToken);
                }
            }
        }

        private void AddNodeOrToken(SyntaxTreeItemInfo parentItem, SyntaxNodeOrToken nodeOrToken)
        {
            if (nodeOrToken.IsNode)
            {
                this.AddNode(parentItem, nodeOrToken.AsNode());
                return;
            }
            this.AddToken(parentItem, nodeOrToken.AsToken());
        }

        private void AddOperation(SyntaxTreeItemInfo parentItem, IOperation operation)
        {
            SyntaxNode node = operation.Syntax;
            string kind = operation.Kind.ToString();
            SyntaxTag syntaxTag = new SyntaxTag
            {
                SyntaxNode = node,
                Category = SyntaxCategory.Operation,
                Span = node.Span,
                FullSpan = node.FullSpan,
                Kind = kind,
                ParentItem = parentItem
            };
            SyntaxTreeItemInfo item = this.CreateSyntaxTreeItemInfo(syntaxTag, syntaxTag.Kind + " " + node.Span.ToString(), node.ContainsDiagnostics);
            //  item.SetResourceReference(System.Windows.Controls.Control.ForegroundProperty, SyntaxVisualizerControl.OperationTextBrushKey);
            if (this.SyntaxTree != null && node.ContainsDiagnostics)
            {
                item.ToolTip = string.Empty;
                foreach (Diagnostic diagnostic in this.SyntaxTree.GetDiagnostics(node))
                {
                    SyntaxTreeItemInfo item2 = item;
                    object toolTip = item2.ToolTip;
                    item2.ToolTip = ((toolTip != null) ? toolTip.ToString() : null) + diagnostic.ToString() + "\n";
                }
                item.ToolTip = item.ToolTip.ToString().Trim();
            }
            item.Selected +=  sender =>
            {
                this._isNavigatingFromTreeToSource = true;
                this.Type = string.Join(", ", GetOperationInterfaces(operation));
                this.Kind = kind;
                var syntaxDetailViewerViewModel = Ioc.Default.GetService<SyntaxDetailViewerViewModel>();
                syntaxDetailViewerViewModel.Init(node);
                item.IsExpanded = true;
                if (!this._isNavigatingFromSourceToTree && this.SyntaxNodeNavigationToSourceRequested != null)
                {
                    this.SyntaxNodeNavigationToSourceRequested(node);
                }
                this._isNavigatingFromTreeToSource = false;
                //   e.Handled = true;
            };
            item.Expanded += sender =>
            {
                if (item.Items.Count == 1 && item.Items[0] == null)
                {
                    item.Items.RemoveAt(0);
                    foreach (IOperation operation3 in operation.Children)
                    {
                        this.AddOperation(item, operation3);
                    }
                }
            };
            if (parentItem == null)
            {
                this.RootItems.Clear();
                this.RootItems.Add(item);
            }
            else
            {
                parentItem.Items.Add(item);
            }
            if (operation.Children.Any<IOperation>())
            {

                foreach (IOperation operation2 in operation.Children)
                {
                    this.AddOperation(item, operation2);
                }
            }

            static IEnumerable<string> GetOperationInterfaces(IOperation operation)
            {
                var interfaces = new List<Type>();
                foreach (var interfaceType in operation.GetType().GetInterfaces())
                {
                    if (interfaceType == typeof(IOperation)
                        || !interfaceType.IsPublic
                        || !interfaceType.GetInterfaces().Contains(typeof(IOperation)))
                    {
                        continue;
                    }

                    interfaces.Add(interfaceType);
                }

                if (interfaces.Count == 0)
                {
                    interfaces.Add(typeof(IOperation));
                }

                return interfaces.OrderByDescending(x => x.GetInterfaces().Length).Select(x => x.Name);
            }
        }

        private void AddToken(SyntaxTreeItemInfo parentItem, SyntaxToken token)
        {
            string kind = token.Kind().ToString();
            SyntaxTag syntaxTag = new SyntaxTag
            {
                SyntaxToken = token,
                Category = SyntaxCategory.SyntaxToken,
                Span = token.Span,
                FullSpan = token.FullSpan,
                Kind = kind,
                ParentItem = parentItem
            };
            SyntaxTreeItemInfo item = this.CreateSyntaxTreeItemInfo(syntaxTag, syntaxTag.Kind + " " + token.Span.ToString(), token.ContainsDiagnostics);
            // item.SetResourceReference(System.Windows.Controls.Control.ForegroundProperty, SyntaxVisualizerControl.SyntaxTokenTextBrushKey);
            if (this.SyntaxTree != null && token.ContainsDiagnostics)
            {
                item.ToolTip = string.Empty;
                foreach (Diagnostic diagnostic in this.SyntaxTree.GetDiagnostics(token))
                {
                    SyntaxTreeItemInfo item2 = item;
                    object toolTip = item2.ToolTip;
                    item2.ToolTip = ((toolTip != null) ? toolTip.ToString() : null) + diagnostic.ToString() + "\n";
                }
                item.ToolTip = item.ToolTip.ToString().Trim();
            }
            item.Background="green";
            item.Selected += sender =>
            {
                this._isNavigatingFromTreeToSource = true;
                this.Type = token.GetType().Name;
                this.Kind = kind;
                var syntaxDetailViewerViewModel = Ioc.Default.GetService<SyntaxDetailViewerViewModel>();
                //syntaxDetailViewerViewModel.Init(token);
                item.IsExpanded = true;
                if (!this._isNavigatingFromSourceToTree && this.SyntaxTokenNavigationToSourceRequested != null)
                {
                    this.SyntaxTokenNavigationToSourceRequested(token);
                }
                this._isNavigatingFromTreeToSource = false;
                //e.Handled = true;
            };
            item.Expanded += sender =>
            {
                if (item.Items.Count == 1 && item.Items[0] == null)
                {
                    item.Items.RemoveAt(0);
                    foreach (SyntaxTrivia trivia3 in token.LeadingTrivia)
                    {
                        this.AddTrivia(item, trivia3, true);
                    }
                    foreach (SyntaxTrivia trivia4 in token.TrailingTrivia)
                    {
                        this.AddTrivia(item, trivia4, false);
                    }
                }
            };
            if (parentItem == null)
            {
                this.RootItems.Clear();
                this.RootItems.Add(item);
            }
            else
            {
                parentItem.Items.Add(item);
            }
            if (token.HasLeadingTrivia || token.HasTrailingTrivia)
            {
                foreach (SyntaxTrivia trivia in token.LeadingTrivia)
                {
                    this.AddTrivia(item, trivia, true);
                }
                foreach (SyntaxTrivia trivia2 in token.TrailingTrivia)
                {
                    this.AddTrivia(item, trivia2, false);
                }
            }
        }

        private void AddTrivia(SyntaxTreeItemInfo parentItem, SyntaxTrivia trivia, bool isLeadingTrivia)
        {
            string kind = trivia.Kind().ToString();
            SyntaxTag syntaxTag = new SyntaxTag
            {
                SyntaxTrivia = trivia,
                Category = SyntaxCategory.SyntaxTrivia,
                Span = trivia.Span,
                FullSpan = trivia.FullSpan,
                Kind = kind,
                ParentItem = parentItem
            };
            SyntaxTreeItemInfo item = this.CreateSyntaxTreeItemInfo(syntaxTag, (isLeadingTrivia ? "Lead: " : "Trail: ") + syntaxTag.Kind + " " + trivia.Span.ToString(), trivia.ContainsDiagnostics);
            // item.SetResourceReference(System.Windows.Controls.Control.ForegroundProperty, SyntaxVisualizerControl.SyntaxTriviaTextBrushKey);
            if (this.SyntaxTree != null && trivia.ContainsDiagnostics)
            {
                item.ToolTip = string.Empty;
                foreach (Diagnostic diagnostic in this.SyntaxTree.GetDiagnostics(trivia))
                {
                    SyntaxTreeItemInfo item2 = item;
                    object toolTip = item2.ToolTip;
                    item2.ToolTip = ((toolTip != null) ? toolTip.ToString() : null) + diagnostic.ToString() + "\n";
                }
                item.ToolTip = item.ToolTip.ToString().Trim();
            }
            item.Background="red";
            item.Selected += sender =>
            {
                this._isNavigatingFromTreeToSource = true;
                this.Type = trivia.GetType().Name;
                this.Kind = kind;
                var syntaxDetailViewerViewModel = Ioc.Default.GetService<SyntaxDetailViewerViewModel>();
                //syntaxDetailViewerViewModel.Init(trivia);
                item.IsExpanded = true;
                if (!this._isNavigatingFromSourceToTree && this.SyntaxTriviaNavigationToSourceRequested != null)
                {
                    this.SyntaxTriviaNavigationToSourceRequested(trivia);
                }
                this._isNavigatingFromTreeToSource = false;
                //e.Handled = true;
            };
            item.Expanded += sender =>
            {
                if (item.Items.Count == 1 && item.Items[0] == null)
                {
                    item.Items.RemoveAt(0);
                    this.AddNode(item, trivia.GetStructure());
                }
            };
            if (parentItem == null)
            {
                this.RootItems.Clear();
                this.RootItems.Add(item);
                //this.typeTextLabel.Visibility = Visibility.Hidden;
                //this.kindTextLabel.Visibility = Visibility.Hidden;
                this.Type = string.Empty;
                this.Kind = string.Empty;
            }
            else
            {
                parentItem.Items.Add(item);
            }
            if (trivia.HasStructure)
            {
                this.AddNode(item, trivia.GetStructure());
            }
        }


        private SyntaxTreeItemInfo CreateSyntaxTreeItemInfo(SyntaxTag tag, string text, bool containsDiagnostics)
        {
            SyntaxTreeItemInfo treeViewItem = new SyntaxTreeItemInfo
            {
                Tag = tag,
                IsExpanded = false,
                // Background = System.Windows.Media.Brushes.Transparent
            };

            {
                treeViewItem.Header = text;//new TextBlock(new Run(text));
            }

            return treeViewItem;
        }

        public delegate void SyntaxNodeDelegate(SyntaxNode? node);
        public event SyntaxNodeDelegate? SyntaxNodeDirectedGraphRequested;
        public event SyntaxNodeDelegate? SyntaxNodeNavigationToSourceRequested;


        public delegate void SyntaxTokenDelegate(SyntaxToken token);
        public event SyntaxTokenDelegate? SyntaxTokenDirectedGraphRequested;
        public event SyntaxTokenDelegate? SyntaxTokenNavigationToSourceRequested;

        public delegate void SyntaxTriviaDelegate(SyntaxTrivia trivia);
        public event SyntaxTriviaDelegate? SyntaxTriviaDirectedGraphRequested;
        public event SyntaxTriviaDelegate? SyntaxTriviaNavigationToSourceRequested;

        private SyntaxTreeItemInfo _currentItem;

        public SyntaxTreeItemInfo CurrentItem
        {
            get { return _currentItem; }
            set
            {
                _currentItem = value;

                OnPropertyChanged();
            }
        }

        private ObservableCollection<SyntaxTreeItemInfo> _rootItems;

        public ObservableCollection<SyntaxTreeItemInfo> RootItems
        {
            get { return _rootItems; }
            set
            {
                _rootItems = value;

                OnPropertyChanged();
            }
        }

        private string _type;

        public string Type
        {
            get { return _type; }
            set
            {
                _type = value;
                OnPropertyChanged();

            }
        }

        private string _kind;

        public string Kind
        {
            get { return _kind; }
            set
            {
                _kind = value;
                OnPropertyChanged();

            }
        }


    }
}
