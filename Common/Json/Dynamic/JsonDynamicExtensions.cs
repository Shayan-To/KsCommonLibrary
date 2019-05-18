using System.Data;
using System.Linq;
using System.Collections.Generic;

namespace Ks
{
    namespace Common
    {
        public static class JsonDynamicExtensions
        {
            public static void WriteValue(this JsonWriter Writer, JsonDynamicBase Obj, bool? MultiLine = default(bool?))
            {
                if (Obj is JsonDynamicDictionary)
                {
                    var T = (JsonDynamicDictionary)Obj;

                    bool ML;
                    if (MultiLine.HasValue)
                        ML = MultiLine.Value;
                    else
                    {
                        ML = T.Count > 1;
                        ML = ML || T.Values.Any(I => !(I is JsonDynamicValue));
                    }

                    using (Writer.OpenDictionary(ML))
                        foreach (var I in T.OrderBy(KV => KV.Key))
                        {
                            Writer.WriteKey(I.Key);
                            Writer.WriteValue(I.Value, MultiLine);
                        }

                    return;
                }

                if (Obj is JsonDynamicList)
                {
                    var T = (JsonDynamicList)Obj;

                    bool ML;
                    if (MultiLine.HasValue)
                        ML = MultiLine.Value;
                    else
                    {
                        ML = T.Count > 10;
                        ML = ML || T.Any(I => !(I is JsonDynamicValue));
                        ML = ML || T.Cast<JsonDynamicValue>().Sum(I => I.Value.Length) > 150;
                    }

                    using (Writer.OpenList(ML))
                        foreach (var I in T)
                            Writer.WriteValue(I, MultiLine);

                    return;
                }

                if (Obj is JsonDynamicValue)
                {
                    var T = (JsonDynamicValue)Obj;
                    Writer.WriteValue(T.Value, T.IsString);

                    return;
                }

                Verify.Fail("Invalid object type.");
            }

            public static JsonDynamicBase ToDynamic(this JsonObject Self)
            {
                if (Self is JsonDictionaryObject)
                {
                    var Res = new JsonDynamicDictionary();
                    foreach (var T in (JsonDictionaryObject)Self)
                        Res.Add(T.Key, T.Value.ToDynamic());
                    return Res;
                }

                if (Self is JsonListObject)
                {
                    var Res = new JsonDynamicList();
                    foreach (var T in (JsonListObject)Self)
                        Res.Add(T.ToDynamic());
                    return Res;
                }

                if (Self is JsonValueObject)
                {
                    var T = (JsonValueObject)Self;
                    return new JsonDynamicValue(T.Value, T.IsString);
                }

                Verify.Fail("Invalid object type.");
                return null;
            }

            public static JsonObject ToConstant(this JsonDynamicBase Self)
            {
                if (Self is JsonDynamicDictionary)
                {
                    var Res = new List<KeyValuePair<string, JsonObject>>();
                    foreach (var T in (JsonDynamicDictionary)Self)
                        Res.Add(new KeyValuePair<string, JsonObject>(T.Key, T.Value.ToConstant()));
                    return new JsonDictionaryObject(Res);
                }

                if (Self is JsonDynamicList)
                {
                    var Res = new List<JsonObject>();
                    foreach (var T in (JsonDynamicList)Self)
                        Res.Add(T.ToConstant());
                    return new JsonListObject(Res);
                }

                if (Self is JsonDynamicValue)
                {
                    var T = (JsonDynamicValue)Self;
                    return new JsonValueObject(T.Value, T.IsString);
                }

                Verify.Fail("Invalid object type.");
                return null;
            }

            public static JsonDynamicList AsList(this JsonDynamicBase Self)
            {
                Verify.NonNull(Self);
                return (JsonDynamicList)Self;
            }

            public static JsonDynamicValue AsValue(this JsonDynamicBase Self)
            {
                Verify.NonNull(Self);
                return (JsonDynamicValue)Self;
            }

            public static JsonDynamicDictionary AsDictionary(this JsonDynamicBase Self)
            {
                Verify.NonNull(Self);
                return (JsonDynamicDictionary)Self;
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
                var T = 0;
                Verify.True(ParseInv.TryInteger(Self.Value, out T), "Value must be an integer.");
                return T;
            }

            public static double GetDouble(this JsonDynamicValue Self)
            {
                var T = 0.0;
                Verify.True(ParseInv.TryDouble(Self.Value, out T), "Value must be a number.");
                return T;
            }
        }
    }
}
