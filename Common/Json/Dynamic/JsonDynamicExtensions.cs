using System.Collections.Generic;
using System.Linq;

namespace Ks.Common
{
    public static class JsonDynamicExtensions
    {
        public static void WriteValue(this JsonWriter Writer, JsonDynamicBase Obj, bool? MultiLine = default)
        {
            switch (Obj)
            {
                case JsonDynamicDictionary dic:
                {
                    bool ML;
                    if (MultiLine.HasValue)
                    {
                        ML = MultiLine.Value;
                    }
                    else
                    {
                        ML = dic.Count > 1;
                        ML = ML || dic.Values.Any(I => !(I is JsonDynamicValue));
                    }

                    using (Writer.OpenDictionary(ML))
                    {
                        foreach (var I in dic.OrderBy(KV => KV.Key))
                        {
                            Writer.WriteKey(I.Key);
                            Writer.WriteValue(I.Value, MultiLine);
                        }
                    }

                    return;
                }
                case JsonDynamicList list:
                {
                    bool ML;
                    if (MultiLine.HasValue)
                    {
                        ML = MultiLine.Value;
                    }
                    else
                    {
                        ML = list.Count > 10;
                        ML = ML || list.Any(I => !(I is JsonDynamicValue));
                        ML = ML || list.Cast<JsonDynamicValue>().Sum(I => I.Value.Length) > 150;
                    }

                    using (Writer.OpenList(ML))
                    {
                        foreach (var I in list)
                        {
                            Writer.WriteValue(I, MultiLine);
                        }
                    }

                    return;
                }
                case JsonDynamicValue v:
                {
                    Writer.WriteValue(v.Value, v.IsString);
                    return;
                }
            }

            Verify.Fail("Invalid object type.");
        }

        public static JsonDynamicBase ToDynamic(this JsonObject Self)
        {
            switch (Self)
            {
                case JsonDictionaryObject dic:
                {
                    var Res = new JsonDynamicDictionary();
                    foreach (var T in dic)
                    {
                        Res.Add(T.Key, T.Value.ToDynamic());
                    }

                    return Res;
                }
                case JsonListObject list:
                {
                    var Res = new JsonDynamicList();
                    foreach (var T in list)
                    {
                        Res.Add(T.ToDynamic());
                    }

                    return Res;
                }
                case JsonValueObject v:
                {
                    return new JsonDynamicValue(v.Value, v.IsString);
                }
            }

            Verify.Fail("Invalid object type.");
            return null;
        }

        public static JsonObject ToConstant(this JsonDynamicBase Self)
        {
            switch (Self)
            {
                case JsonDynamicDictionary dic:
                {
                    var Res = new List<KeyValuePair<string, JsonObject>>();
                    foreach (var T in dic)
                    {
                        Res.Add(new KeyValuePair<string, JsonObject>(T.Key, T.Value.ToConstant()));
                    }

                    return new JsonDictionaryObject(Res);
                }
                case JsonDynamicList list:
                {
                    var Res = new List<JsonObject>();
                    foreach (var T in list)
                    {
                        Res.Add(T.ToConstant());
                    }

                    return new JsonListObject(Res);
                }
                case JsonDynamicValue v:
                {
                    return new JsonValueObject(v.Value, v.IsString);
                }
            }

            Verify.Fail("Invalid object type.");
            return null;
        }

        public static JsonDynamicList AsList(this JsonDynamicBase Self)
        {
            Verify.NonNull(Self);
            return (JsonDynamicList) Self;
        }

        public static JsonDynamicValue AsValue(this JsonDynamicBase Self)
        {
            Verify.NonNull(Self);
            return (JsonDynamicValue) Self;
        }

        public static JsonDynamicDictionary AsDictionary(this JsonDynamicBase Self)
        {
            Verify.NonNull(Self);
            return (JsonDynamicDictionary) Self;
        }

        public static string GetString(this JsonDynamicValue Self)
        {
            return Self.Value;
        }

        public static bool GetBoolean(this JsonDynamicValue Self)
        {
            Verify.True((Self.Value == JsonDynamicValue.True) | (Self.Value == JsonDynamicValue.False), "Value must be a boolean.");
            return Self.Value == JsonDynamicValue.True;
        }

        public static int GetInteger(this JsonDynamicValue Self)
        {
            Verify.True(ParseInv.TryInteger(Self.Value, out var T), "Value must be an integer.");
            return T;
        }

        public static double GetDouble(this JsonDynamicValue Self)
        {
            Verify.True(ParseInv.TryDouble(Self.Value, out var T), "Value must be a number.");
            return T;
        }
    }
}
