#nullable enable

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Ks.Common
{
    public partial class TextGenerationTemplate
    {

        private enum BlockType
        {
            Start = 1,
            End = 2,
            Include = 4,
            Block = 8,
            Macro = 16,
            BlockStart = Block | Start,
            BlockEnd = Block | End,
            MacroStart = Macro | Start,
            MacroEnd = Macro | End,
            MacroInclude = Macro | Include
        }

        private TextGenerationTemplate()
        { }

        public static TextGenerationTemplate CreateTemplate(string template, params TextGenerationTemplate[] bases)
        {
            var res = new TextGenerationTemplate();

            foreach (var b in bases)
            {
                foreach (var (n, m) in b.Macros)
                {
                    res.Macros[n] = m;
                }
            }

            res.InitializeBlock(template);

            return res;
        }

        private void InitializeBlock(string template)
        {
            var lines = template.RegexSplit(@"\r\n|\r|\n", RegexOptions.Singleline);
            var blocks = lines.SplitByElement(s => s.RegexIsMatch(BlockRegex, RegexOptions.IgnorePatternWhitespace),
                s =>
                {
                    (BlockType Type, string? Name) res;

                    if (s == null)
                    {
                        res = (default, null);
                        return res;
                    }

                    var m = s.RegexMatch(BlockRegex, RegexOptions.IgnorePatternWhitespace);
                    Assert.True(m.Success);

                    var type = m.Groups[1].Value switch
                    {
                        BlockStart => BlockType.BlockStart,
                        BlockEnd => BlockType.BlockEnd,
                        MacroStart => BlockType.MacroStart,
                        MacroEnd => BlockType.MacroEnd,
                        MacroInclude => BlockType.MacroInclude,
                        _ => throw Assert.Fail(),
                    };

                    s = s.Remove(0, m.Length);
                    Verify.True(s.RegexIsMatch($"^{IdentifierPattern}$"), $"Block or macro identifier contains illegal characters: '{s}'");

                    res = (type, s);
                    return res;
                });

            this.Block = this.CreateBlock(blocks.GetEnumerator().ToEnumerable());
        }

        private SplitsBlock CreateSplits(IEnumerable<string> lines)
        {
            var splits = lines.SelectMany(line =>
            {
                var lineSplits = line.RegexSplit(SplitRegex, RegexOptions.IgnorePatternWhitespace);

                var spRes = lineSplits.Select(s =>
                {
                    (SplitType Type, string? Name, string? Text) res;

                    SplitType type;

                    if (s.StartsWith(PlaceholderStart))
                    {
                        Verify.True(s.EndsWith(PlaceholderEnd), $"Placeholder beginning found without the ending: '{s.TruncateEnd(20)}'");
                        type = SplitType.Placeholder;
                    }
                    else if (s.StartsWith(ConditionalStart))
                    {
                        Verify.True(s.EndsWith(ConditionalEnd), $"Conditional beginning found without the ending: '{s.TruncateEnd(20)}'");
                        type = SplitType.Conditional;
                    }
                    else
                    {
                        Verify.False(s.EndsWith(PlaceholderEnd), $"Placeholder ending found without the beginning: '{s.TruncateStart(20)}'");
                        Verify.False(s.EndsWith(ConditionalEnd), $"Conditional ending found without the beginning: '{s.TruncateStart(20)}'");

                        type = SplitType.Text;
                    }

                    switch (type)
                    {
                        case SplitType.Text:
                        {
                            res = (type, null, s);
                            return res;
                        }

                        case SplitType.Placeholder:
                        {
                            var match = s.RegexMatch(PlaceholderRegex, RegexOptions.IgnorePatternWhitespace);
                            Verify.True(match.Success, $"Placeholder syntax error: '{s.TruncateMid(30)}'");

                            res = (type, match.Groups[1].Value, null);
                            return res;
                        }

                        case SplitType.Conditional:
                        {
                            var match = s.RegexMatch(ConditionalRegex, RegexOptions.IgnorePatternWhitespace);
                            Verify.True(match.Success, $"Conditional syntax error: '{s.TruncateMid(30)}'");

                            res = (type, match.Groups[1].Value, match.Groups[2].Value);
                            return res;
                        }

                        default:
                            throw Assert.Fail();
                    }
                });
                spRes = spRes.AppendElement((SplitType.Text, null, NewLine));

                return spRes;
            }).ToArray();

            return new SplitsBlock(splits);
        }

        private SubBlock CreateBlock(IEnumerable<((BlockType Type, string? Name) Beginning, IEnumerable<string> Items, (BlockType Type, string? Name) Ending)> e)
        {
            string? blockName = null;
            var isMacro = false;

            var subBlocks = new List<BaseBlock>();
            var first = true;
            var endingFound = false;
            foreach (var b in e)
            {
                Assert.True((b.Beginning.Type == default || (b.Beginning.Type & BlockType.Start) == BlockType.Start) == first);

                if (first)
                {
                    blockName = b.Beginning.Name;
                    isMacro = (b.Beginning.Type & BlockType.Macro) == BlockType.Macro;
                    first = false;
                }

                if (b.Beginning.Type == BlockType.MacroInclude)
                {
                    subBlocks.Add(new MacroIncludeBlock(b.Beginning.Name!));
                }

                subBlocks.Add(this.CreateSplits(b.Items));

                if (b.Ending.Type == BlockType.MacroInclude)
                {
                    continue;
                }

                if ((b.Ending.Type & BlockType.End) == BlockType.End)
                {
                    Verify.True(((b.Ending.Type & BlockType.Macro) == BlockType.Macro) == isMacro, $"No ending found for block/macro '{blockName}'.");
                    Verify.True(b.Ending.Name == blockName, $"Beginning and ending of block/macro mismatch: '{blockName}' vs '{b.Ending.Name}'");
                    endingFound = true;
                    break;
                }

                if (blockName == null && b.Ending.Type == default)
                {
                    endingFound = true;
                    break;
                }

                var subBlock = this.CreateBlock(e);
                if ((b.Ending.Type & BlockType.Block) == BlockType.Block)
                {
                    subBlocks.Add(subBlock);
                }
            };

            Verify.True(endingFound, $"No ending found for block/macro '{blockName}'.");

            var res = new SubBlock(isMacro ? null : blockName, subBlocks.ToArray());
            if (isMacro)
            {
                this.Macros[blockName!] = res;
            }
            return res;
        }

        public string Generate(BlockData data)
        {
            var res = new StringBuilder();
            this.Block.Write(res, new List<BlockData>() { data }, this);
            res.Remove(res.Length - 1, 1);
            return res.ToString();
        }

        private SubBlock Block = null!;
        private readonly Dictionary<string, SubBlock> Macros = new Dictionary<string, SubBlock>();

        private static readonly string BlockRegex = @$"
^\s*
(
    {BlockStart.RegexEscape()}
  | {BlockEnd.RegexEscape()}
  | {MacroStart.RegexEscape()}
  | {MacroEnd.RegexEscape()}
  | {MacroInclude.RegexEscape()}
)";

        private static readonly string SplitRegex = @$"
    (?={PlaceholderStart.RegexEscape()})
  | (?<={PlaceholderEnd.RegexEscape()})
  | (?={ConditionalStart.RegexEscape()})
  | (?<={ConditionalEnd.RegexEscape()})
";

        private static readonly string PlaceholderRegex = @$"^
    {PlaceholderStart.RegexEscape()}
        (?:{CommentEnd.RegexEscape()})?
            ({IdentifierPattern})
        (?:{CommentStart.RegexEscape()})?
    {PlaceholderEnd.RegexEscape()}
$";

        private static readonly string ConditionalRegex = @$"^
    {ConditionalStart.RegexEscape()}
        ({IdentifierPattern})
    {ConditionalStartC.RegexEscape()}
        (?:{CommentEnd.RegexEscape()})?
            (.*?)
        (?:{CommentStart.RegexEscape()})?
    {ConditionalEnd.RegexEscape()}
$";

        private const string IdentifierPattern = "[A-Za-z0-9_]+";
        private const string NewLine = "\n";

        private const string CommentStart = "/*";
        private const string CommentEnd = "*/";

        private const string BlockStart = "//[[";
        private const string BlockEnd = "//]]";

        private const string MacroStart = "//#[[";
        private const string MacroEnd = "//#]]";
        private const string MacroInclude = "//#!";

        private const string PlaceholderStart = CommentStart + "{";
        private const string PlaceholderEnd = "}" + CommentEnd;

        private const string ConditionalStart = CommentStart + "?";
        private const string ConditionalStartC = "{";
        private const string ConditionalEnd = "}?" + CommentEnd;
    }
}
