using Kati.Data_Modules;
using System.Collections.Generic;


namespace Kati.GenericModule {
    /// <summary>
    /// Generic Module class that each module can use as it's raw data library
    /// </summary>


    public class ModuleLib : I_Module {
        //holds all data formated straight from the JSON
        private Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>>> data;
        //hold all of the dialogue topics. Format: {"dream" : ["dream_statement","dream_question", "dream_reponse"] }
        private Dictionary<string, List<string>> keys;
        private string pathToJson;
        public readonly int STATEMENT = 0;
        public readonly int QUESTION = 1;
        public readonly int RESPONSE = 2;

        public Dictionary<string, List<string>> Keys { get => keys; set => keys = value; }
        public Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>>> Data { get => data; set => data = value; }

        public ModuleLib(string path) {
            pathToJson = path;
            //fully loads data and keys
            JsonReader.LoadFromFile(this);
        }

        public string getPathToJson() { return pathToJson; }

        public void SetLibrary(Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>>> lib) {
            Data = lib;
        }

        //@param<string keys>: each phrase of the json branches
        public void SetConversationTypeKeys(string[] keys) {
            foreach (string key in keys) {
                Keys[key] = new List<string>();
                Keys[key].Add(key + "_statement");
                Keys[key].Add(key + "_question");
                Keys[key].Add(key + "_response");
            }
        }

        //Return topic titles from each sub topic in the module
        public List<string> GetTopicKeys() {
            List<string> temp = new List<string>();
            foreach (KeyValuePair <string, Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>>> item in Data) {
                string[] t = item.Key.Split("_");
                if (!temp.Contains(t[0])) {
                    temp.Add(t[0]);
                }
            }
            return temp;
        }

        public void SetConversationTypeKeys(Dictionary<string, List<string>> keys) {
            Keys = keys;
        }

        public int GetType(string type) {
            if (type.Equals(Constants.QUESTION)) {
                return QUESTION;
            }else if (type.Equals(Constants.RESPONSE)) {
                return RESPONSE;
            }
            return STATEMENT;
        }

        //pull individual topics from data list shallow copy
        public Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>> ShallowCopyDictionaryByTopic(string topic, int type) {
            if (Keys.ContainsKey(topic)) {
                return Data[Keys[topic][type]];
            }
            return null;
        }
        //topic: dream  type: _statement
        public Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>> DeepCopyDictionaryByTopic
            (string topic, int type) {
            string key = Keys[topic][type];
            Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>> dict =
                new Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>>();
            foreach (KeyValuePair<string, Dictionary<string, Dictionary<string, List<string>>>> s1 in Data[key]) {
                dict[s1.Key] = new Dictionary<string, Dictionary<string, List<string>>>();
                foreach (KeyValuePair<string, Dictionary<string, List<string>>> s2 in Data[key][s1.Key]) {
                    dict[s1.Key][s2.Key] = new Dictionary<string, List<string>>();
                    foreach (KeyValuePair<string, List<string>> s3 in Data[key][s1.Key][s2.Key]) {
                        dict[s1.Key][s2.Key][s3.Key] = new List<string>();
                        foreach (string element in Data[key][s1.Key][s2.Key][s3.Key]) {
                            dict[s1.Key][s2.Key][s3.Key].Add(element);
                        }
                    }
                }
            }
            return dict;
        }

        public Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>> DeepCopyDictionaryByTopic
           (string topic, string type) {
            string key = topic + "_" + type;
            Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>> dict =
                new Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>>();
            foreach (KeyValuePair<string, Dictionary<string, Dictionary<string, List<string>>>> s1 in Data[key]) {
                dict[s1.Key] = new Dictionary<string, Dictionary<string, List<string>>>();
                foreach (KeyValuePair<string, Dictionary<string, List<string>>> s2 in Data[key][s1.Key]) {
                    dict[s1.Key][s2.Key] = new Dictionary<string, List<string>>();
                    foreach (KeyValuePair<string, List<string>> s3 in Data[key][s1.Key][s2.Key]) {
                        dict[s1.Key][s2.Key][s3.Key] = new List<string>();
                        foreach (string element in Data[key][s1.Key][s2.Key][s3.Key]) {
                            dict[s1.Key][s2.Key][s3.Key].Add(element);
                        }
                    }
                }
            }
            return dict;
        }
    }
}
