using System;
using System.Collections.Generic;

namespace TextGameDemo.Game {
    public class GameData : Kati.Module_Hub.GameData {

        public const float DAY_TIME_PERCENT = 0.6f;

        private bool isNight;
        private string weather;
        private List<string> weatherTypes;

        public string Weather { get => weather;  }
        public bool IsNight { get => isNight; set => isNight = value; }
        public List<string> WeatherTypes { get => weatherTypes; set => weatherTypes = value; }

        public GameData() {
            IsNight = false;
            WeatherTypes = new List<string>() { "Nice", "Nice", "Nice", "Overcast", "Overcast", "Rain" };
            weather = ChangeTheWeather();
        }

        public string ChangeTheWeather() {
            Random rand = new Random();
            weather = WeatherTypes[rand.Next(WeatherTypes.Count)];
            return weather;
        }

        public bool CheckTimeOfDay(int time) {
            IsNight = time > (int)(Timer.MOVE_THRESHOLD * DAY_TIME_PERCENT);
            return IsNight;
        }

        
    }
}
