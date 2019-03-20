using System;
using System.Collections.Generic;

namespace DDZ {
    // 出牌组件。
    interface IOut__ {
        // 获取要打出的牌。
        List<byte> Get(bool isSimulation = false);
        // 模拟出牌。
        void Simulate();
        // 模拟不出牌。
        void SimulatePass();
    }
}