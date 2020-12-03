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

using Attribute = Ks.Common.ReactiveOutputPropertyAttribute;
using Generator = Ks.Analyzers.ReactiveOutputPropertyGenerator;

#pragma warning disable RS2008 // Enable analyzer release tracking

namespace Ks.Analyzers
{
    [Generator]
    public class ReactiveOutputPropertyGenerator : ISourceGenerator
    {

        private static readonly string GeneratorName = typeof(Generator).Name.RegexReplace("Generator$", "");

        public static readonly DiagnosticDescriptor ReactiveOutputPropertyMustBeInReactiveObjectRule =
            new DiagnosticDescriptor(
                nameof(ReactiveOutputPropertyMustBeInReactiveObjectRule),
                nameof(ReactiveOutputPropertyMustBeInReactiveObjectRule),
                "ReactiveOutputProperty attribute can only be used in a class derived from ReactiveObject.",
                "Usage",
                DiagnosticSeverity.Error,
                true);

        public static readonly DiagnosticDescriptor ReactiveOutputPropertyMethodsMustBeParameterlessAndReturnIObservableRule =
            new DiagnosticDescriptor(
                nameof(ReactiveOutputPropertyMethodsMustBeParameterlessAndReturnIObservableRule),
                nameof(ReactiveOutputPropertyMethodsMustBeParameterlessAndReturnIObservableRule),
                "ReactiveOutputProperty attribute can only be used on a parameter-less method that returns IObservable.",
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
            var reactiveObjectSymbol = compilation.GetExistingTypeByMetadataName(Utils.ReactiveUI_ReactiveObject);
            var iObservableSymbol = compilation.GetExistingTypeByMetadataName(Utils.System_IObservable);

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
                if (!compilation.ClassifyConversion(methodSymbol.ContainingType, reactiveObjectSymbol).IsImplicit)
                {
                    context.ReportDiagnostic(Diagnostic.Create(ReactiveOutputPropertyMustBeInReactiveObjectRule, method.Identifier.GetLocation()));
                    continue;
                }
                if (methodSymbol.Arity != 0 || !SymbolEqualityComparer.Default.Equals(methodSymbol.ReturnType.OriginalDefinition, iObservableSymbol))
                {
                    context.ReportDiagnostic(Diagnostic.Create(ReactiveOutputPropertyMethodsMustBeParameterlessAndReturnIObservableRule, method.Identifier.GetLocation()));
                    continue;
                }
                methodSymbols.Add((methodSymbol, CreateAttributeFromData(attributeData)));
            }

            var propertyAttributeHelper = new PropertyAttributeHelper(context);

            foreach (var classGroup in methodSymbols.GroupBy(f => f.MethodSymbol.ContainingType, SymbolEqualityComparer.Default))
            {
                var classSymbol = classGroup.Key;

                var data = TextGenerationTemplate.CreateData()
                    .AddNamespaceAndClasses(classSymbol);

                foreach (var (methodSymbol, attribute) in classGroup)
                {
                    var type = ((INamedTypeSymbol) methodSymbol.ReturnType).TypeArguments.Single();
                    var defaultName = methodSymbol.Name.RegexReplace("Init$", "");

                    data.AddBlock("Property")
                        .Add("MethodName", methodSymbol.Name)
                        .Add("Name", attribute.PropertyName ?? defaultName)
                        .Add("HelperName", attribute.PropertyName ?? $"_{defaultName}Property")
                        .Add("DefaultValueTag", attribute.DefaultValueTag?.ToExpression().ToString() ?? "")
                        .Add("Type", type.ToDisplayString())
                        .Add("AccessModifier", attribute.AccessModifier.GetCSharpString())
                        .Add("HelperAccessModifier", attribute.HelperAccessModifier.GetCSharpString())
                        .Add("Virtual", attribute.Virtual)
                        .Add("New", attribute.New)
                        .AddPropertyAttributes(methodSymbol, propertyAttributeHelper);
                }

                context.AddSourceAndWriteTest(classSymbol.ToDisplayString(NullableFlowState.None), GeneratorName, Template.Generate(data));
            }
        }

        private static Attribute CreateAttributeFromData(AttributeData data)
        {
            var res = new Attribute();
            {
                var kv = data.NamedArguments.SingleOrDefault(a => a.Key == nameof(res.AccessModifier));
                if (kv.Key != null)
                {
                    res.AccessModifier = (AccessModifier) kv.Value.Value!;
                }
            }
            {
                var kv = data.NamedArguments.SingleOrDefault(a => a.Key == nameof(res.HelperAccessModifier));
                if (kv.Key != null)
                {
                    res.HelperAccessModifier = (AccessModifier) kv.Value.Value!;
                }
            }
            {
                var kv = data.NamedArguments.SingleOrDefault(a => a.Key == nameof(res.HelperName));
                if (kv.Key != null)
                {
                    res.HelperName = (string?) kv.Value.Value;
                }
            }
            {
                var kv = data.NamedArguments.SingleOrDefault(a => a.Key == nameof(res.PropertyName));
                if (kv.Key != null)
                {
                    res.PropertyName = (string?) kv.Value.Value;
                }
            }
            {
                var kv = data.NamedArguments.SingleOrDefault(a => a.Key == nameof(res.DefaultValueTag));
                if (kv.Key != null)
                {
                    res.DefaultValueTag = (string?) kv.Value.Value;
                }
            }
            {
                var kv = data.NamedArguments.SingleOrDefault(a => a.Key == nameof(res.Virtual));
                if (kv.Key != null)
                {
                    res.Virtual = (bool) kv.Value.Value!;
                }
            }
            {
                var kv = data.NamedArguments.SingleOrDefault(a => a.Key == nameof(res.New));
                if (kv.Key != null)
                {
                    res.New = (bool) kv.Value.Value!;
                }
            }
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
