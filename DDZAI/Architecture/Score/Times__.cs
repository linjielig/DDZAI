using System;
using System.Collections.Generic;

namespace DDZ {
    // 计算手中牌几次可以出完。
    interface ITimes__ {
        // 获取手上牌手数。
        int Get();
        // 我手中牌几次可以出完。
        int My();
        // 其余玩家当中牌较好的。
        int BetterOther();
    }
    class Times__ : ITimes__ {
        public int Get() {
            return 0;
        }
        // 对顺子进行最优拆分。
        // infos 剩余的需要进行分析的牌的信息。
        // data 如果同时含有三顺，二顺，单顺，则为单顺数据，以包含所有涉及的牌。
        void SplitSequence(SortedDictionary<CardValue, TypeInfo> infos, SequenceData data) {
            // 1. 把包含在顺子中的牌拆分出来。
            SortedDictionary<CardValue, TypeInfo> sequenceInfos;
            SortedDictionary<CardValue, TypeInfo> remainInfos;
            Tools__.SplitDictionary(infos, out sequenceInfos, out remainInfos, prediction, data);
            // 依次拆分为三顺，二顺，单顺。
            SortedDictionary<CardValue, TypeInfo> tempInfos = new SortedDictionary<CardValue, TypeInfo>();
            Tools__.CopyDictionary(sequenceInfos, tempInfos);
        }
        void SplitThreeSequence(SortedDictionary<CardValue, TypeInfo> infos, SortedDictionary<CardValue, TypeInfo> remain) {
            SequenceData data = new SequenceData();
            // 搜索三顺开始结束数据。
            foreach (KeyValuePair<CardValue, TypeInfo> keyValue in infos) {
                if (keyValue.Value.IsSequence(CardType.sequenceThree)) {
                    data = keyValue.Value.GetSequence(CardType.sequenceThree);
                }
            }
            // 从数据中移除三顺数据。
            for (CardValue key = data.start; key <= data.end; key++) {
                if (infos.ContainsKey(key)) {
                    infos[key].Count -= Tools__.sequenceThreeCount;
                }
            }
        }
        // 此张牌是否包含在顺子中。
        bool prediction(SequenceData data, CardValue card) {
            if (card >= data.start && card <= data.end) {
                return true;
            }
            return false;
        }
    }
}