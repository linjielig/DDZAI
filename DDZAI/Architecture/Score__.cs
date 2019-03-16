using System;
using System.Collections.Generic;

namespace DDZ {
    interface IScore__ {
        // 获取玩家手中牌评分。
        int Get(CardsOwner owner);
        // 获取玩家打了别人牌之后手中牌的评分，以决定是否要出牌。
        int GetAfterOut(CardsOwner owner);
        int GetAfterSimulationOut(CardsOwner owner);
    }
}