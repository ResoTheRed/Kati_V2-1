using System;
using System.Collections.Generic;
using System.Text;

namespace Kati.Module_Hub{

    /// <summary>
    /// Contains all of the game meta data needed for dialogue
    /// </summary>
    public class GameData {
        //whats the weather like outside (nice_day)
        private string weather;
        //what sector of the game is the player in? It should be a number if they are outside
        // or a location name if inside. ("sector 1" outside, "The Blonde Mouse Coffee House" inside)
        // these need to be predifined
        private string sector;
        // time of day: morining, afternoon, evening, night
        private string timeOfDay;
        // 1-28 days in month --> all months have 28 days
        private int dayOfMonth;
        // 1-4 may not keep this, can be found using arithmetic
        private int week;
        private int dayOfWeek;
        // spring, summer, fall, winter
        private string season;
        //public event near.  event dates are set in stone and can be found
        //using the dayOfMonth and event calendar list next event.
        private string publicEvent;
        //format: season : { event : day }
        private Dictionary<string, Dictionary<string, int>> eventCalendar;
        private string gameEventTrigger;

        public string Weather { get => weather; set => weather = value; }
        public string Sector { get => sector; set => sector = value; }
        public string TimeOfDay { get => timeOfDay; set => timeOfDay = value; }
        public int DayOfMonth { get => dayOfMonth; set =>  dayOfMonth = value;  }
        public int Week { get => week; set => week = value; }
        public string Season { get => season; set => season = value; }
        public string PublicEvent { get => publicEvent; set => publicEvent = value; }
        public Dictionary<string, Dictionary<string, int>> EventCalendar { get => eventCalendar; set => eventCalendar = value; }
        public int DayOfWeek { get => dayOfWeek; set => dayOfWeek = value; }
        public string GameEventTrigger { get => gameEventTrigger; set => gameEventTrigger = value; }

        public GameData() {
            SetupEventCalendar();
        }

        public GameData(string weather, string sector, string timeOfDay, int dayOfMonth, string season) {
            SetupEventCalendar();
            Weather = weather; Sector = sector; TimeOfDay = timeOfDay; DayOfMonth = dayOfMonth; Season = season;
        }

        private void SetupEventCalendar() {
            EventCalendar = new Dictionary<string, Dictionary<string, int>>();
            EventCalendar["spring"] = new Dictionary<string, int>();
            EventCalendar["summer"] = new Dictionary<string, int>();
            EventCalendar["fall"] = new Dictionary<string, int>();
            EventCalendar["winter"] = new Dictionary<string, int>();
            EventCalendar["spring"]["art_fest"] = 12;
            EventCalendar["spring"]["blueberry_fest"] = 21;
            EventCalendar["summer"]["writers_block"] = 10;
            EventCalendar["summer"]["carnival"] = 24;
            EventCalendar["fall"]["music_fest"] = 15;
            EventCalendar["fall"]["halloween"] = 28;
            EventCalendar["winter"]["bizaar"] = 7;
            EventCalendar["winter"]["yule_tide"] = 26;
        }

        public bool EventIsNear(int dayRangeStart, string season, string _event) {
            try {
                int eventDay = EventCalendar[season][_event];
                return dayOfMonth >= dayRangeStart && dayOfMonth < eventDay;
            } catch (Exception e) {
                Console.WriteLine("Event doesn't exist");
            }
            return false;
        }

        public string EventIsNear() {
            string _event_ = "none";
            int distance = 6;
            foreach (KeyValuePair<string, int> item in EventCalendar[Season]) {
                if (dayOfMonth >= item.Value - distance && dayOfMonth < item.Value)
                    return item.Key;
            }
            return _event_;
        }

        public void SetWeek() {
            Week = DayOfMonth / 7 + 1;
        }

        public void SetDayOfWeek() {
            DayOfWeek = DayOfMonth % 7;
        }

        public void SetPublicEvent() {
            int min = 30;
            int distance = 6;
            foreach (var _event in EventCalendar[Season]) {
                if (_event.Value >= DayOfMonth && _event.Value - DayOfMonth <= min) {
                    if (MathF.Abs(_event.Value - DayOfMonth) <= distance) {
                        PublicEvent = _event.Key;
                        min = _event.Value - DayOfMonth;
                    } else {
                        PublicEvent = "None";
                    }
                }
            }
            if(min>28)
                PublicEvent = "None";
        }

        
    }
   
}
