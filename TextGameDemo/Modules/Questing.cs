using System;
using System.Collections.Generic;
using Kati.Module_Hub;
using Kati.GenericModule;

namespace TextGameDemo.Modules {
    public class Questing : Module{

        public Questing(string path) : base("Questing", path) { }

        override
        public DialoguePackage Run() {
            return null;
        }
    }
}
