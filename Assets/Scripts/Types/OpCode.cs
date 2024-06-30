namespace HarvestFestival.Types
{
    static class OpCodeType
    {
        //pre-game
        public const long MATCH_STATUS = 11;
        public const long MATCH_UPDATE_CHARACTER = 12;
        public const long MATCH_SPAWN_PLAYER = 13;

        // In Game
        public const long GAME_START = 21;
        public const long GAME_PAUSE = 22;
        public const long PLAYER_MOVE = 23;
        public const long PLAYER_ROTATE = 24;
    }
}