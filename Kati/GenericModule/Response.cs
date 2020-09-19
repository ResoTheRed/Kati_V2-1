using Kati.Module_Hub;
using Kati.SourceFiles;
using System;
using System.Collections.Generic;
using System.Windows.Markup;

namespace Kati.GenericModule {

    /// <summary>
    /// Provides four responses based on character stats
    /// </summary>

    public class Response {
        
        //TODO:
        //type must be response
        //match lead to to req
        //find out the relationship status from the NPCs point of view
        //provide one positive+ or positive
        //provide one neutral 
        //provide one negative or negative+
        //provide one based on relationship
        //default each type to relationship based
        //if there is no relationship base then default to neutral
        //provide a set of default neutral responses for any situation

        private Dictionary<string, double> branchValues;
        private List<string> orderedBranches;
        private string relationsip;
        private bool plusFlag;
        private List<Dictionary<string, Dictionary<string, List<string>>>> responses;
        private List<Dictionary<string, Dictionary<string, List<string>>>> applicableResponses;
        private DialoguePackage package;

        protected double responseBiasWeight = 70;
        protected double responseTotalWeight = 100;


        public Response() {
            OrderedBranches = new List<string>();
            BranchValues = new Dictionary<string, double>();
            Relationship = Constants.NEUTRAL;
            Responses = new List<Dictionary<string, Dictionary<string, List<string>>>>();
            PlusFlag = false;
        }

        public string Relationship { get => relationsip; set => relationsip = value; }
        public List<string> OrderedBranches { get => orderedBranches; set => orderedBranches = value; }
        public bool PlusFlag { get => plusFlag; set => plusFlag = value; }
        public Dictionary<string, double> BranchValues { get => branchValues; set => branchValues = value; }
        public DialoguePackage Package { get => package; set => package = value; }
        public List<Dictionary<string, Dictionary<string, List<string>>>> Responses { get => responses; set => responses = value; }
        public List<Dictionary<string, Dictionary<string, List<string>>>> ApplicableResponses { get => applicableResponses; set => applicableResponses = value; }


        /**************************** Setup for Branch Defined Response*******************************/

        //pull branches from BranchDecision CancelAttributes method
        public List<string> OrderRelationshipBranches(ref DialoguePackage package, Dictionary<string, double> branches) {
            BranchValues = branches;
            Package = package;
            Responses = new List<Dictionary<string, Dictionary<string, List<string>>>>();//reset responses each time
            List<string> sorted = new List<string>();
            foreach (KeyValuePair<string, double> item in branches) {
                sorted.Add(item.Key);
            }
            //sort
            sorted = SortBranchesByAttribute(ref branches, ref sorted);
            //set Plus Flag and relationship
            bool nonNeutral = SetPlusFlag(ref branches, ref sorted);
            //set relationship type
            SetRelationship(sorted[0], nonNeutral);
            return sorted;
        }

        protected void SetRelationship(string value, bool nonNeutral) {
            if (!nonNeutral) {
                Relationship = Constants.NEUTRAL;
            } else {
                Relationship = value;
            }
        }

        protected bool AttributeRelationshipIsNegative() {
            return Relationship.Equals(Constants.DISGUST) ||
                   Relationship.Equals(Constants.HATE) ||
                   Relationship.Equals(Constants.RIVALRY);
        }

        //set plus flag to signal the attribute defined response to be a plus version
        protected bool SetPlusFlag
            (ref Dictionary<string, double> branches, ref List<string> sorted) {
            bool nonNeutral = false;
            bool plusReset = true;
            for (int k = 0; k < sorted.Count; k++) {
                if (branches[sorted[k]] >= Constants.RESPONSE_NEUTRAL_THRESHOLD) {
                    nonNeutral = true;
                    if (branches[sorted[k]] >= Constants.RESPONSE_PLUS_THRESHOLD) {
                        plusFlag = true;
                        plusReset = false;
                    }
                }
            }
            if (plusReset)
                PlusFlag = false;
            return nonNeutral;
        }

        //temp contains branch keys, branches stems from CancelAtrribute method in BranchDecision
        protected  List<string> SortBranchesByAttribute
            (ref Dictionary<string, double> branches, ref List<string> temp) {
            bool sort;
            do {
                sort = false;
                for (int j = 1; j < temp.Count; j++) {
                    if (branches[temp[j - 1]] < branches[temp[j]]) {
                        string key = temp[j];
                        temp[j] = temp[j - 1];
                        temp[j - 1] = key;
                        sort = true;
                    }
                }
            } while (sort);
            return temp;
        }

        /*************************************Parse Responses*****************************************/
        //pull response data from Parser
        public void ParseResponses
            (Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>> data) {
            Responses = new List<Dictionary<string, Dictionary<string, List<string>>>>();
            ApplicableResponses = CheckAllRequirements(ref data);
            //pull positive response
            Responses.Add(PullPositive(ref data));
            //pull neutral response
            Responses.Add(PullNeutral(ref data));
            //pull negative response
            Responses.Add(PullNegative(ref data));
            //pull custom response
            Responses.Add(PullCustom(ref data));

        }

        //rework::Remove data that doesn't meet the requirements first
        public List<Dictionary<string, Dictionary<string, List<string>>>> CheckAllRequirements
            (ref Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>> data) {
            var temp = new Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>>();
            var applicable = "";
            foreach (KeyValuePair<string, Dictionary<string, Dictionary<string, List<string>>>> item in data) {
                temp[item.Key] = new Dictionary<string, Dictionary<string, List<string>>>();
                //run for pos+, pos, neutral, neg, neg+
                temp[item.Key] = item.Value;
                temp[item.Key] = CheckRequirements(temp[item.Key]);
                Console.WriteLine("\n");
                foreach (KeyValuePair<string, Dictionary<string, List<string>>> item2 in temp[item.Key]) {
                    Console.WriteLine(item.Key + "::" + item2.Key);
                }
                Console.WriteLine("\n");
            }
            //deep copy to applicable
            data = temp;
            return DeepCopyApplicableDialogue(temp);
        }

        private List<Dictionary<string, Dictionary<string, List<string>>>> DeepCopyApplicableDialogue
            (Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>> temp) {
            var applicable = new List<Dictionary<string, Dictionary<string, List<string>>>>();
            int counter = 0;
            foreach (KeyValuePair<string, Dictionary<string, Dictionary<string, List<string>>>> item in temp) {
                foreach (KeyValuePair<string, Dictionary<string, List<string>>> item2 in temp[item.Key]) {
                    applicable.Add(new Dictionary<string, Dictionary<string, List<string>>>());
                    applicable[counter][item2.Key] = new Dictionary<string, List<string>>();
                    foreach (KeyValuePair<string, List<string>> item3 in temp[item.Key][item2.Key]) {
                        applicable[counter][item2.Key][item3.Key] = new List<string>();
                        foreach (string s in temp[item.Key][item2.Key][item3.Key]) {
                            applicable[counter][item2.Key][item3.Key].Add(s);
                        }
                    }
                }
                counter++;
            }
            return applicable;
        }

        //pulls a dialogue bit chosen from a group of applicable dialogues
        public Dictionary<string, Dictionary<string, List<string>>> AttemptToFindNonRepeatingAvailableResponse() {
            int[] indices = GenerateRandomIndices();
            string key = "";
            for (int i = 0; i < indices.Length; i++) {
                foreach (KeyValuePair<string, Dictionary<string, List<string>>> item in ApplicableResponses[indices[i]]) {
                    key = item.Key;//there should only ever be one dialogue bit at a time here.
                }
                bool keep = true;
                for (int j = 0; j < Responses.Count; j++) {
                    if (Responses[j].ContainsKey(key)) {
                        keep = false;
                    }
                }
                if (keep) {
                    return ApplicableResponses[indices[i]];
                }
            }
            return ApplicableResponses[indices[0]];
        }

        protected int[] GenerateRandomIndices() {
            int size = ApplicableResponses.Count;
            List<int> values = new List<int>();
            while (values.Count < size) {
                int num = (int)(Controller.dice.NextDouble() * size);
                if (!values.Contains(num)) {
                    values.Add(num);
                }
            }
            return values.ToArray();
        }

        



        /**
        * check to see if positive+ exists
        * check to see if positive exists
        * decide which to use with added weight if positive+ flag is true
        * if AttributeIsNegative then heavy weight on just positive
        * check each option for requirements for winning type
        * if no option exists then switch to the losing type
        * if no options exist for either use neutral
        * narrow down until one positive remains
        */
        public Dictionary<string, Dictionary<string, List<string>>> PullPositive
            (ref Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>> data) {
            string value = PickPositive(ref data);
            var positive = data[value];
            if (positive.Count == 0 && value.Equals(Constants.NEGATIVE_PLUS)) {
                positive = data[Constants.NEGATIVE];
            } else if (positive.Count == 0 && value.Equals(Constants.NEGATIVE)) {
                positive = data[Constants.NEGATIVE_PLUS];
            } else {
                positive = data[Constants.NEUTRAL];
            }
            //positive = CheckRequirements(positive);
            Dictionary<string, Dictionary<string, List<string>>> choice = 
                new Dictionary<string, Dictionary<string, List<string>>>();
            string temp = PickResponseOption(ref positive);
            try {
                choice[temp] = positive[temp];
            } catch (Exception) {
                return AttemptToFindNonRepeatingAvailableResponse();
            }
            return choice;
        }

        //70% chance for pos+ if plusFlag else 70% chance for reg pos
        //@return positive+, positive, or neutral
        public string PickPositive
            (ref Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>> data) {
            double plus = (PlusFlag && !AttributeRelationshipIsNegative()) ? 
                responseBiasWeight : responseTotalWeight - responseBiasWeight;
            double choice = Controller.dice.NextDouble() * responseTotalWeight + 1;
            if (plus >= choice && data.ContainsKey(Constants.POSITIVE_PLUS)&&data[Constants.POSITIVE_PLUS].Count>0) {
                return Constants.POSITIVE_PLUS;
            } else if (data.ContainsKey(Constants.POSITIVE) && data[Constants.POSITIVE].Count > 0) {
                return Constants.POSITIVE;
            } else {
                return Constants.NEUTRAL;
            }
        }
        
        public Dictionary<string, Dictionary<string, List<string>>> PullNeutral
            (ref Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>> data) {
            var neutral = CheckRequirements(data[Constants.NEUTRAL]);
            Dictionary<string, Dictionary<string, List<string>>> choice =
               new Dictionary<string, Dictionary<string, List<string>>>();
            string temp = PickResponseOption(ref neutral);
            try {
                choice[temp] = neutral[temp];
            } catch (Exception) {
                return AttemptToFindNonRepeatingAvailableResponse();
            }
            return choice;
        }

        protected void PickNeutral
            (ref Dictionary<string, Dictionary<string, List<string>>> neutral) {
            //remove any choices that already exist in the responses list
            RemoveDuplicates(ref neutral);
            //if neutral list is larger than 0 call pick response option
            if (neutral.Count > 0) {
                PickResponseOption(ref neutral);
            } else { //else provide a value a premade value
                ProvideNeutralResponse(ref neutral);
            }
        }

        public void ProvideNeutralResponse
            (ref Dictionary<string, Dictionary<string, List<string>>> neutral) {
            string key= "...";
            foreach (Dictionary<string, Dictionary<string, List<string>>> resp in Responses) {
                if (!resp.ContainsKey("I don't know.")) {
                    key="I don't know.";
                } else if (!resp.ContainsKey("Okay.")) {
                    key="Okay.";
                } else if (!resp.ContainsKey("Uhhh.. meh.")) {
                    key="Uhhh.. meh.";
                } 
            }
            neutral[key] = new Dictionary<string, List<string>>();
            neutral[key][Constants.REQ] = new List<string>();
            neutral[key][Constants.LEAD_TO] = new List<string>();
        }

        public void RemoveDuplicates(ref Dictionary<string, Dictionary<string, List<string>>> neutral) {
            int i = 0;
            if (Responses.Count > 0) {
                foreach (string key in Responses[i].Keys) {
                    if (neutral.ContainsKey(key)) {
                        neutral.Remove(key);
                    }
                }
            }
        }

        public Dictionary<string, Dictionary<string, List<string>>> PullNegative
            (ref Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>> data) {
            string value = PickNegative(ref data);
            var negative = data[value];
            if (negative.Count == 0 && value.Equals(Constants.NEGATIVE_PLUS)) {
                negative = data[Constants.NEGATIVE];
            } else if (negative.Count == 0 && value.Equals(Constants.NEGATIVE)) {
                negative = data[Constants.NEGATIVE_PLUS];
            } else {
                negative = data[Constants.NEUTRAL];
            } 
            Console.WriteLine(negative.Count+" ");
            negative = CheckRequirements(negative);
            Dictionary<string, Dictionary<string, List<string>>> choice =
               new Dictionary<string, Dictionary<string, List<string>>>();
            string temp = PickResponseOption(ref negative);
            try {
                choice[temp] = negative[temp];
            } catch (Exception) {
                return AttemptToFindNonRepeatingAvailableResponse();
            }
            return choice;
        }

        public string PickNegative
            (ref Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>> data) {
            double plus = (PlusFlag && AttributeRelationshipIsNegative()) ?
                responseBiasWeight : responseTotalWeight - responseBiasWeight;
            double choice = Controller.dice.NextDouble() * responseTotalWeight + 1;
            if (plus >= choice && data.ContainsKey(Constants.NEGATIVE_PLUS) && data[Constants.NEGATIVE_PLUS].Count > 0) {
                return Constants.NEGATIVE_PLUS;
            } else if (data.ContainsKey(Constants.NEGATIVE) && data[Constants.NEGATIVE_PLUS].Count > 0) {
                return Constants.NEGATIVE;
            } else {
                return Constants.NEUTRAL;
            }
        }

        //allows for dulpicate responses
        public Dictionary<string, Dictionary<string, List<string>>> PullCustom
            (ref Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>> data) {

            if (!Relationship.Equals(Constants.NEUTRAL) && !AttributeRelationshipIsNegative()) {
                return PullPositive(ref data);
            } else if (AttributeRelationshipIsNegative())
                return PullNegative(ref data);
            else
                return PullNeutral(ref data);
        }

        virtual public Dictionary<string, Dictionary<string, List<string>>> CheckRequirements
            (Dictionary<string, Dictionary<string, List<string>>> data) {
            if (package == null)
                return data;
            foreach (KeyValuePair<string, List<string>> item in package.LeadTo) {
                foreach (string lead in package.LeadTo[item.Key]) {
                    string[] arr = lead.Split(".");
                    if (arr.Length > 1 && arr[0].Equals(Constants.RESPONSE_TAG)) {
                        foreach (KeyValuePair<string, Dictionary<string, List<string>>> item2 in data) {
                            RemoveElement(ref data, item2.Key, ref arr);
                        }
                    }
                }
            }
            return data;
        }

        public void RemoveElement
            (ref Dictionary<string, Dictionary<string, List<string>>> data, string key, ref string[] arr) {
            bool keep = data[key][Constants.REQ].Count == 0;
            foreach (string s in data[key][Constants.REQ]) {
                string[] arr2 = s.Split(".");
                if (arr2.Length>1 && arr2[1].Equals(arr[1])) {
                    keep = true;
                    break;
                }
            }
            for (int j = 0; j < Responses.Count; j++) {
                if (Responses[j].ContainsKey(key)) {
                    keep = false;
                    System.Console.WriteLine("Duplicate in responses: "+key);
                }
            }
            if (!keep) {
                data.Remove(key);
            }
        }
        //uniform distribution pick 
        public string PickResponseOption
            (ref Dictionary<string, Dictionary<string, List<string>>> option) {
            string arr = "";
            int index = 0;
            int winner = (int)(Controller.dice.NextDouble()*option.Count);
            foreach (string key in option.Keys) {
                if(index == winner)
                    arr=key;
                index++;
            }
            //foreach (string key in arr) {
            //    option.Remove(key);   
            //}
            return arr;
        }

    }
}
