using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

using Microsoft.CodeAnalysis;
using RoslynSyntaxTool.Process;

[assembly: TargetFramework("net6.0", FrameworkDisplayName = "net6.0")]
[assembly: AssemblyTitle("RoslynSyntaxTool.Proxy.ConvertToCSharpProxy")]
[assembly: AssemblyProduct("RoslynSyntaxTool.Proxy.ConvertToCSharpProxy")]
[assembly: AssemblyCopyright("Copyright © MatoApp")]
[assembly: ComVisible(false)]
[assembly: Guid("9b38af72-b313-4b5b-b893-bd102c131dc6")]
[assembly: AssemblyVersion("0.0.0.1")]
[assembly: AssemblyFileVersion("0.0.0.1")]


static SyntaxTree GenerateProxyTree()
{
    return new ProxyTreeGen().Process();
}

Console.WriteLine(GenerateProxyTree().ToString());