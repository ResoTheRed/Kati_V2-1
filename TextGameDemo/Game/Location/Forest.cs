using System;
using System.Collections.Generic;
using System.Text;

namespace TextGameDemo.Game.Location {
    public class Forest : Area{

        public const string  ENTRANCE = "Forest Entrance";
        public const string PATH = "Wooded Path";
        public const string CLEARING = "Clearing";
        public const string MOUNTAIN = "Mountain Base";


        public Forest(string name) : base(name) {
            SetRooms();
            SetRoomExits();
            SetRoomDescriptions();
            Description = "The forest can be a dangerous place";
            SetRoomParentAreas(this);
        }

        override
        public void SetRooms() {
            AddRoom(ENTRANCE, new Room(ENTRANCE));
            AddRoom(PATH, new Room(PATH));
            AddRoom(CLEARING, new Room(CLEARING));
            AddRoom(MOUNTAIN, new Room(MOUNTAIN));
            CurrentRoom = LocationsInArea[ENTRANCE];
        }

        override
        public void SetRoomExits() {
            LocationsInArea[ENTRANCE].AddExit(LocationsInArea[PATH]);
            LocationsInArea[PATH].AddExit(LocationsInArea[ENTRANCE]);
            LocationsInArea[PATH].AddExit(LocationsInArea[CLEARING]);
            LocationsInArea[PATH].AddExit(LocationsInArea[MOUNTAIN]);
            LocationsInArea[CLEARING].AddExit(LocationsInArea[PATH]);
            LocationsInArea[CLEARING].AddExit(LocationsInArea[MOUNTAIN]);
            LocationsInArea[MOUNTAIN].AddExit(LocationsInArea[PATH]);
        }

        override
        public void SetRoomDescriptions() {
            LocationsInArea[ENTRANCE].SetDescription("This is right outside of the village near a forest path.");
            LocationsInArea[PATH].SetDescription("A densely wooded path forks in different directions.");
            LocationsInArea[CLEARING].SetDescription("A clearing in the woods.  It looks like a possible campsite.");
            LocationsInArea[MOUNTAIN].SetDescription("The area where the forest transitions into the mountains.");
        }

    }
}
