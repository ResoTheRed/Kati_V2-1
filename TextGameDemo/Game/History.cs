using System;
using System.Collections.Generic;
using System.IO;
using TextGameDemo.JSON_Files;

namespace TextGameDemo.Game {
    public class History {

        const int MODULE = 0;
        const int ROMANCE = 1, FRIEND = 2, PROFESSIONAL = 3;
        const int AFFINITY = 4, RESPECT = 5;
        const int DISGUST = 6, HATE = 7, RIVAL = 8;

        const int AROUND_TOWN = 0, FIGHTING_WORDS = 1, YOUNG_LOVE = 2;
        const int QUESTING = 3, LERIN = 4, ERROR = 5;

        //{character_name :{number_module : { module_name : {dialogue : [stats] } } } }
        private Dictionary<string, Dictionary<string, Dictionary<string,List<int>>>> entry;
        //dialogue : name
        private Dictionary<string, List<string>> shortTerm;
        Dictionary<string,int> counters;
        
        public Dictionary<string, Dictionary<string, Dictionary<string, List<int>>>> Entry { get => entry; set => entry = value; }
        public Dictionary<string, List<string>> ShortTerm { get => shortTerm; set => shortTerm = value; }



        public History() {
            Entry = new Dictionary<string, Dictionary<string, Dictionary<string, List<int>>>>();
            ShortTerm = new Dictionary<string, List<string>>();
            counters = new Dictionary<string, int>();
            Entry["Collin"] = new Dictionary<string, Dictionary<string, List<int>>>();
            Entry["Benjamin"] = new Dictionary<string, Dictionary<string, List<int>>>();
            Entry["Dan"] = new Dictionary<string, Dictionary<string, List<int>>>();
            Entry["Albrecht"] = new Dictionary<string, Dictionary<string, List<int>>>();
            Entry["Romero"] = new Dictionary<string, Dictionary<string, List<int>>>();
            Entry["Miciah"] = new Dictionary<string, Dictionary<string, List<int>>>();
            Entry["Sivian"] = new Dictionary<string, Dictionary<string, List<int>>>();
            Entry["Lerin"] = new Dictionary<string, Dictionary<string, List<int>>>();
            Entry["Lafitte"] = new Dictionary<string, Dictionary<string, List<int>>>();
            Entry["Teta"] = new Dictionary<string, Dictionary<string, List<int>>>();
            Entry["Quinn"] = new Dictionary<string, Dictionary<string, List<int>>>();
            Entry["Helena"] = new Dictionary<string, Dictionary<string, List<int>>>();
            Entry["Christina"] = new Dictionary<string, Dictionary<string, List<int>>>();
            Entry["Crumples"] = new Dictionary<string, Dictionary<string, List<int>>>();
        }

        public void AddEntry(string character_name, string module_name, string dialogue, List<int> relationship, string key) {
            if (!counters.ContainsKey(character_name)) {
                counters[character_name] = 0;
            }
            if (Entry.ContainsKey(character_name)) {
                var list = new List<int>();
                counters[character_name]++;
                key = counters[character_name].ToString("0000") + "_" + key;
                //Entry[character_name] = new Dictionary<string, Dictionary<string, List<int>>>();
                Entry[character_name][key] = new Dictionary<string, List<int>>();
                list.Add(GetModuleID(module_name));
                foreach (int tone in relationship) {
                    list.Add(tone);
                }
                Entry[character_name][key][dialogue] = list;
            }
            RecordShortTerm(character_name, dialogue);
        }

        public void RecordShortTerm(string name, string dialogue) {
            if (!shortTerm.ContainsKey(name)) {
                shortTerm[name] = new List<string>(); 
            }
            shortTerm[name].Add(dialogue);
        }

        public void ResetShortTerm() {
            var temp = new List<string>();

            foreach (KeyValuePair<string, List<string>> item in shortTerm) {
                temp.Add(item.Key);
            }
            foreach (string item in temp) {
                ShortTerm[item] = new List<string>();
            }
        }

        public void WriteToFile() {
            DateTime time = DateTime.Now;
            string file_name = "temp.txt";//"History_" + time.ToString().Replace(" ", "_");
            //file_name = file_name.Replace("/","-");
            //file_name = file_name.Replace(":","-");
            string path = "\\JSON_Files\\Rule_lists\\"+file_name;
            path = JsonToolkit.GetPath(path);
            if (File.Exists(path)) {
                File.Delete(path);
            }
            using (StreamWriter sw = File.CreateText(path)) {
                sw.WriteLine("New file created: {0}", DateTime.Now.ToString());
            }
            string line;
            using (StreamWriter outputFile = new StreamWriter(path,true)) {
                foreach (KeyValuePair<string, Dictionary<string,Dictionary<string, List<int>>>> item in Entry) {
                    line = item.Key+"\n";
                    foreach (KeyValuePair<string, Dictionary<string, List<int>>> item2 in Entry[item.Key]) {
                        line += item2.Key + ",";
                        foreach (KeyValuePair<string, List<int>> item3 in Entry[item.Key][item2.Key]) {
                            line += item3.Key + ",";
                            foreach (int i in item3.Value) {
                                line += i + ",";
                            }
                        }
                        line += "END\n";
                    }
                    outputFile.WriteLine(line);
                }
                outputFile.Close();
            }
        }

        private int GetModuleID(string module_name) {
            int id = ERROR;
            switch(module_name){
                case JsonToolkit.AROUND_TOWN: { id = AROUND_TOWN; }break;
                case JsonToolkit.FIGHTING_WORDS: { id = FIGHTING_WORDS; }break;
                case JsonToolkit.YOUNG_LOVE: { id = YOUNG_LOVE; }break;
                case JsonToolkit.QUESTING: { id = QUESTING; }break;
                case JsonToolkit.LERIN: { id = LERIN; }break;
            }
            return id;
        }


    }
}
