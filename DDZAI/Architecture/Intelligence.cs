using System;
using System.Collections.Generic;

namespace DDZ {
    public enum IntelligenceType {
        // 可以获取其他玩家手上牌的数据，考虑地主和农民关系，自己的位置关系，计算出牌或者不出牌对三家手上牌的影响。
        cheat,
        // 不可以获取其他玩家手上牌的数据，考虑地主和农民关系，自己的位置关系，计算出牌或者不出牌对自己手上牌的影响，
        // 推算其他玩家手上牌。
        strongSpeculate,
        // 不可以获取其他玩家手上牌的数据，考虑地主和农民关系，自己的位置关系，计算出牌或者不出牌对自己手上牌的影响，
        // 不推算其他玩家手上牌。
        speculate,
        // 计算出牌或者不出牌对自己手上牌的影响。
        weakSpeculate
    }
}