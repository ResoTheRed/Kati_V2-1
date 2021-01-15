using System;
using System.Collections.Generic;
using TextGameDemo.JSON_Files;
using Kati.Module_Hub;
using Kati.GenericModule;

namespace TextGameDemo.Modules {
    public class Benjamin : Module {

        public Benjamin(string path) : base(JsonToolkit.BENJAMIN, path) { }

        override
        public DialoguePackage Run() {
            return null;
        }
    }
}
