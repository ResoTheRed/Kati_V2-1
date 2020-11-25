using System;
using System.Collections.Generic;
using Kati.Module_Hub;
using Kati.GenericModule;

namespace TextGameDemo.Modules {
    public class AroundTown : Module {

        public AroundTown(string path) : base("AroundTown", path) { }

        override
        public DialoguePackage Run() {
            return null;
        }
    }
}
