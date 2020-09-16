using System;
using System.Collections.Generic;
using Kati.GenericModule;
using Kati.Module_Hub;

namespace Kati.JobInterview {


    public class JobInterviewModule {
        
        Controller ctrl;

        public Controller Ctrl { get => ctrl; set => ctrl = value; }

        public JobInterviewModule() {
            Ctrl = new Controller("JobInterview.json");
        }
        
        //run this each visit to the module: everytime dialogue is fetched
        public void RunRound(DialoguePackage package) {
            Ctrl.Package = package;
            Ctrl.Topic.Topic = package.Topic;
            if (package.Type.Length > 0)
                Ctrl.Type.Type = package.Type;
            else
                Ctrl.DefineType(null);
            Ctrl.RunParser();
        }

        public DialoguePackage GetDialoguePackage() {
            return Ctrl.Package;
        }

    }
}
