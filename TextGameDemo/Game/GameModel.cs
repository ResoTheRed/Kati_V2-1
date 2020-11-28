using System;
using System.Collections.Generic;
using TextGameDemo.Game.Characters;
using TextGameDemo.Game.Location;

namespace TextGameDemo.Game {
    
    /// <summary>
    /// Houses all of the game components and works as a central hub
    /// that holds composition with all major elements
    /// </summary>
    
    public class GameModel {

        private World world;
        private Player player;
        private CharacterLib lib;
        private Timer timer;
        private List<Character> charactersInRoom;
       

        public World World { get => world; set => world = value; }
        public Player Player { get => player; set => player = value; }
        public CharacterLib Lib { get => lib; set => lib = value; }
        public List<Character> CharactersInRoom { get => charactersInRoom; set => charactersInRoom = value; }
        public Timer Timer { get => timer; set => timer = value; }

        public GameModel() {
            World = new World();
            Player = new Player();
            Lib = new CharacterLib(Player);
            Timer = new Timer();
            GetCharactersInRoom();
        }

        public void GameLoop() {
            bool isRunning = true;
            while (isRunning) {
                isRunning = DisplayRoomOptions();
            }
        }

        public void GetCharactersInRoom() {
            CharactersInRoom = new List<Character>();
            foreach (KeyValuePair<string, Character> item in Lib.Lib) {
                if (item.Key != Cast.PLAYER) {
                    string characterArea, characterRoom = "";
                    (characterArea, characterRoom) = item.Value.Locations.getLocation();
                    string worldArea, worldRoom = "";
                    (worldArea, worldRoom) = World.GetLocation();
                    if (characterArea.Equals(worldArea) && characterRoom.Equals(worldRoom)) {
                        CharactersInRoom.Add(item.Value);
                    }
                }
            }
        }

        public bool DisplayRoomOptions() {
            string options = "";
            bool continuePlaying = true;
            if (CharactersInRoom.Count > 0) {
                options += "[T|t] talk to person(s) in room\n";
            }
            options += "[M|m] move to a new location\n";
            options += "[I|i] display Player inventory and status\n";
            options += "[Q|q] to quit game.";
            Console.WriteLine(options);
            return ExecuteRoomOptions(continuePlaying);
        }

        public bool ExecuteRoomOptions(bool continuePlaying) {
            string choice = Console.ReadLine();
            if (choice.Equals("T") || choice.Equals("t") && CharactersInRoom.Count > 0) {
                TalkToPeople();
                Timer.TakeTurn();
            } else if (choice.Equals("M") || choice.Equals("m")) {
                MoveLocation();
                Timer.TakeTurn();
            } else if (choice.Equals("I") || choice.Equals("i")) {
                Console.WriteLine(player.BranchAttributesToString());
                //inventory list
                //quest list
                ExecuteRoomOptions(continuePlaying);
            } else {
                continuePlaying = false;
            }
            return continuePlaying;
        }

        public void TalkToPeople() {
            int index = 1;
            Console.WriteLine("Who will you talk to?");
            foreach (Character c in CharactersInRoom) {
                Console.WriteLine(index+". "+c.Name);
                index++;
            }
            string choice = Console.ReadLine();
            index = Int32.Parse(choice) - 1;
            CharactersInRoom[index].Talk();
        }

        public void MoveLocation() {
            World.MoveRoom();
            GetCharactersInRoom();
        }

    }
}
