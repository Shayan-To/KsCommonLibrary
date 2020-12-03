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

using Attribute = Ks.Common.ViewModelAttribute;
using Generator = Ks.Analyzers.ViewWithViewModel;

#pragma warning disable RS2008 // Enable analyzer release tracking

namespace Ks.Analyzers
{
    [Generator]
    public class ViewWithViewModel : ISourceGenerator
    {

        private static readonly string GeneratorName = typeof(Generator).Name.RegexReplace("Generator$", "");

        public static readonly DiagnosticDescriptor ViewModelsMustSpecifyTheirViewTypeRule =
            new DiagnosticDescriptor(
                nameof(ViewModelsMustSpecifyTheirViewTypeRule),
                nameof(ViewModelsMustSpecifyTheirViewTypeRule),
                "View models must specify their view type.",
                "Usage",
                DiagnosticSeverity.Warning,
                true);

        public static readonly DiagnosticDescriptor ViewModelViewsMustDeriveFromStyledElementRule =
            new DiagnosticDescriptor(
                nameof(ViewModelViewsMustDeriveFromStyledElementRule),
                nameof(ViewModelViewsMustDeriveFromStyledElementRule),
                $"View model views must derive from {Utils.Avalonia_StyledElement}.",
                "Usage",
                DiagnosticSeverity.Error,
                true);

        public static readonly DiagnosticDescriptor ViewModelsMustImplementIActivatableViewModelRule =
            new DiagnosticDescriptor(
                nameof(ViewModelsMustImplementIActivatableViewModelRule),
                nameof(ViewModelsMustImplementIActivatableViewModelRule),
                "View models must implement IActivatableViewModel.",
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
            var iActivatableViewModelSymbol = compilation.GetExistingTypeByMetadataName(Utils.ReactiveUI_IActivatableViewModel);
            var styledElementSymbol = compilation.GetExistingTypeByMetadataName(Utils.Avalonia_StyledElement);

            var classSymbols = new List<(INamedTypeSymbol ClassSymbol, INamedTypeSymbol ViewTypeSymbol)>();
            foreach (var @class in syntaxReceiver.CandidateClasses)
            {
                var model = compilation.GetSemanticModel(@class.SyntaxTree);
                var classSymbol = model.GetDeclaredSymbol(@class)!;
                var attributeData = classSymbol.GetAttributeOfType(attributeSymbol);
                if (attributeData == null)
                {
                    continue;
                }
                var viewTypeSymbol = GetViewTypeSymbol(attributeData);
                if (!viewTypeSymbol.HasValue)
                {
                    context.ReportDiagnostic(Diagnostic.Create(ViewModelsMustSpecifyTheirViewTypeRule, @class.Identifier.GetLocation()));
                    continue;
                }
                if (viewTypeSymbol.Value == null)
                {
                    continue;
                }

                if (!compilation.ClassifyConversion(viewTypeSymbol.Value, styledElementSymbol).IsImplicit)
                {
                    context.ReportDiagnostic(Diagnostic.Create(ViewModelViewsMustDeriveFromStyledElementRule, @class.Identifier.GetLocation()));
                }
                if (!compilation.ClassifyConversion(classSymbol, iActivatableViewModelSymbol).IsImplicit)
                {
                    context.ReportDiagnostic(Diagnostic.Create(ViewModelsMustImplementIActivatableViewModelRule, @class.Identifier.GetLocation()));
                }

                classSymbols.Add((classSymbol, viewTypeSymbol.Value));
            }

            foreach (var (classSymbol, viewTypeSymbol) in classSymbols)
            {
                var data = TextGenerationTemplate.CreateData()
                    .AddNamespaceAndClasses(viewTypeSymbol)
                    .Add("ViewModelType", classSymbol.ToDisplayString());

                context.AddSourceAndWriteTest(classSymbol.ToDisplayString(NullableFlowState.None), GeneratorName, Template.Generate(data));
            }
        }

        private static CNullable<INamedTypeSymbol?> GetViewTypeSymbol(AttributeData data)
        {
            var kv = data.NamedArguments.SingleOrDefault(a => a.Key == nameof(Attribute.View));
            if (kv.Key == null)
            {
                return default;
            }
            return new CNullable<INamedTypeSymbol?>((INamedTypeSymbol?) kv.Value.Value);
        }

        public class SyntaxReceiver : ISyntaxReceiver
        {
            public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
            {
                if (syntaxNode is not ClassDeclarationSyntax @class)
                {
                    return;
                }
                if (@class.AttributeLists.Any())
                {
                    this.CandidateClasses.Add(@class);
                }
            }

            public List<ClassDeclarationSyntax> CandidateClasses { get; } = new List<ClassDeclarationSyntax>();
        }

    }
}
