using System;
using System.Collections.Generic;
using System.Text;
using Kati.Module_Hub;
using Kati.GenericModule;

namespace TextGameDemo.Modules {
    public class Benjamin : Module {

        public Benjamin(string path) : base("Benjamin", path) { }

        override
        public DialoguePackage Run() {
            return null;
        }
    }
}
