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
            Console.WriteLine("Enter q to quit.");
            string n;
            DialoguePackage p = game.package;
            p = game.RunFirstRound(p);
            PackageDiagnostics(p);
            n = Console.ReadLine();
            while (!n.Equals("q")) {
                p=game.RunRound(p);
                PackageDiagnostics(p);
                n = Console.ReadLine();
            }
        }

        //view everthing in the package
        public static void PackageDiagnostics(DialoguePackage p) {
            Console.WriteLine("Module: "+p.Module);
            Console.WriteLine("Speaker: "+p.Speaker+"\nResponder: "+p.Responder);
            Console.WriteLine("Topic: "+p.Topic+"\nType: "+p.Type);
            Console.WriteLine("Dialogue: "+p.Dialogue);
            foreach (string item in p.Req[p.Dialogue]) {
                Console.WriteLine("Req: "+ item);
            }
            foreach (string item in p.LeadTo[p.Dialogue]) {
                Console.WriteLine("lead to: "+item);
            }

        }

    }

    class Game {
        
        JobInterviewModule mod;
        public DialoguePackage package;
        JobInterviewParser parse;
        //speaker : dialogue
        public List<(string, string)> history;

        public Game() {
            mod = new JobInterviewModule();
            package = new DialoguePackage();
            parse = new JobInterviewParser(mod.Ctrl);
            history = new List<(string, string)>();
            SetupNPC();
            StartingDialoguePackage();
        }

        public DialoguePackage RunFirstRound(DialoguePackage p) { 
            package = p;
            //Program.PackageDiagnostics(package);
            ParseLeadTo(ref package);
            mod.RunRound(package);
            return mod.GetDialoguePackage();
        }
        
        //called each round by the game
        public DialoguePackage RunRound(DialoguePackage p) {
            SetNextPackage();
            mod.RunRound(package);
            return mod.GetDialoguePackage();
        }

        private void SetPackage() {
            //save history
            if (package.Type.Equals(Constants.RESPONSE)) {
                history.Add(("Hiring Manager",package.Dialogue));
            } else {
                history.Add(("Player", package.Dialogue));
            }
            package = SetNextPackage();
        }


        //figure out where and what needs to happen next
        //set topic, type, and req if available
        public void ParseLeadTo(ref DialoguePackage next) {
            //SetDefaultPackage();
            foreach (string item in next.LeadTo[next.Dialogue]) {
                string[] arr = item.Split(".");
                //need to set default values to topic and type
                if (arr[0].Equals("forced")) {
                    if (arr.Length > 1) {
                        next.Topic = arr[1];
                    }
                    if (arr.Length > 2) {
                        next.Type = arr[2];
                    }
                    if (arr.Length > 3) {
                        next.Req[next.Dialogue].Add(arr[3]);
                    }
                    break;
                }
            }
        }

        private DialoguePackage SetNextPackage() {
            DialoguePackage next = new DialoguePackage();
            //set module name
            next.Module = package.Module;
            //set module speaker and responder
            if (package.Type.Equals(Constants.RESPONSE)) {
                next.Speaker = "Hiring Manager";
                next.Responder = "Player";
            } else {
                next.Responder = "Hiring Manager";
                next.Speaker = "Player";
            }
            //set dialogue (needs to have chosen response as dialogue in package)
            next.Dialogue = package.Dialogue;
            next.Req[package.Dialogue] = new List<string>();//package.Req[package.Dialogue];
            next.LeadTo[package.Dialogue] = new List<string>();//package.LeadTo[package.Dialogue];
            //set topic, type, and directed requirement
            ParseLeadTo(ref next);
            Console.WriteLine("\n\nSetNextPackage\n");
            Program.PackageDiagnostics(next);
            Console.WriteLine("\n");
            return next;
        }

        private void StartingDialoguePackage() {
            package.Module = "JobInterviewModule";
            package.Speaker = "Hiring Manager";
            package.Responder = "Player";
            package.Topic = "welcome";
            package.Type = Constants.STATEMENT;
            package.Dialogue = "next";
            package.Req["next"] = new List<string>();
            package.LeadTo["next"] = new List<string>();
            package.Req["next"].Add("start");
            package.LeadTo["next"].Add("forced.welcome.statement.start");
        }

        private void SetupNPC() {
            CharacterData npc = CharacterData.GetCharacterData();
            Dictionary<string, double> tone = new Dictionary<string, double> {
                ["romance"] = 0,
                ["friend"] = 0,
                ["professional"] = 0,
                ["respect"] = 0,
                ["affinity"] = 0,
                ["disgust"] = 0,
                ["hate"] = 0,
                ["rivalry"] = 0
            };
            Dictionary<string, string> personal = new Dictionary<string, string>();
            Dictionary<string, Dictionary<string, string>> social = new Dictionary<string, Dictionary<string, string>>();
            CharacterData.SetInitiatorCharacterData("Hiring Manager","female",tone,personal,social);
            mod.Ctrl.Npc = npc;
        }

    }

  
}
