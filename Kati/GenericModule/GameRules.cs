using Kati.Module_Hub;
using Kati.SourceFiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kati.GenericModule {

    /// <summary>
    /// parser class that removes all dialogues that req game
    /// rules and are not met.
    /// </summary>
    public class GameRules {

        //Default rule branches from game
        private Controller ctrl;
        private Dictionary<string, List<string>> keys;

        public GameRules(Controller ctrl) {
            Ctrl = ctrl;
            keys = new Dictionary<string, List<string>>();
        }

        public Controller Ctrl { get => ctrl; set => ctrl = value; }
        public Dictionary<string, List<string>> Keys { get => keys; set => keys = value; }

        public void SetKeys(string key, List<string> value) {
            Keys[key] = value;
        }

        /**********************Remove GameData Non aplicable Content**************************/

        //format:: {"dialogue":{"req" : [],"leads to" : []}}
        //all done by reference
        public Dictionary<string, Dictionary<string, List<string>>> ParseGameRequirments
                                (Dictionary<string, Dictionary<string, List<string>>> data) {
            List<string> keysToDelete = new List<string>();
            foreach (KeyValuePair<string, Dictionary<string, List<string>>> item in data) {
                foreach (string req in data[item.Key][Constants.REQ]) {
                    if (RemoveElement(req)) {
                        keysToDelete.Add(item.Key);
                    }
                }
            }
            foreach (string key in keysToDelete) {
                if (data.ContainsKey(key))
                    data.Remove(key);
            }
            return data;
        }

        //fault tolerant method.
        public bool RemoveElement(string req) {
            if (req == null)
                return true;
            string[] arr = req.Split(".");
            if (arr.Length < 3 && arr[0].Equals(Constants.GAME)) {
                return true;
            } else if (arr.Length == 0 || !arr[0].Equals(Constants.GAME)) {
                return false;
            } else {
                string[] temp = new string[arr.Length - 1];
                for (int i = 1; i <= arr.Length - 1; i++) {//remove game Keyword
                    temp[i - 1] = arr[i];
                }
                return RuleDirectory(temp);
            }
        }

        public bool RuleDirectory(string[] arr) {
            string key = arr[0];
            string[] temp = new string[arr.Length - 1];
            for (int i = 1; i <= arr.Length - 1; i++) {
                temp[i - 1] = arr[i];
            }
            switch (key) {
                case Constants.WEATHER: { return CheckWeather(temp); }
                case Constants.SECTOR: { return CheckSector(temp); }
                case Constants.TIME_OF_DAY: { return CheckTimeOfDay(temp); }
                case Constants.DAY_OF_WEEK: { return CheckDayOfWeek(temp); }
                case Constants.SEASON: { return CheckSeason(temp); }
                case Constants.PUBLIC_EVENT: { return CheckPublicEvent(temp); }
                case Constants.TRIGGER_EVENT: { return CheckTriggerEvent(temp); }
                default: { return true; }//if it made it here then there is a typo
            }
        }
        //no rules made yet
        protected bool CheckTriggerEvent(string[] temp) {
            return true;
        }

        protected bool CheckPublicEvent(string[] temp) {
            if (temp[0] == null || temp == null)
                return true;
            if (temp[0].Equals(Constants.NEXT_EVENT)) {
                bool isNear = false;
                //return false if it needs to be saved
                foreach (KeyValuePair<string, int> item in Ctrl.Game.EventCalendar[Ctrl.Game.Season]) {
                    isNear = isNear || Ctrl.Game.EventIsNear(item.Value - 6, Ctrl.Game.Season, item.Key);
                }
                return !isNear;
            }
            return !Ctrl.Game.EventIsNear().Equals(temp[0]);
        }

        protected bool CheckSeason(string[] temp) {
            if (temp[0] == null)
                return true;
            return !temp[0].Equals(Ctrl.Game.Season);
        }

        protected bool CheckDayOfWeek(string[] temp) {
            if (temp[0] == null)
                return true;
            int day = 0;
            switch (temp[0]) {
                case Constants.MON: { day = 1; } break;
                case Constants.TUE: { day = 2; } break;
                case Constants.WED: { day = 3; } break;
                case Constants.THUR: { day = 4; } break;
                case Constants.FRI: { day = 5; } break;
                case Constants.SAT: { day = 6; } break;
                case Constants.SUN: { day = 7; } break;
            }
            return !(day == Ctrl.Game.DayOfWeek);
        }

        protected bool CheckTimeOfDay(string[] temp) {
            if (temp[0] == null)
                return true;
            return !temp[0].Equals(Ctrl.Game.TimeOfDay);
        }

        protected bool CheckSector(string[] temp) {
            if (temp[0] == null)
                return true;
            return !temp[0].Equals(Ctrl.Game.Sector.ToString());
        }

        protected bool CheckWeather(string[] temp) {
            //if(should I check to see if it exists in rule set)
            if (temp[0] == null)
                return true;
            return !temp[0].Equals(Ctrl.Game.Weather);
        }


        /******************Weigh Game Rule for Global dialogue Probability**********************/

        //will most definately be in it's own class
        public Dictionary<string, double> AssignRuleWeight(Dictionary<string, double> data) {
            return data;
        }
    }
}
