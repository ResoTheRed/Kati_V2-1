using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Kati.Module_Hub {

    /// <summary>
    /// Responsible for loading in all module ponters, characters, 
    /// locations, and story node segments into a dict structure
    /// </summary>
    
    public class DataLoader{

        private string path;

        public DataLoader(string path) {
            Path = path;
        }

        public string Path { get => path; set => path = value; }

        public Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>> LoadJsonFile() {
            using StreamReader r = new StreamReader(Path);
            string json = r.ReadToEnd();
            var storyLine = JsonConvert.DeserializeObject
                <Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>>>(json);
            return storyLine;
        }

       
    }

    /*
     "story segment a":{
        "location 1": {
            "Character A" : ["Module 1", "Module 2"],
            "Character B" : ["Module 1", "Module 3"]
        },
        "location 2": {
        
        }
    }
     */
}
