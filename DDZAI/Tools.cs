using System;
using System.Collections.Generic;

namespace DDZ {
    public static class Tools {
        public static void SortedDictionaryCopy<TKey, TValue>(
            SortedDictionary<TKey, TValue> src, 
            SortedDictionary<TKey, TValue> dest) {
            foreach (KeyValuePair<TKey, TValue> keyValue in src) {
                dest.Add(keyValue.Key, keyValue.Value);
            }
        }

        // 搜索可以带的牌。
        // type = CardType.single 搜索单牌。
        // type = CardType.single | CardType.pair 搜索对牌。
        public static List<CardValue> SearchPostfix(SortedDictionary<CardValue, TypeInfo> infos, CardType type) {
            List<CardValue> result = new List<CardValue>();
            foreach (KeyValuePair<CardValue, TypeInfo> info in infos) {
                if (info.Value.type == type) {
                    result.Add(info.Key);
                }
            }
            return result;
        }
    }
}