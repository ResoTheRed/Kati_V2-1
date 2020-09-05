using Kati.SourceFiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kati.GenericModule {

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
        private List<string> leadsToData;
        private string relationsip;
        private bool plusFlag;
        
        Dictionary<string, Dictionary<string, List<string>>> reponses;

        public Response() {
            OrderedBranches = new List<string>();
            LeadsToData = new List<string>();
            BranchValues = new Dictionary<string, double>();
            Reponses = new Dictionary<string, Dictionary<string, List<string>>>();
            Relationsip = Constants.NEUTRAL;
            PlusFlag = false;
        }

        public List<string> LeadsToData { get => leadsToData; set => leadsToData = value; }
        public string Relationsip { get => relationsip; set => relationsip = value; }
        public List<string> OrderedBranches { get => orderedBranches; set => orderedBranches = value; }
        public bool PlusFlag { get => plusFlag; set => plusFlag = value; }
        public Dictionary<string, double> BranchValues { get => branchValues; set => branchValues = value; }
        public Dictionary<string, Dictionary<string, List<string>>> Reponses { get => reponses; set => reponses = value; }
        
        /**************************** Setup for Branch Defined Response*******************************/

        public List<string> OrderRelationshipBranches(Dictionary<string, double> branches) {
            BranchValues = branches;
            List<string> sorted = new List<string>();
            int i = 0;
            foreach (KeyValuePair<string, double> item in branches) {
                sorted[i] = item.Key;
                i++;
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
                Relationsip = Constants.NEUTRAL;
            } else {
                Relationsip = value;
            }
        }

        protected bool AttributeRelationshipIsNegative() {
            return Relationsip.Equals(Constants.DISGUST) ||
                   Relationsip.Equals(Constants.HATE) ||
                   Relationsip.Equals(Constants.RIVALRY);
        }

        //set plus flag to signal the attribute defined response to be a plus version
        protected bool SetPlusFlag
            (ref Dictionary<string, double> branches, ref List<string> sorted) {
            bool nonNeutral = false;
            for (int k = 0; k < sorted.Count; k++) {
                if (branches[sorted[k]] >= Constants.RESPONSE_NEUTRAL_THRESHOLD) {
                    nonNeutral = true;
                    if (branches[sorted[k]] >= Constants.RESPONSE_PLUS_THRESHOLD) {
                        plusFlag = true;
                    }
                }
            }
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
            //pull neutral response
            //pull negative response
            //pull custom response
        }

        protected Dictionary<string, Dictionary<string, List<string>>> PullPositive
            (ref Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>> data) {
            //check to see if positive+ exists
            //check to see if positive exists
            //decide which to use with added weight if positive+ flag is true
            //if AttributeIsNegative then heavy weight on just positive
            //check each option for requirements for winning type
            //if no option exists then switch to the losing type
            //if no options exist for either use neutral
            return null;
        }
        
        protected Dictionary<string, Dictionary<string, List<string>>> PullNeutral
            (ref Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>> data) {
            //check to see if positive+ exists
            //check to see if positive exists
            //decide which to use with added weight if positive+ flag is true
            //if AttributeIsNegative then heavy weight on just positive
            //check each option for requirements for winning type
            //if no option exists then switch to the losing type
            //if no options exist for either use neutral
            return null;
        }
        
        protected Dictionary<string, Dictionary<string, List<string>>> PullNegative
            (ref Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>> data) {
            //check to see if positive+ exists
            //check to see if positive exists
            //decide which to use with added weight if positive+ flag is true
            //if AttributeIsNegative then heavy weight on just positive
            //check each option for requirements for winning type
            //if no option exists then switch to the losing type
            //if no options exist for either use neutral
            return null;
        }




    }
}
