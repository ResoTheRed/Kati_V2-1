using System;
using System.Collections.Generic;
using System.Text;

namespace TextGameDemo.Game.Location {
    public abstract class Area {

        private string name;
        private string description;
        private Room currentRoom;
        private Dictionary<string,Room> locationsInArea;

        public string Name { get => name; }
        public string Description { get => description; set => description = value; }
        public Dictionary<string, Room> LocationsInArea { get => locationsInArea; }
        public Room CurrentRoom { get => currentRoom; set => currentRoom = value; }

        public Area(string name) {
            this.name = name;
            locationsInArea = new Dictionary<string, Room>();
        }

        //adds room locations to the area
        public void AddRoom(string key, Room room) {
            locationsInArea[key]= room;
        }

        public Area MoveRoom() { 
            DisplayRoomInfo();
            try {
                string value = Console.ReadLine();
                if (value.Equals("q")||value.Equals("Q")) {
                    return null;
                }
                int choice = Int32.Parse(value);
                CurrentRoom = LocationsInArea[CurrentRoom.Name].GetExit(choice);
            } catch (Exception) {
                MoveRoom();
            }
            return CurrentRoom.ParentArea;
        }

        public void DisplayRoomInfo() {
            //Console.WriteLine(CurrentRoom);
            Console.WriteLine("What do you want to do?");
            Console.WriteLine(CurrentRoom.GetExits());
        }

        override
        public string ToString() {
            return Name + "\n" + CurrentRoom;
        }

        public void SetAreaRoomExits(Area current, string currentKey, Area exit,  string exitKey) {
            current.LocationsInArea[currentKey].AddExit(exit.LocationsInArea[exitKey]);
        }

        public void SetRoomParentAreas(Area area) {
            foreach (string place in LocationsInArea.Keys) {
                area.LocationsInArea[place].ParentArea = area;
            }
        }

        public abstract void SetRooms();
        public abstract void SetRoomExits();
        public abstract void SetRoomDescriptions();
        
      
    }
}
