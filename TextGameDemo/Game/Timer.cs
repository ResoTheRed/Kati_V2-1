using System;
using System.Collections.Generic;
using System.Text;

namespace TextGameDemo.Game {
    public class Timer {

        public const int MOVE_THRESHOLD = 10;
        private static Timer timer;

        public static Timer Get() {
            if (timer == null) {
                timer = new Timer();
            }
            return timer;
        }
        
        private int allMoves;
        private int moves;
        private int daysPast;

        public int Moves { get => moves; }
        public int TotalMoves { get => allMoves;}
        public int DaysPast { get => daysPast; }

        private Timer() {
            moves = 0;
            allMoves = 0;
            daysPast = 0;
        }

        //returns true on reset (shift everything)
        public bool TakeTurn() {
            moves++;
            allMoves++;
            if (moves > MOVE_THRESHOLD) {
                moves = 0;
                daysPast++;
                return true;
            }
            return false;
        }

    }
}
