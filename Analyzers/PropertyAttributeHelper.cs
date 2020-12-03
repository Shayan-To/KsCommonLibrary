using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Ks.Common;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;

using Attribute = Ks.Common.PropertyAttributeAttribute;

#pragma warning disable RS2008 // Enable analyzer release tracking

namespace Ks.Analyzers
{
    public class PropertyAttributeHelper
    {

        public static readonly DiagnosticDescriptor PropertyAttributeMustHaveSameLengthsOfNamesAndValuesRule =
            new DiagnosticDescriptor(
                nameof(PropertyAttributeMustHaveSameLengthsOfNamesAndValuesRule),
                nameof(PropertyAttributeMustHaveSameLengthsOfNamesAndValuesRule),
                "PropertyAttribute must have same lengths of Names and Values.",
                "Usage",
                DiagnosticSeverity.Error,
                true);

        public PropertyAttributeHelper(GeneratorExecutionContext context)
        {
            this.Context = context;
            this.AttributeSymbol = ((CSharpCompilation) this.Context.Compilation).GetExistingTypeByMetadataName(typeof(Attribute).FullName);
        }

        private AttributeListSyntax? GenerateAttribute(ISymbol symbol, AttributeData data)
        {
            Verify.TrueArg(data.AttributeClass?.ToDisplayString() == typeof(Attribute).FullName, nameof(data));

            var typeSymbol = (INamedTypeSymbol) data.ConstructorArguments.First().Value!;
            var ctorArgs = data.ConstructorArguments.Last().Values;

            string[]? argNames = null;
            ImmutableArray<TypedConstant> argValues = default;
            {
                var kv = data.NamedArguments.SingleOrDefault(a => a.Key == nameof(Attribute.Names));
                if (kv.Key != null)
                {
                    argNames = kv.Value.Values.Select(v => (string) v.Value!).ToArray();
                }
            }
            {
                var kv = data.NamedArguments.SingleOrDefault(a => a.Key == nameof(Attribute.Values));
                if (kv.Key != null)
                {
                    argValues = kv.Value.Values;
                }
            }

            if (!((argNames != null && argValues != default && argNames.Length == argValues.Length) || (argNames == null && argValues == default)))
            {
                this.Context.ReportDiagnostic(Diagnostic.Create(PropertyAttributeMustHaveSameLengthsOfNamesAndValuesRule, symbol.Locations.First()));
                return null;
            }

            var ArgsList = new List<AttributeArgumentSyntax>();

            foreach (var v in ctorArgs)
            {
                ArgsList.Add(SyntaxFactory.AttributeArgument(v.ToExpression()));
            }

            if (argNames != null)
            {
                for (var i = 0; i < argNames.Length; i++)
                {
                    ArgsList.Add(SyntaxFactory.AttributeArgument(SyntaxFactory.NameEquals(argNames[i]), null, argValues[i].ToExpression()));
                }
            }

            return
                SyntaxFactory.AttributeList(
                    SyntaxFactory.SingletonSeparatedList(
                        SyntaxFactory.Attribute(
                            SyntaxFactory.ParseName(typeSymbol.ToDisplayString(), consumeFullText: true),
                            SyntaxFactory.AttributeArgumentList(SyntaxFactory.SeparatedList(ArgsList)))))
                .NormalizeWhitespace();
        }

        public IEnumerable<AttributeListSyntax> GenerateAttributes(ISymbol symbol)
        {
            return symbol.GetAttributes()
                .Where(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, this.AttributeSymbol))
                .Select(a => this.GenerateAttribute(symbol, a))
                .Where(a => a != null)!;
        }

        public INamedTypeSymbol AttributeSymbol { get; }
        public GeneratorExecutionContext Context { get; }

    }
}
