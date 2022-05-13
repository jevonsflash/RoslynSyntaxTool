using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Workshop.Model;

namespace Workshop.Control
{
    public class SyntaxTag
    {

        internal TextSpan Span { get; set; }

        internal TextSpan FullSpan { get; set; }

        internal SyntaxTreeItemInfo ParentItem { get; set; }

        internal string Kind { get; set; }

        internal SyntaxNode SyntaxNode { get; set; }

        internal SyntaxToken SyntaxToken { get; set; }

        internal SyntaxTrivia SyntaxTrivia { get; set; }

        internal SyntaxCategory Category { get; set; }
    }

}
