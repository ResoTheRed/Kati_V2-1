using System;
using System.Collections.Generic;
using Kati.GenericModule;
using Kati.JobInterview;
using Kati.Module_Hub;
using Kati.SourceFiles;

namespace Kati{

    public class Program{

        static Game game = new Game();
        public static void Main(string[] args) {
            Console.WriteLine("Hello World");
        }


        

    }

    class Game {
        
        JobInterviewModule mod;
        DialoguePackage package;
        JobInterviewParser parse;

        public Game() {
            mod = new JobInterviewModule();
            package = new DialoguePackage();
            parse = new JobInterviewParser(mod.Ctrl);
            StartingDialoguePackage();
        }

        //figure out where what needs to happen next
        public void ParseLeadTo() {
            SetDefaultPackage();
            foreach (string item in package.LeadTo[package.Dialogue]) {
                string[] arr = item.Split(".");
                if (arr[0].Equals("forced")) {
                    if (arr.Length > 1) {
                        package.Topic = arr[1];
                    }
                    if (arr.Length > 2) {
                        package.Type = arr[2];
                    }
                    if (arr.Length > 3) {
                        package.Req["next"].Add(arr[3]);
                    }
                    break;
                }
            }
        }

        private void SetDefaultPackage() {
            package.Reset();
            package.Module = "JobInterviewModule";
            package.Speaker = "Hiring Manager";
            package.Responder = "Player";
        }

        private void StartingDialoguePackage() {
            SetDefaultPackage();
            package.Topic = "welcome";
            package.Type = Constants.STATEMENT;
            package.Dialogue = "start";
            package.Req["start"].Add("start");
            package.LeadTo["start"].Add("start");
        }

    }

    class JobInterviewParser : Parser {


        public JobInterviewParser(Controller c): base(c) {}

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
            foreach (KeyValuePair<string, Dictionary<string, List<string>>>item in data) {
                bool keep = true;
                foreach (string item2 in data[item.Key][Constants.REQ]) {
                    string[] arr = item2.Split(".");
                    if (arr.Length >= 4 && arr[0].Equals("forced")) {
                        if (Ctrl.Package.Req.Equals(arr[3])) { //check if
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
