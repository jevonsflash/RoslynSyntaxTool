using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

static SyntaxTree GenerateProxyTree()
{
    return SyntaxFactory.CompilationUnit()
.WithMembers(
    SyntaxFactory.SingletonList<MemberDeclarationSyntax>(
        SyntaxFactory.GlobalStatement(
            SyntaxFactory.LocalFunctionStatement(
                SyntaxFactory.IdentifierName("opt"),
                SyntaxFactory.MissingToken(SyntaxKind.IdentifierToken))
            .WithModifiers(
                SyntaxFactory.TokenList(
                    new[]{
                        SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                        SyntaxFactory.Token(SyntaxKind.StaticKeyword)}))
            .WithBody(
                SyntaxFactory.Block(
                    SyntaxFactory.SingletonList<StatementSyntax>(
                        SyntaxFactory.ReturnStatement(
                            SyntaxFactory.LiteralExpression(
                                SyntaxKind.StringLiteralExpression,
                                SyntaxFactory.Literal("a")))))))))
.NormalizeWhitespace().SyntaxTree;
}

Console.WriteLine(GenerateProxyTree().ToString());