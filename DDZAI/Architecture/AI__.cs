/*
     * 需求：   
     * 1. 根据三家牌的数据，准确判断是否叫地主，加倍。
     * 2. 根据三家牌的数据，自己的位置，自己是否是地主，准确判断是否出牌，出什么牌。
     * 3. 根据需求调整程序判断的准确度。
     * 
     * 架构：
     * 1. 评分组件，根据三家牌的数据，准确的对三家手中牌，手中牌加上底牌或模拟出牌或过牌之后的牌进行评分，
     * 评分越低的牌获胜几率越大。发牌后，三家评分的关系决定是否叫地主，是否加倍。
     * 2. 出牌组件，根据三家评分的关系，自己的位置，自己是否是地主，来判断是否出牌，出什么牌。
     * 3. 环境组件，提供谁是地主，三个玩家的牌数据，三个玩家的位置关系。
     * 4. 智力组件，调整提供数据的多少和判断的准确度。
     */

using System.Collections.Generic;

namespace DDZ {
    enum CardsOwner {
        my,
        other1,
        other2,
    }

    public interface IAI__ {
        // 是否叫地主。
        bool IsLand();
        // 是否加倍。
        bool IsDouble();
        // 是否出牌，出什么牌。返回null表示不出牌。
        List<byte> GetOut();
        // 设置智力级别。
        void SetIntelligence(IntelligenceType type);
    }

    // 由出牌手数和牌型的大小情况来计算评分，评分越低表明获胜几率越大。
    abstract class AI__ : IAI__ {
        IEnvironment__ environment;
        IScore__ score;
        IOut__ out_;

        bool IsLandOrDouble(LimitType type) {
            // 有其他玩家牌的数据。
            if (environment.IsHaveOtherData()) {
                // 自己的牌与其他玩家中最好的牌比较，符合限制。
                if (score.My() - score.BetterOther() < score.Limit(type)) {
                    return true;
                }
            }
            // 没有其他玩家牌的数据
            else {
                // 自己的牌符合限制。
                // 此处IScore.Limit返回的限制与上方的不同，表示的是没有其他玩家数据时的限制。
                if (score.My() < score.Limit(type)) {
                    return true;
                }
            }
            return false;
        }

        // 是否叫地主。
        public virtual bool IsLand() {
            return IsLandOrDouble(LimitType.land);
        }

        // 是否加倍。
        public virtual bool IsDouble() {
            return IsLandOrDouble(LimitType.double_);
        }

        // 是否出牌，出什么牌。
        public List<byte> GetOut() {
            // 是否打别人的牌。
            if (environment.GetLastOut() != null) {
                // 模拟过牌。
                out_.SimulatePass();
                int passRanking = score.SimulateRanking();
                // 模拟出牌。
                out_.Simulate();
                int outRanking = score.SimulateRanking();
                // 出牌后我评分的排名可能下降，不出牌。
                // 如果我是牌比较差的农民，这里返回的是牌比较好的农民评分的排名，
                // 出牌后牌比较好的农民评分的排名可能下降，不出牌。
                if (passRanking > outRanking) {
                    return null;
                }
            }

            return out_.Get();
        }

        // 设置智力级别。
        public void SetIntelligence(IntelligenceType type) {

        }
    }
}