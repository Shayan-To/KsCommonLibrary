using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace Ks
{
    namespace Common
    {
        public class OrderedDictionary<TKey, TValue> : BaseDictionary<TKey, TValue>, IOrderedDictionary<TKey, TValue>
        {

            // ToDo Do the current done changes (in this commit) to the other ordered dictionaries.

            public OrderedDictionary() : this(EqualityComparer<TKey>.Default)
            {
            }

            public OrderedDictionary(IEqualityComparer<TKey> Comparer)
            {
                this.Comparer = Comparer;
                this._Dic = new Dictionary<TKey, TValue>(Comparer);
            }

            public override int Count
            {
                get
                {
                    return this._Keys.Count;
                }
            }

            /// <summary>
        /// When setting, adds the value to the end of collection if not present.
        /// </summary>
            public override TValue this[TKey key]
            {
                get
                {
                    return this._Dic[key];
                }
                set
                {
                    if (!this._Dic.ContainsKey(key))
                        this._Keys.Add(key);
                    this._Dic[key] = value;
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
                                    Static R As IReadOnlyList(Of TKey) = Me._Keys.AsReadOnly()

                     */
                    return R;
                }
            }

            public IReadOnlyList<TValue> ValuesList
            {
                get
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
                                    Static R As IReadOnlyList(Of TValue) = Me._Keys.SelectAsList(Function(K) Me._Dic.Item(K))

                     */
                    return R;
                }
            }

            public KeyValuePair<TKey, TValue> ItemAt
            {
                get
                {
                    var Key = this._Keys[index];
                    return new KeyValuePair<TKey, TValue>(Key, this._Dic[Key]);
                }
                set
                {
                    var Key = this._Keys[index];

                    Assert.True(this._Dic.Remove(Key));

                    this._Dic.Add(value.Key, value.Value);
                    this._Keys[index] = value.Key;
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
                this._Keys.Add(key);
                this._Dic.Add(key, value);
            }

            public override void Clear()
            {
                this._Keys.Clear();
                this._Dic.Clear();
            }

            public void Insert(int index, TKey key, TValue value)
            {
                this._Keys.Insert(index, key);
                this._Dic.Add(key, value);
            }

            public void RemoveAt(int index)
            {
                var Key = this._Keys[index];
                this._Keys.RemoveAt(index);
                Assert.True(this._Dic.Remove(Key));
            }

            public override bool ContainsKey(TKey key)
            {
                return this._Dic.ContainsKey(key);
            }

            public int IndexOf(TKey key)
            {
                var loopTo = this._Keys.Count - 1;
                for (int I = 0; I <= loopTo; I++)
                {
                    if (this.Comparer.Equals(key, this._Keys[I]))
                        return I;
                }
                return -1;
            }

            public override bool Remove(TKey key)
            {
                if (!this._Dic.Remove(key))
                    return false;
                var I = this.IndexOf(key);
                Assert.True(I != -1);
                this._Keys.RemoveAt(I);
                return true;
            }

            public override bool TryGetValue(TKey key, ref TValue value)
            {
                return this._Dic.TryGetValue(key, out value);
            }

            public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
            {
                return this._Keys.Select(K => new KeyValuePair<TKey, TValue>(K, this._Dic[K])).GetEnumerator();
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
                this.ICollection_Add((KeyValuePair<TKey, TValue>)value);
                return this.Count - 1;
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
                var R = this.IndexOf(item.Key);
                if (R == -1)
                    return -1;
                if (!object.Equals(item.Value, this.ItemAt.Value))
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

            private readonly IEqualityComparer<TKey> Comparer;
            private readonly Dictionary<TKey, TValue> _Dic;
            private readonly List<TKey> _Keys = new List<TKey>();
        }
    }
}
