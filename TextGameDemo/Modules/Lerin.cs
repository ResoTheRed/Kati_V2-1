using System;
using System.Collections.Generic; 
using Kati.Module_Hub;
using Kati.GenericModule;
using TextGameDemo.JSON_Files;

namespace TextGameDemo.Modules {
    public class Lerin : Module{

        public Lerin(string path) : base(JsonToolkit.LERIN, path) { }

        override
        public DialoguePackage Run() {
            return null;
        }

    }
}
