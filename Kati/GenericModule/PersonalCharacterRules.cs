using Kati.Module_Hub;
using System;
using System.Collections.Generic;
using Kati.SourceFiles;

namespace Kati.GenericModule {
    public class PersonalCharacterRules {

        private Controller ctrl;
        private CharacterData npc;

        public PersonalCharacterRules(Controller ctrl) {
            Ctrl = ctrl;
            npc = Ctrl.Npc;
        }

        public Controller Ctrl { get => ctrl; set => ctrl = value; }
        public CharacterData Npc { get => npc; set => npc = value; }

        public Dictionary<string, Dictionary<string, List<string>>> ParsePersonalRequirments
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

        public bool RemoveElement(string req) {
            if (req == null)
                return true;
            string[] arr = req.Split(".");
            if (arr.Length < 3 && arr[0].Equals(Constants.PERSONAL)) {
                return true;
            } else if (arr.Length == 0 || !arr[0].Equals(Constants.PERSONAL)) {
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
            foreach (string s in arr) {
            }
            bool remove;
            (string key, string[] temp) = dequeue(arr);
            if (temp.Length < 1)
                return true;
            bool inverse = temp[0].Equals(Constants.NOT);
            if (inverse) {
                var t = dequeue(temp);
                temp = t.Item2;
            }
            remove = RulesDirectory(key, temp);
            if (inverse)
                return !remove;
            return remove;
        }

        private bool RulesDirectory(string key, string[] temp) {
            bool remove;
            switch (key) {
                case Constants.TRAIT: { remove = CheckTrait(temp); } break;
                case Constants.STATUS: { remove = CheckStatus(temp); } break;
                case Constants.INTEREST: { remove = CheckInterest(temp); } break;
                case Constants.PHYSICAL_FEATURES: { remove = CheckPhysicalFeatures(temp); } break;
                case Constants.SCALAR_TRAIT: { remove = CheckScalarTrait(temp); } break;
                default: { return true; }
            }
            return remove;
        }
        //pop first element and return the element and the shortend array
        public (string, string[]) dequeue(string[] arr) {
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

        protected bool CheckScalarTrait(string[] temp) {
            int npcHas;
            int threshold;
            if (temp.Length < 2 || !Ctrl.Npc.InitiatorPersonalList.ContainsKey(temp[0]))
                return true;
            try {
                npcHas = int.Parse(Ctrl.Npc.InitiatorPersonalList[temp[0]]);
                threshold = int.Parse(temp[1]);
            } catch (FormatException) {
                return true;
            }
            //return true to delete dialogue
            return threshold > npcHas;
        }

        protected bool CheckPhysicalFeatures(string[] temp) {
            if (temp.Length < 1)
                return true;
            if (Ctrl.Npc.InitiatorPersonalList.ContainsKey(temp[0])) {
                if (Ctrl.Npc.InitiatorPersonalList[temp[0]].Equals(Constants.PHYSICAL_FEATURES)) {
                    return false;
                }
            }
            return true;
        }

        protected bool CheckInterest(string[] temp) {
            if (temp.Length < 1)
                return true;
            if (Ctrl.Npc.InitiatorPersonalList.ContainsKey(temp[0])) {
                if (Ctrl.Npc.InitiatorPersonalList[temp[0]].Equals(Constants.INTEREST)) {
                    return false;
                }
            }
            return true;
        }

        protected bool CheckStatus(string[] temp) {
            if (temp.Length < 1)
                return true;
            if (Ctrl.Npc.InitiatorPersonalList.ContainsKey(temp[0])) {
                if (Ctrl.Npc.InitiatorPersonalList[temp[0]].Equals(Constants.STATUS)) {
                    return false;
                }
            }
            return true;
        }

        protected bool CheckTrait(string[] temp) {
            if (temp.Length < 1)
                return true;
            if (Ctrl.Npc.InitiatorPersonalList.ContainsKey(temp[0])) {
                if (Ctrl.Npc.InitiatorPersonalList[temp[0]].Equals(Constants.TRAIT)) {
                    return false;
                }
            }
            return true;
        }
    }
}
