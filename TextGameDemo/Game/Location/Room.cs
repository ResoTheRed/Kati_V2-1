using System;
using System.Collections.Generic;
using TextGameDemo.Game.Characters;

namespace TextGameDemo.Game.Location {
    public class Room {

        private string name;
        private string description;
        //location that this room exists in
        private Area parentArea;
        private List<Room> exits;
        //hold all character currently in the room
        private List<Character> currentCharactersInRoom;

        public string Name { get => name; }
        public string Description { get => description; }
       
        public List<Character> CurrentCharactersInRoom { get => currentCharactersInRoom; set => currentCharactersInRoom = value; }
        public List<Room> Exits { get => exits; set => exits = value; }
        public Area ParentArea { get => parentArea; set => parentArea = value; }

        public Room(string name) {
            this.name = name;
            Exits = new List<Room>();
            currentCharactersInRoom = new List<Character>();
        }

        public void SetDescription(string desc) {
            description = desc;
        }

        public string GetExits() {
            string exits = "";
            for (int i = 1; i <= Exits.Count; i++) {
                exits += i + ". " + Exits[i - 1].Name + "\n";
            }
            exits += "[Q|q] to quit game\n";
            return exits;
        }

        public Room GetExit(int index) {
            return Exits[index - 1];
        }

        public void AddExit(Room exit) {
            Exits.Add(exit);
        }

        public string GetCharacterString() {
            string characters = "";
            for (int i = 1; i <= currentCharactersInRoom.Count; i++) {
                characters += i + ". " + currentCharactersInRoom[i - 1] + "\n";
            }
            if (characters.Length == 0) {
                characters = "No one's here.\n";
            }
            return characters;
        }

        public Character GetCharacter(int index) {
            return CurrentCharactersInRoom[index - 1];
        }

        override
        public string ToString() {
            return name + "\n" + description + "\n";
        }

        
    }
}
