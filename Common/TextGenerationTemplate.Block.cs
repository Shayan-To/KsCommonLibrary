#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Ks.Common
{
    partial class TextGenerationTemplate
    {

        private abstract class BaseBlock
        {
            public abstract void Write(StringBuilder sb, IList<BlockData> data, TextGenerationTemplate parent);
        }

        private enum SplitType
        {
            Text,
            Placeholder,
            Conditional
        }

        private class SplitsBlock : BaseBlock
        {
            public SplitsBlock((SplitType Type, string? Name, string? Text)[] splits)
            {
                this.Splits = splits;
            }

            public override void Write(StringBuilder sb, IList<BlockData> data, TextGenerationTemplate parent)
            {
                foreach (var split in this.Splits)
                {
                    switch (split.Type)
                    {
                        case SplitType.Text:
                            sb.Append(split.Text!);
                            break;
                        case SplitType.Placeholder:
                            foreach (var d in data.Reverse())
                            {
                                if (d.PlaceholdersData.TryGetValue(split.Name!, out var value))
                                {
                                    sb.Append(value);
                                    break;
                                }
                            }
                            break;
                        case SplitType.Conditional:
                            foreach (var d in data.Reverse())
                            {
                                if (d.ConditionalsData.TryGetValue(split.Name!, out var value))
                                {
                                    if (value)
                                    {
                                        sb.Append(split.Text!);
                                    }
                                    break;
                                }
                            }
                            break;
                    }
                }
            }

            private readonly (SplitType Type, string? Name, string? Text)[] Splits;
        }

        private class SubBlock : BaseBlock
        {
            public SubBlock(string? blockName, BaseBlock[] subBlocks)
            {
                this.BlockName = blockName;
                this.SubBlocks = subBlocks;
            }

            public override void Write(StringBuilder sb, IList<BlockData> data, TextGenerationTemplate parent)
            {
                if (this.BlockName == null)
                {
                    this.WriteSubBlocks(sb, data, parent);
                    return;
                }

                var subBlocksData = data.Last().SubBlocksData;
                if (subBlocksData != null)
                {
                    foreach (var d in subBlocksData[this.BlockName])
                    {
                        data.Add(d);
                        this.WriteSubBlocks(sb, data, parent);
                        data.RemoveAt(data.Count - 1);
                    }
                }
            }

            private void WriteSubBlocks(StringBuilder sb, IList<BlockData> data, TextGenerationTemplate parent)
            {
                foreach (var b in this.SubBlocks)
                {
                    b.Write(sb, data, parent);
                }
            }

            private readonly string? BlockName;
            private readonly BaseBlock[] SubBlocks;
        }

        private class MacroIncludeBlock : BaseBlock
        {
            public MacroIncludeBlock(string macroName)
            {
                this.MacroName = macroName;
            }

            public override void Write(StringBuilder sb, IList<BlockData> data, TextGenerationTemplate parent)
            {
                if (parent.Macros.TryGetValue(this.MacroName, out var block))
                {
                    block.Write(sb, data, parent);
                }
            }

            private readonly string MacroName;
        }

    }
}
