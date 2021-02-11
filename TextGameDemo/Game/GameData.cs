using System;
using System.Collections.Generic;
using TextGameDemo.Game.Characters;

namespace TextGameDemo.Game {
    public class GameData : Kati.Module_Hub.GameData {

        public const float DAY_TIME_PERCENT = 0.6f;

        private bool isNight;
        private string weather;
        private List<string> weatherTypes;
        private Dictionary<string, int> dailyCharacterConversations;

        public string Weather { get => weather;  }
        public bool IsNight { get => isNight; set => isNight = value; }
        public List<string> WeatherTypes { get => weatherTypes; set => weatherTypes = value; }
        public Dictionary<string, int> DailyCharacterConversations { get => dailyCharacterConversations; set => dailyCharacterConversations = value; }

        public GameData() {
            IsNight = false;
            WeatherTypes = new List<string>() { "Nice", "Nice", "Nice", "Overcast", "Overcast", "Rain" };
            weather = ChangeTheWeather();
            SetDailyCharacterConversations();
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

        public bool IncrementConversationCounter(string name) {
            bool good = DailyCharacterConversations.ContainsKey(name);
            if(good) 
                DailyCharacterConversations[name] += 1;
            return good;
        }

        public int GetConversationCounter(string name) {
            int value = -1;
            if (DailyCharacterConversations.ContainsKey(name)){
                value = DailyCharacterConversations[name];
            }
            return value;
        }

        public void SetDailyCharacterConversations() {
            DailyCharacterConversations = new Dictionary<string, int>();
            DailyCharacterConversations[Cast.COLLIN] = 0;
            DailyCharacterConversations[Cast.BENJAMIN] = 0;
            DailyCharacterConversations[Cast.DAN] = 0;
            DailyCharacterConversations[Cast.ALBRECHT] = 0;
            DailyCharacterConversations[Cast.ROMERO] = 0;
            DailyCharacterConversations[Cast.MICIAH] = 0;
            DailyCharacterConversations[Cast.SIVIAN] = 0;
            DailyCharacterConversations[Cast.LERIN] = 0;
            DailyCharacterConversations[Cast.LAFITTE] = 0;
            DailyCharacterConversations[Cast.TETA] = 0;
            DailyCharacterConversations[Cast.QUINN] = 0;
            DailyCharacterConversations[Cast.HELENA] = 0;
            DailyCharacterConversations[Cast.CHRISTINA] = 0;
            DailyCharacterConversations[Cast.CRUMPLES] = 0;

        }


    }
}
