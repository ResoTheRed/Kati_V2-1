using Kati.GenericModule;
using Kati.Module_Hub;
using Kati.SourceFiles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;

namespace KatiUnitTest.Module_Tests.GlobalModuleTest {
    
    [TestClass]
    public class TestResponse {

        Response res;
        Controller ctrl;
        BranchDecision b;

        [TestInitialize]
        public void Setup() {
            ctrl = new Controller(Constants.TestJson);
            res = new Response();
            double[] d = new double[] {100,100,100,100,100,100,100,100 };
            SetTone(d);
            b = new BranchDecision(ctrl);
        }

        public Dictionary<string, double> SetTone(double[] d) {
            Dictionary<string, double> temp = new Dictionary<string, double>();
            temp["romance"] = d[0];
            temp["friend"] = d[1];
            temp["professional"] = d[2];
            temp["respect"] = d[3];
            temp["affinity"] = d[4];
            temp["disgust"] = d[5];
            temp["hate"] = d[6];
            temp["rivalry"] = d[7];
            CharacterData.SetInitiatorCharacterData("Init","Male",temp,null,null);
            return temp;
        }

        //#################################Testing Relationship Ordering Methods############################################
        [TestMethod]
        public void TestOrderRelationshipBranches() {
            double[] d = new double[] { 800, 700, 600, 500, 400, 300, 200, 100 };
            var item = SetTone(d);
            DialoguePackage package = new DialoguePackage();
            List<string> list = res.OrderRelationshipBranches(ref package, item);
            Assert.IsTrue(list[0].Equals("romance"));
            Assert.IsTrue(list[4].Equals("affinity"));
            Assert.IsTrue(list[7].Equals("rivalry"));
            Assert.IsTrue(res.PlusFlag);
            Assert.IsTrue(res.Relationship.Equals("romance"));
            d = new double[] { 100, 200, 300, 400, 500, 600, 700, 800 };
            item = SetTone(d);
            list = res.OrderRelationshipBranches(ref package, item);
            Assert.IsTrue(list[0].Equals("rivalry"));
            Assert.IsTrue(list[4].Equals("respect"));
            Assert.IsTrue(list[7].Equals("romance"));
            Assert.IsTrue(res.PlusFlag);
            Assert.IsTrue(res.Relationship.Equals("rivalry"));
            d = new double[] { 0, 0, 0, 0, 0, 0, 700, 0 };
            item = SetTone(d);
            list = res.OrderRelationshipBranches(ref package, item);
            Assert.IsTrue(list[0].Equals("hate"));
            Assert.IsTrue(res.PlusFlag);
            Assert.IsTrue(res.Relationship.Equals("hate"));
            d = new double[] { 0, 0, 0, 0, 0, 0, 300, 0 };
            item = SetTone(d);
            list = res.OrderRelationshipBranches(ref package, item);
            Assert.IsTrue(list[0].Equals("hate"));
            Assert.IsTrue(!res.PlusFlag);
            Assert.IsTrue(res.Relationship.Equals("hate"));
            d = new double[] { 0, 0, 0, 0, 0, 40, 30, 20 };
            item = SetTone(d);
            list = res.OrderRelationshipBranches(ref package, item);
            Assert.IsTrue(list[0].Equals("disgust"));
            Assert.IsTrue(list[1].Equals("hate"));
            Assert.IsTrue(list[2].Equals("rivalry"));
            Assert.IsTrue(!res.PlusFlag);
            Assert.IsTrue(res.Relationship.Equals("neutral"));
        }

        //#######################################Testing Helper Methods Methods##############################################

        [TestMethod]
        public void TestCheckRequirements1() {
            double[] d = new double[] { 800, 700, 600, 500, 400, 300, 200, 100 };
            var stuff = SetTone(d);
            DialoguePackage package = new DialoguePackage();
            package.Req["test"] = new List<string>();
            package.Req["test"].Add("response_tag.tag1");
            var master = ctrl.Lib.Data["sample1_response"]["positive+"];
            res.OrderRelationshipBranches(ref package, stuff);
            var item = res.CheckRequirements(master);

            Assert.IsTrue(item.Count == 6);
            Assert.IsTrue(item.ContainsKey("positive+ test 1"));
            Assert.IsTrue(item.ContainsKey("positive+ test 2"));
            Assert.IsTrue(item.ContainsKey("positive+ test 3"));
            Assert.IsTrue(item.ContainsKey("positive+ test 10"));
            Assert.IsTrue(item.ContainsKey("positive+ test 11"));
            Assert.IsTrue(item.ContainsKey("positive+ test 12"));

            master = ctrl.Lib.Data["sample1_response"]["positive"];
            res.OrderRelationshipBranches(ref package, stuff);
            item = res.CheckRequirements(master);
            Assert.IsTrue(item.Count == 6);
            Assert.IsTrue(item.ContainsKey("positive test 1"));
            Assert.IsTrue(item.ContainsKey("positive test 2"));
            Assert.IsTrue(item.ContainsKey("positive test 3"));
            Assert.IsTrue(item.ContainsKey("positive test 10"));
            Assert.IsTrue(item.ContainsKey("positive test 11"));
            Assert.IsTrue(item.ContainsKey("positive test 12"));

            master = ctrl.Lib.Data["sample1_response"]["neutral"];
            res.OrderRelationshipBranches(ref package, stuff);
            item = res.CheckRequirements(master);
            Assert.IsTrue(item.Count == 6);
            Assert.IsTrue(item.ContainsKey("neutral test 1"));
            Assert.IsTrue(item.ContainsKey("neutral test 2"));
            Assert.IsTrue(item.ContainsKey("neutral test 3"));
            Assert.IsTrue(item.ContainsKey("neutral test 10"));
            Assert.IsTrue(item.ContainsKey("neutral test 11"));
            Assert.IsTrue(item.ContainsKey("neutral test 12"));

            master = ctrl.Lib.Data["sample1_response"]["negative"];
            res.OrderRelationshipBranches(ref package, stuff);
            item = res.CheckRequirements(master);
            Assert.IsTrue(item.Count == 6);
            Assert.IsTrue(item.ContainsKey("negative test 1"));
            Assert.IsTrue(item.ContainsKey("negative test 2"));
            Assert.IsTrue(item.ContainsKey("negative test 3"));
            Assert.IsTrue(item.ContainsKey("negative test 10"));
            Assert.IsTrue(item.ContainsKey("negative test 11"));
            Assert.IsTrue(item.ContainsKey("negative test 12"));

            master = ctrl.Lib.Data["sample1_response"]["negative+"];
            res.OrderRelationshipBranches(ref package, stuff);
            item = res.CheckRequirements(master);
            Assert.IsTrue(item.Count == 6);
            Assert.IsTrue(item.ContainsKey("negative+ test 1"));
            Assert.IsTrue(item.ContainsKey("negative+ test 2"));
            Assert.IsTrue(item.ContainsKey("negative+ test 3"));
            Assert.IsTrue(item.ContainsKey("negative+ test 10"));
            Assert.IsTrue(item.ContainsKey("negative+ test 11"));
            Assert.IsTrue(item.ContainsKey("negative+ test 12"));

            package.Req["test"] = new List<string>();
            package.Req["test"].Add("response_tag.tag10");

            master = ctrl.Lib.Data["sample1_response"]["negative+"];
            res.OrderRelationshipBranches(ref package, stuff);
            item = res.CheckRequirements(master);
            Assert.IsTrue(item.Count == 3);
            Assert.IsTrue(item.ContainsKey("negative+ test 10"));
            Assert.IsTrue(item.ContainsKey("negative+ test 11"));
            Assert.IsTrue(item.ContainsKey("negative+ test 12"));

        }

        [TestMethod]
        public void TestCheckRequirements2() {
            double[] d = new double[] { 800, 700, 600, 500, 400, 300, 200, 100 };
            var stuff = SetTone(d);
            DialoguePackage package = new DialoguePackage();
            package.Req["test"] = new List<string>();
            package.Req["test"].Add("response_tag.intro_tier7");
            var master = ctrl.Lib.Data["sample2_response"]["positive+"];
            res.OrderRelationshipBranches(ref package, stuff);
            var item = res.CheckRequirements(master);

            Assert.IsTrue(item.Count == 0);
            
            master = ctrl.Lib.Data["sample2_response"]["positive"];
            res.OrderRelationshipBranches(ref package, stuff);
            item = res.CheckRequirements(master);
            Assert.IsTrue(item.Count == 2);
            Assert.IsTrue(item.ContainsKey("Please call me #player_name#."));
            Assert.IsTrue(item.ContainsKey("I prefer #player_name#. Thank you."));
            
            master = ctrl.Lib.Data["sample2_response"]["neutral"];
            res.OrderRelationshipBranches(ref package, stuff);
            item = res.CheckRequirements(master);
            Assert.IsTrue(item.Count == 3);
            Assert.IsTrue(item.ContainsKey("My name is #player_name#."));
            Assert.IsTrue(item.ContainsKey("#player_name#"));

            master = ctrl.Lib.Data["sample2_response"]["negative"];
            res.OrderRelationshipBranches(ref package, stuff);
            item = res.CheckRequirements(master);
            Assert.IsTrue(item.Count == 2);
            Assert.IsTrue(item.ContainsKey("Have you read my resume? My name is #player_name#"));
            Assert.IsTrue(item.ContainsKey("My name is #player_name# and don't forget it."));

            master = ctrl.Lib.Data["sample2_response"]["negative+"];
            res.OrderRelationshipBranches(ref package, stuff);
            item = res.CheckRequirements(master);
            Assert.IsTrue(item.Count == 0);
        }

        [TestMethod]
        public void TestCheckAllRequirements1() {
            double[] d = new double[] { 800, 700, 600, 500, 400, 300, 200, 100 };
            var stuff = SetTone(d);
            DialoguePackage package = new DialoguePackage();
            package.Req["test"] = new List<string>();
            package.Req["test"].Add("response_tag.intro_tier7");
            var master = ctrl.Lib.Data["sample2_response"];
            res.OrderRelationshipBranches(ref package, stuff);
            var app = res.CheckAllRequirements(ref master);
            System.Console.WriteLine(app.Count);
            Assert.IsTrue(app.Count == 5);
            Assert.IsTrue(app.ContainsKey(Constants.POSITIVE_PLUS));
            Assert.IsTrue(app[Constants.POSITIVE_PLUS].Count == 0);
            Assert.IsTrue(app.ContainsKey(Constants.NEGATIVE_PLUS));
            Assert.IsTrue(app[Constants.NEGATIVE_PLUS].Count == 0);

            Assert.IsTrue(app[Constants.POSITIVE].Count == 2);
            Assert.IsTrue(app[Constants.POSITIVE].ContainsKey("Please call me #player_name#."));
            Assert.IsTrue(app[Constants.POSITIVE].ContainsKey("I prefer #player_name#. Thank you."));

            Assert.IsTrue(app[Constants.NEUTRAL].Count == 3);
            Assert.IsTrue(app[Constants.NEUTRAL].ContainsKey("My name is #player_name#."));
            Assert.IsTrue(app[Constants.NEUTRAL].ContainsKey("#player_name#"));

            Assert.IsTrue(app[Constants.NEGATIVE].Count == 2);
            Assert.IsTrue(app[Constants.NEGATIVE].ContainsKey("Have you read my resume? My name is #player_name#"));
            Assert.IsTrue(app[Constants.NEGATIVE].ContainsKey("My name is #player_name# and don't forget it."));

        }

        [TestMethod]
        public void TestCheckAllRequirements2() {
            double[] d = new double[] { 800, 700, 600, 500, 400, 300, 200, 100 };
            var stuff = SetTone(d);
            DialoguePackage package = new DialoguePackage();
            package.Req["test"] = new List<string>();
            package.Req["test"].Add("response_tag.intro_tier9");
            var master = ctrl.Lib.Data["sample2_response"];
            res.OrderRelationshipBranches(ref package, stuff);
            var app = res.CheckAllRequirements(ref master);
            System.Console.WriteLine(app.Count);
            Assert.IsTrue(app.Count == 5);
            Assert.IsTrue(app[Constants.POSITIVE_PLUS].Count == 0);
            Assert.IsTrue(app[Constants.POSITIVE].Count == 0);
            Assert.IsTrue(app[Constants.NEUTRAL].Count == 6);
            Assert.IsTrue(app[Constants.NEGATIVE].Count == 0);
            Assert.IsTrue(app[Constants.NEGATIVE_PLUS].Count == 0);
        }

        [TestMethod]
        public void TestAttemptToFindSpecificResponse() {
            double[] d = new double[] { 800, 700, 600, 500, 400, 300, 200, 100 };
            var stuff = SetTone(d);
            DialoguePackage package = new DialoguePackage();
            package.Req["test"] = new List<string>();
            package.Req["test"].Add("response_tag.intro_tier9");
            var master = ctrl.Lib.Data["sample2_response"];
            res.OrderRelationshipBranches(ref package, stuff);
            res.ApplicableResponses = res.CheckAllRequirements(ref master);
            Assert.IsTrue(res.ApplicableResponses[Constants.NEUTRAL].Count == 6);
            
            string branch = res.AttemptToFindNonRepeatingSpecificAvailableResponse(Constants.POSITIVE_PLUS);
            Assert.IsTrue(branch.Length==0);
            branch = res.AttemptToFindNonRepeatingSpecificAvailableResponse(Constants.POSITIVE);
            Assert.IsTrue(branch.Length == 0);
            branch = res.AttemptToFindNonRepeatingSpecificAvailableResponse(Constants.NEGATIVE);
            Assert.IsTrue(branch.Length == 0);
            branch = res.AttemptToFindNonRepeatingSpecificAvailableResponse(Constants.NEGATIVE_PLUS);
            Assert.IsTrue(branch.Length == 0);
            branch = res.AttemptToFindNonRepeatingSpecificAvailableResponse(Constants.NEUTRAL);
            Assert.IsTrue(branch.Length > 0);

            var temp = new Dictionary<string, Dictionary<string, List<string>>>();
            temp["I am in control, consistant, perfect, relentless, and fearless"] =
                master[Constants.NEUTRAL]["I am in control, consistant, perfect, relentless, and fearless"];
            res.Responses.Add(temp);
            
            temp = new Dictionary<string, Dictionary<string, List<string>>>();
            temp["I am patient, diligent, result-oriented, proactive, and skillful"] =
                master[Constants.NEUTRAL]["I am patient, diligent, result-oriented, proactive, and skillful"];
            res.Responses.Add(temp);
            
            temp = new Dictionary<string, Dictionary<string, List<string>>>();
            temp["I am confident, maticulous, energetic, motivated, and self-starter"] =
                master[Constants.NEUTRAL]["I am confident, maticulous, energetic, motivated, and self-starter"];
            res.Responses.Add(temp);
            
            temp = new Dictionary<string, Dictionary<string, List<string>>>();
            temp["I am social, jovial, analytical, professional, and ambitious"] =
                master[Constants.NEUTRAL]["I am social, jovial, analytical, professional, and ambitious"];
            res.Responses.Add(temp);
            
            temp = new Dictionary<string, Dictionary<string, List<string>>>();
            temp["I am gloomy, brooding, listener, good direction follower, and a bit melancholy"] =
                master[Constants.NEUTRAL]["I am gloomy, brooding, listener, good direction follower, and a bit melancholy"];
            res.Responses.Add(temp);

            for (int i = 0; i < 100; i++) {
                branch = res.AttemptToFindNonRepeatingSpecificAvailableResponse(Constants.NEUTRAL);
                Assert.IsTrue(branch.Equals("I am calculating, meticulous, exact, punctual, and deadly accurate"));
            }

            temp[branch] = master[Constants.NEUTRAL][branch];
            res.Responses.Add(temp);

            branch = res.AttemptToFindNonRepeatingSpecificAvailableResponse(Constants.NEUTRAL);
            System.Console.WriteLine(branch);
            Assert.IsTrue(branch.Length==0);
        }

        [TestMethod]
        public void TestAttemptToFindResponse() {
            double[] d = new double[] { 800, 700, 600, 500, 400, 300, 200, 100 };
            var stuff = SetTone(d);
            DialoguePackage package = new DialoguePackage();
            package.Req["test"] = new List<string>();
            package.Req["test"].Add("response_tag.intro_tier7");
            var master = ctrl.Lib.Data["sample2_response"];
            res.OrderRelationshipBranches(ref package, stuff);
            res.ApplicableResponses = res.CheckAllRequirements(ref master);
            Assert.IsTrue(res.ApplicableResponses[Constants.POSITIVE].Count == 2);
            Assert.IsTrue(res.ApplicableResponses[Constants.NEUTRAL].Count == 3);
            Assert.IsTrue(res.ApplicableResponses[Constants.NEGATIVE].Count == 2);

            List<string> dialogueInResponses = new List<string>();
            var temp = new Dictionary<string, Dictionary<string, List<string>>>();

            var dialogue = res.AttemptToFindNonRepeatingAvailableResponse();
            Assert.IsTrue(dialogue.Length>0);
            
            temp[dialogue] = Helper1A(dialogue, master);
            res.Responses.Add(temp);
            dialogueInResponses.Add(dialogue);
            for (int i = 0; i < 100; i++) {
                dialogue = res.AttemptToFindNonRepeatingAvailableResponse();
                Assert.IsFalse(dialogueInResponses.Contains(dialogue));
                Assert.IsTrue(dialogue.Length > 0);
            }

            temp[dialogue] = Helper1A(dialogue, master);
            res.Responses.Add(temp);
            dialogueInResponses.Add(dialogue);
            for (int i = 0; i < 100; i++) {
                dialogue = res.AttemptToFindNonRepeatingAvailableResponse();
                Assert.IsFalse(dialogueInResponses.Contains(dialogue));
                Assert.IsTrue(dialogue.Length > 0);
            }

            temp[dialogue] = Helper1A(dialogue, master);
            res.Responses.Add(temp);
            dialogueInResponses.Add(dialogue);
            for (int i = 0; i < 100; i++) {
                dialogue = res.AttemptToFindNonRepeatingAvailableResponse();
                Assert.IsFalse(dialogueInResponses.Contains(dialogue));
                Assert.IsTrue(dialogue.Length > 0);
            }

            temp[dialogue] = Helper1A(dialogue, master);
            res.Responses.Add(temp);
            dialogueInResponses.Add(dialogue);
            for (int i = 0; i < 100; i++) {
                dialogue = res.AttemptToFindNonRepeatingAvailableResponse();
                Assert.IsFalse(dialogueInResponses.Contains(dialogue));
                Assert.IsTrue(dialogue.Length > 0);
            }

            temp[dialogue] = Helper1A(dialogue, master);
            res.Responses.Add(temp);
            dialogueInResponses.Add(dialogue);
            for (int i = 0; i < 100; i++) {
                dialogue = res.AttemptToFindNonRepeatingAvailableResponse();
                Assert.IsFalse(dialogueInResponses.Contains(dialogue));
                Assert.IsTrue(dialogue.Length > 0);
            }

            temp[dialogue] = Helper1A(dialogue, master);
            res.Responses.Add(temp);
            dialogueInResponses.Add(dialogue);
            for (int i = 0; i < 100; i++) {
                dialogue = res.AttemptToFindNonRepeatingAvailableResponse();
                Assert.IsFalse(dialogueInResponses.Contains(dialogue));
                Assert.IsTrue(dialogue.Length > 0);
            }

            temp[dialogue] = Helper1A(dialogue, master);
            res.Responses.Add(temp);
            dialogueInResponses.Add(dialogue);
            Assert.IsTrue(dialogue.Length==0);


        }
        private Dictionary<string, List<string>> Helper1A(string dialogue, Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>> data) {
            if (data[Constants.POSITIVE_PLUS].ContainsKey(dialogue)) {
                return data[Constants.POSITIVE_PLUS][dialogue];
            } else if (data[Constants.POSITIVE].ContainsKey(dialogue)) {
                return data[Constants.POSITIVE][dialogue];
            }else if (data[Constants.NEUTRAL].ContainsKey(dialogue)) {
                return data[Constants.NEUTRAL][dialogue];
            }else if (data[Constants.NEGATIVE].ContainsKey(dialogue)) {
                return data[Constants.NEGATIVE][dialogue];
            }else if (data[Constants.NEGATIVE_PLUS].ContainsKey(dialogue)) {
                return data[Constants.NEGATIVE_PLUS][dialogue];
            }
            return null;
        }

        [TestMethod]
        public void TestHandleEmptyBranchValues() {
            double[] d = new double[] { 800, 700, 600, 500, 400, 300, 200, 100 };
            var stuff = SetTone(d);
            DialoguePackage package = new DialoguePackage();
            package.Req["test"] = new List<string>();
            package.Req["test"].Add("response_tag.intro_tier7");
            var master = ctrl.Lib.Data["sample2_response"];
            res.OrderRelationshipBranches(ref package, stuff);
            res.ApplicableResponses = res.CheckAllRequirements(ref master);
            Assert.IsTrue(res.ApplicableResponses[Constants.POSITIVE].Count == 2);
            Assert.IsTrue(res.ApplicableResponses[Constants.NEUTRAL].Count == 3);
            Assert.IsTrue(res.ApplicableResponses[Constants.NEGATIVE].Count == 2);

            var branch = res.HandleEmptyBranchString(Constants.POSITIVE_PLUS);
            Assert.IsTrue(branch.Equals(Constants.POSITIVE_PLUS));
            branch = res.HandleEmptyBranchString(Constants.NEGATIVE_PLUS);
            Assert.IsTrue(branch.Equals(Constants.NEGATIVE_PLUS));

            for (int i = 0; i < 100; i++) {
                branch = res.HandleEmptyBranchString("");
                Assert.IsTrue(branch.Equals(Constants.POSITIVE) ||
                    branch.Equals(Constants.NEUTRAL) ||
                    branch.Equals(Constants.NEGATIVE) );
            }
            //no possible values
            package.Req["test"] = new List<string>();
            package.Req["test"].Add("response_tag.intro_tier317");
            master = ctrl.Lib.Data["sample2_response"];
            res.OrderRelationshipBranches(ref package, stuff);
            res.ApplicableResponses = res.CheckAllRequirements(ref master);
            Assert.IsTrue(res.ApplicableResponses[Constants.POSITIVE].Count == 0);
            Assert.IsTrue(res.ApplicableResponses[Constants.NEUTRAL].Count == 0);
            Assert.IsTrue(res.ApplicableResponses[Constants.NEGATIVE].Count == 0);

            branch = res.HandleEmptyBranchString(Constants.POSITIVE_PLUS);
            Assert.IsTrue(branch.Equals(Constants.POSITIVE_PLUS));
            branch = res.HandleEmptyBranchString(Constants.NEGATIVE_PLUS);
            Assert.IsTrue(branch.Equals(Constants.NEGATIVE_PLUS));

            for (int i = 0; i < 100; i++) {
                branch = res.HandleEmptyBranchString("");
                Assert.IsTrue(branch.Length==0);
            }

        }

        [TestMethod]
        public void TestPickResponse() {
            double[] d = new double[] { 800, 700, 600, 500, 400, 300, 200, 100 };
            var stuff = SetTone(d);
            DialoguePackage package = new DialoguePackage();
            package.Req["test"] = new List<string>();
            package.Req["test"].Add("response_tag.intro_tier7");
            var master = ctrl.Lib.Data["sample2_response"];
            res.OrderRelationshipBranches(ref package, stuff);
            res.ApplicableResponses = res.CheckAllRequirements(ref master);
            Assert.IsTrue(res.ApplicableResponses[Constants.POSITIVE].Count == 2);
            Assert.IsTrue(res.ApplicableResponses[Constants.NEUTRAL].Count == 3);
            Assert.IsTrue(res.ApplicableResponses[Constants.NEGATIVE].Count == 2);
            var d = res.PickResponse();
        }



        /*

        [TestMethod]
        public void TestPullPositive() {
            double[] d = new double[] { 800, 700, 600, 500, 400, 300, 200, 100 };
            var stuff = SetTone(d);
            DialoguePackage package = new DialoguePackage();
            package.LeadTo["test"] = new List<string>();
            package.LeadTo["test"].Add("response_tag.tag1");
            List<string> list = res.OrderRelationshipBranches(ref package, stuff);
            var master = ctrl.Lib.Data["sample1_response"];
            var item = res.PullPositive(ref master);
            System.Console.WriteLine(item.Count);
            
           
        }

        [TestMethod]
        public void TestPickPositive() {
            double[] d = new double[] { 800, 700, 600, 500, 400, 300, 200, 100 };
            var stuff = SetTone(d);
            DialoguePackage package = new DialoguePackage();
            package.LeadTo["test"] = new List<string>();
            package.LeadTo["test"].Add("response_tag.tag1");
            List<string> list = res.OrderRelationshipBranches(ref package, stuff);
            var master = ctrl.Lib.Data["sample1_response"];
           
        }

        

        [TestMethod]
        public void TestPullNeutral() {
            double[] d = new double[] { 80, 70, 60, 50, 40, 30, 20, 10 };
            var stuff = SetTone(d);
            DialoguePackage package = new DialoguePackage();
            package.LeadTo["test"] = new List<string>();
            package.LeadTo["test"].Add("response_tag.tag1");
            var master = ctrl.Lib.Data["sample1_response"];
            res.OrderRelationshipBranches(ref package, stuff);
            var item = res.PullNeutral(ref master);
            
        }

        [TestMethod]
        public void TestPullNegative() {
            double[] d = new double[] { 80, 70, 60, 50, 40, 400, 20, 10 };
            var stuff = SetTone(d);
            DialoguePackage package = new DialoguePackage();
            package.LeadTo["test"] = new List<string>();
            package.LeadTo["test"].Add("response_tag.tag1");
            var master = ctrl.Lib.Data["sample1_response"];
            res.OrderRelationshipBranches(ref package, stuff);
            var item = res.PullNegative(ref master);
        }

        [TestMethod]
        public void TestPullCustomNeg() {
            double[] d = new double[] { 80, 70, 60, 50, 40, 400, 20, 10 };
            var stuff = SetTone(d);
            DialoguePackage package = new DialoguePackage();
            package.LeadTo["test"] = new List<string>();
            package.LeadTo["test"].Add("response_tag.tag1");
            var master = ctrl.Lib.Data["sample1_response"];
            res.OrderRelationshipBranches(ref package, stuff);
            var item = res.PullCustom(ref master);
        }

        [TestMethod]
        public void TestPullCustomPos() {
            double[] d = new double[] { 800, 70, 60, 50, 40, 40, 20, 10 };
            var stuff = SetTone(d);
            DialoguePackage package = new DialoguePackage();
            package.LeadTo["test"] = new List<string>();
            package.LeadTo["test"].Add("response_tag.tag1");
            var master = ctrl.Lib.Data["sample1_response"];
            res.OrderRelationshipBranches(ref package, stuff);
            var item = res.PullCustom(ref master);
        }

        [TestMethod]
        public void TestPullCustomNeutral() {
            double[] d = new double[] { 80, 70, 60, 50, 40, 30, 20, 10 };
            var stuff = SetTone(d);
            DialoguePackage package = new DialoguePackage();
            package.LeadTo["test"] = new List<string>();
            package.LeadTo["test"].Add("response_tag.tag1");
            var master = ctrl.Lib.Data["sample1_response"];
            res.OrderRelationshipBranches(ref package, stuff);
            var item = res.PullCustom(ref master);
        }

        [TestMethod]
        public void TestParseResponsesPosStats() {
            
        }
        */
    }
}
