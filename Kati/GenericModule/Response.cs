using Kati.Module_Hub;
using Kati.SourceFiles;
using System;
using System.Collections.Generic;

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
            //pull positive response
            Responses.Add(PullPositive(ref data));
            //pull neutral response
            Responses.Add(PullNeutral(ref data));
            //pull negative response
            Responses.Add(PullNegative(ref data));
            //pull custom response
            Responses.Add(PullCustom(ref data));

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
            var positive = data[PickPositive(ref data)];
            positive = CheckRequirements(positive);
            Dictionary<string, Dictionary<string, List<string>>> choice = 
                new Dictionary<string, Dictionary<string, List<string>>>();
            string temp = PickResponseOption(ref positive);
            choice[temp] = positive[temp];
            return choice;
        }

        //70% chance for pos+ if plusFlag else 70% chance for reg pos
        //@return positive+, positive, or neutral
        public string PickPositive
            (ref Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>> data) {
            double plus = (PlusFlag && !AttributeRelationshipIsNegative()) ? 
                responseBiasWeight : responseTotalWeight - responseBiasWeight;
            double choice = Controller.dice.NextDouble() * responseTotalWeight + 1;
            if (plus >= choice && data.ContainsKey(Constants.POSITIVE_PLUS)) {
                return Constants.POSITIVE_PLUS;
            } else if (data.ContainsKey(Constants.POSITIVE)) {
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
            choice[temp] = neutral[temp];
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
            var negative = data[PickNegative(ref data)];
            negative = CheckRequirements(negative);
            Dictionary<string, Dictionary<string, List<string>>> choice =
               new Dictionary<string, Dictionary<string, List<string>>>();
            string temp = PickResponseOption(ref negative);
            choice[temp] = negative[temp];
            return choice;
        }

        public string PickNegative
            (ref Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>> data) {
            double plus = (PlusFlag && AttributeRelationshipIsNegative()) ?
                responseBiasWeight : responseTotalWeight - responseBiasWeight;
            double choice = Controller.dice.NextDouble() * responseTotalWeight + 1;
            if (plus >= choice && data.ContainsKey(Constants.NEGATIVE_PLUS)) {
                return Constants.NEGATIVE_PLUS;
            } else if (data.ContainsKey(Constants.NEGATIVE)) {
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

        public Dictionary<string, Dictionary<string, List<string>>> CheckRequirements
            (Dictionary<string, Dictionary<string, List<string>>> data) {
            if (package == null)
                return data;
            foreach (string lead in package.LeadTo) {
                string[] arr = lead.Split(".");
                if (arr.Length > 1 && arr[0].Equals(Constants.RESPONSE_TAG)) {
                    foreach (KeyValuePair<string, Dictionary<string, List<string>>> item in data) {
                        RemoveElement(ref data, item.Key, ref arr);
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
