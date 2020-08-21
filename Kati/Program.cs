using System;
using System.Collections.Generic;
using Kati.GenericModule;
using Kati.SourceFiles;

namespace Kati{

    class Program{

        static Controller ctrl;

        static void Main(string[] args){
            TestDialogueWeight();
        }

        static void TestDialogueWeight() {
            ctrl = new Controller(Constants.TestJson);
            int[] values = new int[15];
            List<string> keys = new List<string>();
            for (int i = 1; i < 16; ++i) {
                keys.Add("respect test " + i);
            }
            for (int j = 0; j < 10000; j++) {
                var d = ctrl.Parser.Weight.GetDialogue(ctrl.Lib.Data["sample1_statement"]["respect"]);
                for (int k = 0; k < keys.Count; k++) {
                    if (d.ContainsKey(keys[k])) {
                        values[k] += 1;
                    }
                }
            }
            for (int l = 0; l < keys.Count; l++) {
                Console.WriteLine(keys[l]+" "+(values[l]/100.0));
            }
        }

    }
}
