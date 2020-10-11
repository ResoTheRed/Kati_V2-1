using System;
using System.Collections.Generic;
using Kati.GenericModule;
using Kati.JobInterview;
using Kati.Module_Hub;
using Kati.SourceFiles;

namespace Kati{

    public class Program {
        //range for HIGH: from [MID to HIGH) +1 --> 15 to 20
        public const int EXTREME = 50;
        public const int VERY_HIGH = 30;
        public const int HIGH = 20;
        public const int MID = 15;
        public const int LOW = 10;
        public const int VERY_LOW = 5;

        //win thresholds
        public const int WIN_01 = 660;
        public const int WIN_03 = 500;
        public const int WIN_06 = 400;
        
        
        //threshold for displaying the # in stat display
        public const int STAR_MOD = 15;
        static string friend_bar = "", respect_bar = "", disgust_bar = "";

        static Game game = new Game();
        static DialoguePackage package = new DialoguePackage();
        static List<string[]> history = new List<string[]>();
        static Dictionary<string, List<string>> topics = new Dictionary<string, List<string>>() {
            ["welcome"] = new List<string>() { "start" },
            ["qualifications"] = new List<string>() { "pack_1a_0" , "pack_1b_0" , "pack_1c_0" , "pack_1d_0" }, 
            ["getToKnowYou"] = new List<string>() { "hobby_01", "hobby_02", "interests_01", "beliefs_01", 
                "academic_01", "academic_02", "work_01", "generic_01" },
            ["expectations"] = new List<string>() { "start" }, 
            ["got_the_job"]= new List<string>() { "start" }
        };

        public static void Main(string[] args) {
            //must run this line before running the program
            game.SetupNPC();

            RunWelcome();
            RunQualifications();
            RunKnowYou();
            RunExpectations();
            //Test();
            PrintHistory();
        }

        public static void Test() { 
            Console.WriteLine(game.mod.Ctrl.Lib.Data["getToKnowYou_statement"]["neutral"].
                ContainsKey("We've talked a bit about the company, now I would like to open the floor to questions and comments and just talk."));
        }

        public static void RunWelcome() {
            Run1("welcome", Constants.STATEMENT, "start", "forced.welcome.statement.start");
            while (!package.Topic.Equals("qualifications")) {
                try {
                    if (!package.Type.Equals(Constants.RESPONSE)) {
                        Run2();
                    } else {
                        Run3();
                    }
                } catch (Exception) {
                    break;
                }
            }
        }

        public static void RunQualifications() {
            while (topics["qualifications"].Count > 0) {
                int index = (int)(Controller.dice.NextDouble() * topics["qualifications"].Count);
                string req = topics["qualifications"][index];
                string leadTo;
                if (req.Equals("pack_1d_0")) {
                    leadTo = "forced.qualifications.question." + req + "0";
                    Console.WriteLine("Using pack_1d_0");
                } else {
                    leadTo = "forced.qualifications.statement." + req + "1";
                }
                req += "0";
                topics["qualifications"].RemoveAt(index);
                try {
                    Run1("qualifications", Constants.STATEMENT, req, leadTo);
                } catch (Exception) {
                    Console.WriteLine("Caught Exception from run 1: " + req + " " + leadTo);
                    continue;
                }
                while (true) {
                    try {
                        if (!package.Type.Equals(Constants.RESPONSE)) {
                            Run2();
                        } else {
                            Run3();
                        }
                    } catch (Exception) {
                        break;
                    }
                }
            }
        }

        public static void RunKnowYou() {
            //odd one out
            try {
                Run1("getToKnowYou", Constants.STATEMENT, "start", "forced.getToKnowYou.statement.start");
                Run2();
            } catch (Exception) { }
            while (topics["getToKnowYou"].Count > 0 ) {
                int index = (int)(Controller.dice.NextDouble() * topics["getToKnowYou"].Count);
                string req = topics["getToKnowYou"][index];
                string leadTo = "forced.getToKnowYou.question." + req;
                topics["getToKnowYou"].RemoveAt(index);
                try {
                    Run1("getToKnowYou", Constants.QUESTION, req, leadTo);
                } catch (Exception) {
                    Console.WriteLine("Caught Exception from run 1: "+req+" "+leadTo);
                    continue;
                }
                while (true) {
                    try {
                        if (package.Type.Equals(Constants.STATEMENT)) {
                            Run2();
                        } else {
                            Run3();
                        }
                    }catch(Exception) {
                        break;
                    }
                }
            }
        }

        public static void RunExpectations() {
            Run1("expectations", Constants.STATEMENT, "start", "forced.welcome.statement.start");
            while (true) {
                try {
                    if (!package.Type.Equals(Constants.RESPONSE)) {
                        Run2();
                    } else {
                        Run3();
                    }
                } catch (Exception) {
                    break;
                    
                }
            }
        }

        public static void RunGetTheJob() { 
        
        }

        public static void PrintHistory() {
            for (int i = 0; i < game.history.Count; i++) {
                string s, d;
                (s, d) = game.history[i];
                Console.WriteLine(String.Format("{0,13}: " + d + "\n", s));
            }
        }

        //used each time a directed dialogue chain is used
        public static void InitialSetupPackageForTopic(string topic,string type,string req, string leadTo) {
            package.Module = "JobInterviewModule";
            package.Speaker = "Hiring Manager";
            package.Responder = "Player";
            SetPackage(topic,type,new List<string>() { req }, new List<string>() { leadTo });
        }

        public static void SetPackage(string topic, string type, List<string> req, List<string> leadTo) {
            package.Topic = topic;
            package.Type = type;
            package.Dialogue = "next";
            package.Req["next"] = req;
            package.LeadTo["next"] = leadTo;
        }

        //sets the controllers package to the most current package available
        public static void SetupControler() {
            game.mod.Ctrl.Package = package;
            game.mod.Ctrl.Topic.Topic = package.Topic;
            game.mod.Ctrl.Type.Type = package.Type;
        }

        //satifies the topic, type and data requirement of the parser
        public static void SetupParser() {
            var data = game.mod.Ctrl.Lib.DeepCopyDictionaryByTopic(game.mod.Ctrl.Topic.Topic, game.mod.Ctrl.Lib.GetType(game.mod.Ctrl.Type.Type));
            var topic = game.mod.Ctrl.Topic.Topic;
            var type = game.mod.Ctrl.Type.Type;
            game.mod.Ctrl.Parser.Setup(topic, type, data);
        }

        public static void SetSpeakerAndResponder() {
            if (!package.Type.Equals(Constants.RESPONSE)) {
                package.Speaker = "Hiring Manager";
                package.Responder = "player";
            } else {
                package.Speaker = "player";
                package.Responder = "Hiring Manager";
            }
        }

        //changes the global package values to reflect the newly derived info from the next line of dialogue
        public static void ConvertPackageToNextPackage() {
            DialoguePackage next = DeepCopyPackage();
            package.Reset();
            package.Module = next.Module;

            //set topic, type and req based on passed lead to
            game.ParseLeadTo(ref next);
            SetPackage(next.Topic, next.Type, next.Req[next.Dialogue], next.LeadTo[next.Dialogue]);
            SetSpeakerAndResponder();
            SetupControler();
        }

        //run first at the start of each dialogue chain
        public static void Run1(string topic, string type, string req, string leadTo) {
            InitialSetupPackageForTopic(topic, type, req, leadTo);
            SetupControler();
            SetupParser();
            Console.WriteLine();
            game.mod.Ctrl.Parser.Parse();
            PackageDiagnostics(package);
        }

        //statements
        public static void Run2() {
            Console.WriteLine("\nRun2\n");
            Console.WriteLine();

            game.mod.Ctrl.Parser.Parse(); //pick the next dialogue bit
            PackageDiagnostics(package);
            CaptureHistory();
            Console.WriteLine();
            ConvertPackageToNextPackage();
            if(package.Topic.Length>0 && package.Type.Length>0)
                SetupParser();
           
        }

        public static void LoadResponsesIntoPackage(List<Dictionary<string, Dictionary<string, List<string>>>> respond) {
            package.Response = new List<string>();
            for (int i = 0; i < respond.Count; i++) {
                foreach (KeyValuePair<string, Dictionary<string, List<string>>> reply in respond[i]) {
                    Console.WriteLine("" + (i + 1) + ". " + reply.Key);
                    package.Response.Add(reply.Key);
                    package.Req[reply.Key] = respond[i][reply.Key][Constants.REQ];
                    package.LeadTo[reply.Key] = respond[i][reply.Key][Constants.LEAD_TO];
                }
            }
        }

        //adds player's response choice to the package as teh dialogue choice
        public static void PlayerSelectResponeFromResponseList() {
            Console.WriteLine("What will you reply with?");
            string choice = Console.ReadLine();
            //set dialogue based on choice
            if (choice.Equals("1") && package.Response.Count > 0) {
                package.Dialogue = package.Response[0];
            } else if (choice.Equals("2") && package.Response.Count > 0) {
                package.Dialogue = package.Response[1];
            } else if (choice.Equals("3") && package.Response.Count > 0) {
                package.Dialogue = package.Response[2];
            } else if (choice.Equals("4") && package.Response.Count > 0) {
                package.Dialogue = package.Response[3];
            } else {
                package.Dialogue = package.Response[(int)(Controller.dice.NextDouble() * package.Response.Count)];
            }
        }

        public static void CaptureHistory() {
            string[] s = new string[] { package.Speaker, package.Dialogue };
            history.Add(s);
        }

        //response
        public static void Run3() {
            Console.WriteLine("\nRun3\n");

            ConvertPackageToNextPackage();
            
            //OrderRelationshipBranches(ref package, npc_tones);
            game.mod.Ctrl.Parser.Response.OrderRelationshipBranches(ref package,game.mod.Ctrl.Npc.InitiatorsTone);
            SetupControler();
            //pull response data from library
            var data = game.mod.Ctrl.Lib.DeepCopyDictionaryByTopic(game.mod.Ctrl.Topic.Topic, game.mod.Ctrl.Lib.GetType(game.mod.Ctrl.Type.Type));
            //find best response options
            game.mod.Ctrl.Parser.Response.ParseResponses(data);
            //get response options
            var respond = game.mod.Ctrl.Parser.Response.Responses;

            PackageDiagnostics(package);
            

            LoadResponsesIntoPackage(respond);
            PlayerSelectResponeFromResponseList();
            CaptureHistory();
            PackageDiagnostics(package);

            ExecuteLeadToInstructions();            

            ConvertPackageToNextPackage();
            //setup parser for non response dialogue
            if (!package.Type.Equals(Constants.RESPONSE)) {
                SetupParser();
            }
            
        }

        //use and remove Leadto instruction lines
        public static void ExecuteLeadToInstructions() {
            for (int i = 0; i < package.LeadTo[package.Dialogue].Count; i++) {
                string[] arr = package.LeadTo[package.Dialogue][i].Split(".");
                if (arr.Length > 1) {
                    Console.WriteLine(arr[0]+" " + ConvertStatScale(arr[1]));
                    switch (arr[0]) {
                        case "professional": { game.mod.Ctrl.Npc.InitiatorsTone[Constants.PROFESSIONAL] += ConvertStatScale(arr[1]); } break;
                        case "friend": { game.mod.Ctrl.Npc.InitiatorsTone[Constants.FRIEND] += ConvertStatScale(arr[1]);  } break;
                        case "respect": { game.mod.Ctrl.Npc.InitiatorsTone[Constants.RESPECT] += ConvertStatScale(arr[1]); } break;
                        case "disgust": { game.mod.Ctrl.Npc.InitiatorsTone[Constants.DISGUST] += ConvertStatScale(arr[1]); } break;
                    }
                    if (!arr[0].Equals("forced")) {
                        package.LeadTo[package.Dialogue].RemoveAt(i);
                    }
                }
            }
            DisplayRelationshipStatus();
        }

        public static int ConvertStatScale(string level) {
            return level switch
            {
                "extreme" => (int)(Controller.dice.NextDouble() * (EXTREME - VERY_HIGH)) + VERY_HIGH + 1,
                "very_high" => (int)(Controller.dice.NextDouble() * (VERY_HIGH - HIGH)) + HIGH + 1,
                "high" => (int)(Controller.dice.NextDouble() * (HIGH - MID)) + MID + 1,
                "mid" => (int)(Controller.dice.NextDouble() * (MID - LOW)) + LOW + 1,
                "low" => (int)(Controller.dice.NextDouble() * (LOW - VERY_LOW)) + VERY_LOW + 1,
                "very_low" => (int)(Controller.dice.NextDouble() * (VERY_LOW)) + 1,
                "extreme_neg" => ((int)(Controller.dice.NextDouble() * (EXTREME - VERY_HIGH)) + VERY_HIGH + 1) * -1,
                "very_high_neg" => ((int)(Controller.dice.NextDouble() * (VERY_HIGH - HIGH)) + HIGH + 1) * -1,
                "high_neg" => ((int)(Controller.dice.NextDouble() * (HIGH - MID)) + MID + 1) * -1,
                "mid_neg" => ((int)(Controller.dice.NextDouble() * (MID - LOW)) + LOW + 1) * -1,
                "low_neg" => ((int)(Controller.dice.NextDouble() * (LOW - VERY_LOW)) + VERY_LOW + 1) * -1,
                "very_low_neg" => ((int)(Controller.dice.NextDouble() * (VERY_LOW)) + 1) * -1,
                _ => 0,
            };
        }

        public static void DisplayRelationshipStatus() {
            var tones = game.mod.Ctrl.Npc.InitiatorsTone;
            int max = tones["friend"] > tones["respect"] ? (int)tones["friend"] : (int)tones["respect"];
            max = max > (int)tones["disgust"] ? max : (int)tones["disgust"];
            for (int i = 0; i < max; i += STAR_MOD) {
                if (i < tones["friend"])
                    friend_bar += "#";
                if (i < tones["respect"])
                    respect_bar += "#";
                if (i < tones["disgust"])
                    disgust_bar += "#";
            }
            string temp = "Friend Vibe:  " + friend_bar + "\n" + "Respect Vibe: " + respect_bar + "\n" + "Disgust Vibe: " + disgust_bar+"\n";
            //history.Add(new string[]{temp, ""});
            Console.WriteLine("Friend Vibe:  "+friend_bar);
            Console.WriteLine("Respect Vibe: "+respect_bar);
            Console.WriteLine("Disgust Vibe: "+disgust_bar);

        }

        public static string CheckWinThresholds() {
            string score = "win_tier_08";
            int value = 200;
            if (game.mod.Ctrl.Npc.InitiatorsTone[Constants.PROFESSIONAL]>WIN_01) {
                score = "win_tier_01";
            } else if (game.mod.Ctrl.Npc.InitiatorsTone[Constants.PROFESSIONAL] > WIN_03) {
                score = "win_tier_03";
            } else if (game.mod.Ctrl.Npc.InitiatorsTone[Constants.PROFESSIONAL] > WIN_06) {
                score = "win_tier_06";
            }
            Console.WriteLine(score+" "+ game.mod.Ctrl.Npc.InitiatorsTone[Constants.PROFESSIONAL]+"/"+value);
            return score;
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
            game.history.Add((p.Speaker,p.Dialogue));
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
