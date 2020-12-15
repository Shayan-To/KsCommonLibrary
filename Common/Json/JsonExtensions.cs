//#define RelaxedStrings

using System.Data;
using System.Linq;
using System;

namespace Ks
{
    namespace Common
    {
        public static class JsonExtensions
        {
            public static void WriteValue(this JsonWriter Writer, JsonObject Obj, bool? MultiLine = default)
            {
                if (Obj is JsonDictionaryObject)
                {
                    var T = (JsonDictionaryObject)Obj;

                    bool ML;
                    if (MultiLine.HasValue)
                        ML = MultiLine.Value;
                    else
                    {
                        ML = T.Count > 1;
                        ML = ML || T.Values.Any(I => !(I is JsonValueObject));
                    }

                    using (Writer.OpenDictionary(ML))
                        foreach (var I in T.OrderBy(KV => KV.Key))
                        {
                            Writer.WriteKey(I.Key);
                            Writer.WriteValue(I.Value, MultiLine);
                        }

                    return;
                }

                if (Obj is JsonListObject)
                {
                    var T = (JsonListObject)Obj;

                    bool ML;
                    if (MultiLine.HasValue)
                        ML = MultiLine.Value;
                    else
                    {
                        ML = T.Count > 10;
                        ML = ML || T.Any(I => !(I is JsonValueObject));
                        ML = ML || T.Cast<JsonValueObject>().Sum(I => I.Value.Length) > 150;
                    }

                    using (Writer.OpenList(ML))
                        foreach (var I in T)
                            Writer.WriteValue(I, MultiLine);

                    return;
                }

                if (Obj is JsonValueObject)
                {
                    var T = (JsonValueObject)Obj;
                    Writer.WriteValue(T.Value, T.IsString);

                    return;
                }

                Verify.Fail("Invalid object type.");
            }

            public static JsonDictionaryObject AsDictionary(this JsonObject Self)
            {
                var R = Self as JsonDictionaryObject;
                Verify.False(R == null, "Item has to be a dictionary.");
                return R;
            }

            public static JsonListObject AsList(this JsonObject Self)
            {
                var R = Self as JsonListObject;
                Verify.False(R == null, "Item has to be a list.");
                return R;
            }

            private static JsonValueObject AsValue(this JsonObject Self, string ErrorMessage)
            {
                var R = Self as JsonValueObject;
                Verify.False(R == null, ErrorMessage);
                return R;
            }

            public static string GetString(this JsonObject Self)
            {
                Verify.NonNull(Self);
                var V = Self.AsValue("A string value was expected, not a list or dictionary.");

#if !RelaxedStrings
                Verify.True(V.IsString, "Value must be a string.");
#endif
                return V.Value;
            }

            public static bool GetBoolean(this JsonObject Self)
            {
                Verify.NonNull(Self);
                var V = Self.AsValue("A boolean value was expected, not a list or dictionary.");

#if !RelaxedStrings
                Verify.False(V.IsString, "Value must be a boolean.");
#endif
                Verify.True((V.Value == True) | (V.Value == False), "Value must be a boolean.");
                return V.Value == True;
            }

            public static int GetInteger(this JsonObject Self)
            {
                Verify.NonNull(Self);
                var V = Self.AsValue("An integer value was expected, not a list or dictionary.");

#if !RelaxedStrings
                Verify.False(V.IsString, "Value must be an integer.");
#endif
                var T = 0;
                Verify.True(ParseInv.TryInteger(V.Value, out T), "Value must be an integer.");
                return T;
            }

            public static double GetDouble(this JsonObject Self)
            {
                Verify.NonNull(Self);
                var V = Self.AsValue("A number value was expected, not a list or dictionary.");

#if !RelaxedStrings
                Verify.False(V.IsString, "Value must be a number.");
#endif
                var T = 0.0;
                Verify.True(ParseInv.TryDouble(V.Value, out T), "Value must be a number.");
                return T;
            }

            public static int GetEnum(this JsonObject Self, string[] Values)
            {
                var V = Self.GetString();
                var I = Array.IndexOf(Values, V);
                Verify.True(I != -1, "Value must be from within the predefined values.");
                return I;
            }

            private const string True = "true";
            private const string False = "false";
        }
    }
}
