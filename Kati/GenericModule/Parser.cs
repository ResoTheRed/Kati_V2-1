using Kati.Module_Hub;
using Kati.SourceFiles;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace Kati.GenericModule {
    public class Parser {

        private Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>> data;
        private string topic;//main topic "dreams","weather"...
        private string type;//Statement or Question

        private Controller ctrl;
        private BranchDecision branch;
        private GameRules game;
        private PersonalCharacterRules personal;
        private SocialCharacterRules social;
        private DialogueWeigthRules weight;
        private Response response;

        public string Topic { get => topic; set => topic = value; }
        public string Type { get => type; set => type = value; }
        public Controller Ctrl { get => ctrl; set => ctrl = value; }
        public Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>> Data { get => data; set => data = value; }
        public BranchDecision Branch { get => branch; set => branch = value; }
        public GameRules Game { get => game; set => game = value; }
        public PersonalCharacterRules Personal { get => personal; set => personal = value; }
        public SocialCharacterRules Social { get => social; set => social = value; }
        public DialogueWeigthRules Weight { get => weight; set => weight = value; }
        public Response Response { get => response; set => response = value; }

        public Parser(Controller ctrl) {
            Ctrl = ctrl;
            Branch = new BranchDecision(Ctrl);
            Game = new GameRules(Ctrl);
            Personal = new PersonalCharacterRules(Ctrl);
            Social = new SocialCharacterRules(Ctrl);
            Weight = new DialogueWeigthRules(Ctrl);
            Response = new Response();
        }

        public void Setup(string topic, string type,
            Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>> data) {
            Topic = topic;
            Type = type;
            Data = data;
        }

        //used for statements and questions only
        virtual public void Parse() {
            //Data must not point to Lib data but be a copy
            var data = Branch.RunDecision(Data);
            data = Game.ParseGameRequirments(data);
            data = Personal.ParsePersonalRequirments(data);
            data = Social.ParseSocialRequirments(data);
            data = Weight.GetDialogue(data);
            SetPackage(ref data);
        }

        public void RunResponse() { 
        
        }

        //define this method 
        protected void SetPackage(ref Dictionary<string, Dictionary<string, List<string>>> data) {
            Ctrl.Package.Module = "JobInterviewModule";
            //Ctrl.Package.Speaker = (Type.Equals(Constants.RESPONSE)) ? Constants.RESPONDER : Constants.INITIATOR;
            
            foreach (KeyValuePair<string, Dictionary<string, List<string>>> item in data) {
                if (!Type.Equals(Constants.RESPONSE))
                    Ctrl.Package.Dialogue = item.Key;
                else
                    Ctrl.Package.Response.Add(item.Key);
                Ctrl.Package.Req[item.Key] = data[item.Key][Constants.REQ];
                Ctrl.Package.LeadTo[item.Key] = data[item.Key][Constants.LEAD_TO];
            }

        }


    }
}
