using System;
using System.Collections.Generic;
using System.Text;

namespace TextGameDemo.Game {
    public class GameData {

        public const float DAY_TIME_PERCENT = 0.6f;

        private bool isNight;
        private string weather;
        private List<string> weatherTypes;

        
        public GameData() {
            isNight = false;
            weatherTypes = new List<string>() { "Nice", "Nice", "Nice", "Overcast", "Overcast", "Rain" };
            weather = ChangeTheWeather();
        }

        public string Weather { get => weather;  }

        public string ChangeTheWeather() {
            Random rand = new Random();
            weather = weatherTypes[rand.Next(weatherTypes.Count)];
            return weather;
        }

        public bool CheckTimeOfDay(int time) {
            isNight = time > (int)(Timer.MOVE_THRESHOLD * DAY_TIME_PERCENT);
            return isNight;
        }

        
    }
}
