using System;

namespace Ks.Common
{
    public class XmlFormatter : StringFormatterBase
    {
        public XmlFormatter()
        {
            this.Serializers.Add(Serializer<string>.Create(nameof(String), F =>
            {
                return ((XmlFormatter) F.Formatter).OnGetString();
            }, null, (F, O) =>
            {
                ((XmlFormatter) F.Formatter).OnSetString(O);
            }));
        }

        protected override void OnGetStarted()
        {
        }

        protected override void OnGetEnterContext(string Name)
        {
            Name = Name ?? "Value";
            if (Name != null)
            {
                this.XmlReader.ReadStartElement(Name);
            }
        }

        internal string OnGetString()
        {
            return this.XmlReader.ReadElementContentAsString();
        }

        protected override void OnGetExitContext(string Name)
        {
            Name = Name ?? "Value";
            if (Name != null)
            {
                this.XmlReader.ReadEndElement();
            }
        }

        protected override void OnGetFinished()
        {
        }

        protected override void OnSetStarted()
        {
        }

        protected override void OnSetEnterContext(string Name)
        {
            Name = Name ?? "Value";
            if (Name != null)
            {
                this.XmlWriter.WriteStartElement(Name);
            }
        }

        internal void OnSetString(string Str)
        {
            this.XmlWriter.WriteValue(Str);
        }

        protected override void OnSetExitContext(string Name)
        {
            Name = Name ?? "Value";
            if (Name != null)
            {
                this.XmlWriter.WriteEndElement();
            }
        }

        protected override void OnSetFinished()
        {
        }

        public string GetXml<T>(T Obj)
        {
            var SB = new System.Text.StringBuilder();
            this.XmlWriter = System.Xml.XmlWriter.Create(SB, new System.Xml.XmlWriterSettings() { Indent = true });
            this.Set(null, Obj);
            this.XmlWriter.Dispose();
            return SB.ToString();
        }

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value null
        private System.Xml.XmlReader XmlReader; // ToDo
#pragma warning restore CS0649 // Field is never assigned to, and will always have its default value null
        private System.Xml.XmlWriter XmlWriter;
    }
}
