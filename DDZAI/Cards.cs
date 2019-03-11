using System;
using System.Collections.Generic;

namespace DDZAI {
    public class Cards : ICards {
        SortedDictionary<CardValue, TypeInfo> infos = new SortedDictionary<CardValue, TypeInfo>();

        public int GetPoint() {
            return 0;
        }
        public int GetTimes(ICards cards1, ICards cards2, ICards cards12) {
            int times = 0;
            SortedDictionary<CardValue, TypeInfo> tempInfos = new SortedDictionary<CardValue, TypeInfo>();
            Tools.SortedDictionaryCopy(infos, tempInfos);

            // 王炸手数减一。
            if (tempInfos.ContainsKey(CardValue.redJoker) && tempInfos[CardValue.redJoker].IsContain(CardType.rocket)) {
                times -= 1;
                tempInfos.Remove(CardValue.redJoker);
                tempInfos.Remove(CardValue.blackJoker);
            }
            return times;
        }
    }
}