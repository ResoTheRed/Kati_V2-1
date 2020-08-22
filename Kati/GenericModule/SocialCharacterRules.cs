using Kati.Module_Hub;
using System;
using System.Collections.Generic;
using Kati.SourceFiles;

namespace Kati.GenericModule {

    /// <summary>
    /// Handles requirements concerning social rules
    /// 
    /// </summary>
    public class SocialCharacterRules {
        
        private Controller ctrl;
        private CharacterData npc;
        private string targetsName;

        public SocialCharacterRules(Controller ctrl) {
            Ctrl = ctrl;
            npc = Ctrl.Npc;
        }

        public Controller Ctrl { get => ctrl; set => ctrl = value; }
        public CharacterData Npc { get => npc; set => npc = value; }
        public string TargetsName { get => targetsName; set => targetsName = value; }

        public Dictionary<string, Dictionary<string, List<string>>> ParseSocialRequirments
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
            if (TargetsName == null)//default targets name is player's name
                targetsName = Npc.RespondersName;
            return data;
        }
        //social.<branch>.<npc/player>.<optional not>.<stat>.<scalar value if applicable>
        public bool RemoveElement(string req) {
            if (req == null)
                return true;
            string[] arr = req.Split(".");
            if (arr.Length < 4 && arr[0].Equals(Constants.SOCIAL)) {
                return true;
            } else if (arr.Length == 0 || !arr[0].Equals(Constants.SOCIAL)) {
                return false;
            } else {
                string[] temp = new string[arr.Length - 1];
                for (int i = 1; i <= arr.Length - 1; i++) {//remove social Keyword
                    temp[i - 1] = arr[i];
                }
                return RuleDirectory(temp);
            }
        }

        //<branch>.<npc/player>.<optional not>.<stat>.<scalar value if applicable>
        public bool RuleDirectory(string[] temp) {
            bool remove;
            string key;
            (key, temp) = Dequeue(temp);
            switch (key) {
                case Constants.ATTRIBUTE: { remove = CheckAttribute(temp); } break;
                case Constants.RELATIONSHIP: { remove = CheckStaticStat(temp, Constants.RELATIONSHIP); } break;
                case Constants.DIRECTED_STATUS: { remove = CheckStaticStat(temp, Constants.DIRECTED_STATUS); } break;
                default: { return true; }
            }
            return remove;
        }

        //pop first element and return the element and the shortend array
        private (string, string[]) Dequeue(string[] arr) {
            string key = "";
            string[] temp = { };
            if (arr.Length > 0) {
                key = arr[0];
                temp = new string[arr.Length - 1];
                for (int i = 1; i <= arr.Length - 1; i++) {
                    temp[i - 1] = arr[i];
                }
            }
            return (key, temp);
        }

        //<npc/player>.<optional not>.<stat>.<scalar value if applicable>
        protected bool CheckAttribute(string[] temp) {
            string key;
            (key, temp) = Dequeue(temp);
            if (temp.Length < 1)
                return true;
            switch (key) {
                case Constants.NPC: { return GetAnyNpcAttributes(temp); }
                case Constants.PLAYER: { return GetScalarStat(temp, Npc.RespondersName); }
                default: { TargetsName = key; return GetScalarStat(temp, TargetsName); }
            }
        }
        //<optional not>.<stat>.<scalar value if applicable>
        protected bool GetAnyNpcAttributes(string[] temp) {
            (string key, bool inverse) = HandleNot(ref temp);
            return RandomizeCharacterAttributes(key, ref temp, inverse);
        }
        //run through each npc and see if the stat beats the threshold
        //select a random character from all applicants
        protected bool RandomizeCharacterAttributes(string key, ref string[] origin, bool inverse) {
            List<string> temp = new List<string>();
            foreach (KeyValuePair<string, Dictionary<string, string>> item1 in Npc.InitiatorSocialList) {
                foreach (KeyValuePair<string, string> item2 in Npc.InitiatorSocialList[item1.Key]) {
                    if (!item1.Key.Equals(Npc.RespondersName)) {
                        try {
                            if (item2.Key.Equals(key)) {
                                if (!inverse && int.Parse(item2.Value) >= int.Parse(origin[0])) {
                                    temp.Add(item1.Key);
                                } else if (inverse && int.Parse(item2.Value) < int.Parse(origin[0])) {
                                    temp.Add(item1.Key);
                                }
                            }
                        } catch (Exception) { }
                    }
                }
            }
            if (temp.Count < 1)
                return true;
            TargetsName = temp[Controller.dice.Next(temp.Count)];
            return false;
        }

        protected bool GetScalarStat(string[] temp, string name) {
            (string key, bool inverse) = HandleNot(ref temp);
            bool removed = true;
            try {
                removed = int.Parse(Npc.InitiatorSocialList[name][key]) < int.Parse(temp[0]);
                if (inverse)
                    removed = !removed;
                if (!removed)
                    TargetsName = name;
            } catch (Exception) { }
            return removed;
        }

        protected bool CheckStaticStat(string[] temp, string type) {
            string key;
            (key, temp) = Dequeue(temp);
            if (temp.Length < 1)
                return true;
            switch (key) {
                case Constants.NPC: { return GetAnyNpcStaticStat(ref temp); }
                case Constants.PLAYER: {
                        TargetsName = Npc.RespondersName;
                        return GetStaticStat(ref temp, TargetsName, type);
                    }
                default: {
                        TargetsName = key;
                        return GetStaticStat(ref temp, TargetsName, type);
                    }
            }
        }

        protected bool GetAnyNpcStaticStat(ref string[] temp) {
            (string key, bool inverse) = HandleNot(ref temp);
            bool removed = RandomizeCharacterBoolStat(ref temp, key, inverse);
            return removed;
        }

        //run through each npc and see if the stat beats the threshold
        //select a random character from all applicants
        protected bool RandomizeCharacterBoolStat(ref string[] origin, string key, bool inverse) {
            List<string> temp = new List<string>();
            foreach (KeyValuePair<string, Dictionary<string, string>> item1 in Npc.InitiatorSocialList) {
                foreach (KeyValuePair<string, string> item2 in Npc.InitiatorSocialList[item1.Key]) {
                    if (!item1.Key.Equals(Npc.RespondersName)) {
                        try {
                            if (!inverse && item2.Key.Equals(key)) {
                                temp.Add(item1.Key);
                            } else if (inverse && !Npc.InitiatorSocialList[item1.Key].ContainsKey(key)) {
                                temp.Add(item1.Key);
                                //Console.WriteLine(item1.Key+" "+key+" "+ !Npc.InitiatorSocialList.ContainsKey(key));
                            }
                        } catch (Exception) { }
                    }
                }
            }
            if (temp.Count < 1)
                return true;
            TargetsName = temp[Controller.dice.Next(temp.Count)];
            return false;
        }

        protected bool GetStaticStat(ref string[] temp, string name, string type) {
            (string key, bool inverse) = HandleNot(ref temp);
            bool removed = true;
            try {
                removed = !(Npc.InitiatorSocialList[name].ContainsKey(key) &&
                    Npc.InitiatorSocialList[name][key].Equals(type));
                if (inverse)
                    removed = !removed;
            } catch (Exception) { }
            return removed;
        }

        //raises flag if array contains "not"
        protected (string, bool) HandleNot(ref string[] temp) {
            string key;
            bool inverse = false;
            (key, temp) = Dequeue(temp);
            if (key.Equals(Constants.NOT)) {
                inverse = true;
                (key, temp) = Dequeue(temp);
            }
            return (key, inverse);
        }

    }
}
