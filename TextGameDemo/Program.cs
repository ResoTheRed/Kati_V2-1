using System;

namespace TextGameDemo {
    public class Program {
        static void Main(string[] args) {
            TestLocations();
        }

        public static void TestLocations() {
            bool isRunning = true;
            Game.Location.World world = new Game.Location.World();
            while (isRunning) {
                isRunning = world.MoveRoom();
            }
        }
    }
}
