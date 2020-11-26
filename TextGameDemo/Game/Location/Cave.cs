using System;
using System.Collections.Generic;
using System.Text;

namespace TextGameDemo.Game.Location {
    public class Cave : Area {

        public const string ENTRANCE = "Cave Entrance";
        public const string PASSAGE = "Narrow Passage";
        public const string SMALL_CAMBER = "Small Chamber";
        public const string LARGE_CHAMBER = "Large Chamber";


        public Cave(string name) : base(name) {
            SetRooms();
            SetRoomExits();
            SetRoomDescriptions();
            Description = "The cave is dark and damp.";
            SetRoomParentAreas(this);
        }

        override
        public void SetRooms() {
            AddRoom(ENTRANCE, new Room(ENTRANCE));
            AddRoom(PASSAGE, new Room(PASSAGE));
            AddRoom(SMALL_CAMBER, new Room(SMALL_CAMBER));
            AddRoom(LARGE_CHAMBER, new Room(LARGE_CHAMBER));
            CurrentRoom = LocationsInArea[ENTRANCE];
        }

        override
        public void SetRoomExits() {
            LocationsInArea[ENTRANCE].AddExit(LocationsInArea[PASSAGE]);
            LocationsInArea[PASSAGE].AddExit(LocationsInArea[ENTRANCE]);
            LocationsInArea[PASSAGE].AddExit(LocationsInArea[SMALL_CAMBER]);
            LocationsInArea[PASSAGE].AddExit(LocationsInArea[LARGE_CHAMBER]);
            LocationsInArea[SMALL_CAMBER].AddExit(LocationsInArea[PASSAGE]);
            LocationsInArea[LARGE_CHAMBER].AddExit(LocationsInArea[PASSAGE]);
        }

        override
        public void SetRoomDescriptions() {
            LocationsInArea[ENTRANCE].SetDescription("The light quickly fades as the enterance narrows into a passage.");
            LocationsInArea[PASSAGE].SetDescription("A dark damp rocky passage.");
            LocationsInArea[SMALL_CAMBER].SetDescription("A small open chamber holding many geological features.");
            LocationsInArea[LARGE_CHAMBER].SetDescription("A large open chamber.  It looks like something lives here.");
        }
    }
}
