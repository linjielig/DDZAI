using System;
using System.Collections.Generic;

namespace DDZ {
    interface IOut__ {
        // 获取要打出的牌。
        List<byte> Get(bool isSimulation = false);
    }
}