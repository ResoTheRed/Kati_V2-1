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
        virtual public Dictionary<string, Dictionary<string, List<string>>> ParseGameRequirments
                                (Dictionary<string, Dictionary<string, List<string>>> data) {
           
            return data;
        }

        //fault tolerant method.
        virtual public bool RemoveElement(string req) {
            return true;
        }

    }
}
