#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;

namespace Ks.Common
{
    partial class TextGenerationTemplate
    {

        public static BlockData CreateData()
        {
            return new BlockData();
        }

        public class BlockData
        {

            public BlockData Add(string placeholderName, string value)
            {
                this.PlaceholdersData.Add(placeholderName, value);
                return this;
            }

            public BlockData Add(string placeholderName, bool value)
            {
                this.ConditionalsData.Add(placeholderName, value);
                return this;
            }

            public BlockData AddBlock(string blockName)
            {
                var data = new BlockData();
                if (this.SubBlocksData == null)
                {
                    this.SubBlocksData = new MultiDictionary<string, BlockData>();
                }
                this.SubBlocksData[blockName].Add(data);
                return data;
            }

            public MultiDictionary<string, BlockData>? SubBlocksData { get; private set; }
            public Dictionary<string, string> PlaceholdersData { get; } = new Dictionary<string, string>();
            public Dictionary<string, bool> ConditionalsData { get; } = new Dictionary<string, bool>();

        }

    }
}
