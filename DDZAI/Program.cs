using System;
using System.Collections.Generic;

namespace DDZAI {
    enum CardValue {
        none = 0,
        three = 3,
        four = 4,
        five = 5,
        six = 6,
        seven = 7,
        eight = 8,
        nine = 9,
        ten = 10,
        jack = 11,
        queen = 12,
        king = 13,
        ace = 14,
        two = 15,
        blackJoker = 16,
        redJoker = 17
    }

    [Flags]
    enum CardType {
        none = 1,
        single = 1 << 1,
        pair = 1 << 2,
        three = 1 << 3,
        threeSingle = 1 << 4,
        threePair = 1 << 5,
        bombSingle = 1 << 6,
        bombPair = 1 << 7,
        sequence = 1 << 8,
        sequencePair = 1 << 9,
        sequenceThree = 1 << 10,
        sequenceThreeSingle = 1 << 11,
        sequenceThreePair = 1 << 12,
        bomb = 1 << 13,
        rocket = 1 << 14
    }

    class Sequence {
        public CardValue start;
        public CardValue end;
    }

    class TypeInfo {
        public CardType type { get; set; }
        public CardValue value { get; set; }
        public int Count { get; set; }
        Sequence[] sequence = {
            new Sequence(),
            new Sequence(),
            new Sequence()
        };
        public List<CardValue> postfix = new List<CardValue>();

        public Sequence GetSequence(CardType type) {
            switch (type) {
                case CardType.sequence:
                    return sequence[0];
                case CardType.sequencePair:
                    return sequence[1];
                case CardType.sequenceThree:
                    return sequence[2];
                default:
                    return null;
            }
        }

        public bool IsContain(CardType checkType) {
            if ((type & checkType) != 0) {
                return true;
            }
            return false;
        }

        public override string ToString() {
            string str = "\r\n" + Count + " 个 " + value + "\t牌型信息\r\n";
            str += "牌型：\t" + type + "\r\n";
            str += "顺子信息:\r\n";
            for (int i = 0; i < sequence.Length; i++) {
                str += i + 1 + "顺 起：\t" + sequence[i].start + ", 止：\t" + sequence[i].end + "\r\n";
            }
            str += "带牌信息：";
            for (int i = 0; i < postfix.Count; i++) {
                str += postfix[i] + ",\t";
            }
            return str;
        }

        public TypeInfo() {
            type = CardType.single;
            Count = 1;
        }
    }

    class Log {
        static Log instance;
        public static Log Instance {
            get {
                if (instance == null) {
                    instance = new Log();
                }
                return instance;
            }
        }
        public void Show(string message) {

        }
    }

    class Logic {
        // 所有牌的数据。
        List<byte> allDatas = new List<byte> {
                0x1,    0x2,    0x3,    0x4,    0x5,    0x6,    0x7,    0x8,    0x9,    0xa,    0xb,    0xc,    0xd,
                0x11,   0x12,   0x13,   0x14,   0x15,   0x16,   0x17,   0x18,   0x19,   0x1a,   0x1b,   0x1c,   0x1d,
                0x21,   0x22,   0x23,   0x24,   0x25,   0x26,   0x27,   0x28,   0x29,   0x2a,   0x2b,   0x2c,   0x2d,
                0x31,   0x32,   0x33,   0x34,   0x35,   0x36,   0x37,   0x38,   0x39,   0x3a,   0x3b,   0x3c,   0x3d,
                0x4e,   0x4f
                };

        // 作弊模式知道其他玩家的牌，使用cards1, cards2的数据进行计算。
        // 非作弊模式不知道其他玩家的牌，使用cards12的数据进行计算。
        // 其他玩家的牌型信息。
        SortedDictionary<CardValue, TypeInfo> infos1 = new SortedDictionary<CardValue, TypeInfo>();
        SortedDictionary<CardValue, TypeInfo> infos2 = new SortedDictionary<CardValue, TypeInfo>();
        SortedDictionary<CardValue, TypeInfo> infos12 = new SortedDictionary<CardValue, TypeInfo>();
        // 自己的牌型信息。
        SortedDictionary<CardValue, TypeInfo> infos = new SortedDictionary<CardValue, TypeInfo>();

        public string GetInfos() {
            string str = "\r\n所有玩家牌型信息\r\n";
            str += "\r\n 当前玩家 infos 信息:\r\n";
            foreach (KeyValuePair<CardValue, TypeInfo> keyValuePair in infos) {
                str += keyValuePair.Value.ToString();
            }
            str += "\r\n 其他玩家 infos1 信息:\r\n";
            foreach (KeyValuePair<CardValue, TypeInfo> keyValuePair in infos1) {
                str += keyValuePair.Value.ToString();
            }
            str += "\r\n 其他玩 infos2 信息:\r\n";
            foreach (KeyValuePair<CardValue, TypeInfo> keyValuePair in infos2) {
                str += keyValuePair.Value.ToString();
            }
            str += "\r\n 其他玩 infos12 信息:\r\n";
            foreach (KeyValuePair<CardValue, TypeInfo> keyValuePair in infos12) {
                str += keyValuePair.Value.ToString();
            }
            return str;
        }

        public List<byte> GenerateDatas() {
            Random rnd = new Random();
            List<byte> datas = new List<byte>();
            for (int i = 0; i < 20; i++) {
                datas.Add(allDatas[rnd.Next(0, 54)]);
            }
            return datas;
        }

        public void FillInfos(List<byte> datas, List<byte> datas1, List<byte> datas2) {
            // 自己的牌型信息。
            Fill(infos, datas);
            // 除去自己的牌之后剩余的牌。
            List<byte> temp = new List<byte>(allDatas);
            for (int i = 0; i < datas.Count; i++) {
                temp.Remove(datas[i]);
            }
            // 没有其他玩家的纸牌数据，使用除去自己的牌剩余的牌的数据。
            if (datas1 == null && datas2 == null) {
                Fill(infos12, temp);
            }
            // 有其他玩家的数据。
            else {
                // 有玩家2的数据，计算玩家1的数据。
                if (datas1 == null && datas2 != null) {
                    for (int i = 0; i < datas2.Count; i++) {
                        temp.Remove(datas2[i]);
                    }
                    datas1 = temp;
                }
                // 有玩家1的数据，计算玩家2的数据。
                else if (datas1 != null && datas2 == null) {
                    for (int i = 0; i < datas1.Count; i++) {
                        temp.Remove(datas1[i]);
                    }
                    datas2 = temp;
                }
                Fill(infos1, datas1);
                Fill(infos2, datas2);
            }

        }

        public CardValue GetCardValue(byte data) {
            byte value = (byte)(data & 0xf);
            // blackJoker = 16, redJoker = 17.
            if (value == 14 || value == 15) {
                value += 2;
            }
            // ace = 14, two = 15.
            if (value == 1 || value == 2) {
                value += 13;
            }
            bool isSuccess = false;
            isSuccess |= ((int)CardValue.three <= value && (int)CardValue.redJoker >= value);
            if (isSuccess) {
                return (CardValue)value;
            }
            Log.Instance.Show("纸牌数据错误：\tdata = " + data);
            return CardValue.none;
        }

        bool Fill(SortedDictionary<CardValue, TypeInfo> infos_, List<byte> datas) {
            infos_.Clear();
            for (int i = 0; i < datas.Count; i++) {
                CardValue key = GetCardValue(datas[i]);
                if (key != CardValue.none) {
                    if (infos_.ContainsKey(key)) {
                        infos_[key].Count++;
                        if (infos_[key].Count == 2) {
                            infos_[key].type |= CardType.pair;
                        }
                        if (infos_[key].Count== 3) {
                            infos_[key].type |= CardType.three;
                        }
                        if (infos_[key].Count == 4) {
                            infos_[key].type |= CardType.bomb;
                        }
                    } else {
                        infos_.Add(key, new TypeInfo());
                        infos_[key].value = key;
                    }
                } else {
                    return false;
                }
            }
            return true;
        }

        int GetPower(TypeInfo info, List<TypeInfo> infos1, List<TypeInfo> infos2) {
            if (info.IsContain(CardType.rocket)) {
                return -1;
            }
            return 0;
        }
    }
    class Program {
        static void Main(string[] args) {
            Logic l = new Logic();
            List<byte> listByte = l.GenerateDatas();
            l.FillInfos(listByte, null, null);
            Console.WriteLine(l.GetInfos());
        }
    }
}

