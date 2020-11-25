using System;
using System.Collections.Generic;
using Kati.Module_Hub;
using Kati.GenericModule;

namespace TextGameDemo.Modules {
    public class Lafitte : Module{

        public Lafitte(string path) : base("Lafitte", path) { }

        override
        public DialoguePackage Run() {
            return null;
        }
    }
}
