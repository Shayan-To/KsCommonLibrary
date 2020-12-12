using System;
using System.Windows;
using System.Collections.Generic;
using System.Collections;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using Ks.Common.MVVM;

namespace Ks.Common.Controls
{

    // FText help:
    //
    // ` -> Escape character.
    // {{ObjNum(,[+-]?##)?(:.*)?}} -> No change.
    // {ObjNum(,[+-]?##)?(:.*)?} -> Common corrections.
    // [[.*]] -> Common corrections.
    // [.*] -> Translate.
    //
    // {}s could exist inside []s.
    // A '{}' at the beginning is ignored.
    //
    // If just Obj is set, only common corrections will be applied.

    public class TextBlock : Control
    {
        static TextBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBlock), new FrameworkPropertyMetadata(typeof(TextBlock)));
        }

        public TextBlock()
        {
            this._Objs = new ObjList(this);
        }

        internal void ReportObjGotIn(Obj Obj)
        {
            this.AddLogicalChild(Obj);
        }

        internal void ReportObjWentOut(Obj Obj)
        {
            this.RemoveLogicalChild(Obj);
        }

        internal void ReportObjChanged()
        {
            Verify.True(this.Text == null, "Cannot use objs with Text. FText must be used.");
            Verify.True(this.Obj == null | (this.Objs.Count == 0), "Cannot use both Obj abd Objs.");
            this.UpdateText(this.FText);
        }

        private void UpdateText()
        {
            var Text = this.Text;
            if (Text != null)
            {
                var Lang = GetKsLanguage(this);
                this.OutText = CorrectString(Lang?.GetTranslation(Text) ?? Text, Lang);
                return;
            }

            this.UpdateText(this.FText);
        }

        private void UpdateText(string S)
        {
            if (S == null)
                S = "{0}";

            Verify.False(Regex.IsMatch(S, @"`[^`\[\]\{\}]"), "Invalid escape sequence.");

            S = Regex.Replace(S, @"`[\[\]\{\}]", M => "`" + "[]{}".IndexOf(M.Value[1]).ToString());
            Func<string, string> UnEscape = T => Regex.Replace(T, "`([0123])", M => "[]{}"[ParseInv.Integer(M.Groups[1].Value)].ToString());

            if (S.StartsWith("{}"))
                S = S.Substring(2);

            var Lang = GetKsLanguage(this);

            IList<Obj> Objs;
            var Obj = this.Obj;
            if (Obj != null)
            {
                Objs = this.Obj1Array;
                Objs[0].Objt = Obj;
            }
            else
                Objs = this.Objs;

            // ToDo If the value returned by the object (by String.Format) contains braces or brackets, the code will break. Fix this.
            S = Regex.Replace(S, @"\{\{(\d+)((?:,[+-]?\d+)?(?::[^\{\}]*)?)\}\}", M =>
            {
                var I = ParseInv.Integer(M.Groups[1].Value);
                if (I >= Objs.Count)
                    return "";

                var T = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0" + UnEscape.Invoke(M.Groups[2].Value) + "}", Objs[I].Objt);
                return T;
            });
            S = Regex.Replace(S, @"\{(\d+)((?:,[+-]?\d+)?(?::[^\{\}]*)?)\}", M =>
            {
                var I = ParseInv.Integer(M.Groups[1].Value);
                if (I >= Objs.Count)
                    return "";

                var T = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0" + UnEscape.Invoke(M.Groups[2].Value) + "}", Objs[I].Objt);
                return CorrectString(T, Lang);
            });

            Verify.False(S.Contains("{") | S.Contains("}"), "Invalid format string.");

            S = Regex.Replace(S, @"\[\[([^\[\]]*)\]\]", M =>
            {
                var T = UnEscape.Invoke(M.Groups[1].Value);
                return CorrectString(T, Lang);
            });
            S = Regex.Replace(S, @"\[([^\[\]]*)\]", M =>
            {
                var T = UnEscape.Invoke(M.Groups[1].Value);
                return Lang?.GetTranslation(T) ?? T;
            });

            Verify.False(S.Contains("[") | S.Contains("]"), "Invalid format string.");

            S = UnEscape.Invoke(S);
            this.OutText = S;
        }

        private static string CorrectString(string S, KsLanguage Lang)
        {
            if (Lang == null)
                return S;

            if (Lang.Id.ToLowerInvariant() == "pes")
            {
                var OldDigits = "0123456789";
                var NewDigits = "۰۱۲۳۴۵۶۷۸۹";
                for (var I = 0; I < 10; I++)
                    S = S.Replace(OldDigits[I], NewDigits[I]);
                return S;
            }
            if (Lang.Id.ToLowerInvariant() == "arb")
            {
                var OldDigits = "0123456789";
                var NewDigits = "٠١٢٣٤٥٦٧٨٩";
                for (var I = 0; I < 10; I++)
                    S = S.Replace(OldDigits[I], NewDigits[I]);
                return S;
            }

            return S;
        }

        protected override IEnumerator LogicalChildren
        {
            get
            {
                return this.Objs.GetEnumerator();
            }
        }

        public static readonly DependencyProperty KsLanguageProperty = DependencyProperty.RegisterAttached("KsLanguage", typeof(KsLanguage), typeof(TextBlock), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits, KsLanguage_Changed, KsLanguage_Coerce));

        private static object KsLanguage_Coerce(DependencyObject D, object BaseValue)
        {
            // Dim Self = TryCast(D, UIElement)
            // If Self Is Nothing Then
            // Return KsLanguageProperty.DefaultMetadata.DefaultValue
            // End If

            // Dim Value = DirectCast(BaseValue, KsLanguage)

            return BaseValue;
        }

        private static void KsLanguage_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
            var Self = D as TextBlock;

            if (Self == null)
                return;

            // Dim OldValue = DirectCast(E.OldValue, KsLanguage)
            // Dim NewValue = DirectCast(E.NewValue, KsLanguage)

            Self.UpdateText();
        }

        public static KsLanguage GetKsLanguage(DependencyObject Element)
        {
            Verify.NonNullArg(Element, nameof(Element));
            return (KsLanguage) Element.GetValue(KsLanguageProperty);
        }

        public static void SetKsLanguage(DependencyObject Element, KsLanguage Value)
        {
            Verify.NonNullArg(Element, nameof(Element));
            Element.SetValue(KsLanguageProperty, Value);
        }

        private static readonly DependencyPropertyKey OutTextPropertyKey = DependencyProperty.RegisterReadOnly("OutText", typeof(string), typeof(TextBlock), new PropertyMetadata(null));
        public static readonly DependencyProperty OutTextProperty = OutTextPropertyKey.DependencyProperty;

        public string OutText
        {
            get
            {
                return (string) this.GetValue(OutTextProperty);
            }
            private set
            {
                this.SetValue(OutTextPropertyKey, value);
            }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(TextBlock), new PropertyMetadata(null, Text_Changed, Text_Coerce));

        private static object Text_Coerce(DependencyObject D, object BaseValue)
        {
            // Dim Self = DirectCast(D, TextBlock)

            // Dim Value = DirectCast(BaseValue, String)

            return BaseValue;
        }

        private static void Text_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
            var Self = (TextBlock) D;

            // Dim OldValue = DirectCast(E.OldValue, String)
            var NewValue = (string) E.NewValue;

            Verify.True(Self.FText == null, "Cannot set both Text and FText.");

            Self.UpdateText();
        }

        public string Text
        {
            get
            {
                return (string) this.GetValue(TextProperty);
            }
            set
            {
                this.SetValue(TextProperty, value);
            }
        }

        public static readonly DependencyProperty FTextProperty = DependencyProperty.Register("FText", typeof(string), typeof(TextBlock), new PropertyMetadata(null, FText_Changed, FText_Coerce));

        private static object FText_Coerce(DependencyObject D, object BaseValue)
        {
            var Self = (TextBlock) D;

            var Value = (string) BaseValue;

            return BaseValue;
        }

        private static void FText_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
            var Self = (TextBlock) D;

            // Dim OldValue = DirectCast(E.OldValue, String)
            var NewValue = (string) E.NewValue;

            Verify.True(Self.Text == null, "Cannot set both Text and FText.");

            Self.UpdateText(NewValue);
        }

        public string FText
        {
            get
            {
                return (string) this.GetValue(FTextProperty);
            }
            set
            {
                this.SetValue(FTextProperty, value);
            }
        }

        public static readonly DependencyProperty ObjProperty = DependencyProperty.Register("Obj", typeof(object), typeof(TextBlock), new PropertyMetadata(null, Obj_Changed, Obj_Coerce));

        private static object Obj_Coerce(DependencyObject D, object BaseValue)
        {
            // Dim Self = DirectCast(D, TextBlock)

            // Dim Value = DirectCast(BaseValue, Object)

            return BaseValue;
        }

        private static void Obj_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
            var Self = (TextBlock) D;

            // Dim OldValue = DirectCast(E.OldValue, Object)
            // Dim NewValue = DirectCast(E.NewValue, Object)

            Self.ReportObjChanged();
        }

        public object Obj
        {
            get
            {
                return (object) this.GetValue(ObjProperty);
            }
            set
            {
                this.SetValue(ObjProperty, value);
            }
        }

        private readonly ObjList _Objs;

        public ObjList Objs
        {
            get
            {
                return this._Objs;
            }
        }

        public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register("TextAlignment", typeof(TextAlignment), typeof(TextBlock), new PropertyMetadata(TextAlignment.Left, TextAlignment_Changed, TextAlignment_Coerce));

        private static object TextAlignment_Coerce(DependencyObject D, object BaseValue)
        {
            // Dim Self = DirectCast(D, TextBlock)

            // Dim Value = DirectCast(BaseValue, TextAlignment)

            return BaseValue;
        }

        private static void TextAlignment_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
        }

        public TextAlignment TextAlignment
        {
            get
            {
                return (TextAlignment) this.GetValue(TextAlignmentProperty);
            }
            set
            {
                this.SetValue(TextAlignmentProperty, value);
            }
        }

        public static readonly DependencyProperty TextWrappingProperty = DependencyProperty.Register("TextWrapping", typeof(TextWrapping), typeof(TextBlock), new PropertyMetadata(TextWrapping.NoWrap, TextWrapping_Changed, TextWrapping_Coerce));

        private static object TextWrapping_Coerce(DependencyObject D, object BaseValue)
        {
            // Dim Self = DirectCast(D, TextBlock)

            // Dim Value = DirectCast(BaseValue, TextWrapping)

            return BaseValue;
        }

        private static void TextWrapping_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
        }

        public TextWrapping TextWrapping
        {
            get
            {
                return (TextWrapping) this.GetValue(TextWrappingProperty);
            }
            set
            {
                this.SetValue(TextWrappingProperty, value);
            }
        }

        private readonly Obj[] Obj1Array = new[] { new Obj() };
    }
}
