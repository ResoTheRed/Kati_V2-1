using Kati.Module_Hub;
using System;
using System.Collections.Generic;

namespace Kati.GenericModule {
    /// <summary>
    /// Generic parent of all Module Controllers
    /// </summary>
    public class Controller {

        public static Random dice = new Random();

        private ModuleLib lib;
        private GameData game;
        private CharacterData npc;
        private Parser parser;
        private DeviseTopic topic;
        private DeviseType type;
        private DialoguePackage package;

        public Controller(string path) {
            Lib = new ModuleLib(path);
            Game = new GameData();
            Npc = CharacterData.GetCharacterData();
            parser = new Parser(this);
            topic = new DeviseTopic(Lib.GetTopicKeys());
            Type = new DeviseType();
        }

        public ModuleLib Lib { get => lib; set => lib = value; }
        public GameData Game { get => game; set => game = value; }
        public CharacterData Npc { get => npc; set => npc = value; }
        public Parser Parser { get => parser; set => parser = value; }
        public DeviseTopic Topic { get => topic; set => topic = value; }
        public DeviseType Type { get => type; set => type = value; }
        public DialoguePackage Package { get => package; set => package = value; }

        //need to update game data
        //need to update character data
        //call everytime a new dialogue bit is required
        public void Update(ref GameData game, ref CharacterData character) {
            Game = game;
            Npc = character;
        }

        public void ParseDialoguePacket(DialoguePackage package) {
            Package = package;//might need to reset this after Topic and Type are defined
            //check for Forced Topics
            DefineTopic(Package.ForcedTopic);
            //check for Forced Types
            DefineType(Package.ForcedType);
        }

        //broken -> does not full function
        public void RunParser() {
            string key = Topic.Topic + "_" + Type.Type;
            Console.WriteLine("null lib: "+Lib.DeepCopyDictionaryByTopic(Topic.Topic, Lib.GetType(Type.Type))==null);
            
            Parser.Setup(Topic.Topic, Type.Type, Lib.DeepCopyDictionaryByTopic(Topic.Topic,Lib.GetType(Type.Type)));
            //Parser.Setup(Topic.Topic, Type.Type, Lib.Data[key]);
            Parser.Parse();
        }

        //need to decide which topic
        protected void DefineTopic(Dictionary<string, double> forced) {
            if (forced.Count == 0) {
                Topic.GetTopic();
            } else if (forced.Count == 1) {
                foreach (KeyValuePair<string, double> item in forced) {
                    Topic.SetSingleTopicWeight(item.Key, item.Value);
                    Topic.ForcedTopic(item.Key);
                }
            } else {
                Topic.SetMultiWeights(forced);
            }
        }

        public void DefineType(Dictionary<string, double> forced) {
            if (forced != null && forced.Count >= 1) {
                if (forced.ContainsKey(Constants.STATEMENT))
                    Type.SetWeights(forced[Constants.STATEMENT], null);
                if (forced.ContainsKey(Constants.QUESTION))
                    Type.SetWeights(null, forced[Constants.QUESTION]);
            } else{ //if (forced.Count > 1) {//******************************************Something is not right here************************************************
                try {
                    Type.SetWeights(forced[Constants.STATEMENT], forced[Constants.QUESTION]);
                } catch (Exception) { }
            }
            Type.GetTopicType();
        }
        //need to decide which type of topic 


    }
}
