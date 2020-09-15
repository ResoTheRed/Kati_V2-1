using System;
using System.Collections.Generic;
using Kati.GenericModule;
using Kati.Module_Hub;

namespace Kati.JobInterview {


    public class JobInterviewModule {
        
        Controller ctrl;
        
        public JobInterviewModule() {
            ctrl = new Controller("JobInterview.json");
        }
        
        //run this each visit to the module: everytime dialogue is fetched
        public void RunRound(DialoguePackage package) {
            ctrl.Package = package;
            ctrl.Topic.Topic = package.Topic;
            if (package.Type.Length > 0)
                ctrl.Type.Type = package.Type;
            else
                ctrl.DefineType(null);
            ctrl.RunParser();
        }

        public DialoguePackage GetDialoguePackage() {
            return ctrl.Package;
        }

    }
}
