using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynQuoter;
using Workshop.Common;
using Workshop.Control;
using Workshop.Helper;
using Workshop.Manager;
using Workshop.Model;
using Workshop.Service;
using TextDocument = ICSharpCode.AvalonEdit.Document.TextDocument;

namespace Workshop.ViewModel
{
    public class ConvertToCSharpPageViewModel : ObservableObject
    {

        public ConvertToCSharpPageViewModel()
        {
            CurrentContent = new TextDocument();
            ResponseContent = new TextDocument();
            ContinueCommand = new RelayCommand(ContinueAction);
            CopyToClipboardCommand = new RelayCommand(CopyToClipboardAction);

        }



        private void CopyToClipboardAction()
        {

            if (string.IsNullOrEmpty(ResponseContentText))
            {
                return;
            }
            Clipboard.SetText(ResponseContentText);
        }


        private void ContinueAction()
        {
            var currentContentText = CurrentContent.Text;


            var prefix = "\r\n\treturn ";
            var suffix = ".SyntaxTree;";
            if (currentContentText.Length>0)
            {
                currentContentText= currentContentText.Insert(0, prefix);
                currentContentText= currentContentText.Insert(currentContentText.Length, suffix);
            }

            var task = InvokeHelper.InvokeOnUi(null, () =>
            {
                var settingInfo = LocalDataService.ReadObjectLocal<SettingInfo>();

                var responseText = String.Empty;

                if (string.IsNullOrEmpty(currentContentText))
                {
                    responseText = "Please specify the source text.";
                }
                //else if (currentContentText.Length > 2000)
                //{
                //    responseText = "Only strings shorter than 2000 characters are supported; your input string is " + currentContentText.Length + " characters long.";
                //}
                else
                {
                    try
                    {
                        var syntaxTree = CSharpSyntaxTree.ParseText(currentContentText).GetCompilationUnitRoot();

                        var treeFrame = SyntaxFactory.CompilationUnit()
.WithUsings(
    SyntaxFactory.List<UsingDirectiveSyntax>(
        new UsingDirectiveSyntax[]{
            SyntaxFactory.UsingDirective(
                SyntaxFactory.QualifiedName(
                    SyntaxFactory.IdentifierName("Microsoft"),
                    SyntaxFactory.IdentifierName("CodeAnalysis")))
            .WithUsingKeyword(
                SyntaxFactory.Token(
                    SyntaxFactory.TriviaList(),
                    SyntaxKind.UsingKeyword,
                    SyntaxFactory.TriviaList(
                        SyntaxFactory.Space)))
            .WithSemicolonToken(
                SyntaxFactory.Token(
                    SyntaxFactory.TriviaList(),
                    SyntaxKind.SemicolonToken,
                    SyntaxFactory.TriviaList(
                        SyntaxFactory.CarriageReturnLineFeed))),
            SyntaxFactory.UsingDirective(
                SyntaxFactory.QualifiedName(
                    SyntaxFactory.QualifiedName(
                        SyntaxFactory.IdentifierName("Microsoft"),
                        SyntaxFactory.IdentifierName("CodeAnalysis")),
                    SyntaxFactory.IdentifierName("CSharp")))
            .WithUsingKeyword(
                SyntaxFactory.Token(
                    SyntaxFactory.TriviaList(),
                    SyntaxKind.UsingKeyword,
                    SyntaxFactory.TriviaList(
                        SyntaxFactory.Space)))
            .WithSemicolonToken(
                SyntaxFactory.Token(
                    SyntaxFactory.TriviaList(),
                    SyntaxKind.SemicolonToken,
                    SyntaxFactory.TriviaList(
                        SyntaxFactory.CarriageReturnLineFeed))),
            SyntaxFactory.UsingDirective(
                SyntaxFactory.QualifiedName(
                    SyntaxFactory.QualifiedName(
                        SyntaxFactory.QualifiedName(
                            SyntaxFactory.IdentifierName("Microsoft"),
                            SyntaxFactory.IdentifierName("CodeAnalysis")),
                        SyntaxFactory.IdentifierName("CSharp")),
                    SyntaxFactory.IdentifierName("Syntax")))
            .WithUsingKeyword(
                SyntaxFactory.Token(
                    SyntaxFactory.TriviaList(),
                    SyntaxKind.UsingKeyword,
                    SyntaxFactory.TriviaList(
                        SyntaxFactory.Space)))
            .WithSemicolonToken(
                SyntaxFactory.Token(
                    SyntaxFactory.TriviaList(),
                    SyntaxKind.SemicolonToken,
                    SyntaxFactory.TriviaList(
                        SyntaxFactory.CarriageReturnLineFeed)))}))
.WithMembers(
    SyntaxFactory.SingletonList<MemberDeclarationSyntax>(
        SyntaxFactory.NamespaceDeclaration(
            SyntaxFactory.QualifiedName(
                SyntaxFactory.IdentifierName("RoslynSyntaxTool"),
                SyntaxFactory.IdentifierName(
                    SyntaxFactory.Identifier(
                        SyntaxFactory.TriviaList(),
                        "Process",
                        SyntaxFactory.TriviaList(
                            SyntaxFactory.CarriageReturnLineFeed)))))
        .WithNamespaceKeyword(
            SyntaxFactory.Token(
                SyntaxFactory.TriviaList(
                    SyntaxFactory.CarriageReturnLineFeed),
                SyntaxKind.NamespaceKeyword,
                SyntaxFactory.TriviaList(
                    SyntaxFactory.Space)))
        .WithOpenBraceToken(
            SyntaxFactory.Token(
                SyntaxFactory.TriviaList(),
                SyntaxKind.OpenBraceToken,
                SyntaxFactory.TriviaList(
                    SyntaxFactory.CarriageReturnLineFeed)))
        .WithMembers(
            SyntaxFactory.SingletonList<MemberDeclarationSyntax>(
                SyntaxFactory.ClassDeclaration(
                    SyntaxFactory.Identifier(
                        SyntaxFactory.TriviaList(),
                        "ProxyTreeGen",
                        SyntaxFactory.TriviaList(
                            SyntaxFactory.CarriageReturnLineFeed)))
                .WithModifiers(
                    SyntaxFactory.TokenList(
                        SyntaxFactory.Token(
                            SyntaxFactory.TriviaList(
                                SyntaxFactory.Whitespace("    ")),
                            SyntaxKind.PublicKeyword,
                            SyntaxFactory.TriviaList(
                                SyntaxFactory.Space))))
                .WithKeyword(
                    SyntaxFactory.Token(
                        SyntaxFactory.TriviaList(),
                        SyntaxKind.ClassKeyword,
                        SyntaxFactory.TriviaList(
                            SyntaxFactory.Space)))
                .WithOpenBraceToken(
                    SyntaxFactory.Token(
                        SyntaxFactory.TriviaList(
                            SyntaxFactory.Whitespace("    ")),
                        SyntaxKind.OpenBraceToken,
                        SyntaxFactory.TriviaList(
                            SyntaxFactory.CarriageReturnLineFeed)))
                .WithMembers(
                    SyntaxFactory.SingletonList<MemberDeclarationSyntax>(
                        SyntaxFactory.MethodDeclaration(
                            SyntaxFactory.IdentifierName(
                                SyntaxFactory.Identifier(
                                    SyntaxFactory.TriviaList(),
                                    "SyntaxTree",
                                    SyntaxFactory.TriviaList(
                                        SyntaxFactory.Space))),
                            SyntaxFactory.Identifier("Process"))
                        .WithModifiers(
                            SyntaxFactory.TokenList(
                                SyntaxFactory.Token(
                                    SyntaxFactory.TriviaList(
                                        SyntaxFactory.Whitespace("        ")),
                                    SyntaxKind.PublicKeyword,
                                    SyntaxFactory.TriviaList(
                                        SyntaxFactory.Space))))
                        .WithParameterList(
                            SyntaxFactory.ParameterList()
                            .WithCloseParenToken(
                                SyntaxFactory.Token(
                                    SyntaxFactory.TriviaList(),
                                    SyntaxKind.CloseParenToken,
                                    SyntaxFactory.TriviaList(
                                        SyntaxFactory.CarriageReturnLineFeed))))
                        .WithBody(
                            SyntaxFactory.Block(
                                SyntaxFactory.SingletonList<StatementSyntax>(
                                    ((GlobalStatementSyntax)syntaxTree.Members[0]).Statement
                                    ))
                            .WithOpenBraceToken(
                                SyntaxFactory.Token(
                                    SyntaxFactory.TriviaList(
                                        SyntaxFactory.Whitespace("        ")),
                                    SyntaxKind.OpenBraceToken,
                                    SyntaxFactory.TriviaList(
                                        SyntaxFactory.CarriageReturnLineFeed)))
                            .WithCloseBraceToken(
                                SyntaxFactory.Token(
                                    SyntaxFactory.TriviaList(
                                        SyntaxFactory.Whitespace("        ")),
                                    SyntaxKind.CloseBraceToken,
                                    SyntaxFactory.TriviaList(
                                        SyntaxFactory.CarriageReturnLineFeed))))))
                .WithCloseBraceToken(
                    SyntaxFactory.Token(
                        SyntaxFactory.TriviaList(
                            SyntaxFactory.Whitespace("    ")),
                        SyntaxKind.CloseBraceToken,
                        SyntaxFactory.TriviaList(
                            SyntaxFactory.CarriageReturnLineFeed)))))
        .WithCloseBraceToken(
            SyntaxFactory.Token(
                SyntaxFactory.TriviaList(),
                SyntaxKind.CloseBraceToken,
                SyntaxFactory.TriviaList(
                    SyntaxFactory.CarriageReturnLineFeed)))))
.WithEndOfFileToken(
    SyntaxFactory.Token(
        SyntaxFactory.TriviaList(
            SyntaxFactory.CarriageReturnLineFeed),
        SyntaxKind.EndOfFileToken,
        SyntaxFactory.TriviaList())).SyntaxTree;




                        var stream = CompilationUtilitys.CompileClientProxy(new List<SyntaxTree>() { treeFrame });

                        using (stream)
                        {
                            var assembly = Assembly.Load(stream.ToArray());
                            _generatedServiceProxyAssembly = assembly;

                        }
                        var serviceProxyTypes = _generatedServiceProxyAssembly.GetExportedTypes();

                        object serviceProxyObject = null;
                        Type serviceProxyObjectType = null;

                        foreach (var serviceProxyType in serviceProxyTypes)
                        {
                            var typeInfo = serviceProxyType.GetTypeInfo();
                            if (typeInfo.FullName=="RoslynSyntaxTool.Process.ProxyTreeGen")
                            {
                                serviceProxyObjectType=serviceProxyType;
                                var instance = serviceProxyType.GetTypeInfo().GetConstructors().First().Invoke(null);
                                serviceProxyObject=instance;
                            }



                        }
                        var result = "没有结果，请检查输入代码";
                        var processor = serviceProxyObjectType.GetMethod("Process");
                        if (processor!=null)
                        {
                            result = processor.Invoke(serviceProxyObject, null).ToString();
                        }

                        responseText =result;
                    }
                    catch (Exception ex)
                    {
                        responseText = ex.ToString();

                    }
                }

                //ResponseContent.Remove(0, ResponseContent.TextLength);
                //MessageBox.Show(responseText);
                return responseText;

            }, (t) =>
            {
                ResponseContent.Text = t;

            });
        }



        public string CurrentContentText => CurrentContent.Text;
        public string ResponseContentText => ResponseContent.Text;

        private TextDocument _currentContent;

        public TextDocument CurrentContent
        {
            get { return _currentContent; }
            set
            {
                _currentContent = value;
                OnPropertyChanged(nameof(CurrentContent));
            }
        }

        private TextDocument _responseContent;

        public TextDocument ResponseContent
        {
            get { return _responseContent; }
            set
            {
                _responseContent = value;
                OnPropertyChanged(nameof(ResponseContent));
            }
        }

        private bool _isComplete;
        private Assembly _generatedServiceProxyAssembly;

        public bool IsComplete
        {
            get { return _isComplete; }
            set
            {
                _isComplete = value;
                OnPropertyChanged(nameof(IsComplete));
            }
        }

        public RelayCommand ContinueCommand { get; set; }
        public RelayCommand CopyToClipboardCommand { get; set; }
    }
}
