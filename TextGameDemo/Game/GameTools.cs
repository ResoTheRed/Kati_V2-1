using System;
using System.Collections.Generic;
using System.Text;

namespace TextGameDemo.Game {
    public class GameTools {

        private static GameTools tools;

        public static GameTools Tools() {
            if (tools == null) {
                tools = new GameTools();
            }
            return tools;
        }

        private Random rand;

        private GameTools() {
            rand = new Random();
        }

        private Random Seed() {
            int seed = rand.Next();
            return new Random(seed);
        }

        public int Next() {
            return Seed().Next();
        }

        public int Next(int max) {
            return Seed().Next(max);
        }

        public int Next(int min, int max) {
            return Seed().Next(min, max);
        }

        
    
    }
}
