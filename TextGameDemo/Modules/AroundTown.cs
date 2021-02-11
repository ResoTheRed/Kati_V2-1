using System;
using System.Collections.Generic;
using Kati.Module_Hub;
using Kati.GenericModule;
using TextGameDemo.JSON_Files;
using TextGameDemo.Game;
using TextGameDemo.Game.Characters;

namespace TextGameDemo.Modules {
    public class AroundTown : Module {

        const string GREETING = "Greeting";
        const string SINGLE_ROOM = "SingleRooms";
        const string HISTORY_LESSON = "HistoryLessons";
        const string PEOPLE_IN_TOWN = "PeopleInTown";

        private string[] topics = { GREETING, SINGLE_ROOM, HISTORY_LESSON, PEOPLE_IN_TOWN };

        private GameModel model;

        public GameModel Model { get => model; set => model = value; }
        private string character;

        public AroundTown(string path, GameModel model) : base(JsonToolkit.AROUND_TOWN, path) {
            AroundTownParser parse = new AroundTownParser(Ctrl);
            parse.SetGameRules(Model.GameData);
            parse.SetLocationRules(Model.Player);
            Ctrl.Parser = parse;
            Ctrl.Game = model.GameData;
            AroundTownResponse res = new AroundTownResponse();
            Ctrl.Parser.Response = res;
            Model = model;
        }
        /* to run module
            SetCurrentCharacter()
            Run()
         */


        override
        public DialoguePackage Run() {
            SetupController();
            Ctrl.RunParser();
            return Ctrl.Package;
        }

        override
        public void SetCurrentCharacter(string character) {
            this.character = character;
        }

        public void SetupController() {
            Ctrl.Package = Game.DialoguePackageHandler.Get();
            Ctrl.Topic.Topic = SetTopic();
            Ctrl.Type.Type = SetType(Ctrl.Package,Ctrl.Topic.Topic);
        }

        public string SetTopic() {
            string topic = "";
            if (Model.GameData.GetConversationCounter(character) == 0) { 
                topic = GREETING;
            } else if (Model.GameData.GetConversationCounter(character) < 5) { //uniform distribution
                topic = topics[GameTools.Tools().Next(topics.Length)];
            } else { //annoy character
                AnnoyCharacter();                
            }
            return topic;
        }

        private void AnnoyCharacter() {
            int value = GameTools.Tools().Next(10);
            int num = Model.GameData.GetConversationCounter(character);
            num += GameTools.Tools().Next(num);
            if (value < 4) {
                Model.ChangeAttribute(num, Kati.Constants.DISGUST, Model.Lib.Lib[character]);
            } else if (value < 8) {
                Model.ChangeAttribute(num, Kati.Constants.DISGUST, Model.Lib.Lib[character]);
            } else { 
                Model.ChangeNegative(3, Model.Lib.Lib[character]);
            }
        }

        public string SetType(DialoguePackage pack,string topic) {
            string type = "";
            if (pack != null && pack.Type == Kati.Constants.RESPONSE) {
                type = Kati.Constants.RESPONSE;
            } else if (!topic.Equals("")) {
                if (GameTools.Tools().Next(10) > 7 && topic.Equals(GREETING) || topic.Equals(SINGLE_ROOM)) {
                    type = Kati.Constants.QUESTION;
                } else {
                    type = Kati.Constants.STATEMENT;
                }
            }
            return type;
        }

    }

    class AroundTownParser : Parser {

        PersonalRules personal;
        GameRules gRules;
        LocationRules local;
        Game.Characters.Character npc;
        GameModel model;

        internal PersonalRules Personal { get => personal; set => personal = value; }
        public Character Npc { get => npc; set => npc = value; }
        public GameRules GRules { get => gRules; set => gRules = value; }
        public GameModel Model { get => model; set => model = value; }

        public AroundTownParser(Controller ctrl) : base(ctrl){
            Personal = new PersonalRules(ctrl); ;
            //GRules = new GameRules(Model);
        }

        public void SetGameRules(Game.GameData game) {
            GRules = new GameRules(game);
        }
        public void SetLocationRules(CharacterLocations local) {
            this.local = new LocationRules(local);
        }

        override
        public void Parse() {
            //Data must not point to Lib data but be a copy
            var data = Branch.RunDecision(Data);
            data = ParseHelper(data);
            //if branch doesn't contain data move to neutral
            if (data == null) {
                data = Ctrl.Lib.DeepCopyDictionaryByTopic(Ctrl.Topic.Topic, Ctrl.Lib.GetType(Ctrl.Type.Type))[Kati.Constants.NEUTRAL];
                data = ParseHelper(data);
            }
        }

        private Dictionary<string, Dictionary<string, List<string>>> ParseHelper(Dictionary<string, Dictionary<string, List<string>>> data) {
            try {
                data = Game.ParseGameRequirments(data);
                data = Personal.ParsePersonalRequirments(data,Npc);
                data = Social.ParseSocialRequirments(data);
                //data = ForcedNextRequirement(data);  //this is something to add to the default version
                data = Weight.GetDialogue(data);
                SetPackage(ref data);
            } catch (Exception e) {
                Console.WriteLine(e);
                data = null;
            }
            return data;
        }

        private Dictionary<string, Dictionary<string, List<string>>> ForcedNextRequirement
                                   (Dictionary<string, Dictionary<string, List<string>>> data) {
            var temp = new Dictionary<string, Dictionary<string, List<string>>>();
            foreach (KeyValuePair<string, Dictionary<string, List<string>>> item in data) {
                bool keep = true;
                foreach (string item2 in data[item.Key][Kati.Constants.REQ]) {
                    string[] arr = Ctrl.Package.LeadTo[Ctrl.Package.Dialogue][0].Split('.');//////////////////////////this needs to look at every leads to not just index 0
                    if (arr.Length >= 4 && arr[0].Equals("forced")) {
                        //Console.WriteLine(item2 + "==" + arr[3]);
                        if (item2.Equals(arr[3])) { //check if
                            //Console.WriteLine("req: " + item2 + " lead to " + Ctrl.Package.LeadTo[Ctrl.Package.Dialogue][0] + " dialogue " + item.Key);
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

    class AroundTownResponse : Response {

       override
      public Dictionary<string, Dictionary<string, List<string>>> CheckRequirements
          (Dictionary<string, Dictionary<string, List<string>>> data) {
            if (Package == null)
                return data;
            var copy = DeepCopy(data);
            foreach (KeyValuePair<string, List<string>> item in Package.Req) {
                foreach (string req in Package.Req[item.Key]) {
                    string[] arr = req.Split('.');
                    if (arr.Length > 1 && arr[0].Equals(Kati.Constants.RESPONSE_TAG)) {
                        foreach (KeyValuePair<string, Dictionary<string, List<string>>> item2 in data) {
                            RemoveElement(ref copy, item2.Key, ref arr);
                        }
                    }
                }
            }
            return copy;
        }

        private Dictionary<string, Dictionary<string, List<string>>> DeepCopy
            (Dictionary<string, Dictionary<string, List<string>>> data) {
            Dictionary<string, Dictionary<string, List<string>>> copy = new Dictionary<string, Dictionary<string, List<string>>>();
            foreach (KeyValuePair<string, Dictionary<string, List<string>>> item1 in data) {
                copy[item1.Key] = new Dictionary<string, List<string>>();
                foreach (KeyValuePair<string, List<string>> item2 in data[item1.Key]) {
                    copy[item1.Key][item2.Key] = new List<string>();
                    foreach (string item3 in data[item1.Key][item2.Key]) {
                        copy[item1.Key][item2.Key].Add(item3);
                    }
                }
            }
            return copy;
        }
    }

    class PersonalRules {

        Controller ctrl;

        public PersonalRules(Controller ctrl) {
            this.ctrl = ctrl;
        }

        public Dictionary<string, Dictionary<string, List<string>>> ParsePersonalRequirments
                                        (Dictionary<string, Dictionary<string, List<string>>> data, Character npc) {
            var temp = new Dictionary<string, Dictionary<string, List<string>>>();
            foreach (KeyValuePair<string, Dictionary<string, List<string>>> item in data) {
                bool keep = CharacterNameRule(data[item.Key]["req"], npc);
                if (keep)
                    temp[item.Key] = data[item.Key];
            }
            return temp;
        }

        private bool CharacterNameRule(List<string> req,Character npc) {
            bool keep = true;
            foreach (string rule in req) {
                if (rule.Length > 0) {
                    var elements = rule.Split(".");
                    if (elements[0].Equals("Character")) {
                        if (elements.Length > 1 && !elements[1].Equals(npc.Name)) {
                            keep = false;
                        }
                        break;
                    }
                }
            }
            return keep;
        }
        /* req rules
            Character.Teta : Greeting_statement
            Character.Dan : Greeting_statement
            Character.Romero : Greeting_statement
            Character.Machiah : Greeting_statement
            Character.Albrecht : Greeting_statement
            Character.Benjamin : Greeting_statement
            Character.Sivian : Greeting_statement
            Character.Christina : Greeting_statement
            Character.Quinn : Greeting_statement
            Character.Helena : Greeting_statement
         */
    }

    class GameRules {

        Game.GameData game;

        public GameRules(Game.GameData game) {
            this.game = game;
        }

        public Dictionary<string, Dictionary<string, List<string>>> ParseGameRequirments
                                           (Dictionary<string, Dictionary<string, List<string>>> data) {
            var temp = new Dictionary<string, Dictionary<string, List<string>>>();
            foreach (KeyValuePair<string, Dictionary<string, List<string>>> item in data) {
                bool keep = GameReq(data[item.Key]["req"]);
            }
            return temp;
        }

        private bool GameReq(List<string> req) {
            bool keep = true;
            foreach (string rule in req) {
                if (rule.Length > 0) {
                    if (rule.Equals("game.time.not.isNight") && game.IsNight) {
                        keep = false;
                        break;
                    } else if (rule.Equals("game.time.isNight") && !game.IsNight) {
                        keep = false;
                        break;
                    } else if (rule.Equals("game.weather.nice_day") && !game.Weather.Equals("Nice")) {
                        keep = false;
                        break;
                    }
                }
            }
            return keep;
        }

        //play_once

        //game

        /* req rules
            play_once : Greeting_statement

                responseTag.status : Greeting_response
                responseTag.nice_day : Greeting_response
                responseTag.nice_weather : Greeting_response

            
                village_a1 : SingleRooms_statement
                village_a2 : SingleRooms_statement
                village_b1 : SingleRooms_statement
                village_b2 : SingleRooms_statement
           
                Smith_a1 : SingleRooms_statement
            
                responseTag.like_town : SingleRooms_response
                responseTag.like_town_center : SingleRooms_response
                responseTag.like_store : SingleRooms_response
                responseTag.like_blacksmith : SingleRooms_response
                responseTag.entered_forest : SingleRooms_response
                lesson_a1 : HistoryLessons_statement
                lesson_a2 : HistoryLessons_statement
                lesson_a3 : HistoryLessons_statement
                lesson_b1 : HistoryLessons_statement
                lesson_c1 : HistoryLessons_statement
                lesson_c2 : HistoryLessons_statement
                lesson_d1 : HistoryLessons_statement
                lesson_e1 : HistoryLessons_statement
                lesson_f1 : HistoryLessons_statement
                lesson_g1 : HistoryLessons_statement
                lesson_g2 : HistoryLessons_statement
                lesson_h1 : HistoryLessons_statement
                lesson_h2 : HistoryLessons_statement
                lesson_h3 : HistoryLessons_statement
                lesson_h4 : HistoryLessons_statement
                lesson_h5 : HistoryLessons_statement
                lesson_h6 : HistoryLessons_statement
                lesson_h7 : HistoryLessons_statement
                lesson_romance_a1 : HistoryLessons_statement
                friend_a1 : HistoryLessons_statement
                friend_a2 : HistoryLessons_statement
                friend_a3 : HistoryLessons_statement
                friend_b1 : HistoryLessons_statement

         */
    }

    class LocationRules {

        CharacterLocations local;

        public LocationRules(CharacterLocations local) {
            this.local = local;
        }

        public Dictionary<string, Dictionary<string, List<string>>> ParseLocationRequirments
                                        (Dictionary<string, Dictionary<string, List<string>>> data) {
            var temp = new Dictionary<string, Dictionary<string, List<string>>>();
            foreach (KeyValuePair<string, Dictionary<string, List<string>>> item in data) {
                bool keep = LocationRule(data[item.Key]["req"]);
                if (keep)
                    temp[item.Key] = data[item.Key];
            }
            return temp;
        }

        private bool LocationRule(List<string> req) {
            bool keep = true;
            foreach (string rule in req) {
                if (rule.Length > 0) {
                    var elements = rule.Split(".");
                    if (elements[0].Equals("Location")) {
                        (string area, string room) = local.GetLocation();
                        if (elements.Length >= 3) {
                            if (!area.Equals(elements[1]) || !room.Equals(elements[2])) {
                                keep = false; 
                            }
                        } else if (elements.Length >= 2) {
                            if (!area.Equals(elements[1])) {
                                keep = false;
                            }
                        }
                        break;
                    }
                }
            }
            return keep;
        }
        //Location Area Room

        /*
            Location.Town.Town_Center : SingleRooms_question
            Location.Town.store : SingleRooms_statement
            Location.Town.Black_Smith : SingleRooms_question
            Location.Town.Lerin : SingleRooms_statement
            Location.Town.Laffite : SingleRooms_statement
            Location.Forest : SingleRooms_statement
            Location.Town.Dans : SingleRooms_question
         */

    }

    class AroundTownBranchDecision : BranchDecision {

        public AroundTownBranchDecision(Controller ctrl) : base(ctrl) { }

        override
        protected void SetThresholds() {
            //customize thresholds        
        }

        override
       public double ProbabilityOffset(Dictionary<string, double> copy) {
            if (IsNeutral) return 0;
            double total = 0;
            ReduceAttributeValue(copy);
            //total = ProbabilityOffsetSingle(copy, Constants.DISGUST, total, GameConstants.DISGUST_OFFSET);
            //total = ProbabilityOffsetSingle(copy, Constants.FRIEND, total, GameConstants.FREIND_OFFSET);
            //total = ProbabilityOffsetSingle(copy, Constants.RESPECT, total, GameConstants.RESPECT_OFFSET);
            return total;
        }

        override
        public List<string> PickAtttibutes(Dictionary<string, double> copy, double max) {
            List<string> sort = new List<string>();
            if (IsNeutral) {
                sort.Add(Kati.Constants.NEUTRAL);
                return sort;
            }
            copy = new Dictionary<string, double>();
            //copy[Constants.RESPECT] = Ctrl.Npc.InitiatorsTone[Constants.RESPECT];
            //copy[Constants.FRIEND] = Ctrl.Npc.InitiatorsTone[Constants.FRIEND];
            //copy[Constants.DISGUST] = Ctrl.Npc.InitiatorsTone[Constants.DISGUST];

            List<string> temp = SortAttributes(copy);
            string item = "";
            for (int i = 0; i < temp.Count; i++) {
                item += temp[i] + ": " + Ctrl.Npc.InitiatorsTone[temp[i]] + "\n";
            }
            return temp;
        }

        //sort high to low
        private List<string> SortAttributes(Dictionary<string, double> tone) {
            List<string> items = new List<string>();
            List<Dictionary<string, double>> temp = new List<Dictionary<string, double>>();
            double max = 0;
            string key = "";
            int count = 0;
            List<string> keys = new List<string>();
            foreach (KeyValuePair<string, double> item in tone) {
                keys.Add(item.Key);
            }
            while (tone.Count > 0 && count < 9) {
                for (int i = 0; i < keys.Count; i++) {
                    if (tone[keys[i]] >= max) {
                        max = tone[keys[i]];
                        key = keys[i];
                    }
                }
                items.Add(key);
                max = 0;
                tone.Remove(key);
                count++;
                keys.Remove(key);
            }
            return items;
        }

        override
        protected double DisgustRule(Dictionary<string, double> tone) {
            return tone[Kati.Constants.DISGUST];
        }

        override
        protected double RespectRule(Dictionary<string, double> tone) {
            return tone[Kati.Constants.RESPECT];
        }

        override
        protected double FriendRule(Dictionary<string, double> tone) {
            return tone[Kati.Constants.FRIEND];
        }
    }

}
