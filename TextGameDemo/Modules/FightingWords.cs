using System;
using System.Collections.Generic;
using TextGameDemo.JSON_Files;
using Kati.Module_Hub;
using Kati.GenericModule;

namespace TextGameDemo.Modules {
    public class FightingWords : Module {

        public FightingWords(string path) : base(JsonToolkit.FIGHTING_WORDS, path) { }

        override
        public DialoguePackage Run() {
            return null;
        }

        override
        public void SetCurrentCharacter(string character) {

        }
    }
}
