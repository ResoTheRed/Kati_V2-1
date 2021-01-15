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

        public static void Run() {
            GameModel game = new GameModel();
            game.GameLoop();
        }

        private World world;
        private Player player;
        private CharacterLib lib;
        private Timer timer;
        private List<Character> charactersInRoom;
        private KatiConnection connect;
        private GameData gameData;
        private bool nextDay;

        public World World { get => world; set => world = value; }
        public Player Player { get => player; set => player = value; }
        public CharacterLib Lib { get => lib; set => lib = value; }
        public List<Character> CharactersInRoom { get => charactersInRoom; set => charactersInRoom = value; }
        public Timer Timer { get => timer; set => timer = value; }
        public KatiConnection Connect { get => connect; set => connect = value; }

        public GameModel() {
            World = new World();
            gameData = new GameData();
            Player = new Player();
            PlayersStats.Get();
            Lib = new CharacterLib(Player);
            Timer = Timer.Get();
            connect = new KatiConnection();
            GetCharactersInRoom();
            TUI.Menus.SetupMenus(gameData,  Lib);
            nextDay = false;
        }

        public void GameLoop() {
            bool isRunning = true;
            //each iteration is a tick
            while (isRunning) {
                isRunning = DisplayRoomOptions();
                if (nextDay) {
                    UpdateNextDay();
                }
            }
        }

        //sets list CharactersInRoom to all characters currently in the room
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
                options += "[T|t] talk to person(s) in room\n\t";
                foreach (Character c in CharactersInRoom) {
                    options += c.Name+", ";
                }
                options = options.Remove(options.Length - 2);
                options += "\n";
            }
            options += "[M|m] move to a new location\n";
            options += "[I|i] display Player inventory and status\n";
            options += "[W|w] to wait\n";
            options += "[Q|q] to quit game.";
            Console.WriteLine(options);
            return ExecuteRoomOptions(continuePlaying);
        }

        public bool ExecuteRoomOptions(bool continuePlaying) {
            string choice = Console.ReadLine();
            if (choice.Equals("T") || choice.Equals("t") && CharactersInRoom.Count > 0) {
                TalkToPeople();
                nextDay = Timer.TakeTurn();
            } else if (choice.Equals("M") || choice.Equals("m")) {
                MoveLocation();
                nextDay = Timer.TakeTurn();
            } else if (choice.Equals("I") || choice.Equals("i")) {
                TUI.Menus.Get().InventoryLoop();
                ExecuteRoomOptions(continuePlaying);
            } else if (choice.Equals("w") || choice.Equals("W")) {
                nextDay = Timer.TakeTurn();
            } else {
                continuePlaying = false;
            }
            return continuePlaying;
        }

        /*********************************Talking Methods********************************/
        public void TalkToPeople() {
            int index = 1;
            Console.WriteLine("Who will you talk to?");
            foreach (Character c in CharactersInRoom) {
                Console.WriteLine(index+". "+c.Name);
                index++;
            }
            string choice = Console.ReadLine();
            index = Int32.Parse(choice) - 1;

            TUI.Menus.Get().TextBox(CharactersInRoom[index].Name, Talk(index));
            //temp debug line
            ChangePositive(10,CharactersInRoom[index]);
        }

        public string Talk(int index) {
            string speech = CharactersInRoom[index].Talk()+" - ";
            (string area, string room) = lib.Lib[Characters.Cast.PLAYER].Locations.getLocation();
            speech += connect.GetModuleInfo(room, CharactersInRoom[index].Name);
            return speech;
        }

        /************************Change Character's Relationship statuses*************************/
        //likely make this it's own class
        public void ChangePositive(int value, Character character) {
            Random rand = new Random();
            int index = rand.Next(3); ;
            string[] boy = { Social.FRIEND, Social.RESPECT, Social.PROFESSIONAL};
            string[] girl = { Social.FRIEND, Social.ROMANCE, Social.AFFINITY, Social.PROFESSIONAL };
            if (character.IsMale) {
                ChangeAttribute(value,boy[index],character);
            } else {
                index = rand.Next(4);
                ChangeAttribute(value, girl[index], character);
            }
        }

        //update relationship stats
        public void ChangeNegative(int value, Character character) {
            Random rand = new Random();
            int index = rand.Next(3);
            string[] at = { Social.DISGUST, Social.HATE, Social.RIVALRY};
            ChangeAttribute(value, at[index],character);
        }

        public void ChangeAttribute(int value, string attribute, Character character) {
            Player.BranchAttributes[character.Name][attribute] += value;
        }
        /*******************************************************************************************/


        public void UpdateNextDay() {
            Lib.ChangeLocations();
            GetCharactersInRoom();
            nextDay = false;
        }

        public void MoveLocation() {
            World.MoveRoom();
            SetPlayersLocation(World.CurrentArea.Name,World.CurrentArea.CurrentRoom.Name);
            GetCharactersInRoom();
        }

        public void SetPlayersLocation(string area, string room) {
            Lib.Lib[Cast.PLAYER].Locations.CurrentArea = area;
            Lib.Lib[Cast.PLAYER].Locations.CurrentRoom = room;
        }

    }
}
