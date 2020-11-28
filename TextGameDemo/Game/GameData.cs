using System;
using System.Collections.Generic;
using System.Text;

namespace TextGameDemo.Game {
    public class GameData {

        private bool isNight;
        private string weather;
        private List<string> weatherTypes;

        
        public GameData() {
            isNight = false;
            weatherTypes = new List<string>() { "Nice", "Nice", "Nice", "Overcast", "Overcast", "Rain" };
        }

        public string ChangeTheWeather() {
            Random rand = new Random();
            weather = weatherTypes[rand.Next(weatherTypes.Count)];
            return weather;
        }

        public bool CheckTimeOfDay(int time) {
            isNight = time > 60;
            return isNight;
        }

        
    }
}
