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

namespace Ks.Analyzers
{
    internal static class Utils
    {
        public static readonly string BaseTestOutputDir = @"D:\Shayan\MyDocuments\Coding\Temp\GenerationTest";

        public static readonly string System_IObservable = "System.IObservable`1";

        public static readonly string Avalonia_StyledElement = "Avalonia.StyledElement";

        public static readonly string ReactiveUI_ReactiveObject = "ReactiveUI.ReactiveObject";
        public static readonly string ReactiveUI_IActivatableViewModel = "ReactiveUI.IActivatableViewModel";

        public static readonly TextGenerationTemplate NamespaceClassTemplate =
            TextGenerationTemplate.CreateTemplate(GetResourceText($"{nameof(NamespaceClassTemplate).RegexReplace("Template$", "")}.cs"));

        public static string PrepareTestOutputDir(string generatorName)
        {
            var testOutputDir = Path.Combine(BaseTestOutputDir, generatorName);

            if (Directory.Exists(testOutputDir))
            {
                Directory.Delete(testOutputDir, true);
            }
            Directory.CreateDirectory(testOutputDir);

            return testOutputDir;
        }

        public static string GetResourceText(string resourceName)
        {
            using var resStream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"Ks.Analyzers.Resources.{resourceName}");
            return new StreamReader(resStream).ReadToEnd();
        }

        public static ExpressionSyntax ToExpression(this TypedConstant constant)
        {
            return SyntaxFactory.ParseExpression(constant.ToCSharpString(), consumeFullText: true);
        }

        public static ExpressionSyntax ToExpression(this string? constant)
        {
            return constant == null ?
                SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression) :
                SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(constant));
        }

        public static INamedTypeSymbol GetExistingTypeByMetadataName(this CSharpCompilation compilation, string fullyQualifiedMetadataName)
        {
            return compilation.GetTypeByMetadataName(fullyQualifiedMetadataName) ?? throw new InvalidOperationException($"{fullyQualifiedMetadataName} is not available.");
        }

        public static IEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey, TKeyBase>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKeyBase> comparer) where TKey : TKeyBase
        {
            return Enumerable.GroupBy(source, keySelector, (IEqualityComparer<TKey>) comparer);
        }

        public static AttributeData? GetAttributeOfType(this ISymbol symbol, ITypeSymbol attributeSymbol)
        {
            return symbol.GetAttributes().SingleOrDefault(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, attributeSymbol));
        }

        public static void AddSourceAndWriteTest(this GeneratorExecutionContext context, string hintName, string generatorName, string source)
        {
            hintName = Utilities.Text.ReplaceInvalidCharacters(hintName, Path.GetInvalidFileNameChars());
            File.AppendAllText(Path.Combine(BaseTestOutputDir, generatorName, $"{hintName}.cs"), source);
            context.AddSource($"{hintName}.{generatorName}", SourceText.From(source, Encoding.UTF8));
        }

        public static TextGenerationTemplate.BlockData AddPropertyAttributes(this TextGenerationTemplate.BlockData data, ISymbol symbol, PropertyAttributeHelper helper)
        {
            foreach (var propAttribute in helper.GenerateAttributes(symbol))
            {
                data.AddBlock("Attribute")
                    .Add("Attribute", propAttribute.ToString());
            }

            return data;
        }

        [InteractiveRunnable(true)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members")]
        static void abc()
        {

        }

        public static string GenerateClassDeclaration(INamedTypeSymbol classSymbol)
        {
            var classSyntax = SyntaxFactory.ClassDeclaration(classSymbol.Name);

            if (classSymbol.IsGenericType)
            {
                classSyntax = classSyntax.WithTypeParameterList(
                        SyntaxFactory.TypeParameterList(SyntaxFactory.SeparatedList(
                                classSymbol.TypeArguments.Select(ta => SyntaxFactory.TypeParameter(SyntaxFactory.Identifier(ta.Name))))));
            }

            classSyntax = classSyntax.NormalizeWhitespace();
            var spanStart = classSyntax.Identifier.SpanStart;
            var spanEnd = classSyntax.TypeParameterList?.Span.End ?? classSyntax.Identifier.Span.End;

            return classSyntax.GetText().GetSubText(new TextSpan(spanStart, spanEnd - spanStart)).ToString();
        }

        public static TextGenerationTemplate.BlockData AddNamespaceAndClasses(this TextGenerationTemplate.BlockData data, INamedTypeSymbol classSymbol)
        {
            if (!classSymbol.ContainingNamespace.IsGlobalNamespace)
            {
                data.AddBlock("Namespace")
                    .Add("Name", classSymbol.ContainingNamespace.ToDisplayString());
            }

            data.Add("ClassName", GenerateClassDeclaration(classSymbol));

            while (true)
            {
                classSymbol = classSymbol.ContainingType;

                if (classSymbol == null)
                {
                    break;
                }

                data.AddBlock("Class")
                    .Add("Name", GenerateClassDeclaration(classSymbol));
            }

            var classBlocks = data.SubBlocksData!["Class"];
            classBlocks.ReverseSelf();

            return data;
        }

    }
}
