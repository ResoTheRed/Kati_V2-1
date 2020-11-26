using System;
using System.Collections.Generic;
using System.Text;

namespace TextGameDemo.Game.Location {
    public class World {

        private Town town;
        private Forest forest;
        private Cave cave;
        private Area currentArea;
        

        public Town _Town { get => town; set => town = value; }
        public Forest _Forest { get => forest; set => forest = value; }
        public Cave _Cave { get => cave; set => cave = value; }
        public Area CurrentArea { get => currentArea; set => currentArea = value; }

        public World() {
            _Town = new Town("Village of Biggs");
            _Forest = new Forest("Gringor's Forest");
            _Cave = new Cave("Crystal Cave");
            currentArea = _Town;
            SetAreaRoomExits();
        }

        public void SetAreaRoomExits() {
            _Town.LocationsInArea[Town.GATE].AddExit(_Forest.LocationsInArea[Forest.ENTRANCE]);
            _Forest.LocationsInArea[Forest.ENTRANCE].AddExit(_Town.LocationsInArea[Town.GATE]);
            _Cave.LocationsInArea[Cave.ENTRANCE].AddExit(_Forest.LocationsInArea[Forest.MOUNTAIN]);
            _Forest.LocationsInArea[Forest.MOUNTAIN].AddExit(_Cave.LocationsInArea[Cave.ENTRANCE]);
        }

        public bool MoveRoom() {
            Console.WriteLine(CurrentArea);
            var area = CurrentArea.MoveRoom();
            if (area == null) {
                return false;
            }
            return SetRoomIfAreaChanged(area);
        }

        private bool SetRoomIfAreaChanged(Area area) {
            if (area != CurrentArea) {
                if (area.Name.Equals(_Forest.Name) && CurrentArea.Name.Equals(_Town.Name)) {
                    area.CurrentRoom = area.LocationsInArea[Forest.ENTRANCE];
                } else if (area.Name.Equals(_Town.Name) && CurrentArea.Name.Equals(_Forest.Name)) {
                    area.CurrentRoom = area.LocationsInArea[Town.GATE];
                } else if (area.Name.Equals(_Forest.Name) && CurrentArea.Name.Equals(_Cave.Name)) {
                    area.CurrentRoom = area.LocationsInArea[Forest.MOUNTAIN];
                } else if (area.Name.Equals(_Cave.Name) && CurrentArea.Name.Equals(_Forest.Name)) {
                    area.CurrentRoom = area.LocationsInArea[Cave.ENTRANCE];
                }
                CurrentArea = area;
            }
            return true;
        }

    }
}
