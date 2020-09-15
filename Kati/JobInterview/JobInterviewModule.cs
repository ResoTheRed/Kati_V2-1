using System;
using System.Collections.Generic;
using Kati.GenericModule;

namespace Kati.JobInterview {


    public class JobInterviewModule {
        Controller ctrl;
        //any special rules
        //prevent repeating dialogue
        //TODO: 9/14/2020:
        //get module producing values
        public JobInterviewModule() {
            ctrl = new Controller("JobInterview.json");
        }


    }
}
