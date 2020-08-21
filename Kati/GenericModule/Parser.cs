using System;
using System.Collections.Generic;
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

        public string Topic { get => topic; set => topic = value; }
        public string Type { get => type; set => type = value; }
        public Controller Ctrl { get => ctrl; set => ctrl = value; }
        public Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>> Data { get => data; set => data = value; }
        public BranchDecision Branch { get => branch; set => branch = value; }
        public GameRules Game { get => game; set => game = value; }
        public PersonalCharacterRules Personal { get => personal; set => personal = value; }
        public SocialCharacterRules Social { get => social; set => social = value; }
        public DialogueWeigthRules Weight { get => weight; set => weight = value; }

        public Parser(Controller ctrl) {
            Ctrl = ctrl;
            Branch = new BranchDecision(Ctrl);
            Game = new GameRules(Ctrl);
            Personal = new PersonalCharacterRules(Ctrl);
            Social = new SocialCharacterRules(Ctrl);
            Weight = new DialogueWeigthRules(Ctrl);
        }

        public void Setup(string topic, string type,
            Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>> data) {
            Topic = topic;
            Type = type;
            Data = data;
        }

        //used for statements and questions only
        public void Parse() {
            //Data most not point to Lib data but be a copy
            var data = Branch.RunDecision(Data);
            data = Game.ParseGameRequirments(data);
            data = Personal.ParsePersonalRequirments(data);
            data = Social.ParseSocialRequirments(data);
            data = Weight.GetDialogue(data);
            //package data
        }

    }
}
