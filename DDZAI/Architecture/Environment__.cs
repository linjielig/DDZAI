using System;
using System.Collections.Generic;

namespace DDZ {
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

    interface IEnvironment__ {
        SortedDictionary<CardValue, TypeInfo> MyCards();
        SortedDictionary<CardValue, TypeInfo> Other1Cards();
        SortedDictionary<CardValue, TypeInfo> Other2Cards();
        bool IsHaveOtherData();
        // 谁是地主。
        CardsOwner GetLand();
        // 其他玩家打出的牌，如果为空则说明自己先出牌，或者没人要自己打出的牌。
        List<byte> GetLastOut();
        // 我是否是地主的上家。
        bool IsMyBeforeLand();
        // 上次出牌的人是否是地主。
        bool IsLandLastOut();
        // 那个农民的牌比较好。
        CardsOwner GetBetterFarmer();
        // 其他两家是否有大于此牌型的牌。
        bool IsHaveGreater(TypeInfo info);
        // 叫地主限制。
        ScoreData LandLimit();
        // 加倍限制。
        ScoreData DoubleLimit();
    }
}