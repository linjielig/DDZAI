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
        ScoreData Limit(LimitType type);
        // 我手中牌评分。
        ScoreData My();
        // 其他玩家中较好的手中牌评分。
        ScoreData BetterOther();
        // 模拟出牌或过牌评分排名。如果我是地主或者是牌比较好的农民，返回的是自己的排名。
        // 如果我是牌比较差的农名，返回的是牌比较好的农民评分的排名。
        int SimulateRanking();
    }

    class ScoreData {
        // 牌的手数。最大的炸弹-1，最大的其他牌型0，不是最大的牌型1.总手数为-1必赢，总手数为0，
        // 需要在不拆牌的情况下再出一手牌必赢。
        int times;
        // 与其他两家相比，牌的评分。最大的牌型评分为0，其他两家每有一个大于我的牌型的牌，
        // 则我的评分加1，手数相同时，评分越低说明牌越好。
        uint score;

        public static bool operator<(ScoreData a, ScoreData b) {
            if (a.times == b.times) {
                return a.score < b.score;
            }
            return a.times < b.times;
        }

        public static bool operator>(ScoreData a, ScoreData b) {
            if (a == b) {
                return false;
            }
            return !(a < b);
        }

        public static bool operator==(ScoreData a, ScoreData b) {
            return a.times == b.times && a.score == b.score;
        }

        public static bool operator!=(ScoreData a, ScoreData b) {
            return !(a == b);
        }

        public static ScoreData operator-(ScoreData a, ScoreData b) {
            return new ScoreData(a.times - b.times, a.score - b.score);
        }

        public ScoreData(int times_, uint score_) {
            times = times_;
            score = score_;
        }
    }

    abstract class Score__ : IScore__ {
        IEnvironment__ environment;
        ITimes__ times;
        ICompareScore__ compareScore;
        public ScoreData Limit(LimitType type) {
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
         * 2. 大小比较组件，与其他两家相比，牌的评分。最大的牌型评分为0，其他两家每有一个大于我的牌型的牌，
         * 则我的评分加1，手数相同时，评分越低说明牌越好。。      
         */
        public ScoreData My() {
            return new ScoreData(times.My(), compareScore.My());
        }

        public ScoreData BetterOther() {
            return new ScoreData(times.BetterOther(), compareScore.BetterOther());
        }
    }
}