using System;
using System.Collections.Generic;
using Kati.Module_Hub;
using Kati.GenericModule;
using TextGameDemo.JSON_Files;

namespace TextGameDemo.Modules {
    public class Questing : Module{

        public Questing(string path) : base(JsonToolkit.QUESTING, path) { }

        override
        public DialoguePackage Run() {
            return null;
        }

        override
        public void SetCurrentCharacter(string character) {

        }
    }
}
