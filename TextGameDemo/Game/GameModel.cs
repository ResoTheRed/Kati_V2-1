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

        const string pos = "Positive", neg = "Negative", rom = "Romance", dis ="Disgust", rand = "Random";

        public static void Run() {
            
            GameModel game = new GameModel();
            game.GameLoop();
            game.connect.Exit();
        }

        private readonly int relationshipDelta = 10;
        private string botType = rom;

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
        public GameData GameData { get => gameData; set => gameData = value; }

        public GameModel() {
            World = new World();
            GameData = new GameData();
            Player = new Player();
            PlayersStats.Get();
            Lib = new CharacterLib(Player);
            Timer = Timer.Get();
            GetCharactersInRoom();
            TUI.Menus.SetupMenus(GameData,  Lib);
            nextDay = false;
            connect = new KatiConnection(this);
        }

        public void GameLoop() {
            bool isRunning = true;
            //each iteration is a tick
            while (isRunning) {
                Console.WriteLine("In game loop");
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
                    (characterArea, characterRoom) = item.Value.Locations.GetLocation();
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
            ChangeCharacterRelationships(relationshipDelta,CharactersInRoom[index]);
        }

        public string Talk(int index) {
            string speech = ""; //CharactersInRoom[index].Talk()+" - ";
            (string area, string room) = lib.Lib[Characters.Cast.PLAYER].Locations.GetLocation();
            speech += RunDialogueConnection(area, room, CharactersInRoom[index].Name,speech);
            return speech;
        }

        public string RunDialogueConnection(string area, string room,string name, string speech) {
            (Kati.Module_Hub.DialoguePackage pack, bool isChain) = connect.RunSystem(area, room, name);
            speech += pack.Dialogue;
            if (isChain) {
                return RunDialogueConnection(area, room, name, speech + " #chained_dialogue# ");
            } else if (pack.IsResponse) {
                connect.RunSystem(area,room,name);
                string value = TUI.Menus.GetResponseOption(pack.Response);
                speech += " #player_response# "+value;
                connect.RecordPlayerResponse(name, value);
                pack.IsResponse = false;
            }
            return speech;
        }

        /************************Change Character's Relationship statuses*************************/
        //"Positive","Negative","Romance","Disgust","Random"
        public void ChangeCharacterRelationships(int value, Character character) {
            switch (botType) {
                case pos: { ChangePositive(value, character); }break;
                case neg: { ChangeNegative(value, character); }break;
                case rom: { ChangeRomantic(value, character); }break;
                case dis: { ChangeAttribute(value,Social.DISGUST,character); }break;
                case rand: { ChangeRandom(value, character); } break;
            }
        }

        //likely make this it's own class
        public void ChangePositive(int value, Character character) {
            int index = GameTools.Tools().Next(3);
            string[] boy = { Social.FRIEND, Social.RESPECT, Social.PROFESSIONAL};
            string[] girl = { Social.FRIEND, Social.ROMANCE, Social.AFFINITY, Social.PROFESSIONAL };
            if (character.IsMale) {
                ChangeAttribute(value,boy[index],character);
            } else {
                index = GameTools.Tools().Next(4);
                ChangeAttribute(value, girl[index], character);
            }
        }

        //update relationship stats
        public void ChangeNegative(int value, Character character) {
            int index = GameTools.Tools().Next(3);
            string[] at = { Social.DISGUST, Social.HATE, Social.RIVALRY};
            ChangeAttribute(value, at[index],character);
        }

        public void ChangeRomantic(int value, Character character) {
            if (character.IsMale) {
                ChangePositive(value, character);
            } else {
                ChangeAttribute(value, Social.ROMANCE, character);
            }
        }

        public void ChangeRandom(int value, Character character) {
            string[] boy = { Social.FRIEND, Social.RESPECT, Social.PROFESSIONAL, Social.DISGUST, Social.HATE, Social.RIVALRY };
            string[] girl = { Social.FRIEND, Social.ROMANCE, Social.AFFINITY, Social.PROFESSIONAL, Social.DISGUST, Social.HATE, Social.RIVALRY };
            int index;
            if (character.IsMale) {
                index = GameTools.Tools().Next(boy.Length);
                ChangeAttribute(value, boy[index], character);
            } else {
                index = GameTools.Tools().Next(girl.Length);
                ChangeAttribute(value, girl[index], character);
            }
        }

        public void ChangeAttribute(int value, string attribute, Character character) {
            Player.BranchAttributes[character.Name][attribute] += value;
        }
        /*******************************************************************************************/


        public void UpdateNextDay() {
            GameData.ChangeTheWeather();
            Lib.ChangeLocations();
            GetCharactersInRoom();
            connect.GameHistory.ResetShortTerm();
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
