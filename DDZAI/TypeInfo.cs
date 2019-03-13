using System;
using System.Collections.Generic;

namespace DDZ {

    public class Sequence {
        public CardValue start;
        public CardValue end;
    }

    public class TypeInfo {
        public CardType type { get; set; }
        public CardType compareType { get; set; }
        public CardValue value { get; set; }
        public int Count { get; set; }

        Dictionary<CardType, Sequence> sequence = new Dictionary<CardType, Sequence> {
            { CardType.sequence, new Sequence() },
            { CardType.sequencePair, new Sequence() },
            { CardType.sequenceThree, new Sequence() },

        };
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

        public Sequence GetSequence(CardType type) {
            if (sequence.ContainsKey(type)) {
                return sequence[type];
            } else {
                return null;
            }
        }

        public bool SetSequence(CardType type, CardValue start, CardValue end) {
            if (sequence.ContainsKey(type)) {
                sequence[type].start = start;
                sequence[type].end = end;
            } else {
                return false;
            }
            return true;
        }
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
        // 不拆分时除去顺子的牌型。
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
        public bool IsOnlySingle() {
            if (type == CardType.single) {
                return true;
            }
            return false;
        }
        public bool IsOnlyPair() {
            if (type == (CardType.single | CardType.pair)) {
                return true;
            }
            return false;
        }
        public override string ToString() {
            string str = "\r\n" + Count + " 个 " + value + "\t牌型信息\r\n";
            str += "牌型：\t" + type + "\r\n";
            str += "顺子信息:\r\n";
            foreach (KeyValuePair<CardType, Sequence> keyValue in sequence) {
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
}