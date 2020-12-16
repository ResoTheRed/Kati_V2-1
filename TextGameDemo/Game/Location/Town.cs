using System;
using System.Collections.Generic;
using System.Text;

namespace TextGameDemo.Game.Location {
    public class Town : Area{

        public const string NAME = "Village of Biggs";
        public const string TOWN_CENTER = "Town Center";
        public const string ALBRECHT = "Albrecht's House";
        public const string STORE = "General Store";
        public const string LAFITTE = "Lafitte's House";
        public const string SMITHY = "Black Smith's Shop";
        public const string LERIN = "Lerin's House";
        public const string BEN = "Benjamin's House";
        public const string PEASANT = "Dirty Peasant House";
        public const string GATE = "Town Gate";
        public const string TOWN_HALL = "Town Hall";

        

        public Town(string name) : base(name) {
            SetRooms();
            SetRoomExits();
            SetRoomDescriptions();
            Description = "Small town in the forest region";
            SetRoomParentAreas(this);
        }

        override
        public void SetRooms() {
            AddRoom(TOWN_CENTER, new Room(TOWN_CENTER));
            AddRoom(ALBRECHT, new Room(ALBRECHT));
            AddRoom(STORE, new Room(STORE));
            AddRoom(LAFITTE, new Room(LAFITTE));
            AddRoom(SMITHY, new Room(SMITHY));
            AddRoom(LERIN, new Room(LERIN));
            AddRoom(BEN, new Room(BEN));
            AddRoom(PEASANT, new Room(PEASANT));
            AddRoom(GATE,new Room(GATE));
            AddRoom(TOWN_HALL, new Room(TOWN_HALL));
            CurrentRoom = LocationsInArea[TOWN_CENTER];
        }

        override
        public void SetRoomExits() {
            LocationsInArea[TOWN_CENTER].AddExit(LocationsInArea[ALBRECHT]);
            LocationsInArea[TOWN_CENTER].AddExit(LocationsInArea[STORE]);
            LocationsInArea[TOWN_CENTER].AddExit(LocationsInArea[LAFITTE]);
            LocationsInArea[TOWN_CENTER].AddExit(LocationsInArea[SMITHY]);
            LocationsInArea[TOWN_CENTER].AddExit(LocationsInArea[LERIN]);
            LocationsInArea[TOWN_CENTER].AddExit(LocationsInArea[BEN]);
            LocationsInArea[TOWN_CENTER].AddExit(LocationsInArea[PEASANT]);
            LocationsInArea[TOWN_CENTER].AddExit(LocationsInArea[GATE]);
            LocationsInArea[TOWN_CENTER].AddExit(LocationsInArea[TOWN_HALL]);

            LocationsInArea[ALBRECHT].AddExit(LocationsInArea[TOWN_CENTER]);
            LocationsInArea[STORE].AddExit(LocationsInArea[TOWN_CENTER]);
            LocationsInArea[LAFITTE].AddExit(LocationsInArea[TOWN_CENTER]);
            LocationsInArea[SMITHY].AddExit(LocationsInArea[TOWN_CENTER]);
            LocationsInArea[LERIN].AddExit(LocationsInArea[TOWN_CENTER]);
            LocationsInArea[BEN].AddExit(LocationsInArea[TOWN_CENTER]);
            LocationsInArea[PEASANT].AddExit(LocationsInArea[TOWN_CENTER]);
            LocationsInArea[GATE].AddExit(LocationsInArea[TOWN_CENTER]);
            LocationsInArea[TOWN_HALL].AddExit(LocationsInArea[TOWN_CENTER]);
        }

        override
        public void SetRoomDescriptions() {
            LocationsInArea[TOWN_CENTER].SetDescription("Then Town Center is the heart of "+Name+".  It's always busy.");
            LocationsInArea[ALBRECHT].SetDescription("This is a humble home of a craftsman and hunter.");
            LocationsInArea[STORE].SetDescription("Goods line shelves along the walls");
            LocationsInArea[LAFITTE].SetDescription("This house looks to belong to a warrior.");
            LocationsInArea[SMITHY].SetDescription("It's grimy, but loaded with iron goods.");
            LocationsInArea[LERIN].SetDescription("There is a strange scent in the air and the aura of magic about.");
            LocationsInArea[BEN].SetDescription("Looks like a guard's post, not a home");
            LocationsInArea[PEASANT].SetDescription("The humble dwelling of a farmer.");
            LocationsInArea[GATE].SetDescription("This is the main entrance into "+Name+".\nYou can enter the village or head to the forest.");
            LocationsInArea[TOWN_HALL].SetDescription("A finely crafted log lodge that is home to the town council and elders.");
        }

    }
}
