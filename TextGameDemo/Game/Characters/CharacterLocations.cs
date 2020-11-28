using System;
using System.Collections.Generic;
using System.Text;

namespace TextGameDemo.Game.Characters {
    public class CharacterLocations {

        Dictionary<string, List<string>> locals;
        List<string> keys;

        string currentArea;
        string currentRoom;

        public CharacterLocations() {
            locals = new Dictionary<string, List<string>>();
            keys = new List<string>();
        }

        public void SetAreaKeys(List<string> keys) {
            this.keys = keys;
            foreach (string key in keys) {
                locals[key] = new List<string>();
            }
        }

        public void SetAreaRooms(string key, List<string> rooms) {
            locals[key] = rooms;
        }

        public void ChangeRooms() {
            Random rand = new Random();
            if (keys.Count>=1) { 
                currentArea = keys[rand.Next(keys.Count)];
                currentRoom = locals[currentArea][rand.Next(locals[currentArea].Count)];
            } 
        }

        public (string, string) getLocation() {
            return (currentArea, currentRoom);
            }

    }
}
