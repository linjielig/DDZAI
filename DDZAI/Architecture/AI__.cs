/*
     * 需求：   
     * 1. 根据三个玩家牌的好坏来决定是否叫地主，是否加倍。
     * 2. 根据当时的环境来决定是否出牌，出什么牌最有利。
     * 3. 根据库存调整智力。    
     * 
     * 架构：
     * 1. 手数计算，根据三家的牌计算自己的牌几手可以出完。
     * 2. 分数计算，根据三家的牌计算自己的牌的评分。
     * 3. 出牌计算，根据三家的牌和当时的环境判断是否出牌，出什么牌。
     * 4. 环境数据，谁是地主，谁是农民，上家出牌的数据，自己和其他玩家手上牌的数据。
     * 5. 牌型计算，计算打出的是什么牌型。
     * 6. 智力计算，根据库存来调整AI智力。
     */

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

    enum CardsOwner {
        my,
        other1,
        other2,
        other12
    }

    // 分数限制。
    public enum LimitType {
        // 叫地主。
        land,
        // 加倍。
        double_,
        // 地主出牌。
        landOut,
        // 农民出牌。
        famerOut,
    }
    public interface IAI__ {
        // 是否叫地主。
        bool IsLand();
        // 是否加倍。
        bool IsDouble();
        // 是否出牌，出什么牌。返回null表示不出牌。
        List<byte> GetOut();
        // 设置智力级别。
        void SetIntelligence();
    }
    public abstract class AI__ : IAI__ {
        IEnvironment__ environment;
        IScore__ score;
        IOut__ out_;
        // 获取限制数据。
        protected abstract int Limit(LimitType limit);
        public virtual bool IsLand() {
            int limit = Limit(LimitType.land);
            int myScore = score.Get(CardsOwner.my);

            // 有其他玩家牌的数据。
            if (environment.Cards(CardsOwner.other1) != null) {
                // 自己的牌比另外两家好的程度可以叫地主。
                if (myScore - score.Get(CardsOwner.other1) < limit &&
                    myScore - score.Get(CardsOwner.other2) < limit) {
                    return true;
                }
            } 
            // 没有其他玩家牌的数据
            // 自己的牌好的程度可以叫地主。
            else {
                if (myScore < limit) {
                    return true;
                }
            }
            return false;
        }
        public virtual bool IsDouble() {
            if (score.Get(CardsOwner.my) < Limit(LimitType.double_)) {
                return true;
            }
            return false;
        }
        List<byte> GetOut() {
            // 是否打别人的牌。
            if (environment.GetLastOut() != null) {
                // 我是地主。
                if (environment.GetLand() == CardsOwner.my) {
                    // 打了农民牌之后牌太差了，就不出牌了。
                    if (score.GetAfterOut(CardsOwner.my) > Limit(LimitType.landOut)) {
                        return null;
                    }
                }
                // 我是农民。
                else {
                    // 我是牌比较好的农民。
                    if (environment.GetBetterFarmer() == CardsOwner.my) {
                        // 打了之后牌太差，就不出。
                        if (score.GetAfterOut(CardsOwner.my) > Limit(LimitType.famerOut)) {
                            return null;
                        }
                    }
                    // 我是牌比较差的农民。
                    else {
                        if (!environment.IsLandLastOut()) {
                            out_.Get(true);
                            CardsOwner other = environment.GetLand() == CardsOwner.other1 ? CardsOwner.other2 : CardsOwner.other1;
                            // 地主牌变好。
                            if (score.GetAfterSimulationOut(environment.GetLand()) < score.GetAfterOut(environment.GetLand()) ||
                                // 农民牌变差。
                                score.GetAfterSimulationOut(other) > score.GetAfterOut(other)
                            ) {
                                return null;
                            }
                        }
                    }

                }
            }

            return out_.Get();
        }
    }
}