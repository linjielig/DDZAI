using System;
using System.Collections.Generic;

namespace DDZ {
    // 出牌组件。
    interface IOut__ {
        // 获取要打出的牌。
        List<byte> Get();
        /* 模拟仅计算一轮之后的情况，即自己出牌或者过牌之后，再次轮到自己出牌，此时三家牌的评分关系。
         * 此为可能的情况，不是100%的准确。       
         */
        // 模拟出牌。
        void Simulate();
        // 模拟不出牌。
        void SimulatePass();
    }
}