using System;
using System.Collections.Generic;
using Kati.Module_Hub;
using Kati.GenericModule;
using TextGameDemo.JSON_Files;

namespace TextGameDemo.Modules {
    public class YoungLove : Module {

        public YoungLove(string path) : base(JsonToolkit.YOUNG_LOVE, path) { }

        override
        public DialoguePackage Run() {
            return null;
        }

        override
        public void SetCurrentCharacter(string character) {

        }
    }
}
