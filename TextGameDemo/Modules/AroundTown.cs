using System;
using System.Collections.Generic;
using Kati.Module_Hub;
using Kati.GenericModule;
using TextGameDemo.JSON_Files;

namespace TextGameDemo.Modules {
    public class AroundTown : Module {

        public AroundTown(string path) : base(JsonToolkit.AROUND_TOWN, path) { }

        override
        public DialoguePackage Run() {
            return null;
        }
    }
}
