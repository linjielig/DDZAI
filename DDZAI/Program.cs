using System;
using System.Collections.Generic;

namespace DDZ {
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
            Console.WriteLine(message);
        }
    }

    class Logic {
        // 所有牌的数据。
        public static List<byte> allDatas = new List<byte> {
                0x1,    0x2,    0x3,    0x4,    0x5,    0x6,    0x7,    0x8,    0x9,    0xa,    0xb,    0xc,    0xd,
                0x11,   0x12,   0x13,   0x14,   0x15,   0x16,   0x17,   0x18,   0x19,   0x1a,   0x1b,   0x1c,   0x1d,
                0x21,   0x22,   0x23,   0x24,   0x25,   0x26,   0x27,   0x28,   0x29,   0x2a,   0x2b,   0x2c,   0x2d,
                0x31,   0x32,   0x33,   0x34,   0x35,   0x36,   0x37,   0x38,   0x39,   0x3a,   0x3b,   0x3c,   0x3d,
                0x4e,   0x4f
                };

        // 0 自己的牌的数据。
        // 1 玩家1的牌的数据。
        // 2 玩家2的牌的数据。
        // 3 没有其他玩家的数据，根据自己的牌推算的其他两个玩家的总数据。
        SortedDictionary<CardValue, TypeInfo>[] infos = {
            new SortedDictionary<CardValue, TypeInfo>(),
            new SortedDictionary<CardValue, TypeInfo>(),
            new SortedDictionary<CardValue, TypeInfo>(),
            new SortedDictionary<CardValue, TypeInfo>()
        };
        public string GetInfos() {
            string str = "\r\n所有玩家牌数据\r\n";
            int count = 0;
            foreach (SortedDictionary<CardValue, TypeInfo> info in infos) {
                str += "\r\n ------------>>>>>>>>>>>>>>>牌数据infos[" + count + "]的 信息:\r\n";
                foreach (KeyValuePair<CardValue, TypeInfo> keyValue in info) {
                    str += keyValue.Value.ToString();
                }
                count++;
            }
            return str;
        }
        public string ShowList(string title, List<byte> datas) {
            string str = "\r\n" + title + "\r\n";
            for (int i = 0; i < datas.Count; i++) {
                str += datas[i].ToString("X") + ",\t";
            }
            return str;
        }
        public List<byte> GenerateDatas(List<byte> remainDatas) {
            Random rnd = new Random();
            List<byte> datas = new List<byte>();
            for (int i = 0; i < 17; i++) {
                int index = rnd.Next(0, remainDatas.Count);
                datas.Add(remainDatas[index]);
                remainDatas.RemoveAt(index);
            }
            return datas;
        }
        void RemoveListFromList(List<byte> list, List<byte> remove) {
            for (int i = 0; i < remove.Count; i++) {
                list.Remove(remove[i]);
            }
        }
        public void FillInfos(List<byte> datas, List<byte> datas1, List<byte> datas2) {
            // 自己牌的数据。
            Fill(infos[0], datas);
            // 除去自己的牌之后剩余的牌。
            List<byte> temp = new List<byte>(allDatas);
            RemoveListFromList(temp, datas);
            // 没有其他玩家的纸牌数据，使用除去自己的牌剩余的牌的数据。
            if (datas1 == null && datas2 == null) {
                Fill(infos[3], temp);
            }
            // 有其他玩家的数据。
            else {
                // 有玩家2的数据，计算玩家1的数据。
                if (datas1 == null && datas2 != null) {
                    RemoveListFromList(temp, datas2);
                    datas1 = temp;
                }
                // 有玩家1的数据，计算玩家2的数据。
                else if (datas1 != null && datas2 == null) {
                    RemoveListFromList(temp, datas1);
                    datas2 = temp;
                }
                Fill(infos[1], datas1);
                Fill(infos[2], datas2);
            }
            for (int i = 0; i < infos.Length; i++) {
                FillSequenceInfos(infos[i], CardType.sequence);
                FillSequenceInfos(infos[i], CardType.sequencePair);
                FillSequenceInfos(infos[i], CardType.sequenceThree);
            }

        }
        const int sequenceRequireLength = 5;
        const int sequenceRequireCount = 1;
        const int sequencePairRequireLength = 3;
        const int sequencePairRequireCount = 2;
        const int sequenceThreeRequireLength = 2;
        const int sequenceThreeRequireCount = 3;

        bool GetSequenceRequire(CardType type, out int length, out int count) {
            length = 5;
            count = 1;
            switch (type) {
                case CardType.sequence:
                    break;
                case CardType.sequencePair:
                    length = 3;
                    count = 2;
                    break;

                case CardType.sequenceThree:
                case CardType.sequenceThreeSingle:
                case CardType.sequenceThreePair:
                    length = 2;
                    count = 3;
                    break;
                default:
                    return false;

            }
            return true;
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

        int GetTimes_(TypeInfo info, SortedDictionary<CardValue, TypeInfo> otherInfos1,
            SortedDictionary<CardValue, TypeInfo> otherInfos2) {
            // 存在王炸，返回-1。避免重复计算，只在检测小王时返回。
            if (info.IsContain(CardType.rocket)) {
                if (info.value == CardValue.blackJoker) {
                    return -1;
                }
                return 0;
            }
            return 0;
        }
        int GetTimes(SortedDictionary<CardValue, TypeInfo> checkInfos,
            SortedDictionary<CardValue, TypeInfo> otherInfos1,
            SortedDictionary<CardValue, TypeInfo> otherInfos2) {
            int times = 0;
            foreach (KeyValuePair<CardValue, TypeInfo> keyValue in checkInfos) {
                times += GetTimes_(keyValue.Value, otherInfos1, otherInfos2);
            }
            return times;
        }
    }
    class Program {
        static void Main(string[] args) {
            Logic l = new Logic();
            List<byte> all = new List<byte>(Logic.allDatas);
            List<byte> datas = l.GenerateDatas(all);
            List<byte> datas1 = l.GenerateDatas(all);
            List<byte> datas2 = l.GenerateDatas(all);
            List<byte> landDatas = all;
            l.FillInfos(datas, datas1, datas2);
            Console.WriteLine(l.GetInfos());
        }
    }
}

