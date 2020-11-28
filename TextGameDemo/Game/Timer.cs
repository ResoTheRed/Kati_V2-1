using System;
using System.Collections.Generic;
using System.Text;

namespace TextGameDemo.Game {
    public class Timer {

        public const int MOVE_THRESHOLD = 100;
        private int allMoves;
        private int moves;

        public int Moves { get => moves; }
        public int TotalMoves { get => allMoves;}

        public Timer() {
            moves = 0;
            allMoves = 0;
        }

        public void TakeTurn() {
            moves++;
            allMoves++;
            if (moves > MOVE_THRESHOLD) {
                moves = 0;
            }
        }

    }
}
