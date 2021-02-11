using Kati.GenericModule;
using System;
using System.Collections.Generic;

namespace Kati.Module_Hub {
    public class ModuleHub {
        // { storyline : { location : { character : [modules_that_the_character_has] } } }
        private Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>> storyLine;
        private DataLoader masterData;
        private Dictionary<string, Kati.GenericModule.Module> modules;
        private DialoguePackage package;
        

        public Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>> StoryLine 
            { get => storyLine; set => storyLine = value; }
        public DataLoader MasterData { get => masterData; set => masterData = value; }
        public Dictionary<string, Module> Modules { get => modules; set => modules = value; }
        public DialoguePackage Package { get => package; set => package = value; }

        public ModuleHub(string startJsonPath) {
            masterData = new DataLoader(startJsonPath);
            StoryLine = masterData.LoadJsonFile();
            Modules = new Dictionary<string, Module>();
            Package = new DialoguePackage();
        }

        public DialoguePackage RunIteration(string moduleName) {
            //  get story line segment, location, character
            //  pick module to use from list associated with character
            var module = PickModule(moduleName);
            if (module != null || package.Status != ModuleStatus.EXIT) {
                package = module.Run();
                return package;
            }
            //game is responsible for shutting not allowing null conversations to happen
            return null;
        }

        public Module GetModule(string moduleName) {
            return PickModule(moduleName);
        }


        private Module PickModule(string name) {
            try {
                return Modules[name];
            } catch (KeyNotFoundException) {
                return null;
            }
        }

        
    }
}
