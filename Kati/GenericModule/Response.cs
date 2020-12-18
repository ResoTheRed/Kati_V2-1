using Kati.Module_Hub;
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
        private Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>> applicableResponses;
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
        public Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>> ApplicableResponses { get => applicableResponses; set => applicableResponses = value; }


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
            var dat = PullPositive(ref data);
            if(dat != null)
                Responses.Add(dat);
            //pull neutral response
            dat = PullNeutral(ref data);
            if(dat != null)
                Responses.Add(dat);
            //pull negative response
            dat = PullNegative(ref data);
            if(dat != null)
                Responses.Add(dat);
            //pull custom response
            dat = PullCustom(ref data);
            if(dat != null)
                Responses.Add(dat);

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
        //############################################# Positive Methods ####################################################
        
        //pulls the positive single response if one exists or returns null if non exist
        public Dictionary<string, Dictionary<string, List<string>>> PullPositive
            (ref Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>> data) {
            string branchValue = PickPositive(ref data);
            branchValue = HandleEmptyBranchString(branchValue);
            return PickResponse(branchValue);
        }

        //70% chance for pos+ if plusFlag else 70% chance for reg pos
        //@return "positive+", "positive", "neutral." or an empty string if no branch is available
        public string PickPositive
            (ref Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>> applicable) {
            double plus = (PlusFlag && !AttributeRelationshipIsNegative()) ?
                responseBiasWeight : responseTotalWeight - responseBiasWeight;
            double choice = Controller.dice.NextDouble() * responseTotalWeight + 1;
            bool pos = applicable.ContainsKey(Constants.POSITIVE) && applicable[Constants.POSITIVE].Count>0;
            bool posPlus = applicable.ContainsKey(Constants.POSITIVE_PLUS) && applicable[Constants.POSITIVE_PLUS].Count > 0;
            return PositiveSelection(plus, choice, pos, posPlus);
        }
        //boolean checks for PickPositive method
        private string PositiveSelection(double plus, double choice, bool pos, bool posPlus) {
            if (!posPlus && pos) {
                return Constants.POSITIVE;
            } else if (posPlus && !pos) {
                return Constants.POSITIVE_PLUS;
            } else if (posPlus && pos) {
                if (plus >= choice) {
                    return Constants.POSITIVE_PLUS;
                } else {
                    return Constants.POSITIVE;
                }
            }else if (applicableResponses.ContainsKey(Constants.NEUTRAL)) {
                return Constants.NEUTRAL;
            }
            return "";
        }

        //############################################# Neutral Methods ####################################################

        //pulls the neutral single response if one exists or returns null if none exist
        public Dictionary<string, Dictionary<string, List<string>>> PullNeutral
            (ref Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>> data) {
            string branchValue = PickNeutral(data[Constants.NEUTRAL]);
            branchValue = HandleEmptyBranchString(branchValue);
            return PickResponse(branchValue);
        }
        //returns "neutral" or empty string if neutral doesn't exist
        protected string PickNeutral
            (Dictionary<string, Dictionary<string, List<string>>> neutral) {
            if (applicableResponses.ContainsKey(Constants.NEUTRAL)) {
                return Constants.NEUTRAL;
            }
            return "";
        }

        //############################################# Negative Methods ####################################################
        //pulls the negative single response if one exists or returns null if non exist
        public Dictionary<string, Dictionary<string, List<string>>> PullNegative
            (ref Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>> data) {
            string branchValue = PickNegative(ref data);
            branchValue = HandleEmptyBranchString(branchValue);
            return PickResponse(branchValue);
        }


        //70% chance for neg+ if plusFlag else 70% chance for reg neg
        //@return negative+, negative, neutral, or empty string if no applicable branches are found
        public string PickNegative
            (ref Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>> applicable) {
            double plus = (PlusFlag && AttributeRelationshipIsNegative()) ?
                responseBiasWeight : responseTotalWeight - responseBiasWeight;
            double choice = Controller.dice.NextDouble() * responseTotalWeight + 1;
            bool pos = applicable.ContainsKey(Constants.NEGATIVE) && applicable[Constants.NEGATIVE].Count > 0;
            bool posPlus = applicable.ContainsKey(Constants.NEGATIVE_PLUS) && applicable[Constants.NEGATIVE_PLUS].Count > 0;
            return NegativeSelection(plus, choice, pos, posPlus);
        }
        //boolean checks for PickNegative
        private string NegativeSelection(double plus, double choice, bool pos, bool posPlus) {
            if (!posPlus && pos) {
                return Constants.NEGATIVE;
            } else if (posPlus && !pos) {
                return Constants.NEGATIVE_PLUS;
            } else if (posPlus && pos) {
                if (plus >= choice) {
                    return Constants.NEGATIVE_PLUS;
                } else {
                    return Constants.NEGATIVE;
                }
            } else if (applicableResponses.ContainsKey(Constants.NEUTRAL)) {
                return Constants.NEUTRAL;
            }
            return "";
        }

        //############################################# Custom Methods ####################################################

        //
        public Dictionary<string, Dictionary<string, List<string>>> PullCustom
            (ref Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>> data) {
            string branchValue = "";
            if (!Relationship.Equals(Constants.NEUTRAL) && !AttributeRelationshipIsNegative()) {
                branchValue = FindUniqueCustomResponse(Constants.POSITIVE, Constants.POSITIVE_PLUS);
                return  (branchValue.Length==0) ?  null : PickResponse(branchValue);
            } else if (AttributeRelationshipIsNegative()) {
                branchValue = FindUniqueCustomResponse(Constants.NEGATIVE, Constants.NEGATIVE_PLUS);
                return (branchValue.Length == 0) ? null : PickResponse(branchValue);
            } else
                return PullNeutral(ref data);
        }

        //############################################# Helper Methods ####################################################

        //creates a data strucute that holds all d
        public Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>> CheckAllRequirements
            (ref Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>> data) {
            var temp = new Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>>();
            foreach (KeyValuePair<string, Dictionary<string, Dictionary<string, List<string>>>> item in data) {
                temp[item.Key] = new Dictionary<string, Dictionary<string, List<string>>>();
                //run for pos+, pos, neutral, neg, neg+
                temp[item.Key] = item.Value;
                temp[item.Key] = CheckRequirements(temp[item.Key]);
                //Console.WriteLine("Check All Requirements\n");
                foreach (KeyValuePair<string, Dictionary<string, List<string>>> item2 in temp[item.Key]) {
                    //Console.WriteLine(item.Key + "::" + item2.Key);
                }
                //Console.WriteLine("\n");
            }
            //deep copy to applicable
            //data = temp;
            return DeepCopyApplicableDialogue(temp);
        }

        private Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>> DeepCopyApplicableDialogue
            (Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>> temp) {
            var applicable = new Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>>();
            foreach (KeyValuePair<string, Dictionary<string, Dictionary<string, List<string>>>> item in temp) {
                applicable[item.Key] = new Dictionary<string, Dictionary<string, List<string>>>();
                foreach (KeyValuePair<string, Dictionary<string, List<string>>> item2 in temp[item.Key]) {
                    applicable[item.Key][item2.Key] = new Dictionary<string, List<string>>();
                    foreach (KeyValuePair<string, List<string>> item3 in temp[item.Key][item2.Key]) {
                        applicable[item.Key][item2.Key][item3.Key] = new List<string>();
                        foreach (string s in temp[item.Key][item2.Key][item3.Key]) {
                            applicable[item.Key][item2.Key][item3.Key].Add(s);
                        }
                    }
                }
            }
            return applicable;
        }

        //pulls a dialogue bit chosen from a group of applicable dialogues
        //looks for any unique dialogue in a specific branch, the branch ordering is randomized
        //returns unique dialogue bit or an empty string
        public string AttemptToFindNonRepeatingSpecificAvailableResponse(string branch) {
            string key = "";
            var app = new List<Dictionary<string, Dictionary<string, List<string>>>>();
            foreach (KeyValuePair<string, Dictionary<string, List<string>>> item2 in ApplicableResponses[branch]) {
                //load each possible response in a list
                app.Add(new Dictionary<string, Dictionary<string, List<string>>>() {
                    [item2.Key] = ApplicableResponses[branch][item2.Key]
                });
            }
            return AttemptContinued(ref key, app);
        }


        //pulls a dialogue bit chosen from a group of applicable dialogues
        //looks for any unique dialogue in all branch types, the branch order is randomized
        //returns unique dialogue bit or an empty string
        public string AttemptToFindNonRepeatingAvailableResponse() {
            string key = "";
            var app = new List<Dictionary<string, Dictionary<string, List<string>>>>();
            foreach (KeyValuePair<string, Dictionary<string, Dictionary<string, List<string>>>> item in ApplicableResponses) {
                foreach (KeyValuePair<string, Dictionary<string, List<string>>> item2 in ApplicableResponses[item.Key]) {
                    //load each possible response in a list
                    app.Add(new Dictionary<string, Dictionary<string, List<string>>>() {
                        [item2.Key] = ApplicableResponses[item.Key][item2.Key]
                    });
                }
            }
            return AttemptContinued(ref key, app);
        }
        //check for a dialogue string that is not in the responses a returns it.
        //if no string exists then an empty string is returned
        private string AttemptContinued(ref string key, List<Dictionary<string, Dictionary<string, List<string>>>> app) {
            int[] indices = GenerateRandomIndices(app.Count);
            for (int i = 0; i < indices.Length; i++) {
                foreach (KeyValuePair<string, Dictionary<string, List<string>>> item in app[indices[i]]) {
                    key = item.Key;
                }
                bool keep = true;
                for (int j = 0; j < Responses.Count; j++) {
                    if (Responses[j].ContainsKey(key)) {
                        keep = false;
                    }
                }
                if (keep) {
                    return key;
                }
            }
            return "";
        }

        protected int[] GenerateRandomIndices(int size) {
            List<int> values = new List<int>();
            while (values.Count < size) {
                int num = (int)(Controller.dice.NextDouble() * size);
                if (!values.Contains(num)) {
                    values.Add(num);
                }
            }
            return values.ToArray();
        }

        virtual public Dictionary<string, Dictionary<string, List<string>>> CheckRequirements
            (Dictionary<string, Dictionary<string, List<string>>> data) {
            if (Package == null)
                return data;
            foreach (KeyValuePair<string, List<string>> item in Package.Req) {
                foreach (string req in Package.Req[item.Key]) {
                    string[] arr = req.Split(".");
                    if (arr.Length > 1 && arr[0].Equals(Constants.RESPONSE_TAG)) {
                        foreach (KeyValuePair<string, Dictionary<string, List<string>>> item2 in data) {
                            RemoveElement(ref data, item2.Key, ref arr);
                        }
                    }
                }
            }
            return data;
        }
        protected void RemoveElement
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

        //handles the case for an empty branch string.
        //@return: branchValue (pos+,pos,neut,neg,neg+) or null if non exist
        public string HandleEmptyBranchString(string branchValue) {
            if (branchValue.Equals("")) {//no available Pos+, Pos, or Neutral 
                var dialogue = AttemptToFindNonRepeatingAvailableResponse();//look for any dialogue item from all branch values
                branchValue = DialogueToBranchConversion(dialogue);
            }
            return branchValue;
        }

        private string DialogueToBranchConversion(string dialogue) {
            if (dialogue.Equals("")) {//there is nothing available in any category 
                return "";
            } else if (ApplicableResponses[Constants.POSITIVE].ContainsKey(dialogue)) {
                return Constants.POSITIVE;
            } else if (ApplicableResponses[Constants.POSITIVE_PLUS].ContainsKey(dialogue)) {
                return Constants.POSITIVE_PLUS;
            } else if (ApplicableResponses[Constants.NEUTRAL].ContainsKey(dialogue)) {
                return Constants.NEUTRAL;
            } else if (ApplicableResponses[Constants.NEGATIVE].ContainsKey(dialogue)) {
                return Constants.NEGATIVE;
            } else if (ApplicableResponses[Constants.NEGATIVE_PLUS].ContainsKey(dialogue)) {
                return Constants.NEGATIVE_PLUS;
            }
            return "";
        }

        //######################################## Pick Response Method Chain #########################################

        //job is to pick an available response that is not already located in the response list
        public Dictionary<string, Dictionary<string, List<string>>> PickResponse(string branchValue) {
            if (branchValue.Length == 0)
                return null;
            var temp = ApplicableResponses[branchValue];
            string responseKey = PickResponseOption(ref temp);
            if (responseKey.Length == 0)
                return null;
            var temp2 = new Dictionary<string, Dictionary<string, List<string>>>();
            temp2[responseKey] = ApplicableResponses[branchValue][responseKey];
            return temp2;
        }
        //uniform distribution pick 
        private string PickResponseOption
            (ref Dictionary<string, Dictionary<string, List<string>>> option) {
            string arr = "";
            int index = 0;
            int winner = (int)(Controller.dice.NextDouble() * option.Count);
            return FindUniqueResponse(option, ref arr, ref index, winner);
        }

        private string FindUniqueResponse
            (Dictionary<string, Dictionary<string, List<string>>> option, ref string arr, ref int index, int winner) {
            List<string> possible = new List<string>();
            foreach (string key in option.Keys) {
                CheckIfKeyExistsInResponses(ref possible, key);
                if (index == winner && possible.Contains(key)) {
                    arr = key;
                    break;
                }
                index++;
            }
            if (arr.Equals("") && possible.Count > 0) {
                arr = possible[Controller.dice.Next(possible.Count)];
            }
            return arr;
        }
        //loads ref list possible with any key options that don't exist in the response class
        private void CheckIfKeyExistsInResponses(ref List<string> possible, string key) {
            if (Responses.Count > 0) {
                bool keep = true;
                for (int i = 0; i < Responses.Count; i++) {
                    if (Responses[i].ContainsKey(key)) {
                        keep = false;
                    }
                }
                if (keep) {
                    possible.Add(key);
                }
            } else {
                possible.Add(key);
            }
        }

        //checks for unique dialogue strings in a directed manner
        //returns the branch type pos+, pos, neutral, neg, neg+
        public string FindUniqueCustomResponse(string val1, string val2) {
            string branchValue = "";
            branchValue = AttemptToFindNonRepeatingSpecificAvailableResponse(val1);
            if (branchValue.Length == 0) 
                branchValue = AttemptToFindNonRepeatingSpecificAvailableResponse(val2);
            if (branchValue.Length == 0)
                branchValue = AttemptToFindNonRepeatingSpecificAvailableResponse(Constants.NEUTRAL);
            if (branchValue.Length == 0)
                branchValue = AttemptToFindNonRepeatingAvailableResponse();
            return DialogueToBranchConversion(branchValue);
        }

    }
}
