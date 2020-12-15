using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using Ks.Common;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

using Attribute = Ks.Common.ReactivePropertyAttribute;
using Generator = Ks.Analyzers.ReactivePropertyGenerator;

#pragma warning disable RS2008 // Enable analyzer release tracking

namespace Ks.Analyzers
{
    [Generator]
    public class ReactivePropertyGenerator : ISourceGenerator
    {

        private static readonly string GeneratorName = typeof(Generator).Name.RegexReplace("Generator$", "");

        public static readonly DiagnosticDescriptor ReactivePropertyMustBeInReactiveObjectRule =
            new DiagnosticDescriptor(
                nameof(ReactivePropertyMustBeInReactiveObjectRule),
                nameof(ReactivePropertyMustBeInReactiveObjectRule),
                "ReactiveProperty attribute can only be used in a class derived from ReactiveObject.",
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

            var fieldSymbols = new List<(IFieldSymbol FieldSymbol, Attribute AttributeData)>();
            foreach (var field in syntaxReceiver.CandidateFields)
            {
                var model = compilation.GetSemanticModel(field.SyntaxTree);
                foreach (var variable in field.Declaration.Variables)
                {
                    var fieldSymbol = (IFieldSymbol) model.GetDeclaredSymbol(variable)!;
                    var attributeData = fieldSymbol.GetAttributeOfType(attributeSymbol);
                    if (attributeData == null)
                    {
                        continue;
                    }
                    if (!compilation.ClassifyConversion(fieldSymbol.ContainingType, reactiveObjectSymbol).IsImplicit)
                    {
                        context.ReportDiagnostic(Diagnostic.Create(ReactivePropertyMustBeInReactiveObjectRule, variable.GetLocation()));
                        continue;
                    }
                    fieldSymbols.Add((fieldSymbol, CreateAttributeFromData(attributeData)));
                }
            }

            var propertyAttributeHelper = new PropertyAttributeHelper(context);

            foreach (var classGroup in fieldSymbols.GroupBy(f => f.FieldSymbol.ContainingType, SymbolEqualityComparer.Default))
            {
                var classSymbol = classGroup.Key;

                var data = TextGenerationTemplate.CreateData()
                    .AddNamespaceAndClasses(classSymbol);

                foreach (var (fieldSymbol, attribute) in classGroup)
                {
                    var pdata = data.AddBlock("Property")
                        .Add("FieldName", fieldSymbol.Name)
                        .Add("Name", GetPropertyName(fieldSymbol.Name, attribute.PropertyName))
                        .Add("Type", fieldSymbol.Type.ToDisplayString())
                        .Add("AccessModifier", attribute.AccessModifier.GetCSharpString())
                        .Add("Virtual", attribute.Virtual)
                        .Add("New", attribute.New)
                        .AddPropertyAttributes(fieldSymbol, propertyAttributeHelper);

                    if (attribute.GetterAccessModifier != default)
                    {
                        pdata.Add("GetterAccessModifier", attribute.GetterAccessModifier.GetCSharpString());
                    }
                    if (attribute.SetterAccessModifier != default)
                    {
                        pdata.Add("SetterAccessModifier", attribute.SetterAccessModifier.GetCSharpString());
                    }
                }

                context.AddSourceAndWriteTest(classSymbol.ToDisplayString(NullableFlowState.None), GeneratorName, Template.Generate(data));
            }
        }

        private static string GetPropertyName(string fieldName, string? propertyName)
        {
            if (propertyName != null)
            {
                return propertyName;
            }
            if (fieldName.StartsWith("_"))
            {
                fieldName = fieldName.Substring(1);
            }
            return char.ToUpper(fieldName[0]) + fieldName.Substring(1);
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
                var kv = data.NamedArguments.SingleOrDefault(a => a.Key == nameof(res.GetterAccessModifier));
                if (kv.Key != null)
                {
                    res.GetterAccessModifier = (AccessModifier) kv.Value.Value!;
                }
            }
            {
                var kv = data.NamedArguments.SingleOrDefault(a => a.Key == nameof(res.SetterAccessModifier));
                if (kv.Key != null)
                {
                    res.SetterAccessModifier = (AccessModifier) kv.Value.Value!;
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
                if (syntaxNode is not FieldDeclarationSyntax field)
                {
                    return;
                }
                if (field.AttributeLists.Any())
                {
                    this.CandidateFields.Add(field);
                }
            }

            public List<FieldDeclarationSyntax> CandidateFields { get; } = new List<FieldDeclarationSyntax>();
        }

    }
}
