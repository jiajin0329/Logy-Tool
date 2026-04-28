namespace Logy.Analytics
{
    public static class AnalyticsEventName
    {
        private static class General
        {
            internal const string attack_ejaculation = ":attack_ejaculation";
            internal const string win = ":win";
            internal const string lose = ":lose";
        }

        public static class Progress
        {
            private const string group = "progress";
            public const string launch_game = group + ":launch_game";
            public const string start_scene2 = group + ":start_scene2";
        }

        public static class Scene2
        {
            private const string group = "scene2";
            public const string hold_hint_first_show = group + ":hold_hint_first_show";
            public const string hold_hint_first_close = group + ":hold_hint_first_close";
            public const string hold_hint_first_costTime = group + ":hold_hint_first_costTime";
            public const string attack_hint_first_show = group + ":attack_hint_first_show";
            public const string attack_hint_first_close = group + ":attack_hint_first_close";
            public const string attack_hint_first_costTime = group + ":attack_hint_first_costTime";
            public const string first_finish = group + ":first_finish";
            public const string first_finish_lose = group + ":first_finish_lose";
            public const string lose = group + General.lose;
            public const string attack_ejaculation = group + General.attack_ejaculation;
            public const string first_finish_win = group + ":first_finish_win";
            public const string win = group + General.win;
            public const string restart_hint_first_show = group + ":restart_hint_first_show";
            public const string restart_hint_first_close = group + ":restart_hint_first_close";
            public const string restart_hint_first_costTime = group + ":restart_hint_first_costTime";
        }

        public enum Enum
        {
            launch_game,
            start_scene1,
            start_scene2,
            start_scene3
        }
    }
}