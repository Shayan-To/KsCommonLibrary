//#define RelaxedStrings

using System.Collections.Generic;
using System;

namespace Ks.Common
{
        public class JsonParser
        {
            public JsonObject Parse(string Input)
            {
                this.Input = Input.ToCharArray();
                this.Index = 0;
                var Res = this.Parse();
                Verify.True(this.ReadToken().Type == TokenType.None, "Invalid JSON format. Expected end of data.");
                this.StringBuilder.Clear();
                return Res;
            }

            private JsonObject Parse()
            {
                var Token = this.ReadToken();

                Verify.False(Token.Type == TokenType.None, "Invalid JSON format. Unexpected end of data.");

                if ((Token.Type & TokenType.Value) == TokenType.Value)
                    return new JsonValueObject(Token.Value, Token.Type == TokenType.QuotedValue);

                Assert.True(Token.Type == TokenType.Operator);

                Verify.True((Token.Value == "{") | (Token.Value == "["), "Invalid JSON format. Unexpected token.");

                if (Token.Value == "{")
                {
                    var List = new List<KeyValuePair<string, JsonObject>>();

                    Token = this.PeekToken();

                    if ((Token.Type == TokenType.Operator) & (Token.Value == "}"))
                    {
                        this.ReadToken();
                        return new JsonDictionaryObject(List);
                    }

                    while (true)
                    {
                        Token = this.ReadToken();
#if RelaxedStrings
                        Verify.True((Token.Type & TokenType.Value) == TokenType.Value, "Invalid JSON format. Key must be a value.");
#else
                        Verify.True(Token.Type == TokenType.QuotedValue, "Invalid JSON format. Key must be a string.");
#endif
                        var Key = Token.Value;
                        Token = this.ReadToken();
                        Verify.True((Token.Type == TokenType.Operator) & (Token.Value == ":"), "Invalid JSON format. Expected ':'.");
                        List.Add(new KeyValuePair<string, JsonObject>(Key, this.Parse()));
                        Token = this.ReadToken();
                        Verify.True((Token.Type == TokenType.Operator) & ((Token.Value == ",") | (Token.Value == "}")), "Invalid JSON format. Expected ',' or '}'.");
                        if (Token.Value == "}")
                            break;
                    }

                    return new JsonDictionaryObject(List);
                }
                else
                {
                    var List = new List<JsonObject>();

                    Token = this.PeekToken();

                    if ((Token.Type == TokenType.Operator) & (Token.Value == "]"))
                    {
                        this.ReadToken();
                        return new JsonListObject(List);
                    }

                    while (true)
                    {
                        List.Add(this.Parse());
                        Token = this.ReadToken();
                        Verify.True((Token.Type == TokenType.Operator) & ((Token.Value == ",") | (Token.Value == "]")), "Invalid JSON format. Expected ',' or ']'.");
                        if (Token.Value == "]")
                            break;
                    }

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
                    this._PeekedToken = default;
                    return R;
                }

                this.SkipWhiteSpace();

                if (this.Index == this.Input.Length)
                    return default;

                var Ch = this.Input[this.Index];

                if (Array.IndexOf(Operators, Ch) != -1)
                {
                    this.Index += 1;
                    return new Token(this.Input[this.Index - 1].ToString(), TokenType.Operator);
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
                for (var I = this.Index + 1; I < this.Input.Length; I++)
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
                            var Ch = default(char);
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
                var end = I + 4;
                for (; I < end; I++)
                {
                    var Ch = this.Input[I];
                    R *= 16;
                    if (('0' <= Ch) & (Ch <= '9'))
                        R += Ch - '0';
                    else if (('a' <= Ch) & (Ch <= 'f'))
                        R += Ch - 'a' + 10;
                    else if (('A' <= Ch) & (Ch <= 'F'))
                        R += Ch - 'A' + 10;
                    else
                        Verify.Fail("Invalid JSON format. Invalid escape sequence.");
                }

                return (char)R;
            }

            private string ReadNonQuotedValue()
            {
                var StartIndex = this.Index;
                for (; this.Index < this.Input.Length; this.Index++)
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

            private static readonly Dictionary<char, char> EscapeDic = new Dictionary<char, char>()
            {
                {'"', '"'},
                {'/', '/'},
                {'\\', '\\'},
                {'b', (char)0x08},
                {'f', (char)0x0C},
                {'n', (char)0x0A},
                {'r', (char)0x0D},
                {'t', (char)0x09}
            };
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
