using System;
using System.Collections.Generic;
using TextGameDemo.Game.Characters;

namespace TextGameDemo {
    public class Program {
        static void Main(string[] args) {
            //TestLocations();
            //TestPlayer();
            //TestCharacterLocations();
            TestGameModel();
        }

        public static void TestGameModel() {
            Game.GameModel game = new Game.GameModel();
            game.GameLoop();
        }

        public static void TestCharacterLocations() {
            Player player = new Player();
            CharacterLib lib = new CharacterLib(player);
            foreach (KeyValuePair<string, Character> item in lib.Lib) {
                string area, room = "";
                (area, room)= item.Value.Locations.getLocation();
                Console.WriteLine(item.Key + " " + area + " " + room);
                lib.Lib[item.Key].Locations.ChangeRooms();
            }
            foreach (KeyValuePair<string, Character> item in lib.Lib) {
                string area, room = "";
                (area, room) = item.Value.Locations.getLocation();
                Console.WriteLine(item.Key + " " + area + " " + room);
            }
        }

        public static void TestPlayer() {
            Player player = new Player();
            CharacterLib lib = new CharacterLib(player);
            Console.WriteLine(((Player)lib.Lib[Cast.PLAYER]).BranchAttributesToString());
            lib.Lib[Cast.LERIN].BranchAttributes[Cast.PLAYER][Social.ROMANCE] = 900;
            lib.Lib[Cast.LERIN].BranchAttributes[Cast.PLAYER][Social.FRIEND] = 500;
            lib.Lib[Cast.LERIN].BranchAttributes[Cast.PLAYER][Social.PROFESSIONAL] = 600;
            player.UpdateBranchAttributes(Cast.LERIN, lib.Lib[Cast.LERIN].BranchAttributes[Cast.PLAYER]);
            Console.WriteLine(((Player)lib.Lib[Cast.PLAYER]).BranchAttributesToString());
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
