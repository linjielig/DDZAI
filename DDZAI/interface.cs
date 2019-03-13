using System;
using System.Collections.Generic;

namespace DDZ {
    // interface AI.
    public interface IAI {
        // 是否叫地主。
        bool IsLand();
        // 是否加倍。
        bool IsDouble();
        // 是否出牌，出什么牌。返回null表示不出牌。
        List<byte> GetOut(List<byte> datas);
    }

    public interface ICards {
        // 手中牌评分，根据出牌手数和牌值大小确定，越低代表越好。
        int GetPoint();
        // 打了其他玩家牌之后的评分,datas 是其他玩家上次出牌的数据，如果 datas == null则是自己出一手牌之后的评分。
        int GetPointAfter(List<byte> datas);
        // 是否有数据。
        bool IsHaveData();
        // 是否含有大于此牌型的牌。
        bool IsHaveGreater(TypeInfo info);
        // 是否是两个农民中，牌比较好的一方。
        bool IsMainFarmer();
        // 是否是地主的上家。
        bool IsBeforeLand();
        bool IsLand();
    }

    public interface IType {
        // 获取牌型信息。
        TypeInfo GetType(List<byte> datas);
    }


    public enum CardsType {
        my = 0,
        other1,
        other2,
        other12
    }

    // Abstract AI.
    public abstract class AAI : IAI {
        // 0 自己的牌。
        // 1 玩家1的牌。
        // 2 玩家2的牌。
        // 3 没有其他玩家的牌，根据自己的牌，推算的其他玩家的牌。
        ICards[] cards;
        public ICards Cards(CardsType type) {
            return cards[(int)type];
        }
        // 剩余几手牌。
        public abstract int GetTimes(CardsType type);

        // type:
        //  0 返回叫地主限制。
        //  1 返回加倍限制。
        //  2 返回农民打农民牌限制。
        //  3 返回农民打地主牌限制。
        //  4 返回地主打农民牌限制。
        public abstract int GetLimit(LimitType type);
        public virtual bool CheckLimit(int limit) {
            // 有其他玩家的数据。
            if (Cards(CardsType.other1).IsHaveData()) {
                // 自己的牌比其他玩家的牌好。
                if (Cards(CardsType.my).GetPoint() - Cards(CardsType.other1).GetPoint() < limit &&
                    Cards(CardsType.my).GetPoint() - Cards(CardsType.other2).GetPoint() < limit) {
                    return true;
                }
            }
            // 没有其他玩家的数据。
            else {
                // 自己的牌好到什么程度。
                if (Cards(CardsType.my).GetPoint() < limit) {
                    return true;
                }
            }
            return false;
        }

        public virtual bool IsLand() {
            return CheckLimit(GetLimit(LimitType.landLimit));
        }

        public virtual bool IsDouble() {
            return CheckLimit(GetLimit(LimitType.doubleLimit));
        }

        public virtual List<byte> GetOut(List<byte> datas) {
            // 没人出牌，轮到自己出牌.
            if (datas == null) {
                if (Cards(CardsType.my).IsLand()) {

                }
            }

            // 我是地主。
            if (cards[0].IsLand()) {
                // 出牌之后评分差大于限制则不出。
                if (cards[0].GetPointAfter(datas) - cards[0].GetPoint() > GetLimit(LimitType.LandOutFarmerLimit)) {
                    return null;
                }
            } 

            // 我是农民。
            // 农民是否打农民的牌。
            if (!cards[0].IsLand() && !cards[0].IsLandOut()) {
                // 如果我是牌比较好的农民。
                if (cards[0].IsMainFarmer()) {
                    // 出牌之后评分差太大就不出。
                    if (cards[0].GetPointAfter(datas) - cards[0].GetPoint() > GetLimit(LimitType.farmerOutFarmerLimit)) {
                        return null;
                    }
                }
                // 如果我是牌比较差的农民。
                else {
                    // 地主要不起，我也不出。
                    if (!cards[cards[0].GetLandIndex()].IsHaveGreater()) {
                        return null;
                    }
                    List<byte> tempResult = cards[0].GetOut(datas, cards[1], cards[2], cards[3]);
                    // 我出牌之后地主牌变好了。
                    if (cards[cards[0].GetLandIndex()].GetPointAfter(tempResult) < cards[cards[0].GetLandIndex()].GetPointAfter(datas) ||
                        // 或者，我出牌之后队友牌变差了。
                        cards[0].GetPointAfter(tempResult) > cards[0].GetPointAfter(datas)) {
                        // 那我就不出牌。
                        return null;
                    }
                }
            }
            return cards[0].GetOut(datas, cards[1], cards[2], cards[3]);
        }
    }
}
