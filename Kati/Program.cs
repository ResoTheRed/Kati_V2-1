using System;
using System.Collections.Generic;
using Kati.GenericModule;
using Kati.JobInterview;
using Kati.Module_Hub;
using Kati.SourceFiles;

namespace Kati{

    public class Program{

        static Game game = new Game();
        static DialoguePackage package = new DialoguePackage();

        public static void Main(string[] args) {
            Run1();
            while (!package.Req[package.Dialogue].Equals("intro_tier30")) {
                if (!package.Type.Equals(Constants.RESPONSE)) {
                    Run2();
                } else {
                    Run3();
                }
                
            }
        }

        public static void Run1() {
            //inital npc setup
            game.SetupNPC();
            //setting up the package
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
            PackageDiagnostics(package);
            //set up the controller
            game.mod.Ctrl.Package = package;
            game.mod.Ctrl.Topic.Topic = package.Topic;
            game.mod.Ctrl.Type.Type = package.Type;
            //setup the parser
            var data = game.mod.Ctrl.Lib.DeepCopyDictionaryByTopic(game.mod.Ctrl.Topic.Topic, game.mod.Ctrl.Lib.GetType(game.mod.Ctrl.Type.Type));
            var topic = game.mod.Ctrl.Topic.Topic;
            var type = game.mod.Ctrl.Type.Type;
            game.mod.Ctrl.Parser.Setup(topic, type, data);
            //run the parser
            Console.WriteLine();
            game.mod.Ctrl.Parser.Parse();
            PackageDiagnostics(package);
        }
        //statements
        public static void Run2() {
            Console.WriteLine("\nRun2\n");
            PackageDiagnostics(package);
            Console.WriteLine();
            game.mod.Ctrl.Parser.Parse();
            PackageDiagnostics(package);
            Console.WriteLine();

            DialoguePackage next = DeepCopyPackage();
            package.Reset();
            package.Module = next.Module;
            //set topic, type and req based on passed lead to
            game.ParseLeadTo(ref next);
            package.Topic = next.Topic;
            package.Type = next.Type;
            package.Dialogue = "next";
            package.Req["next"] = next.Req[next.Dialogue];
            package.LeadTo["next"] = next.LeadTo[next.Dialogue];
            //set speaker and responder
            if (!package.Type.Equals(Constants.RESPONSE)) {
                package.Speaker = "Hiring Manager";
                package.Responder = "player";
            } else {
                package.Speaker = "player";
                package.Responder = "Hiring Manager";
            }
            //PackageDiagnostics(package);
            //set up the controller
            game.mod.Ctrl.Package = package;
            game.mod.Ctrl.Topic.Topic = package.Topic;
            game.mod.Ctrl.Type.Type = package.Type;
            //setup the parser
            var data = game.mod.Ctrl.Lib.DeepCopyDictionaryByTopic(game.mod.Ctrl.Topic.Topic, game.mod.Ctrl.Lib.GetType(game.mod.Ctrl.Type.Type));
            var topic = game.mod.Ctrl.Topic.Topic;
            var type = game.mod.Ctrl.Type.Type;
            game.mod.Ctrl.Parser.Setup(topic, type, data);
            //run the parser
            
            
        }

        //response
        public static void Run3() {
            Console.WriteLine("\nRun3\n");
            DialoguePackage next = DeepCopyPackage();
            Console.WriteLine();
            package.Reset();
            package.Module = next.Module;
            //set topic, type and req based on passed lead to
            game.ParseLeadTo(ref next);
            package.Topic = next.Topic;
            package.Type = next.Type;
            package.Dialogue = "next";
            package.Req["next"] = next.Req[next.Dialogue];
            package.LeadTo["next"] = next.LeadTo[next.Dialogue];
            //set speaker and responder
            if (!package.Type.Equals(Constants.RESPONSE)) {
                package.Speaker = "Hiring Manager";
                package.Responder = "player";
            } else {
                package.Speaker = "player";
                package.Responder = "Hiring Manager";
            }
            PackageDiagnostics(package);
            //set up the controller
            game.mod.Ctrl.Package = package;
            game.mod.Ctrl.Topic.Topic = package.Topic;
            game.mod.Ctrl.Type.Type = package.Type;

            //setup responses
            //OrderRelationshipBranches(ref package, npc_tones);
            game.mod.Ctrl.Parser.Response.OrderRelationshipBranches(ref package,game.mod.Ctrl.Npc.InitiatorsTone);
            //ParseResponse(Lib_data);
            game.mod.Ctrl.Package = package;
            game.mod.Ctrl.Topic.Topic = package.Topic;
            game.mod.Ctrl.Type.Type = package.Type;
            //setup the parser
            var data = game.mod.Ctrl.Lib.DeepCopyDictionaryByTopic(game.mod.Ctrl.Topic.Topic, game.mod.Ctrl.Lib.GetType(game.mod.Ctrl.Type.Type));
            game.mod.Ctrl.Parser.Response.ParseResponses(data);
            var respond = game.mod.Ctrl.Parser.Response.Responses;
            Console.WriteLine("What will you reply with?");
            //print options to screen and save in package
            PackageDiagnostics(package);
            package.Response = new List<string>();
            for (int i = 0; i < respond.Count; i++) {
                foreach (KeyValuePair<string, Dictionary<string, List<string>>> reply in respond[i]) {
                    Console.WriteLine(""+(i+1)+". "+reply.Key);
                    package.Response.Add(reply.Key);
                    package.Req[reply.Key] = respond[i][reply.Key][Constants.REQ];
                    package.LeadTo[reply.Key] = respond[i][reply.Key][Constants.LEAD_TO];
                }
            }
            string choice = Console.ReadLine();
            //set dialogue based on choice
            if (choice.Equals("1")&&package.Response.Count>0) {
                package.Dialogue = package.Response[0];
            } else if (choice.Equals("2") && package.Response.Count > 0) {
                package.Dialogue = package.Response[1];
            } else if (choice.Equals("3") && package.Response.Count > 0) {
                package.Dialogue = package.Response[2];
            } else if (choice.Equals("4") && package.Response.Count > 0) {
                package.Dialogue = package.Response[3];
            } else {
                package.Dialogue = package.Response[(int)(Controller.dice.NextDouble()*package.Response.Count)];
            }
            //use stat attributes
            for (int i=0; i<package.LeadTo[package.Dialogue].Count;i++) {
                string[] arr = package.LeadTo[package.Dialogue][i].Split(".");
                if (arr.Length > 0 && !arr[0].Equals("forced")) {
                    package.LeadTo[package.Dialogue].RemoveAt(i);
                }
            }

            
            next = DeepCopyPackage();
            package.Reset();
            package.Module = next.Module;
            //set topic, type and req based on passed lead to
            game.ParseLeadTo(ref next);
            package.Topic = next.Topic;
            package.Type = next.Type;
            package.Dialogue = "next";
            package.Req["next"] = next.Req[next.Dialogue];
            package.LeadTo["next"] = next.LeadTo[next.Dialogue];
            //set speaker and responder
            if (!package.Type.Equals(Constants.RESPONSE)) {
                package.Speaker = "Hiring Manager";
                package.Responder = "player";
            } else {
                package.Speaker = "player";
                package.Responder = "Hiring Manager";
            }
            //set up the controller
            game.mod.Ctrl.Package = package;
            game.mod.Ctrl.Topic.Topic = package.Topic;
            game.mod.Ctrl.Type.Type = package.Type;

            PackageDiagnostics(package);

            if (!package.Type.Equals(Constants.RESPONSE)) {
                //setup the parser
                data = game.mod.Ctrl.Lib.DeepCopyDictionaryByTopic(game.mod.Ctrl.Topic.Topic, game.mod.Ctrl.Lib.GetType(game.mod.Ctrl.Type.Type));
                var topic = game.mod.Ctrl.Topic.Topic;
                var type = game.mod.Ctrl.Type.Type;
                game.mod.Ctrl.Parser.Setup(topic, type, data);
            }
            
        }

        public static DialoguePackage DeepCopyPackage() {
            DialoguePackage next = new DialoguePackage();
            next.Module = package.Module;
            next.Speaker = package.Speaker;
            next.Responder = package.Responder;
            next.Dialogue = package.Dialogue;
            next.Response = package.Response;
            next.Req[package.Dialogue] = new List<string>();
            next.LeadTo = package.LeadTo;
            return next;
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
        
        public JobInterviewModule mod;
        public DialoguePackage package;
        public JobInterviewParser parse;
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
                        if (next.Type.Equals(Constants.RESPONSE)) {
                            next.Req[next.Dialogue].Add(Constants.RESPONSE_TAG+"."+arr[3]);
                        }
                        next.Req[next.Dialogue].Add(arr[3]);
                    }
                    break;
                }
            }
        }


        public void StartingDialoguePackage() {
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

        public void SetupNPC() {
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
