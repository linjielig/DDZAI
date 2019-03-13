using System;
using System.Collections.Generic;

namespace DDZ {

    public class AI : AAI {
        public int LandLimit { get; set; }
        public int DoubleLimit { get; set; }
        public int FarmerOutFarmerLimit { get; set; }
        public int FarmerOutLandLimit { get; set; }
        public int LandOutFarmerLimit { get; set; }
        // type:
        //  0 返回叫地主限制。
        //  1 返回加倍限制。
        //  2 返回农民打农民牌限制。
        //  3 返回农民打地主牌限制。
        //  4 返回地主打农民牌限制。
        public override int GetLimit(LimitType type) {
            switch (type) {
                case LimitType.landLimit:
                    return LandLimit;
                case LimitType.doubleLimit:
                    return DoubleLimit;
                case LimitType.farmerOutFarmerLimit:
                    return FarmerOutFarmerLimit;
                case LimitType.FarmerOutLandLimit:
                    return FarmerOutLandLimit;
                case LimitType.LandOutFarmerLimit:
                    return LandOutFarmerLimit;
            }
            return -1000;
        }
    }
}