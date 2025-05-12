using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using App;

public class Compiler
{
    public static Assembly CompileGeneratedCode(string code, params Type[] referencedTypes)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(code);

        var references = new List<MetadataReference>
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
            MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location),
            MetadataReference.CreateFromFile(typeof(App.Ioc).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(ICommand).Assembly.Location)
        };

        foreach (var type in referencedTypes.Distinct())
        {
            references.Add(MetadataReference.CreateFromFile(type.Assembly.Location));
        }

        var compilation = CSharpCompilation.Create(
            assemblyName: "DynamicAssembly",
            syntaxTrees: new[] { syntaxTree },
            references: references,
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        using var ms = new System.IO.MemoryStream();
        var emitResult = compilation.Emit(ms);

        ms.Seek(0, System.IO.SeekOrigin.Begin);
        return Assembly.Load(ms.ToArray());
    }
}