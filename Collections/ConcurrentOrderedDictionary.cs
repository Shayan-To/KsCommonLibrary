using System.Collections.Generic;
using System.Collections;

namespace Ks
{
    namespace Common
    {
        public class ConcurrentOrderedDictionary<TKey, TValue> : BaseDictionary<TKey, TValue>, IOrderedDictionary<TKey, TValue>
        {
            public ConcurrentOrderedDictionary() : this(new OrderedDictionary<TKey, TValue>())
            {
            }

            public ConcurrentOrderedDictionary(OrderedDictionary<TKey, TValue> BaseDictionary) : this(BaseDictionary, new object())
            {
            }

            public ConcurrentOrderedDictionary(OrderedDictionary<TKey, TValue> BaseDictionary, object LockObject)
            {
                this.BaseDic = BaseDictionary;
                this.LockObject = LockObject;
            }

            public override int Count
            {
                get
                {
                    lock (this.LockObject)
                        return this.BaseDic.Count;
                }
            }

            public override TValue this[TKey key]
            {
                get
                {
                    lock (this.LockObject)
                        return this.BaseDic[key];
                }
                set
                {
                    lock (this.LockObject)
                        this.BaseDic[key] = value;
                }
            }

            public override ICollection<TKey> Keys
            {
                get
                {
                    return (ICollection<TKey>)this.KeysList;
                }
            }

            public override ICollection<TValue> Values
            {
                get
                {
                    return (ICollection<TValue>)this.ValuesList;
                }
            }

            public IReadOnlyList<TKey> KeysList
            {
                get
                {
                    lock (this.LockObject)
                    {
                        ;
#error Cannot convert LocalDeclarationStatementSyntax - see comment for details
                        /* Cannot convert LocalDeclarationStatementSyntax, System.NotSupportedException: StaticKeyword not supported!
                           at ICSharpCode.CodeConverter.CSharp.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
                           at ICSharpCode.CodeConverter.CSharp.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
                           at ICSharpCode.CodeConverter.CSharp.CommonConversions.<ConvertModifiersCore>d__23.MoveNext()
                           at System.Linq.Enumerable.WhereEnumerableIterator`1.MoveNext()
                           at Microsoft.CodeAnalysis.SyntaxTokenList.CreateNode(IEnumerable`1 tokens)
                           at Microsoft.CodeAnalysis.CSharp.SyntaxFactory.TokenList(IEnumerable`1 tokens)
                           at ICSharpCode.CodeConverter.CSharp.CommonConversions.ConvertModifiers(IEnumerable`1 modifiers, TokenContext context, Boolean isVariableOrConst, Boolean isConstructor)
                           at ICSharpCode.CodeConverter.CSharp.VisualBasicConverter.MethodBodyVisitor.VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
                           at Microsoft.CodeAnalysis.VisualBasic.Syntax.LocalDeclarationStatementSyntax.Accept[TResult](VisualBasicSyntaxVisitor`1 visitor)
                           at Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxVisitor`1.Visit(SyntaxNode node)
                           at ICSharpCode.CodeConverter.CSharp.VisualBasicConverter.ByRefParameterVisitor.AddLocalVariables(VisualBasicSyntaxNode node)
                           at ICSharpCode.CodeConverter.CSharp.VisualBasicConverter.ByRefParameterVisitor.VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
                           at Microsoft.CodeAnalysis.VisualBasic.Syntax.LocalDeclarationStatementSyntax.Accept[TResult](VisualBasicSyntaxVisitor`1 visitor)
                           at Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxVisitor`1.Visit(SyntaxNode node)
                           at ICSharpCode.CodeConverter.CSharp.CommentConvertingMethodBodyVisitor.ConvertWithTrivia(SyntaxNode node)
                           at ICSharpCode.CodeConverter.CSharp.CommentConvertingMethodBodyVisitor.DefaultVisit(SyntaxNode node)

                        Input: 
                                            Static R As IReadOnlyList(Of TKey) = New ConcurrentList(Of TKey)(DirectCast(Me.BaseDic.KeysList, IList(Of TKey)), Me.LockObject)

                         */
                        return R;
                    }
                }
            }

            public IReadOnlyList<TValue> ValuesList
            {
                get
                {
                    lock (this.LockObject)
                    {
                        ;
#error Cannot convert LocalDeclarationStatementSyntax - see comment for details
                        /* Cannot convert LocalDeclarationStatementSyntax, System.NotSupportedException: StaticKeyword not supported!
                           at ICSharpCode.CodeConverter.CSharp.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
                           at ICSharpCode.CodeConverter.CSharp.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
                           at ICSharpCode.CodeConverter.CSharp.CommonConversions.<ConvertModifiersCore>d__23.MoveNext()
                           at System.Linq.Enumerable.WhereEnumerableIterator`1.MoveNext()
                           at Microsoft.CodeAnalysis.SyntaxTokenList.CreateNode(IEnumerable`1 tokens)
                           at Microsoft.CodeAnalysis.CSharp.SyntaxFactory.TokenList(IEnumerable`1 tokens)
                           at ICSharpCode.CodeConverter.CSharp.CommonConversions.ConvertModifiers(IEnumerable`1 modifiers, TokenContext context, Boolean isVariableOrConst, Boolean isConstructor)
                           at ICSharpCode.CodeConverter.CSharp.VisualBasicConverter.MethodBodyVisitor.VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
                           at Microsoft.CodeAnalysis.VisualBasic.Syntax.LocalDeclarationStatementSyntax.Accept[TResult](VisualBasicSyntaxVisitor`1 visitor)
                           at Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxVisitor`1.Visit(SyntaxNode node)
                           at ICSharpCode.CodeConverter.CSharp.VisualBasicConverter.ByRefParameterVisitor.AddLocalVariables(VisualBasicSyntaxNode node)
                           at ICSharpCode.CodeConverter.CSharp.VisualBasicConverter.ByRefParameterVisitor.VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
                           at Microsoft.CodeAnalysis.VisualBasic.Syntax.LocalDeclarationStatementSyntax.Accept[TResult](VisualBasicSyntaxVisitor`1 visitor)
                           at Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxVisitor`1.Visit(SyntaxNode node)
                           at ICSharpCode.CodeConverter.CSharp.CommentConvertingMethodBodyVisitor.ConvertWithTrivia(SyntaxNode node)
                           at ICSharpCode.CodeConverter.CSharp.CommentConvertingMethodBodyVisitor.DefaultVisit(SyntaxNode node)

                        Input: 
                                            Static R As IReadOnlyList(Of TValue) = Me.KeysList.SelectAsList(Function(K) Me.Item(K))

                         */
                        return R;
                    }
                }
            }

            public KeyValuePair<TKey, TValue> ItemAt
            {
                get
                {
                    lock (this.LockObject)
                        return this.BaseDic.ItemAt;
                }
                set
                {
                    lock (this.LockObject)
                        this.BaseDic.ItemAt = value;
                }
            }

            private bool IList_IsReadOnly
            {
                get
                {
                    return false;
                }
            }

            private bool IList_IsFixedSize
            {
                get
                {
                    return false;
                }
            }

            public override void Add(TKey key, TValue value)
            {
                lock (this.LockObject)
                    this.BaseDic.Add(key, value);
            }

            public override void Clear()
            {
                lock (this.LockObject)
                    this.BaseDic.Clear();
            }

            public void Insert(int index, TKey key, TValue value)
            {
                lock (this.LockObject)
                    this.BaseDic.Insert(index, key, value);
            }

            public void RemoveAt(int index)
            {
                lock (this.LockObject)
                    this.BaseDic.RemoveAt(index);
            }

            public override bool ContainsKey(TKey key)
            {
                lock (this.LockObject)
                    return this.BaseDic.ContainsKey(key);
            }

            public int IndexOf(TKey key)
            {
                lock (this.LockObject)
                    return this.BaseDic.IndexOf(key);
            }

            public override bool Remove(TKey key)
            {
                lock (this.LockObject)
                    return this.BaseDic.Remove(key);
            }

            public override bool TryGetValue(TKey key, ref TValue value)
            {
                lock (this.LockObject)
                    return this.BaseDic.TryGetValue(key, ref value);
            }

            public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
            {
                lock (this.LockObject)
                {
                    foreach (var KV in this.BaseDic)
                        yield return KV;
                }
            }

            protected override IEnumerator<KeyValuePair<TKey, TValue>> IEnumerator_1_GetEnumerator()
            {
                return this.GetEnumerator();
            }

            private object IList_ItemAt
            {
                get
                {
                    return this.ItemAt;
                }
                set
                {
                    this.ItemAt = (KeyValuePair<TKey, TValue>)value;
                }
            }

            private int IList_Add(object value)
            {
                lock (this.LockObject)
                {
                    this.ICollection_Add((KeyValuePair<TKey, TValue>)value);
                    return this.Count - 1;
                }
            }

            private void IList_Insert(int index, KeyValuePair<TKey, TValue> item)
            {
                this.Insert(index, item.Key, item.Value);
            }

            private void IList_Insert(int index, object value)
            {
                this.IList_Insert(index, (KeyValuePair<TKey, TValue>)value);
            }

            private void IOrderedDictionary_Insert(int index, object key, object value)
            {
                this.Insert(index, (TKey)key, (TValue)value);
            }

            private void IList_Remove(object value)
            {
                this.ICollection_Remove((KeyValuePair<TKey, TValue>)value);
            }

            private int IList_IndexOf(KeyValuePair<TKey, TValue> item)
            {
                int R = default(int);
                TValue T;

                lock (this.LockObject)
                {
                    R = this.IndexOf(item.Key);
                    T = this.ItemAt.Value;
                }

                if (R == -1)
                    return -1;
                if (!object.Equals(item.Value, T))
                    return -1;
                return R;
            }

            private int IList_IndexOf(object value)
            {
                return this.IList_IndexOf((KeyValuePair<TKey, TValue>)value);
            }

            private IDictionaryEnumerator IOrderedDictionary_GetEnumerator()
            {
                return this.IDictionary_GetEnumerator();
            }

            private bool IList_Contains(object value)
            {
                return this.ICollection_Contains((KeyValuePair<TKey, TValue>)value);
            }

            private readonly OrderedDictionary<TKey, TValue> BaseDic;
            private readonly object LockObject;
        }
    }
}
