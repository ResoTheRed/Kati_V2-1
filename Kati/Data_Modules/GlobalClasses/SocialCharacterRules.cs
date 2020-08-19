using Kati.Module_Hub;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kati.Data_Modules.GlobalClasses {
    
    /// <summary>
    /// Handles requirements concerning social rules
    /// 
    /// </summary>
    public class SocialCharacterRules {

        public const string SOCIAL = "social";
        public const string NPC = "npc";
        public const string PLAYER = "player";
        public const string ATTRIBUTE = "attribute";
        public const string RELATIONSHIP = "relationship";
        public const string DIRECTED_STATUS = "directed";
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
                foreach (string req in data[item.Key]["req"]) {
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
        //social.<branch>.<npc/player>.<optional not>.<stat>.<scalar value if applicable>
        public bool RemoveElement(string req) {
            if (req == null)
                return true;
            string[] arr = req.Split(".");
            if (arr.Length < 4 && arr[0].Equals(SOCIAL)) {
                return true;
            } else if (arr.Length == 0 || !arr[0].Equals(SOCIAL)) {
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
                case ATTRIBUTE: { remove = CheckAttribute(temp); } break;
                case RELATIONSHIP: { remove = CheckStaticStat(temp,RELATIONSHIP); } break;
                case DIRECTED_STATUS: { remove = CheckStaticStat(temp,DIRECTED_STATUS); } break;
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
                case NPC: { return GetAnyNpcAttributes(temp); }
                case PLAYER: { return GetScalarStat(temp,Npc.RespondersName); }
                default: { TargetsName = temp[0]; return GetScalarStat(temp,TargetsName); }
            }
        }
        //<optional not>.<stat>.<scalar value if applicable>
        protected bool GetAnyNpcAttributes(string[] temp) {
            (string key, bool inverse) = HandleNot(ref temp);
            return RandomizeCharacterAttributes(key, ref temp,inverse);
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
                                Console.WriteLine(inverse + " " + item2.Value + " " + origin[0]);
                                if (!inverse && Int32.Parse(item2.Value) >= Int32.Parse(origin[0])) {
                                    temp.Add(item1.Key);
                                } else if (Int32.Parse(item2.Value) < Int32.Parse(origin[0])) {
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

        protected bool GetScalarStat(string[] temp,string name) {
            (string key, bool inverse) = HandleNot(ref temp);
            bool removed = true;
            try {
                removed = Int32.Parse(Npc.InitiatorSocialList[name][ATTRIBUTE]) < Int32.Parse(temp[1]);
                if (inverse)
                    removed = !removed;
            } catch (Exception) { }
            return removed;
        }

        protected bool CheckStaticStat(string[] temp,string type) {
            string key;
            (key, temp) = Dequeue(temp);
            if (temp.Length < 1)
                return true;
            switch (key) {
                case NPC: { return GetAnyNpcStaticStat(ref temp); }
                case PLAYER: { return GetStaticStat(ref temp,Npc.RespondersName,type); }
                default: { TargetsName = temp[0]; return GetStaticStat(ref temp,TargetsName,type); }
            }
        }

        protected bool GetAnyNpcStaticStat(ref string[] temp) {
            (string key, bool inverse) = HandleNot(ref temp);
            bool removed = RandomizeCharacterBoolStat(ref temp);
            if (inverse)
                removed = !removed;
            return removed;
        }

        //run through each npc and see if the stat beats the threshold
        //select a random character from all applicants
        protected bool RandomizeCharacterBoolStat(ref string[] origin) {
            List<string> temp = new List<string>();
            foreach (KeyValuePair<string, Dictionary<string, string>> item1 in Npc.InitiatorSocialList) {
                foreach (KeyValuePair<string, string> item2 in Npc.InitiatorSocialList[item1.Key]) {
                    if (!item1.Key.Equals(Npc.RespondersName)) {
                        try {
                            if (item2.Key.Equals(origin[0])) {
                                temp.Add(item1.Key);
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

        protected bool GetStaticStat(ref string[] temp, string name,string type) {
            (string key, bool inverse) = HandleNot(ref temp);
            bool removed = true;
            try {
                removed = Npc.InitiatorSocialList[name].ContainsKey(temp[1]) &&
                    Npc.InitiatorSocialList[name][temp[1]].Equals(type);
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
            if (key.Equals("not")) {
                inverse = true;
                (key, temp) = Dequeue(temp);
            }
            return (key, inverse);
        }

    }
}
