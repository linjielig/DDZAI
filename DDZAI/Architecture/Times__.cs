using System;
using System.Collections.Generic;

namespace DDZ {
    interface ITimes__ {
        // 获取手上牌手数。
        int Get();
    }
    class Times__ : ITimes__ {
        public int Get() {
            return 0;
        }
        // 对顺子进行最优拆分。
        void SplitSequence(SortedDictionary<CardValue, TypeInfo> infos, SequenceData data) {
            // 单顺或双顺为最长顺子，以单顺为基础进行分析，以包含所有涉及的牌。
            // 1. 把包含在顺子中的牌拆分出来。
            SortedDictionary<CardValue, TypeInfo> sequenceInfos = new SortedDictionary<CardValue, TypeInfo>();
            SortedDictionary<CardValue, TypeInfo> remainInfos = new SortedDictionary<CardValue, TypeInfo>();
            for (CardValue v = data.start; v <= data.end; v++) {
                if ()
            }
        }
        delegate bool Prediction(CardValue value);
        void SplitDictionary(
            SortedDictionary<CardValue, TypeInfo> source,
            SortedDictionary<CardValue, TypeInfo> result,
            SortedDictionary<CardValue, TypeInfo> remain,
            Prediction prediction) {
            foreach (KeyValuePair<CardValue, TypeInfo> keyValue in source) {

            }
        }
    }
}