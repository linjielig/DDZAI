using System;
using System.Collections.Generic;

namespace DDZ {
    // 根据条件将符合要求的项目分到一个dictionary,不符合要求的分到另一个dictionary。
    delegate bool Prediction(SequenceData data, CardValue value);
    static class Tools__ {
        public static void CopyDictionary<TKey, TValue>(
            SortedDictionary<TKey, TValue> src, 
            SortedDictionary<TKey, TValue> dest) {
            foreach (KeyValuePair<TKey, TValue> keyValue in src) {
                dest.Add(keyValue.Key, keyValue.Value);
            }
        }

        public static void SplitDictionary(
            SortedDictionary<CardValue, TypeInfo> source,
            out SortedDictionary<CardValue, TypeInfo> result,
            out SortedDictionary<CardValue, TypeInfo> remain,
            Prediction prediction,
            SequenceData data) {
            result = new SortedDictionary<CardValue, TypeInfo>();
            remain = new SortedDictionary<CardValue, TypeInfo>();
            foreach (KeyValuePair<CardValue, TypeInfo> keyValue in source) {
                if (prediction(data, keyValue.Key)) {
                    result.Add(keyValue.Key, keyValue.Value);
                } else {
                    remain.Add(keyValue.Key, keyValue.Value);
                }
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
        public const int sequenceLength = 5;
        public const int sequenceCount = 2;
        public const int sequencePairLength = 3;
        public const int sequencePairCount = 2;
        public const int sequenceThreeLength = 2;
        public const int sequenceThreeCount = 3;
        // 获取判断单顺，二顺，三顺的条件。
        public static bool GetSequenceRequire(CardType type, out int length, out int count) {
            length = sequenceLength;
            count = sequenceCount;
            switch (type) {
                case CardType.sequence:
                    break;
                case CardType.sequencePair:
                    length = sequencePairLength;
                    count = sequencePairCount;
                    break;

                case CardType.sequenceThree:
                case CardType.sequenceThreeSingle:
                case CardType.sequenceThreePair:
                    length = sequenceThreeLength;
                    count = sequenceThreeCount;
                    break;
                default:
                    return false;

            }
            return true;
        }

        // 写入顺子数据。
        public static bool FillSequenceInfos(SortedDictionary<CardValue, TypeInfo> infos_, CardType sequenceType) {
            // 顺子长度和牌的数量要求。
            if (!GetSequenceRequire(sequenceType, out int requireLength, out int requireCount)) {
                return false;
            }

            CardValue startValue = CardValue.three;
            CardValue endValue = CardValue.three;
            int length = 0;
            // checkValue <= CardValue.two 而不是 checkValue <= CardValue.ace 是为了方便处理 3 到 ace 的顺子。
            for (CardValue checkValue = CardValue.three; checkValue <= CardValue.two; checkValue++) {
                if (infos_.ContainsKey(checkValue) && infos_[checkValue].Count >= requireCount && checkValue <= CardValue.ace) {
                    endValue = checkValue;
                    length++;
                } else {
                    // 是否存在顺子。
                    FillSequenceData(infos_, length, requireLength, startValue, endValue, sequenceType);
                    startValue = checkValue + 1;
                    length = 0;
                }

            }
            return true;
        }
        // 写入顺子开始结束数据。
        public static void FillSequenceData(SortedDictionary<CardValue, TypeInfo> infos_, int length, int requireLength,
            CardValue start, CardValue end, CardType sequenceType) {
            if (length >= requireLength) {
                for (CardValue value = start; value <= end; value++) {
                    infos_[value].type |= sequenceType;
                    infos_[value].SetSequence(sequenceType, start, end);
                }
            }
        }
    }
}