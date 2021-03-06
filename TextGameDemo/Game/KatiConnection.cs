﻿using System;
using System.Collections.Generic;
using Kati.Module_Hub;
using TextGameDemo.JSON_Files;
using TextGameDemo.Modules;
using TextGameDemo.Game.Characters;

namespace TextGameDemo.Game {
    /// <summary>
    /// Work as the Module Hub maybe
    /// </summary>
    public class KatiConnection {

        public const string STORY_SEG_1 = "story part 1";
        private ModuleHub hub;
        private GameModel model;
        private GameTools tools;
        private DialoguePackage package;
        private History gameHistory;
        private string previousModuleName;

        public ModuleHub Hub { get => hub; set => hub = value; }
        public GameModel Model { get => model; set => model = value; }
        public GameTools Tools { get => tools; set => tools = value; }
        public DialoguePackage Package { get => package; set => package = value; }
        public History GameHistory { get => gameHistory; set => gameHistory = value; }

        public KatiConnection(GameModel model) {
            Hub = new ModuleHub(JsonToolkit.GetPath("JSON_Files\\_startup.json"));
            Model = model;
            GameHistory = new History();
            LoadModules();
            Tools = GameTools.Tools();
        }

        //load all modules
        private void LoadModules() {
            string path = JsonToolkit.Get(JsonToolkit.AROUND_TOWN);
            Hub.Modules[JsonToolkit.AROUND_TOWN] = new AroundTown(path, Model);
            //path = JsonToolkit.Get(JsonToolkit.BENJAMIN);
            //Hub.Modules[JsonToolkit.BENJAMIN] = new Benjamin(path);
            path = JsonToolkit.Get(JsonToolkit.FIGHTING_WORDS);
            Hub.Modules[JsonToolkit.FIGHTING_WORDS] = new FightingWords(path, Model);
            path = JsonToolkit.Get(JsonToolkit.FOREST_TALK);
            Hub.Modules[JsonToolkit.FOREST_TALK] = new ForestTalk(path,Model);
            //path = JsonToolkit.Get(JsonToolkit.LAFFITE);
            //Hub.Modules[JsonToolkit.LAFFITE] = new Lafitte(path);
            path = JsonToolkit.Get(JsonToolkit.LERIN);
            Hub.Modules[JsonToolkit.LERIN] = new Lerin(path,Model);
            path = JsonToolkit.Get(JsonToolkit.QUESTING);
            Hub.Modules[JsonToolkit.QUESTING] = new Questing(path);
            //path = JsonToolkit.Get(JsonToolkit.SHOPPING);
            //Hub.Modules[JsonToolkit.SHOPPING] = new Shopping(path);
            path = JsonToolkit.Get(JsonToolkit.YOUNG_LOVE);
            Hub.Modules[JsonToolkit.YOUNG_LOVE] = new YoungLove(path,Model);
        }


        public (DialoguePackage,bool) RunSystem(string area, string room, string character) {
            UpdateCharacterData(Model.Lib.Lib[character]);
            string moduleName = PickModule(area,room, character); //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ temp.  Undo comment.
            Kati.GenericModule.Module module = Hub.GetModule(moduleName);
            module.SetCurrentCharacter(character);
            module.Run();
            string key = moduleName + "_" + module.Ctrl.Topic.Topic + "_" + module.Ctrl.Type.Type;
            //if(!module.Ctrl.Package.IsResponse)
            RecordHistory(character,moduleName,module.Ctrl.Package.Dialogue,key);
            bool isChain = module.Ctrl.Package.IsChain;
            package = module.Ctrl.Package;
            return (package,isChain);
        }


        //Test method: ~~~~ delete ~~~~ 
        public string GetModuleInfo(string location, string character) {
            var temp = Hub.StoryLine[STORY_SEG_1][location][character];
            string print = location + ", " + character;
            foreach (string s in temp) {
                print += ", " + s; 
            }
            return print;
        }

        /////////////////////////////////// Update Module System ////////////////////////////////////////

        public void UpdateCharacterData(Character c) {
            string gender = c.IsMale ? "Male" : "Female";
            Dictionary<string, string> personal = new Dictionary<string, string>();
            Dictionary<string, Dictionary<string, string>> social = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, double> tones = new Dictionary<string, double>();
            social[Cast.PLAYER] = new Dictionary<string, string>();
            //Console.Write("Update Character Tones");
            foreach (KeyValuePair<string, int> item in Model.Lib.Lib[Cast.PLAYER].BranchAttributes[c.Name]) {
                //Console.Write(item.Value);
                tones[item.Key] = (double)item.Value;
            }
            foreach (string item in c.PersonalAttributes) {
                personal[item] = "characterTrait";
            }
            foreach (KeyValuePair<string, List<string>> item in c.SocialAttributes) {
                social[item.Key] = new Dictionary<string, string>();
                foreach (string item2 in c.SocialAttributes[Cast.PLAYER]) {
                    social[item.Key][item2] = "boolean";
                }
            }
            CharacterData.SetInitiatorCharacterData(c.Name,gender,tones,personal,social);
        }


        /////////////////////////////////// Module picking Rules ////////////////////////////////////////

        public void RecordHistory(string name, string module_name,string dialogue,string key) {
            var c = Model.Lib.Lib[Cast.PLAYER];
            
            List<int> list = new List<int>();
            list.Add(c.BranchAttributes[name][Kati.Constants.ROMANCE]);
            list.Add(c.BranchAttributes[name][Kati.Constants.FRIEND]);
            list.Add(c.BranchAttributes[name][Kati.Constants.PROFESSIONAL]);
            list.Add(c.BranchAttributes[name][Kati.Constants.AFFINITY]);
            list.Add(c.BranchAttributes[name][Kati.Constants.RESPECT]);
            list.Add(c.BranchAttributes[name][Kati.Constants.DISGUST]);
            list.Add(c.BranchAttributes[name][Kati.Constants.HATE]);
            list.Add(c.BranchAttributes[name][Kati.Constants.RIVALRY]);
            GameHistory.AddEntry(name,module_name, dialogue,list,key);
        }

        public void RecordPlayerResponse(string name, string dialogue) {
            List<int> list = new List<int> {0,0,0,0,0,0,0,0 };
            GameHistory.AddEntry(name,"-", dialogue, list, "-");
        }

        public void Exit() {
            GameHistory.WriteToFile();
        }

        /////////////////////////////////// Module picking Rules ////////////////////////////////////////

        /*
         ~~~ things to consider ~~~
            1. Narrow possiblities by location specific modules
            2. Does the character have more than one option ? continue : use single option
                a. character has no option then no dialogue
            3. Narrow possiblities by relationship status
                a. add in status values
                b. add in personal and social values
                c. select based on probability using Branch Decision stats
            4. quest branches or empty branches
         */

        //if returned value is empty, cancel dialogue 
        public string PickModule(string area,string room, string character) {
            string mod = "";
            if (Package != null && (package.IsChain || package.IsResponse)) {
                return previousModuleName; 
            }
            if (character.Equals("Lerin"))
                return character;
            var possibleMods = Hub.StoryLine[STORY_SEG_1][room][character];
            possibleMods = GetLocationBasedModules(possibleMods,area);
            if (possibleMods.Count == 1) {
                return possibleMods[0];
            } else if (possibleMods.Count == 0) {
                return mod;
            }
            possibleMods = GetBranchToneBasedModules(possibleMods, character);
            mod = PickModule(possibleMods, character);
            previousModuleName = mod;
            return mod;
        }

        public string PickModule(List<string> usable,string character) {
            string mod = "";
            int percent = tools.Next(1, 100);
            if (usable.Contains(JsonToolkit.FIGHTING_WORDS)) {
                if (Model.Lib.Lib[character].Statuses[Status.ANGRY] && percent <= 60) {
                    return JsonToolkit.FIGHTING_WORDS;
                } else if (Model.Lib.Lib[character].Statuses[Status.ANNOYED] && percent <= 35) {
                    return JsonToolkit.FIGHTING_WORDS;
                } else if (percent <= 5) {
                    return JsonToolkit.FIGHTING_WORDS;
                } else {
                    usable.Remove(JsonToolkit.FIGHTING_WORDS);
                }
            } else if (usable.Contains(JsonToolkit.YOUNG_LOVE)) {
                if ((Model.Lib.Lib[character].Statuses[Status.FLIRTY] || 
                    Model.Lib.Lib[Cast.PLAYER].BranchAttributes[character][Kati.Constants.ROMANCE]>400) && percent <= 75) {
                    return JsonToolkit.YOUNG_LOVE;
                } else if (percent <= 50) {
                    return JsonToolkit.YOUNG_LOVE;
                } else if (Model.Lib.Lib[character].Statuses[Status.ANNOYED] && percent <=5) {
                    return JsonToolkit.YOUNG_LOVE;
                } else {
                    usable.Remove(JsonToolkit.YOUNG_LOVE);
                }
            }
            if (usable.Contains(JsonToolkit.QUESTING))
                usable.Remove(JsonToolkit.QUESTING);
            if (usable.Count == 1)
                return usable[0];
            if (usable.Count > 1)
                return usable[Tools.Next(usable.Count)];
            return mod;
        } 

        public List<string> GetLocationBasedModules(List<string> mods, string location) {
            List<string> usable = new List<string>();
            if (location.Equals(Areas.TOWN)) {
                foreach (string mod in mods) {
                    if (!mod.Equals(JsonToolkit.FOREST_TALK))
                        usable.Add(mod);
                }
            } else if (location.Equals(Areas.FOREST)) {
                foreach (string mod in mods) {
                    if (!mod.Equals(JsonToolkit.AROUND_TOWN))
                        usable.Add(mod);
                }
            } else if (location.Equals(Areas.CAVE)) {
                foreach (string mod in mods) {
                    if (!mod.Equals(JsonToolkit.AROUND_TOWN) || !mod.Equals(JsonToolkit.FOREST_TALK))
                        usable.Add(mod);
                }
            }
            return usable;
        }

        public List<string> GetBranchToneBasedModules(List<string> mods, string character) {
            List<string> usable = new List<string>();
            Dictionary<string, int> tones = ApplyStatusEffects(character);
            usable = CheckForYoungLove(mods, character);
            usable = CheckForFightingWords(usable, character);
            return usable;
        }

        private List<string> CheckForFightingWords(List<string> usable, string character) {
            int hate = Model.Lib.Lib[Cast.PLAYER].BranchAttributes[character][Kati.Constants.HATE];
            int dis = Model.Lib.Lib[Cast.PLAYER].BranchAttributes[character][Kati.Constants.DISGUST];
            int fri = Model.Lib.Lib[Cast.PLAYER].BranchAttributes[character][Kati.Constants.FRIEND];
            int rom = Model.Lib.Lib[Cast.PLAYER].BranchAttributes[character][Kati.Constants.ROMANCE];
            if (usable.Contains(JsonToolkit.FIGHTING_WORDS)) {
                if ((hate + dis > fri + rom + 75) || Model.Lib.Lib[character].Statuses[Status.ANGRY]) {
                    return usable;
                }
                usable.Remove(JsonToolkit.FIGHTING_WORDS);
            }
            return usable;
        }

        private List<string> CheckForYoungLove(List<string> usable, string character) {
            int rom = Model.Lib.Lib[Cast.PLAYER].BranchAttributes[character][Kati.Constants.ROMANCE];
            int hate = Model.Lib.Lib[Cast.PLAYER].BranchAttributes[character][Kati.Constants.HATE];
            int dis = Model.Lib.Lib[Cast.PLAYER].BranchAttributes[character][Kati.Constants.DISGUST];
            if (usable.Contains(JsonToolkit.YOUNG_LOVE)) {
                if (rom >= 200) {
                    if (hate + dis + 150 < rom)
                        return usable;
                }
                usable.Remove(JsonToolkit.YOUNG_LOVE);
            }
            return usable;
        }

        public Dictionary<string, int> ApplyStatusEffects(string character) {
            //need to pass character data back and forth
            Dictionary<string, int> tones = new Dictionary<string, int>();
            foreach (KeyValuePair<string, int> item in Model.Lib.Lib[Cast.PLAYER].BranchAttributes[character]) {
                tones[item.Key] = item.Value;     
            }
            if (Model.Lib.Lib[character].Statuses[Status.ANGRY]) {
                tones[Kati.Constants.HATE] += 100;
                tones[Kati.Constants.DISGUST] += 50;
                tones[Kati.Constants.RIVALRY] += 25;
            }
            if (Model.Lib.Lib[character].Statuses[Status.ANNOYED]) {
                tones[Kati.Constants.HATE] += 25;
                tones[Kati.Constants.DISGUST] += 100;
                tones[Kati.Constants.RIVALRY] += 50;
            }
            if (Model.Lib.Lib[character].Statuses[Status.FLIRTY]) {
                tones[Kati.Constants.ROMANCE] += 100;
                tones[Kati.Constants.AFFINITY] += 25;
            }
            if (Model.Lib.Lib[character].Statuses[Status.HAPPY]) {
                tones[Kati.Constants.FRIEND] += 100;
                tones[Kati.Constants.PROFESSIONAL] += 50;
                tones[Kati.Constants.RESPECT] += 25;
            }
            if (Model.Lib.Lib[character].Statuses[Status.SAD]) {
                foreach (KeyValuePair<string, int> item in tones) {
                    tones[item.Key] -= 50;
                    if (tones[item.Key] < 0)
                        tones[item.Key] = 0;
                }
            }
            return tones;
        }



    }
}
