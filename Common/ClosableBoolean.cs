using System;

namespace Ks
{
    namespace Common
    {
        [Obsolete("Cannot be used due to stupidity of the compiler.")]
        public struct ClosableBoolean : IDisposable
        {
            public static implicit operator bool(ClosableBoolean O)
            {
                return O._Value;
            }

            public static implicit operator ClosableBoolean(bool O)
            {
                return new ClosableBoolean() { _Value = O };
            }

            public static bool operator !(ClosableBoolean O)
            {
                return !O._Value;
            }

            public static bool operator &(ClosableBoolean O1, bool O2)
            {
                return O1._Value & O2;
            }

            public static bool operator |(ClosableBoolean O1, bool O2)
            {
                return O1._Value | O2;
            }

            class _failedMemberConversionMarker1
            {
            }
#error Cannot convert OperatorBlockSyntax - see comment for details
            /* Cannot convert OperatorBlockSyntax, System.NotSupportedException: XorKeyword not supported!
               at ICSharpCode.CodeConverter.CSharp.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
               at ICSharpCode.CodeConverter.CSharp.SyntaxKindExtensions.ConvertToken(SyntaxToken t, TokenContext context)
               at ICSharpCode.CodeConverter.CSharp.VisualBasicConverter.NodesVisitor.VisitOperatorStatement(OperatorStatementSyntax node)
               at Microsoft.CodeAnalysis.VisualBasic.Syntax.OperatorStatementSyntax.Accept[TResult](VisualBasicSyntaxVisitor`1 visitor)
               at Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxVisitor`1.Visit(SyntaxNode node)
               at ICSharpCode.CodeConverter.CSharp.CommentConvertingNodesVisitor.DefaultVisit(SyntaxNode node)
               at Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxVisitor`1.VisitOperatorStatement(OperatorStatementSyntax node)
               at Microsoft.CodeAnalysis.VisualBasic.Syntax.OperatorStatementSyntax.Accept[TResult](VisualBasicSyntaxVisitor`1 visitor)
               at ICSharpCode.CodeConverter.CSharp.VisualBasicConverter.NodesVisitor.VisitOperatorBlock(OperatorBlockSyntax node)
               at Microsoft.CodeAnalysis.VisualBasic.Syntax.OperatorBlockSyntax.Accept[TResult](VisualBasicSyntaxVisitor`1 visitor)
               at Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxVisitor`1.Visit(SyntaxNode node)
               at ICSharpCode.CodeConverter.CSharp.CommentConvertingNodesVisitor.DefaultVisit(SyntaxNode node)
               at Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxVisitor`1.VisitOperatorBlock(OperatorBlockSyntax node)
               at Microsoft.CodeAnalysis.VisualBasic.Syntax.OperatorBlockSyntax.Accept[TResult](VisualBasicSyntaxVisitor`1 visitor)
               at ICSharpCode.CodeConverter.CSharp.VisualBasicConverter.NodesVisitor.ConvertMember(StatementSyntax member)

            Input: 

                    Public Shared Operator Xor(ByVal O1 As ClosableBoolean, ByVal O2 As Boolean) As Boolean
                        Return O1._Value Xor O2
                    End Operator

             */
            public static bool operator &(bool O1, ClosableBoolean O2)
            {
                return O1 & O2._Value;
            }

            public static bool operator |(bool O1, ClosableBoolean O2)
            {
                return O1 | O2._Value;
            }

            class _failedMemberConversionMarker2
            {
            }
#error Cannot convert OperatorBlockSyntax - see comment for details
            /* Cannot convert OperatorBlockSyntax, System.NotSupportedException: XorKeyword not supported!
               at ICSharpCode.CodeConverter.CSharp.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
               at ICSharpCode.CodeConverter.CSharp.SyntaxKindExtensions.ConvertToken(SyntaxToken t, TokenContext context)
               at ICSharpCode.CodeConverter.CSharp.VisualBasicConverter.NodesVisitor.VisitOperatorStatement(OperatorStatementSyntax node)
               at Microsoft.CodeAnalysis.VisualBasic.Syntax.OperatorStatementSyntax.Accept[TResult](VisualBasicSyntaxVisitor`1 visitor)
               at Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxVisitor`1.Visit(SyntaxNode node)
               at ICSharpCode.CodeConverter.CSharp.CommentConvertingNodesVisitor.DefaultVisit(SyntaxNode node)
               at Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxVisitor`1.VisitOperatorStatement(OperatorStatementSyntax node)
               at Microsoft.CodeAnalysis.VisualBasic.Syntax.OperatorStatementSyntax.Accept[TResult](VisualBasicSyntaxVisitor`1 visitor)
               at ICSharpCode.CodeConverter.CSharp.VisualBasicConverter.NodesVisitor.VisitOperatorBlock(OperatorBlockSyntax node)
               at Microsoft.CodeAnalysis.VisualBasic.Syntax.OperatorBlockSyntax.Accept[TResult](VisualBasicSyntaxVisitor`1 visitor)
               at Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxVisitor`1.Visit(SyntaxNode node)
               at ICSharpCode.CodeConverter.CSharp.CommentConvertingNodesVisitor.DefaultVisit(SyntaxNode node)
               at Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxVisitor`1.VisitOperatorBlock(OperatorBlockSyntax node)
               at Microsoft.CodeAnalysis.VisualBasic.Syntax.OperatorBlockSyntax.Accept[TResult](VisualBasicSyntaxVisitor`1 visitor)
               at ICSharpCode.CodeConverter.CSharp.VisualBasicConverter.NodesVisitor.ConvertMember(StatementSyntax member)

            Input: 

                    Public Shared Operator Xor(ByVal O1 As Boolean, ByVal O2 As ClosableBoolean) As Boolean
                        Return O1 Xor O2._Value
                    End Operator

             */
            class _failedMemberConversionMarker3
            {
            }
#error Cannot convert OperatorBlockSyntax - see comment for details
            /* Cannot convert OperatorBlockSyntax, System.NotSupportedException: IsTrueKeyword not supported!
               at ICSharpCode.CodeConverter.CSharp.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
               at ICSharpCode.CodeConverter.CSharp.SyntaxKindExtensions.ConvertToken(SyntaxToken t, TokenContext context)
               at ICSharpCode.CodeConverter.CSharp.VisualBasicConverter.NodesVisitor.VisitOperatorStatement(OperatorStatementSyntax node)
               at Microsoft.CodeAnalysis.VisualBasic.Syntax.OperatorStatementSyntax.Accept[TResult](VisualBasicSyntaxVisitor`1 visitor)
               at Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxVisitor`1.Visit(SyntaxNode node)
               at ICSharpCode.CodeConverter.CSharp.CommentConvertingNodesVisitor.DefaultVisit(SyntaxNode node)
               at Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxVisitor`1.VisitOperatorStatement(OperatorStatementSyntax node)
               at Microsoft.CodeAnalysis.VisualBasic.Syntax.OperatorStatementSyntax.Accept[TResult](VisualBasicSyntaxVisitor`1 visitor)
               at ICSharpCode.CodeConverter.CSharp.VisualBasicConverter.NodesVisitor.VisitOperatorBlock(OperatorBlockSyntax node)
               at Microsoft.CodeAnalysis.VisualBasic.Syntax.OperatorBlockSyntax.Accept[TResult](VisualBasicSyntaxVisitor`1 visitor)
               at Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxVisitor`1.Visit(SyntaxNode node)
               at ICSharpCode.CodeConverter.CSharp.CommentConvertingNodesVisitor.DefaultVisit(SyntaxNode node)
               at Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxVisitor`1.VisitOperatorBlock(OperatorBlockSyntax node)
               at Microsoft.CodeAnalysis.VisualBasic.Syntax.OperatorBlockSyntax.Accept[TResult](VisualBasicSyntaxVisitor`1 visitor)
               at ICSharpCode.CodeConverter.CSharp.VisualBasicConverter.NodesVisitor.ConvertMember(StatementSyntax member)

            Input: 

                    Public Shared Operator IsTrue(ByVal O As ClosableBoolean) As Boolean
                        Return O._Value
                    End Operator

             */
            class _failedMemberConversionMarker4
            {
            }
#error Cannot convert OperatorBlockSyntax - see comment for details
            /* Cannot convert OperatorBlockSyntax, System.NotSupportedException: IsFalseKeyword not supported!
               at ICSharpCode.CodeConverter.CSharp.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
               at ICSharpCode.CodeConverter.CSharp.SyntaxKindExtensions.ConvertToken(SyntaxToken t, TokenContext context)
               at ICSharpCode.CodeConverter.CSharp.VisualBasicConverter.NodesVisitor.VisitOperatorStatement(OperatorStatementSyntax node)
               at Microsoft.CodeAnalysis.VisualBasic.Syntax.OperatorStatementSyntax.Accept[TResult](VisualBasicSyntaxVisitor`1 visitor)
               at Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxVisitor`1.Visit(SyntaxNode node)
               at ICSharpCode.CodeConverter.CSharp.CommentConvertingNodesVisitor.DefaultVisit(SyntaxNode node)
               at Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxVisitor`1.VisitOperatorStatement(OperatorStatementSyntax node)
               at Microsoft.CodeAnalysis.VisualBasic.Syntax.OperatorStatementSyntax.Accept[TResult](VisualBasicSyntaxVisitor`1 visitor)
               at ICSharpCode.CodeConverter.CSharp.VisualBasicConverter.NodesVisitor.VisitOperatorBlock(OperatorBlockSyntax node)
               at Microsoft.CodeAnalysis.VisualBasic.Syntax.OperatorBlockSyntax.Accept[TResult](VisualBasicSyntaxVisitor`1 visitor)
               at Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxVisitor`1.Visit(SyntaxNode node)
               at ICSharpCode.CodeConverter.CSharp.CommentConvertingNodesVisitor.DefaultVisit(SyntaxNode node)
               at Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxVisitor`1.VisitOperatorBlock(OperatorBlockSyntax node)
               at Microsoft.CodeAnalysis.VisualBasic.Syntax.OperatorBlockSyntax.Accept[TResult](VisualBasicSyntaxVisitor`1 visitor)
               at ICSharpCode.CodeConverter.CSharp.VisualBasicConverter.NodesVisitor.ConvertMember(StatementSyntax member)

            Input: 

                    Public Shared Operator IsFalse(ByVal O As ClosableBoolean) As Boolean
                        Return Not O._Value
                    End Operator

             */
            public void Dispose()
            {
                this._Value = false;
            }

            private bool _Value;

            public bool Value
            {
                get
                {
                    return this._Value;
                }
            }
        }
    }
}
