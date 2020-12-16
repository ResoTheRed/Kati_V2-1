using System;
using System.Collections.Generic;
using TextGameDemo.Game.Characters;

namespace TextGameDemo {
    public class Program {
        static void Main(string[] args) {
            Game.GameModel.Run();
            //TestTextBox();
            
        }

        public static void TestTextBox() {
            Game.GameModel game = new Game.GameModel();
            Game.TUI.Menus menus = Game.TUI.Menus.Get();
            string text1 = "This is a test piece of dialogue. It should not be the big tab but the small tab.  Lets push it a little to the limits.";
            string text2 = "What say you? can you love the gentleman? This night you shall behold him at our feast; 465 Read o'er the volume of young Paris' face,";
            text2+= " And find delight writ there with beauty's pen; Examine every married lineament, And see how one another lends content";
            text2 += " And what obscured in this fair volume lies470 Find written in the margent of his eyes. This precious book of love, this unbound lover,";
            text2 += " To beautify him, only lacks a cover: The fish lives in the sea, and 'tis much pride For fair without the fair within to hide:475";
            text2 += " That book in many's eyes doth share the glory, That in gold clasps locks in the golden story; So shall you share all that he doth possess,";
            text2 += " By having him, making yourself no less.";
            menus.TextBox("Test", text1);
            menus.TextBox("Capulet Lady",text2);
        }

        public static void TestQuestTracker() {
            Game.QuestTracker.Tracker().QuestIsComplete[Game.QuestTitles.MUSHROOM] = true;
            Console.WriteLine(Game.QuestTracker.Tracker().QuestIsComplete[Game.QuestTitles.MUSHROOM]);
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
        /*
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
        */
        public static void TestLocations() {
            bool isRunning = true;
            Game.Location.World world = new Game.Location.World();
            while (isRunning) {
                isRunning = world.MoveRoom();
            }
        }
    }
}
