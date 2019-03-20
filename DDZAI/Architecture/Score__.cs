using System;
using System.Collections.Generic;

namespace DDZ {
    // 分数限制。
    enum LimitType {
        // 叫地主。
        land,
        // 加倍。
        double_
    }
    // 评分组件。
    interface IScore__ {
        // 各种评分限制。
        int Limit(LimitType type);
        // 我手中牌评分。
        int My();
        // 其他玩家中较好的手中牌评分。
        int BetterOther();
        // 模拟出牌或过牌评分排名。如果我是地主或者是牌比较好的农民，返回的是自己的排名。
        // 如果我是牌比较差的农名，返回的是牌比较好的农民评分的排名。
        int SimulateRanking();
    }
}