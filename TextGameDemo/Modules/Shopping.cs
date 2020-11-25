using System;
using System.Collections.Generic;
using Kati.Module_Hub;
using Kati.GenericModule;

namespace TextGameDemo.Modules {
    public class Shopping : Module{

        public Shopping(string path) : base("Shopping",path) { }

        override
        public DialoguePackage Run() {
            return null;
        }
    }
}
