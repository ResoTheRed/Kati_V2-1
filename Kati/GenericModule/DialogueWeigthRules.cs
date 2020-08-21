using System;
using System.Collections.Generic;

namespace Kati.GenericModule {
    
    /// <summary>
    /// Class weighs all dialogue bits based on requirements and returns
    /// one of them
    /// </summary>
    
    public class DialogueWeigthRules {

        private Controller ctrl;
        private Dictionary<string, Dictionary<string, List<string>>> data;
        private double publicEvent = 20;
        private double trigger = 100;
        private double personalTrait = 40;
        private double personalStatus = 80;
        private double personalInterest = 20;
        private double personalFeature = 30;
        private double socialDirected = 50;
        private double socialRelationship = 25;
        private double baseValue = 50;

        public DialogueWeigthRules(Controller ctrl) {
            Ctrl = ctrl;
        }

        public Controller Ctrl { get => ctrl; set => ctrl = value; }
        public Dictionary<string, Dictionary<string, List<string>>> Data { get => data; set => data = value; }
        protected double PublicEvent { get => publicEvent; set => publicEvent = value; }
        protected double PersonalTrait { get => personalTrait; set => personalTrait = value; }
        protected double PersonalStatus { get => personalStatus; set => personalStatus = value; }
        protected double PersonalInterest { get => personalInterest; set => personalInterest = value; }
        protected double SocialDirected { get => socialDirected; set => socialDirected = value; }
        protected double SocialRelationship { get => socialRelationship; set => socialRelationship = value; }
        protected double Trigger { get => trigger; set => trigger = value; }
        public double PersonalFeature { get => personalFeature; set => personalFeature = value; }

        public Dictionary<string, Dictionary<string, List<string>>> GetDialogue
                            (Dictionary<string, Dictionary<string, List<string>>> data) {
            Data = data;
            Dictionary<string, double> weights = new Dictionary<string, double>();
            ConvertToBaseWeights(ref weights);
            Dictionary<string, Dictionary<string, List<string>>> temp = 
                new Dictionary<string, Dictionary<string, List<string>>>();
            temp[ChooseDialogue(ref weights)] = Data[ChooseDialogue(ref weights)];
            return temp;
        }

        public Dictionary<string, double> ConvertToBaseWeights(ref Dictionary<string, double> weights) {
            foreach (KeyValuePair<string, Dictionary<string, List<string>>> item1 in Data) {
                weights[item1.Key] = 50;
            }
            ParseIndividualDialogueRules(ref weights);
            return weights;
        }

        protected void ParseIndividualDialogueRules(ref Dictionary<string, double> weights) {
            foreach (KeyValuePair<string, Dictionary<string, List<string>>> item1 in Data) {
                double total = 0;
                foreach (string s in Data[item1.Key]["req"]) {
                    double value = RuleDirectoryTop(s);
                    if (value > total) {
                        total = value;
                    }
                }
                weights[item1.Key] += total;
            }
        }

        public double RuleDirectoryTop(string rule) {
            string[] arr = rule.Split(".");
            if (arr.Length < 3)
                return 0;
            switch (arr[0]) {
                case "personal": { return RuleDirectoryPersonal(ref arr); }
                case "social": { return RuleDirectorySocial(ref arr); }
                case "game": { return RuleDirectoryGame(ref arr); }
                default: return 0;
            }
        }

        protected double RuleDirectoryGame(ref string[] arr) {
            switch (arr[1]) {
                case "publicEvent": { return PublicEvent; }
                case "trigger": { return Trigger; }
                default: return 0;
            }
        }

        protected double RuleDirectorySocial(ref string[] arr) {
            switch (arr[1]) {
                case "attribute": { return HandleScalar(arr); }
                case "directed": { return SocialDirected; }
                case "relationship": { return SocialRelationship; }
                default: return 0;
            }
        }

        protected double RuleDirectoryPersonal(ref string[] arr) {
            switch (arr[1]) {
                case "trait": return PersonalTrait;
                case "interest": return personalInterest;
                case "status": return PersonalStatus;
                case "physicalFeature": return personalFeature;
                case "scalarTrait": return HandleScalar(arr);
                default: return 0;
            }
        }

        protected double HandleScalar(string[] arr) {
            double value = 0;
            try {
                value = (double)Int32.Parse(arr[arr.Length - 1]);
            } catch (Exception) { }
            return value;
        }

        public string ChooseDialogue(ref Dictionary<string, double> weights) {
            (double total, List<string> keys)= GetTotal(ref weights);
            int threshold = (int)(Controller.dice.NextDouble() * total);
            string dialogue = null;
            for (int i = 0; i < keys.Count; i++) {
                if (weights[keys[i]] >= threshold) {
                    dialogue = keys[i];
                    break;
                }
            }
            if (dialogue == null)
                dialogue = keys[(int)(Controller.dice.NextDouble()*keys.Count)];
            return dialogue;
        }

        protected (double, List<string>) GetTotal(ref Dictionary<string, double> weights) {
            List<string> keys = new List<string>();
            foreach (KeyValuePair<string, double> s in weights) {
                keys.Add(s.Key);
            }
            for (int i = 0; i < weights.Count; i++) {
                if (i > 0) {
                    weights[keys[i]] += weights[keys[i - 1]];
                }
            }
            return (weights[keys[keys.Count - 1]], keys);
        }

    }
}
