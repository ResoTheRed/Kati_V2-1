using System;
using System.Collections.Generic;
using System.Text;
using Kati.Module_Hub;
using Kati.GenericModule;

namespace TextGameDemo.Modules {
    public class FightingWords : Module {

        public FightingWords(string path) : base("FightingWords", path) { }

        override
        public DialoguePackage Run() {
            return null;
        }
    }
}
