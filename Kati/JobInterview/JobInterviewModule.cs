using System;
using System.Collections.Generic;
using Kati.GenericModule;
using Kati.Module_Hub;
using Kati.SourceFiles;

namespace Kati.JobInterview {


    public class JobInterviewModule {
        
        Controller ctrl;

        public Controller Ctrl { get => ctrl; set => ctrl = value; }

        public JobInterviewModule() {
            Ctrl = new Controller("C:/Users/User/Documents/Kati_V2-1/Kati/JobInterview/JobInterview.json");
            JobInterviewParser parse = new JobInterviewParser(Ctrl);
            Ctrl.Parser = parse;
        }
        
        //run this each visit to the module: everytime dialogue is fetched
        public void RunRound(DialoguePackage package) {
            Ctrl.Package = package;
            Ctrl.Topic.Topic = package.Topic;
            if (package.Type.Length > 0)
                Ctrl.Type.Type = package.Type;
            else
                Ctrl.DefineType(null);
            Ctrl.Parser.Setup(Ctrl.Topic.Topic, Ctrl.Type.Type, Ctrl.Lib.DeepCopyDictionaryByTopic(Ctrl.Topic.Topic, Ctrl.Lib.GetType(Ctrl.Type.Type)));
            Ctrl.Parser.Parse();
        }

        public DialoguePackage GetDialoguePackage() {
            return Ctrl.Package;
        }

    }

    class JobInterviewParser : Parser {

        public JobInterviewParser(Controller c) : base(c) { }

        override
        public void Parse() {
            //Data must not point to Lib data but be a copy
            var data = Branch.RunDecision(Data);
            data = Game.ParseGameRequirments(data);
            data = Personal.ParsePersonalRequirments(data);
            data = Social.ParseSocialRequirments(data);
            data = ForcedNextRequirement(data);  //this is something to add to the default version
            data = Weight.GetDialogue(data);
            SetPackage(ref data);
        }


        private Dictionary<string, Dictionary<string, List<string>>> ForcedNextRequirement
                                    (Dictionary<string, Dictionary<string, List<string>>> data) {
            var temp = new Dictionary<string, Dictionary<string, List<string>>>();

            foreach (KeyValuePair<string, Dictionary<string, List<string>>> item in data) {
                bool keep = true;
                foreach (string item2 in data[item.Key][Constants.REQ]) {
                    string[] arr = Ctrl.Package.LeadTo[Ctrl.Package.Dialogue][0].Split(".");//////////////////////////this needs to look at every leads to not just index 0
                    

                    if (arr.Length >= 4 && arr[0].Equals("forced")) {
                        //Console.WriteLine(item2 + "==" + arr[3]);
                        if (item2.Equals(arr[3])) { //check if
                            Console.WriteLine("req: " + item2 + " lead to " + Ctrl.Package.LeadTo[Ctrl.Package.Dialogue][0] + " dialogue " + item.Key);
                            keep = true;
                        } else {
                            keep = false;
                        }
                    }
                }
                if (keep) {
                    temp[item.Key] = data[item.Key];
                }
            }
            return temp;
        }
        
    }
}
