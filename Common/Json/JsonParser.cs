//#define RelaxedStrings

using Microsoft.VisualBasic;
using System.Collections.Generic;
using System;
using Microsoft.VisualBasic.CompilerServices;

namespace Ks
{
    namespace Common
    {
        public class JsonParser
        {
            public JsonObject Parse(string Input)
            {
                this.Input = Input.ToCharArray();
                this.Index = 0;
                var Res = this.Parse();
                Verify.True((int)this.ReadToken().Type == (int)TokenType.None, "Invalid JSON format. Expected end of data.");
                this.StringBuilder.Clear();
                return Res;
            }

            private JsonObject Parse()
            {
                var Token = this.ReadToken();

                Verify.False((int)Token.Type == (int)TokenType.None, "Invalid JSON format. Unexpected end of data.");

                if ((int)(Token.Type & TokenType.Value) == (int)TokenType.Value)
                    return new JsonValueObject(Token.Value, (int)Token.Type == (int)TokenType.QuotedValue);

                Assert.True((int)Token.Type == (int)TokenType.Operator);

                Verify.True((Token.Value == "{") | (Token.Value == "["), "Invalid JSON format. Unexpected token.");

                if (Token.Value == "{")
                {
                    var List = new List<KeyValuePair<string, JsonObject>>();

                    Token = this.PeekToken();

                    if (((int)Token.Type == (int)TokenType.Operator) & (Token.Value == "}"))
                    {
                        this.ReadToken();
                        return new JsonDictionaryObject(List);
                    }

                    do
                    {
                        Token = this.ReadToken();
#if RelaxedStrings
                        Verify.True((Token.Type & TokenType.Value) == TokenType.Value, "Invalid JSON format. Key must be a value.");
#else
                        Verify.True((int)Token.Type == (int)TokenType.QuotedValue, "Invalid JSON format. Key must be a string.");
#endif
                        var Key = Token.Value;
                        Token = this.ReadToken();
                        Verify.True(((int)Token.Type == (int)TokenType.Operator) & (Token.Value == ":"), "Invalid JSON format. Expected ':'.");
                        List.Add(new KeyValuePair<string, JsonObject>(Key, this.Parse()));
                        Token = this.ReadToken();
                        Verify.True(((int)Token.Type == (int)TokenType.Operator) & ((Token.Value == ",") | (Token.Value == "}")), "Invalid JSON format. Expected ',' or '}'.");
                        if (Token.Value == "}")
                            break;
                    }
                    while (true);

                    return new JsonDictionaryObject(List);
                }
                else
                {
                    var List = new List<JsonObject>();

                    Token = this.PeekToken();

                    if (((int)Token.Type == (int)TokenType.Operator) & (Token.Value == "]"))
                    {
                        this.ReadToken();
                        return new JsonListObject(List);
                    }

                    do
                    {
                        List.Add(this.Parse());
                        Token = this.ReadToken();
                        Verify.True(((int)Token.Type == (int)TokenType.Operator) & ((Token.Value == ",") | (Token.Value == "]")), "Invalid JSON format. Expected ',' or ']'.");
                        if (Token.Value == "]")
                            break;
                    }
                    while (true);

                    return new JsonListObject(List);
                }
            }

            private Token PeekToken()
            {
                if (!this._PeekedToken.HasValue)
                    this._PeekedToken = this.ReadToken();
                return this._PeekedToken.Value;
            }

            private Token ReadToken()
            {
                if (this._PeekedToken.HasValue)
                {
                    var R = this._PeekedToken.Value;
                    this._PeekedToken = default(Token?);
                    return R;
                }

                this.SkipWhiteSpace();

                if (this.Index == this.Input.Length)
                    return default(Token);

                char Ch = this.Input[this.Index];

                if (Array.IndexOf(Operators, Ch) != -1)
                {
                    this.Index += 1;
                    return new Token(Conversions.ToString(this.Input[this.Index - 1]), TokenType.Operator);
                }

                if (Ch == '"')
                    return new Token(ReadQuotedValue(), TokenType.QuotedValue);

                return new Token(ReadNonQuotedValue(), TokenType.Value);
            }

            private string ReadQuotedValue()
            {
                var Res = this.StringBuilder.Clear();

                Assert.True(this.Input[this.Index] == '"');

                var PrevStart = this.Index + 1;
                var loopTo = this.Input.Length - 1;
                for (var I = this.Index + 1; I <= loopTo; I++)
                {
                    if (this.Input[I] == '"')
                    {
                        Res.Append(this.Input, PrevStart, I - PrevStart);
                        this.Index = I + 1;
                        break;
                    }

                    if (this.Input[I] == '\\')
                    {
                        Res.Append(this.Input, PrevStart, I - PrevStart);

                        I += 1;
                        Verify.True(I < this.Input.Length, "Invalid JSON format. Unexpected end of data.");

                        if (this.Input[I] == 'u')
                        {
                            Res.Append(this.GetCharFromHex(I + 1));
                            I += 4;
                        }
                        else
                        {
                            char Ch = default(char);
                            Verify.True(EscapeDic.TryGetValue(this.Input[I], out Ch), "Invalid JSON format. Invalid escape sequence.");
                            Res.Append(Ch);
                        }

                        PrevStart = I + 1;

                        continue;
                    }
                }

                return Res.ToString();
            }

            private char GetCharFromHex(int I)
            {
                Verify.True((I + 3) < this.Input.Length, "Invalid JSON format. Unexpected end of data.");

                var R = 0;
                var loopTo = I + 3;
                for (I = I; I <= loopTo; I++)
                {
                    var Ch = this.Input[I];
                    R *= 16;
                    if (('0' <= Ch) & (Ch <= '9'))
                        R += Strings.AscW(Ch) - Strings.AscW('0');
                    else if (('a' <= Ch) & (Ch <= 'f'))
                        R += (Strings.AscW(Ch) - Strings.AscW('a')) + 10;
                    else if (('A' <= Ch) & (Ch <= 'F'))
                        R += (Strings.AscW(Ch) - Strings.AscW('A')) + 10;
                    else
                        Verify.Fail("Invalid JSON format. Invalid escape sequence.");
                }

                return (char)R;
            }

            private string ReadNonQuotedValue()
            {
                var StartIndex = this.Index;
                var loopTo = this.Input.Length - 1;
                for (this.Index = this.Index; this.Index <= loopTo; this.Index++)
                {
                    if (char.IsWhiteSpace(this.Input[this.Index]))
                        break;
                    if (Array.IndexOf(Operators, this.Input[this.Index]) != -1)
                        break;
                }

                return new string(this.Input, StartIndex, this.Index - StartIndex);
            }

            private void SkipWhiteSpace()
            {
                while (this.Index < this.Input.Length && char.IsWhiteSpace(this.Input[this.Index]))
                    this.Index += 1;
            }

            private static readonly Dictionary<char, char> EscapeDic = (() => new Dictionary<char, char>()
            {
                {
                    '"',
                    '"'
                },
                {
                    '/',
                    '/'
                },
                {
                    '\\',
                    '\\'
                },
                {
                    'b',
                    (char)0x8
                },
                {
                    'f',
                    (char)0xC
                },
                {
                    'n',
                    (char)0xA
                },
                {
                    'r',
                    (char)0xD
                },
                {
                    't',
                    (char)0x9
                }
            }).Invoke();
            private static readonly char[] Operators = "{}[],:".ToCharArray();

            private readonly System.Text.StringBuilder StringBuilder = new System.Text.StringBuilder();

            private Token? _PeekedToken;

            private char[] Input;
            private int Index;

            private struct Token
            {
                public Token(string Value, TokenType Type)
                {
                    this.Value = Value;
                    this.Type = Type;
                }

                public readonly string Value;
                public readonly TokenType Type;
            }

            private enum TokenType
            {
                None = 0,
                Value = 1,
                QuotedValue = 3,
                Operator = 4
            }
        }
    }
}
