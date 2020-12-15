using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using Ks.Common;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

using Attribute = Ks.Common.CollectedCalledAttribute;
using Generator = Ks.Analyzers.CollectedCalledGenerator;

#pragma warning disable RS2008 // Enable analyzer release tracking

namespace Ks.Analyzers
{
    [Generator]
    public class CollectedCalledGenerator : ISourceGenerator
    {

        private static readonly string GeneratorName = typeof(Generator).Name.RegexReplace("Generator$", "");

        public static readonly DiagnosticDescriptor CollectedCalledMethodsMustBeVoidAndParameterlessRule =
            new DiagnosticDescriptor(
                nameof(CollectedCalledMethodsMustBeVoidAndParameterlessRule),
                nameof(CollectedCalledMethodsMustBeVoidAndParameterlessRule),
                "CollectedCalled attribute can only be used on a void and parameter-less method.",
                "Usage",
                DiagnosticSeverity.Error,
                true);

        private static readonly TextGenerationTemplate Template =
            TextGenerationTemplate.CreateTemplate(Utils.GetResourceText($"{GeneratorName}.cs"), Utils.NamespaceClassTemplate);

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());

            Utils.PrepareTestOutputDir(GeneratorName);
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var syntaxReceiver = (SyntaxReceiver) context.SyntaxReceiver!;
            var compilation = (CSharpCompilation) context.Compilation;

            var attributeSymbol = compilation.GetExistingTypeByMetadataName(typeof(Attribute).FullName);

            var methodSymbols = new List<(IMethodSymbol MethodSymbol, Attribute AttributeData)>();
            foreach (var method in syntaxReceiver.CandidateMethods)
            {
                var model = compilation.GetSemanticModel(method.SyntaxTree);
                var methodSymbol = model.GetDeclaredSymbol(method)!;
                var attributeData = methodSymbol.GetAttributeOfType(attributeSymbol);
                if (attributeData == null)
                {
                    continue;
                }
                if (methodSymbol.Arity != 0 || methodSymbol.ReturnType.SpecialType != SpecialType.System_Void)
                {
                    context.ReportDiagnostic(Diagnostic.Create(CollectedCalledMethodsMustBeVoidAndParameterlessRule, method.Identifier.GetLocation()));
                    continue;
                }
                methodSymbols.Add((methodSymbol, CreateAttributeFromData(attributeData)));
            }

            foreach (var classGroup in methodSymbols.GroupBy(m => m.MethodSymbol.ContainingType, SymbolEqualityComparer.Default))
            {
                var classSymbol = classGroup.Key;

                var data = TextGenerationTemplate.CreateData()
                    .AddNamespaceAndClasses(classSymbol);

                foreach (var methodGroup in classGroup.GroupBy(m => m.AttributeData.MethodName))
                {
                    var mdata = data.AddBlock("Method")
                        .Add("Name", methodGroup.Key);

                    var isStatic = true;
                    foreach (var (methodSymbol, attribute) in methodGroup)
                    {
                        var cdata = mdata.AddBlock("Call")
                            .Add("Name", methodSymbol.Name);

                        if (!methodSymbol.IsStatic)
                        {
                            cdata.Add("This", true);
                            isStatic = false;
                        }
                    }

                    mdata.Add("static", isStatic);
                }

                context.AddSourceAndWriteTest(classSymbol.ToDisplayString(NullableFlowState.None), GeneratorName, Template.Generate(data));
            }
        }

        private static Attribute CreateAttributeFromData(AttributeData data)
        {
            var args = data.ConstructorArguments.Select(a => a.Value).ToArray();
            var res = new Attribute((string) args[0]!);
            return res;
        }

        public class SyntaxReceiver : ISyntaxReceiver
        {
            public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
            {
                if (syntaxNode is not MethodDeclarationSyntax method)
                {
                    return;
                }
                if (method.AttributeLists.Any())
                {
                    this.CandidateMethods.Add(method);
                }
            }

            public List<MethodDeclarationSyntax> CandidateMethods { get; } = new List<MethodDeclarationSyntax>();
        }

    }
}
