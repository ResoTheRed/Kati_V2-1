using System;
using System.Collections.Generic;
using Kati.Module_Hub;
using Kati.GenericModule;

namespace TextGameDemo.Modules {
    public class ForestTalk : Module{

        public ForestTalk(string path) : base("ForestTalk", path) { }

        override
        public DialoguePackage Run() {
            return null;
        }
    }
}
