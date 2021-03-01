using System;
using System.Collections.Generic;
using TextGameDemo.JSON_Files;
using Kati.Module_Hub;
using Kati.GenericModule;
using TextGameDemo.Game;
using TextGameDemo.Game.Characters;
using Kati;

namespace TextGameDemo.Modules {

    //FightingWords
    public class FightingWords : Module {

        const string ARGUMENT = "argument";
        const string RIVAL_SPAT = "rivalSpat";
        const string HATRED = "hatred";
        const string IS_HURT = "isHurt";

        private string[] topics = { ARGUMENT, RIVAL_SPAT, HATRED, IS_HURT };
        private FightingWordsParser parse;

        private GameModel model;

        public GameModel Model { get => model; set => model = value; }
        private string character;

        public FightingWords(string path, GameModel model) : base(JsonToolkit.AROUND_TOWN, path) {
            FightingWordsParser parse = new FightingWordsParser(Ctrl);
            parse.SetGameRules(model.GameData);
            parse.SetLocationRules(model.Player);
            Ctrl.Parser = parse;
            Ctrl.Game = model.GameData;
            FightingWordsResponse res = new FightingWordsResponse();
            Ctrl.Parser.Response = res;
            Model = model;
            this.parse = parse;
        }

        override
        public DialoguePackage Run() {
            if (parse.HRules == null)
                parse.SetHistoryRules(Model.Connect.GameHistory);
            SetupController();
            if (Ctrl.Topic.Topic == "") {
                // fix topics issue with annoying character
                // there should be specific or no dialogue here 
            }
            if (Ctrl.Package.IsChain)
                RunChain();
            else if (Ctrl.Package.IsResponse)
                RunResponse();
            else
                Ctrl.RunParser();
            return Ctrl.Package;
        }

        //response
        public void RunResponse() {
            //OrderRelationshipBranches(ref package, npc_tones);
            DialoguePackage p = Ctrl.Package;
            Ctrl.Parser.Response.OrderRelationshipBranches(ref p, Ctrl.Npc.InitiatorsTone);
            //pull response data from library
            var data = Ctrl.Lib.DeepCopyDictionaryByTopic(Ctrl.Package.NextTopic, Ctrl.Package.NextType);
            //find best response options
            Ctrl.Parser.Response.ParseResponses(data);
            //get response options
            var respond = Ctrl.Parser.Response.Responses;

            LoadResponsesIntoPackage(respond);
        }

        public void LoadResponsesIntoPackage(List<Dictionary<string, Dictionary<string, List<string>>>> resp) {
            Ctrl.Package.Response = new List<string>();
            for (int i = 0; i < resp.Count; i++) {
                foreach (KeyValuePair<string, Dictionary<string, List<string>>> item in resp[i]) {
                    //adding dialogue only.  loosing leads to and req
                    Ctrl.Package.Response.Add(item.Key);
                }
            }
        }

        override
        public void SetCurrentCharacter(string character) {
            this.character = character;
            FightingWordsParser v = (FightingWordsParser)Ctrl.Parser;
            v.Npc = Model.Lib.Lib[character];
        }

        public void SetupController() {
            Ctrl.Package = Game.DialoguePackageHandler.Get();
            Ctrl.Topic.Topic = SetTopic();
            Ctrl.Type.Type = SetType(Ctrl.Package, Ctrl.Topic.Topic);
        }

        public string SetTopic() {
            string topic = "";
            var stats = Model.Lib.Lib[Cast.PLAYER].BranchAttributes[this.character];
            int positive = stats[Constants.FRIEND] + stats[Constants.ROMANCE] + stats[Constants.PROFESSIONAL] + stats[Constants.AFFINITY] + stats[Constants.RESPECT];
            int negative = stats[Constants.DISGUST] + stats[Constants.HATE] + stats[Constants.RIVALRY];
            if (Math.Abs(positive - negative) < 100) {
                topic = IS_HURT;
            } else if (stats[Constants.DISGUST] + 50 < stats[Constants.RIVALRY] && 
                stats[Constants.HATE] + 50 < stats[Constants.RIVALRY] && stats[Constants.RIVALRY] > 100) {
                topic = RIVAL_SPAT;
            } else if (stats[Constants.DISGUST] < 500 && stats[Constants.HATE] < 500 && stats[Constants.RIVALRY] < 500) {
                topic = ARGUMENT;
            } else {
                topic = HATRED;
            }
            Model.GameData.IncrementConversationCounter(character);
            //int value = Model.GameData.GetConversationCounter(character);
            return topic;
        }

        public string SetType(DialoguePackage pack, string topic) {
            string type = "";
            if (pack != null && pack.Type == Constants.RESPONSE) {
                type = Kati.Constants.RESPONSE;
            } else if (!topic.Equals("")) {
                if (GameTools.Tools().Next(10) > 7 && topic.Equals(ARGUMENT) || topic.Equals(RIVAL_SPAT)) {
                    type = Kati.Constants.QUESTION;
                } else {
                    type = Kati.Constants.STATEMENT;
                }
            }
            return type;
        }


        //run chain
        public void RunChain() {
            //int key = Ctrl.Package.Type == "statement" ? 0 : Ctrl.Package.Type == "question" ? 1 : 2;
            var temp = Ctrl.Lib.DeepCopyDictionaryByTopic(Ctrl.Package.NextTopic, Ctrl.Package.NextType);
            var temp2 = new Dictionary<string, Dictionary<string, List<string>>>();
            bool found = false;
            foreach (KeyValuePair<string, Dictionary<string, List<string>>> item in temp[Ctrl.Package.NextTone]) {
                foreach (string value in temp[Ctrl.Package.NextTone][item.Key]["req"]) {
                    //Console.WriteLine(value + " : " + Ctrl.Package.NextReq);
                    if (value.Equals(Ctrl.Package.NextReq)) {
                        temp2[item.Key] = temp[Ctrl.Package.NextTone][item.Key];
                        found = true; 
                        break;
                    }
                }
                if (found)
                    break;
            }
            if (found) {
                parse.SetPackage(ref temp2);
                var p = DialoguePackageHandler.Get();
                p = Ctrl.Package;
                parse.LeadTo.ParseLeadTo(temp2, p, Ctrl, Ctrl.Package.NextTone);
                parse.SetPackage(ref temp2);
            } else {
                Ctrl.Package.NotAChain();
            }
        }

    }

    class FightingWordsParser : Parser {

        FightingWordsPersonalRules personal;
        FightingWordsGameRules gRules;
        FightingWordsHistoryRules hRules;
        FightingWordsLocationRules local;
        FightingWordsLeadToRules leadTo;
        Game.Characters.Character npc;
        GameModel model;

        public FightingWordsPersonalRules Personal { get => personal; set => personal = value; }
        public Character Npc { get => npc; set => npc = value; }
        public FightingWordsGameRules GRules { get => gRules; set => gRules = value; }
        public GameModel Model { get => model; set => model = value; }
        public FightingWordsHistoryRules HRules { get => hRules; set => hRules = value; }
        internal FightingWordsLeadToRules LeadTo { get => leadTo; set => leadTo = value; }

        public FightingWordsParser(Controller ctrl) : base(ctrl) {
            Personal = new FightingWordsPersonalRules(ctrl);
            LeadTo = new FightingWordsLeadToRules();
            //GRules = new GameRules(Model);
        }

        public void SetHistoryRules(History h) {
            HRules = new FightingWordsHistoryRules(h);
        }

        public void SetGameRules(Game.GameData game) {
            GRules = new FightingWordsGameRules(game);
        }
        public void SetLocationRules(Player local) {
            this.local = new FightingWordsLocationRules(local);
        }

        override
        public void Parse() {
            //Data must not point to Lib data but be a copy
            (string tone, var data) = Branch.RunDecision(Data);
            data = ParseHelper(data, tone);
            //if branch doesn't contain data move to neutral
            if (data == null) {
                //data = Ctrl.Lib.DeepCopyDictionaryByTopic(Ctrl.Topic.Topic, Ctrl.Lib.GetType(Ctrl.Type.Type))[Kati.Constants.NEUTRAL];
                //ParseHelper(data,tone);
                Console.WriteLine("Data is null in parser");
            }
        }

        private Dictionary<string, Dictionary<string, List<string>>> ParseHelper(Dictionary<string, Dictionary<string, List<string>>> data, string tone) {
            try {
                data = HRules.ParseHistoryRequirments(data, Npc.Name);
                data = GRules.ParseGameRequirments(data);
                data = Personal.ParsePersonalRequirments(data, Npc);
                data = Social.ParseSocialRequirments(data);
                data = LeadTo.DialogueChainReqRules(data, Ctrl.Package);
                data = GetDialogueWeights(data);
                HRules.AddPlayOnceDialogue(data, Npc.Name);
                LeadTo.ParseLeadTo(data, Ctrl.Package, Ctrl, tone);
                SetPackage(ref data);
            } catch (Exception e) {
                Console.WriteLine(e);
                data = null;
            }
            return data;
        }

        public Dictionary<string, Dictionary<string, List<string>>> GetDialogueWeights
            (Dictionary<string, Dictionary<string, List<string>>> data) {
            int count = GameTools.Tools().Next(data.Count);
            int counter = 0;
            Dictionary<string, Dictionary<string, List<string>>> temp = new Dictionary<string, Dictionary<string, List<string>>>();
            foreach (KeyValuePair<string, Dictionary<string, List<string>>> item in data) {
                if (count == counter) {
                    temp[item.Key] = data[item.Key];
                    break;
                }
                counter++;
            }
            return temp;
        }

    }

    class FightingWordsResponse : Response {

        override
       public Dictionary<string, Dictionary<string, List<string>>> CheckRequirements
           (Dictionary<string, Dictionary<string, List<string>>> data) {
            if (Package == null)
                return data;
            //var copy = DeepCopy(data);
            var copy = new Dictionary<string, Dictionary<string, List<string>>>();
            string rule = ParseReq(Package.NextReq);

            foreach (KeyValuePair<string, Dictionary<string, List<string>>> item in data) {
                foreach (string req in data[item.Key]["req"]) {
                    string[] arr = req.Split('.');
                    if (arr.Length > 1) {
                        if (arr[1].Equals(rule)) {
                            copy[item.Key] = data[item.Key];
                        }
                    }
                }
            }
            return copy;
        }

        private string ParseReq(string req) {
            string rule = "";
            if (Package.NextReq.Length > 0) {
                //string[] arr = Package.NextReq.Split(".");
                //if (arr.Length > 1)
                rule = req;
            }
            return rule;
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

    class FightingWordsPersonalRules {

        Controller ctrl;

        public FightingWordsPersonalRules(Controller ctrl) {
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

        private bool CharacterNameRule(List<string> req, Character npc) {
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

    class FightingWordsGameRules {

        Game.GameData game;

        public FightingWordsGameRules(Game.GameData game) {
            this.game = game;
        }

        public Dictionary<string, Dictionary<string, List<string>>> ParseGameRequirments
                                           (Dictionary<string, Dictionary<string, List<string>>> data) {
            var temp = new Dictionary<string, Dictionary<string, List<string>>>();
            foreach (KeyValuePair<string, Dictionary<string, List<string>>> item in data) {
                bool keep = GameReq(data[item.Key]["req"]);
                if (keep) {
                    temp[item.Key] = data[item.Key];
                }
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


        /* req rules
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

    class FightingWordsLocationRules {

        Player local;

        public FightingWordsLocationRules(Player local) {
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
                        (string area, string room) = local.Locations.GetLocation();
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

    class FightingWordsHistoryRules {

        private History h;
        private Dictionary<string, Dictionary<string, bool>> playOnce;

        public FightingWordsHistoryRules(History h) {
            this.h = h;
            playOnce = new Dictionary<string, Dictionary<string, bool>>();
        }

        public Dictionary<string, Dictionary<string, List<string>>> ParseHistoryRequirments
                                       (Dictionary<string, Dictionary<string, List<string>>> data, string name) {
            var temp = new Dictionary<string, Dictionary<string, List<string>>>();
            foreach (KeyValuePair<string, Dictionary<string, List<string>>> item in data) {
                bool keep = ShortTermHistoryRule(item.Key, name) && LongTermHistory(item.Key, data[item.Key]["req"], name);
                if (keep)
                    temp[item.Key] = data[item.Key];
            }
            return temp;
        }

        private bool ShortTermHistoryRule(string dialogue, string name) {
            bool keep = true;
            if (h.ShortTerm.ContainsKey(name)) {
                foreach (string d in h.ShortTerm[name]) {
                    if (dialogue.Equals(d)) {
                        keep = false; break;
                    }
                }
            }
            return keep;
        }

        private bool LongTermHistory(string dialogue, List<string> req, string name) {
            bool keep = true;
            if (!playOnce.ContainsKey(name))
                playOnce[name] = new Dictionary<string, bool>();
            foreach (string item in req) {
                if (item.Equals("play_once") && playOnce[name].ContainsKey(dialogue)) {
                    Console.WriteLine(name + ":" + dialogue);
                    keep = false;
                    break;
                }
            }
            return keep;
        }

        public void AddPlayOnceDialogue(Dictionary<string, Dictionary<string, List<string>>> data, string name) {
            foreach (KeyValuePair<string, Dictionary<string, List<string>>> item in data) {
                if (!playOnce.ContainsKey(name))
                    playOnce[name] = new Dictionary<string, bool>();
                foreach (string value in data[item.Key]["req"]) {
                    //Console.WriteLine(name + ":" + item.Key);
                    if (value.Equals("play_once") && !playOnce[name].ContainsKey(item.Key)) {
                        //Console.WriteLine(name + ":" + item.Key);
                        playOnce[name][item.Key] = true;
                        break;
                    }
                }
            }
        }


    }

    class FightingWordsLeadToRules {

        public FightingWordsLeadToRules() { }

        //play first
        public Dictionary<string, Dictionary<string, List<string>>> DialogueChainReqRules
            (Dictionary<string, Dictionary<string, List<string>>> data, DialoguePackage package) {
            var temp = new Dictionary<string, Dictionary<string, List<string>>>();
            if (package.IsChain) {
                // continue the chain by checking lead to
            } else {
                foreach (KeyValuePair<string, Dictionary<string, List<string>>> item in data) {
                    foreach (string req in data[item.Key]["req"]) {
                        var arr = req.Split("_");
                        if (arr.Length > 0) {
                            // only keep options that don't have chainRules 
                            if (CheckRule(arr[0]))
                                temp[item.Key] = data[item.Key];
                        }
                    }
                    if (data[item.Key]["req"].Count == 0)
                        temp[item.Key] = data[item.Key];
                }
            }
            return temp;
        }

        private bool CheckRule(string req) {
            bool keep = true;
            switch (req) {
                case "village": { keep = false; } break;
                case "Smith": { keep = false; } break;
                case "lesson": { keep = false; } break;
                case "friend": { keep = false; } break;
            }
            return keep;
        }

        //toggle dialogue chain on and off
        public void ParseLeadTo(Dictionary<string, Dictionary<string, List<string>>> data, DialoguePackage package, Controller ctrl, string tone) {
            //should only ever run once
            foreach (KeyValuePair<string, Dictionary<string, List<string>>> item in data) {
                foreach (string lead in data[item.Key]["lead to"]) {
                    var arr = lead.Split(".");
                    if (arr.Length > 0) {
                        if (LeadToAction(lead, arr[0], package, ctrl, tone))
                            return;
                    }
                }
                if (data[item.Key]["lead to"].Count == 0) {
                    package.NotAChain();
                }
            }
        }
        //toggle dialogue chain on and off
        private bool LeadToAction(string lead, string command, DialoguePackage package, Controller ctrl, string tone) {
            bool isAction = true;
            var arr = lead.Split(".");
            if (arr.Length >= 2)
                lead = arr[1];
            //Console.WriteLine(lead);
            switch (command) {
                case "end_conversation": { Console.WriteLine("end conversation"); } break;
                case "response": { package.SetForResponse(ctrl.Topic.Topic, "response", tone, lead); } break;
                case "argument_statement": { package.SetForChain(ctrl.Topic.Topic, "statement", tone, lead); } break;
                default: { isAction = false; package.NotAChain(); package.NotAResponse(); } break;
            }
            return isAction;
        }
        /* lead to
                end_conversation : SingleRooms_question
                response.weather.nice_day : Greeting_question
                response.status : Greeting_question
                response.like_town : SingleRooms_question
                response.like_town_center : SingleRooms_question
                response.like_store : SingleRooms_question
                response.like_blacksmith : SingleRooms_question
                response.entered_forest : SingleRooms_question

                SingleRooms_statement.village_a1 : SingleRooms_statement
                SingleRooms_statement.village_a2 : SingleRooms_statement
                SingleRooms_statement.village_b1 : SingleRooms_statement
                SingleRooms_statement.village_b2 : SingleRooms_statement
                SingleRooms_statement.Smith_a1 : SingleRooms_statement
                
                HistoryLesson_statement.lesson_a1 : HistoryLessons_statement
                HistoryLesson_statement.lesson_a2 : HistoryLessons_statement
                HistoryLesson_statement.lesson_a3 : HistoryLessons_statement
                HistoryLesson_statement.lesson_b1 : HistoryLessons_statement
                HistoryLesson_statement.lesson_c1 : HistoryLessons_statement
                HistoryLesson_statement.lesson_c2 : HistoryLessons_statement
                HistoryLesson_statement.lesson_d1 : HistoryLessons_statement
                HistoryLesson_statement.lesson_e1 : HistoryLessons_statement
                HistoryLesson_statement.lesson_f1 : HistoryLessons_statement
                HistoryLesson_statement.lesson_g1 : HistoryLessons_statement
                HistoryLesson_statement.lesson_g2 : HistoryLessons_statement
                HistoryLesson_statement.lesson_h1 : HistoryLessons_statement
                HistoryLesson_statement.lesson_h2 : HistoryLessons_statement
                HistoryLesson_statement.lesson_h3 : HistoryLessons_statement
                HistoryLesson_statement.lesson_h4 : HistoryLessons_statement
                HistoryLesson_statement.lesson_h5 : HistoryLessons_statement
                HistoryLesson_statement.lesson_h6 : HistoryLessons_statement
                HistoryLesson_statement.lesson_h7 : HistoryLessons_statement
                HistoryLesson_statement.lesson_romance_a1 : HistoryLessons_statement
                HistoryLesson_statement.friend_a1 : HistoryLessons_statement
                HistoryLesson_statement.friend_a2 : HistoryLessons_statement
                HistoryLesson_statement.friend_a3 : HistoryLessons_statement
                HistoryLesson_statement.friend_b1 : HistoryLessons_statement
              
         */
    }

    class FightingWordsBranchDecision : BranchDecision {



        public FightingWordsBranchDecision(Controller ctrl) : base(ctrl) { }

        override
        protected void SetThresholds() {
            //customize thresholds        
        }

        override
       public double ProbabilityOffset(Dictionary<string, double> copy) {
            if (IsNeutral) return 0;
            double total = 0;
            ReduceAttributeValue(copy);
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
