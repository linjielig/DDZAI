using System;
using System.Collections.Generic;

namespace DDZ {
    // 计算每手牌与三家相比的大小。
    interface ICompareScore__ {
        // 我手中牌与其他三家相比得分。
        uint My();
        // 其他玩家中牌型对比评分较高的玩家。
        uint BetterOther();
    }
}