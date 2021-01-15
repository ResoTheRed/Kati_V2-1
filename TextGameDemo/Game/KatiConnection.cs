using System;
using System.Collections.Generic;
using Kati.Module_Hub;
using TextGameDemo.JSON_Files;
using TextGameDemo.Modules;

namespace TextGameDemo.Game {
    /// <summary>
    /// Work as the Module Hub maybe
    /// </summary>
    public class KatiConnection {

        public const string STORY_SEG_1 = "story part 1";
        private ModuleHub hub;


        public ModuleHub Hub { get => hub; set => hub = value; }

        public KatiConnection() {
            Hub = new ModuleHub(JsonToolkit.GetPath("JSON_Files\\_startup.json"));
            LoadModules();
        }

        //load all modules
        private void LoadModules() {
            string path = JsonToolkit.Get(JsonToolkit.AROUND_TOWN);
            Hub.Modules[JsonToolkit.AROUND_TOWN] = new AroundTown(path);
            path = JsonToolkit.Get(JsonToolkit.BENJAMIN);
            Hub.Modules[JsonToolkit.BENJAMIN] = new Benjamin(path);
            path = JsonToolkit.Get(JsonToolkit.FIGHTING_WORDS);
            Hub.Modules[JsonToolkit.FIGHTING_WORDS] = new FightingWords(path);
            path = JsonToolkit.Get(JsonToolkit.FOREST_TALK);
            Hub.Modules[JsonToolkit.FOREST_TALK] = new ForestTalk(path);
            path = JsonToolkit.Get(JsonToolkit.LAFFITE);
            Hub.Modules[JsonToolkit.LAFFITE] = new Lafitte(path);
            path = JsonToolkit.Get(JsonToolkit.LERIN);
            Hub.Modules[JsonToolkit.LERIN] = new Lerin(path);
            path = JsonToolkit.Get(JsonToolkit.QUESTING);
            Hub.Modules[JsonToolkit.QUESTING] = new Questing(path);
            path = JsonToolkit.Get(JsonToolkit.SHOPPING);
            Hub.Modules[JsonToolkit.SHOPPING] = new Shopping(path);
            path = JsonToolkit.Get(JsonToolkit.YOUNG_LOVE);
            Hub.Modules[JsonToolkit.YOUNG_LOVE] = new YoungLove(path);
        }

        //call specific module based on who you are talking to.
        //call module based on where the player is
        //call module based on history

        //Test method: ~~~~ delete ~~~~ 
        public string GetModuleInfo(string location, string character) {
            var temp = Hub.StoryLine[STORY_SEG_1][location][character];
            string print = location + ", " + character;
            foreach (string s in temp) {
                print += ", " + s; 
            }
            return print;
        }

    }
}
