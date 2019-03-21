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

    class ScoreData {
        // 牌的手数。
        int times;
        // 与其他两家相比，牌的评分。
        uint score;

        public static bool operator<=(ScoreData a, ScoreData b) {
            if (a.times == b.times) {
                return a.score < b.score;
            }
            return a.times < b.times;
        }

        public static bool operator>=(ScoreData a, ScoreData b) {
            return !(a <= b);
        }

        public static bool operator==(ScoreData a, ScoreData b) {
            return a.times == b.times && a.score == b.score;
        }
        public static bool operator!=(ScoreData a, ScoreData b) {
            return !(a == b);
        }
    }

    abstract class Score__ : IScore__ {
        IEnvironment__ environment;
        public int Limit(LimitType type) {
            if (type == LimitType.land) {
                return environment.LandLimit();
            }
            return environment.DoubleLimit();
        }

        /* 需求：
         * 1. 根据手中牌手数和每手牌的大小，对自己的牌进行评分，评分越低表明获胜几率越大。
         * 
         * 架构：
         * 1. 手数组件，计算手中牌几手可以出完。
         * 2. 大小组件，计算每手牌在三家手中牌的大小，牌型越大，分数越低。      
         */
        public int My() {

        }
    }
}