using System;
using System.Collections.Generic;

namespace DDZ {

    class SequenceData {
        public CardValue start;
        public CardValue end;
        public int Count {
            get {
                return (end - start + 1);
            }
        }
    }

    class TypeInfo {
        // 此张牌可以组成的所有牌型信息。
        public CardType type { get; set; }
        // 以此类型来进行大小比较。
        public CardType compareType { get; set; }
        // 以此类型出牌最有利。
        public CardType bestType { get; set; }
        public CardValue value { get; set; }
        public int Count { get; set; }
        // 此张牌组成的顺子信息。
        Dictionary<CardType, SequenceData> sequenceData = new Dictionary<CardType, SequenceData> {
            { CardType.sequence, new SequenceData() },
            { CardType.sequencePair, new SequenceData() },
            { CardType.sequenceThree, new SequenceData() },

        };
        // 三张，四张，飞机带牌的数据。
        public List<CardValue> postfix = new List<CardValue>();

        public static bool operator <(TypeInfo a, TypeInfo b) {
            if (a.compareType == CardType.rocket) {
                return false;
            }
            if (a.compareType != CardType.bomb && b.IsContain(CardType.bomb)) {
                return true;
            }
            return false;
        }

        public static bool operator >(TypeInfo a, TypeInfo b) {
            if (a.compareType == CardType.rocket) {
                return false;
            }
            if (a.compareType != CardType.bomb && b.IsContain(CardType.bomb)) {
                return true;
            }
            return false;
        }

        public SequenceData GetSequence(CardType type) {
            if (sequenceData.ContainsKey(type)) {
                return sequenceData[type];
            }
            return null;
        }

        public bool SetSequence(CardType type, CardValue start, CardValue end) {
            if (sequenceData.ContainsKey(type)) {
                sequenceData[type].start = start;
                sequenceData[type].end = end;
            } else {
                return false;
            }
            return true;
        }
        // 是否可以组成 checkType 牌型。
        public bool IsContain(CardType checkType) {
            if ((type & checkType) != 0) {
                return true;
            }
            return false;
        }
        public bool IsSequence() {
            if ((type & (CardType.sequence |
                CardType.sequencePair |
                CardType.sequenceThree |
                CardType.sequenceThreeSingle |
                CardType.sequenceThreePair)) != 0) {
                return true;
            }
            return false;
        }
        public bool IsSequence(CardType checkType) {
            if ((type & checkType) != 0) {
                return true;
            }
            return false;
        }
        // 除去顺子之外，实际上的牌型，不进行拆分。
        public CardType GetRealType() {
            switch (Count) {
                case 1:
                    return CardType.single;
                case 2:
                    return CardType.pair;
                case 3:
                    return CardType.three;
                case 4:
                    return CardType.bomb;
                default:
                    return CardType.none;
            }
        }
        public override string ToString() {
            string str = "\r\n" + Count + " 个 " + value + "\t牌型信息\r\n";
            str += "牌型：\t" + type + "\r\n";
            str += "顺子信息:\r\n";
            foreach (KeyValuePair<CardType, SequenceData> keyValue in sequenceData) {
                str += keyValue.Key + "\t起：\t" + keyValue.Value.start + ", 止：\t" + keyValue.Value.end + "\r\n";
            }
            str += "带牌信息：";
            for (int i = 0; i < postfix.Count; i++) {
                str += postfix[i] + ",\t";
            }
            str += "\r\n";
            return str;
        }

        public TypeInfo() {
            type = CardType.single;
            Count = 1;
        }
    }
    public class Type__ {
    }
}
