using System;

namespace Pong.Match
{
    class MatchState
    {
        public PlayerType PlayerOne { get; set; }

        public PlayerType PlayerTwo { get; set; }

        public MatchState()
        {
            this.PlayerOne = PlayerType.Local;
            this.PlayerTwo = PlayerType.Bot;
        }
    }

    enum PlayerType
    {
        Local,
        Remote,
        Bot,
    }
}