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
        SortedDictionary<CardValue, TypeInfo> Cards(CardsOwner owner);
    }
}