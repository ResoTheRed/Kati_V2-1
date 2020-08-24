using Kati.GenericModule;
using Kati.Module_Hub;
using Kati.SourceFiles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace KatiUnitTest.Module_Tests.GlobalModuleTest {

    [TestClass]
    public class TestSocialCharacterRules {
        private Controller ctrl;
        private SocialCharacterRules rules;

        [TestInitialize]
        public void Start() {
            ctrl = new Controller(Constants.TestJson);
            rules = ctrl.Parser.Social;
            Setup();
        }

        private void Setup() {
            CharacterData c = ctrl.Npc;
            Dictionary<string, Dictionary<string, string>> temp = new Dictionary<string, Dictionary<string, string>>();
            string[] names = { "Player","Dudley", "Vernon", "Mary" };
            foreach (string name in names) {
                temp[name] = new Dictionary<string, string>();
            }
            string[] arr = { "romance", "friend", "professional", "disgust", "hate", 
                "rivalry", "respect", "affinity" };
            foreach (string att in arr) {
                temp["Player"][att] = "500";
            }
            foreach (string att in arr) {
                temp["Mary"][att] = "200";
            }
            foreach (string att in arr) {
                temp["Dudley"][att] = "400";
            }
            foreach (string att in arr) {
                temp["Vernon"][att] = "300";
            }
            temp["Player"]["trust"] = Constants.RELATIONSHIP;
            temp["Player"]["friends with"] = Constants.RELATIONSHIP;
            temp["Mary"]["trust"] = Constants.RELATIONSHIP;
            temp["Mary"]["loyal to"] = Constants.RELATIONSHIP;
            temp["Dudley"]["trust"] = Constants.RELATIONSHIP;
            temp["Dudley"]["related to"] = Constants.RELATIONSHIP;
            temp["Vernon"]["trust"] = Constants.RELATIONSHIP;
            temp["Vernon"]["works with"] = Constants.RELATIONSHIP;
            arr = new string[]{"interested in","attracted to","curious about","in love with","betrayed by","worried about", "snubbed by" };
            int max = 3;
            int min = 0;
            foreach (string name in names) {
                for (int i = min; i < max; ++i) {
                    temp[name][arr[i]] = Constants.DIRECTED_STATUS;
                }
                max++;
                min++;
            }
            //only need to set initators socialAttributes
            CharacterData.SetInitiatorCharacterData("Initiator","female",null,null,temp);
            CharacterData.SetResponderCharacterData("Player","male",null);
        }

        [TestMethod]
        public void TestSetup() {
            Assert.IsTrue(ctrl.Npc.InitiatorSocialList.Count==4);
            Assert.IsTrue(ctrl.Npc.InitiatorSocialList["Player"].Count == 13);
        }

        [TestMethod]
        public void TestCheckAttributeAny() {
            for (int i = 0; i < 100; i++) {
                rules.TargetsName = "Pumpkin";
                string[] arr = { "attribute", "npc", "romance", "210" };
                Assert.IsFalse(rules.RuleDirectory(arr));
                Assert.IsFalse(rules.TargetsName.Equals("Mary"));
                arr = new string[] { "attribute", "npc", "not", "romance", "300" };
                Assert.IsFalse(rules.RuleDirectory(arr));
                Assert.IsTrue(rules.TargetsName.Equals("Mary"));
                arr = new string[] { "attribute", "npc", "not", "romance", "500" };
                Assert.IsFalse(rules.RuleDirectory(arr));
                Assert.IsFalse(rules.TargetsName.Equals("Player"));
                arr = new string[] { "attribute", "npc", "romance", "500" };
                Assert.IsTrue(rules.RuleDirectory(arr));
                //cannot == "Player"
                Assert.IsFalse(rules.TargetsName.Equals("Player"));
                arr = new string[] { "attribute", "npc", "not" };
                Assert.IsTrue(rules.RuleDirectory(arr));
                arr = new string[] { "attribute", "npc" };
                Assert.IsTrue(rules.RuleDirectory(arr));
                //should never be null because of methods that call it
            }
        }

        [TestMethod]
        public void TestCheckAttributePlayer() {
            for (int i = 0; i < 50; i++) {
                string[] arr = { "attribute", "player", "romance", "300" };
                Assert.IsFalse(rules.RuleDirectory(arr));
                Assert.IsTrue(rules.TargetsName.Equals("Player"));
                arr = new string[] { "attribute", "player", "not", "romance", "300" };
                rules.TargetsName = "none";
                Assert.IsTrue(rules.RuleDirectory(arr));
                Assert.IsTrue(rules.TargetsName.Equals("none"));

                arr = new string[] { "attribute", "player", "not", "romance", "500" };
                Assert.IsTrue(rules.RuleDirectory(arr));
                Assert.IsTrue(rules.TargetsName.Equals("none"));
                arr = new string[] { "attribute", "player", "romance", "500" };
                Assert.IsFalse(rules.RuleDirectory(arr));
                Assert.IsTrue(rules.TargetsName.Equals("Player"));
                arr = new string[] { "attribute", "player", "not" };
                Assert.IsTrue(rules.RuleDirectory(arr));
                arr = new string[] { "attribute", "player" };
                Assert.IsTrue(rules.RuleDirectory(arr));
            }
        }

        [TestMethod]
        public void TestCheckAttributeNpc() {
            for (int i = 0; i < 50; i++) {
                string[] arr = { "attribute", "Vernon", "romance", "300" };
                Assert.IsFalse(rules.RuleDirectory(arr));
                Assert.IsTrue(rules.TargetsName.Equals("Vernon"));
                arr = new string[] { "attribute", "Vernon", "not", "romance", "300" };
                rules.TargetsName = "none";
                Assert.IsTrue(rules.RuleDirectory(arr));
                Assert.IsTrue(rules.TargetsName.Equals("Vernon"));
                
                arr = new string[] { "attribute", "Vernon", "not", "romance", "500" };
                Assert.IsFalse(rules.RuleDirectory(arr));
                Assert.IsTrue(rules.TargetsName.Equals("Vernon"));
                arr = new string[] { "attribute", "blinker", "romance", "500" };
                Assert.IsTrue(rules.RuleDirectory(arr));
                Assert.IsTrue(rules.TargetsName.Equals("blinker"));
                arr = new string[] { "attribute", "Vernon", "not" };
                Assert.IsTrue(rules.RuleDirectory(arr));
                arr = new string[] { "attribute", "psakjdfhka" };
                Assert.IsTrue(rules.RuleDirectory(arr));
            }
        }

        [TestMethod]
        public void TestCheckStaticStatAny() {
            string[] arr = { "relationship", "npc", "trust"};
            Assert.IsFalse(rules.RuleDirectory(arr));
            Assert.IsFalse(rules.TargetsName.Equals("Player"));
            
            arr = new string[] { "relationship", "npc", "friends with"};
            rules.TargetsName = "turtle";
            Assert.IsTrue(rules.RuleDirectory(arr));
            Assert.IsTrue(rules.TargetsName.Equals("turtle"));
            arr = new string[] { "relationship", "npc","not", "friends with" };
            rules.TargetsName = "turtle";
            Assert.IsFalse(rules.RuleDirectory(arr));
            Assert.IsFalse(rules.TargetsName.Equals("turtle"));

            arr = new string[] { "relationship", "npc", "loyal to" };
            rules.TargetsName = "turtle";
            Assert.IsFalse(rules.RuleDirectory(arr));
            Assert.IsTrue(rules.TargetsName.Equals("Mary"));
            arr = new string[] { "relationship", "npc", "not", "loyal to" };
            rules.TargetsName = "turtle";
            Assert.IsFalse(rules.RuleDirectory(arr));
            Assert.IsFalse(rules.TargetsName.Equals("Mary"));
            
            arr = new string[] { "relationship", "npc", "related to" };
            rules.TargetsName = "turtle";
            Assert.IsFalse(rules.RuleDirectory(arr));
            Assert.IsTrue(rules.TargetsName.Equals("Dudley"));
            
            arr = new string[] { "relationship", "npc", "works with" };
            rules.TargetsName = "turtle";
            Assert.IsFalse(rules.RuleDirectory(arr));
            Assert.IsTrue(rules.TargetsName.Equals("Vernon"));
            
            arr = new string[] { "relationship", "npc"};
            rules.TargetsName = "turtle";
            Assert.IsTrue(rules.RuleDirectory(arr));
            Assert.IsTrue(rules.TargetsName.Equals("turtle"));
        }

        [TestMethod]
        public void TestCheckStaticStatAny2() {
            string[] arr = { "directed", "npc", "interested in" };
            rules.TargetsName = "turtle";
            Assert.IsTrue(rules.RuleDirectory(arr));
            Assert.IsFalse(rules.TargetsName.Equals("Player"));
            
            arr = new string[] { "directed", "npc", "attracted to" };
            rules.TargetsName = "turtle";
            Assert.IsFalse(rules.RuleDirectory(arr));
            Assert.IsTrue(rules.TargetsName.Equals("Dudley"));
            arr = new string[] { "directed", "npc", "not", "attracted to" };
            rules.TargetsName = "turtle";
            Assert.IsFalse(rules.RuleDirectory(arr));
            Assert.IsFalse(rules.TargetsName.Equals("Dudley"));
            
            arr = new string[] { "directed", "npc", "curious about" };
            rules.TargetsName = "turtle";
            Assert.IsFalse(rules.RuleDirectory(arr));
            Assert.IsTrue(rules.TargetsName.Equals("Dudley")|| rules.TargetsName.Equals("Vernon"));
            arr = new string[] { "directed", "npc", "not", "curious about" };
            rules.TargetsName = "turtle";
            Assert.IsFalse(rules.RuleDirectory(arr));
            Assert.IsFalse(rules.TargetsName.Equals("Dudley")|| rules.TargetsName.Equals("Vernon"));
            
            arr = new string[] { "directed", "npc", "betrayed by" };
            rules.TargetsName = "turtle";
            Assert.IsFalse(rules.RuleDirectory(arr));
            Assert.IsTrue(rules.TargetsName.Equals("Mary") || rules.TargetsName.Equals("Vernon"));
            
            arr = new string[] { "directed", "npc", "worried about" };
            rules.TargetsName = "turtle";
            Assert.IsFalse(rules.RuleDirectory(arr));
            Assert.IsTrue(rules.TargetsName.Equals("Mary"));
            
            arr = new string[] { "directed", "npc", "snubbed by" };
            rules.TargetsName = "turtle";
            Assert.IsTrue(rules.RuleDirectory(arr));
            Assert.IsTrue(rules.TargetsName.Equals("turtle"));
        }

        [TestMethod]
        public void TestCheckStaticStatPlayer() {
            string[] arr = { "relationship", "player", "trust" };
            Assert.IsFalse(rules.RuleDirectory(arr));
            Assert.IsTrue(rules.TargetsName.Equals("Player"));
            arr = new string[] { "relationship", "player", "friends with" };
            Assert.IsFalse(rules.RuleDirectory(arr));
            Assert.IsTrue(rules.TargetsName.Equals("Player"));
            arr = new string[] { "relationship", "player", "enemies with" };
            Assert.IsTrue(rules.RuleDirectory(arr));
            Assert.IsTrue(rules.TargetsName.Equals("Player"));
            arr = new string[] { "relationship", "player", "not", "trust" };
            Assert.IsTrue(rules.RuleDirectory(arr));
            Assert.IsTrue(rules.TargetsName.Equals("Player"));
            arr = new string[] { "relationship", "player", "not", "friends with" };
            Assert.IsTrue(rules.RuleDirectory(arr));
            Assert.IsTrue(rules.TargetsName.Equals("Player"));
            arr = new string[] { "relationship", "player", "not", "enemies with" };
            Assert.IsFalse(rules.RuleDirectory(arr));
            Assert.IsTrue(rules.TargetsName.Equals("Player"));

            arr = new string[] { "directed", "player", "interested in" };
            Assert.IsFalse(rules.RuleDirectory(arr));
            Assert.IsTrue(rules.TargetsName.Equals("Player"));
            arr = new string[] { "directed", "player", "not", "interested in" };
            Assert.IsTrue(rules.RuleDirectory(arr));
            Assert.IsTrue(rules.TargetsName.Equals("Player"));
            arr = new string[] { "directed", "player", "snubbed by" };
            Assert.IsTrue(rules.RuleDirectory(arr));
            Assert.IsTrue(rules.TargetsName.Equals("Player"));
            arr = new string[] { "directed", "player", "not", "snubbed by" };
            Assert.IsFalse(rules.RuleDirectory(arr));
            Assert.IsTrue(rules.TargetsName.Equals("Player"));

        }
        
        
        [TestMethod]
        public void TestCheckStaticStatNpc() {
            string[] arr = { "relationship", "Mary", "trust" };
            Assert.IsFalse(rules.RuleDirectory(arr));
            Assert.IsTrue(rules.TargetsName.Equals("Mary"));
            arr = new string[] { "relationship", "Mary", "friends with" };
            Assert.IsTrue(rules.RuleDirectory(arr));
            Assert.IsTrue(rules.TargetsName.Equals("Mary"));
            arr = new string[] { "relationship", "Vernon", "enemies with" };
            Assert.IsTrue(rules.RuleDirectory(arr));
            Assert.IsTrue(rules.TargetsName.Equals("Vernon"));
            arr = new string[] { "relationship", "Dudley", "not", "trust" };
            Assert.IsTrue(rules.RuleDirectory(arr));
            Assert.IsTrue(rules.TargetsName.Equals("Dudley"));
            arr = new string[] { "relationship", "Mary", "not", "friends with" };
            Assert.IsFalse(rules.RuleDirectory(arr));
            Assert.IsTrue(rules.TargetsName.Equals("Mary"));
            arr = new string[] { "relationship", "V", "not", "enemies with" };
            Assert.IsTrue(rules.RuleDirectory(arr));
            Assert.IsTrue(rules.TargetsName.Equals("V"));

            arr = new string[] { "directed", "Dudley", "interested in" };
            Assert.IsTrue(rules.RuleDirectory(arr));
            Assert.IsTrue(rules.TargetsName.Equals("Dudley"));
            arr = new string[] { "directed", "Mary", "not", "interested in" };
            Assert.IsFalse(rules.RuleDirectory(arr));
            Assert.IsTrue(rules.TargetsName.Equals("Mary"));
            arr = new string[] { "directed", "Vernon", "snubbed by" };
            Assert.IsTrue(rules.RuleDirectory(arr));
            Assert.IsTrue(rules.TargetsName.Equals("Vernon"));
            arr = new string[] { "directed", "Vernon", "not", "snubbed by" };
            Assert.IsFalse(rules.RuleDirectory(arr));
            Assert.IsTrue(rules.TargetsName.Equals("Vernon"));

        }

        [TestMethod]
        public void TestRemoveElementScalar() {
            string s = "social.attribute.npc.hate.100";
            Assert.IsFalse(rules.RemoveElement(s));
            Assert.IsFalse(rules.TargetsName.Equals("Player"));
            s = "social.attribute.npc.not.hate.100";
            Assert.IsTrue(rules.RemoveElement(s));
            Assert.IsFalse(rules.TargetsName.Equals("Player"));
            
            s = "social.attribute.player.hate.100";
            Assert.IsFalse(rules.RemoveElement(s));
            Assert.IsTrue(rules.TargetsName.Equals("Player"));
            s = "social.attribute.player.not.hate.100";
            Assert.IsTrue(rules.RemoveElement(s));
            Assert.IsTrue(rules.TargetsName.Equals("Player"));
            
            s = "social.attribute.Vernon.hate.500";
            Assert.IsTrue(rules.RemoveElement(s));
            Assert.IsTrue(rules.TargetsName.Equals("Vernon"));
            s = "social.attribute.Vernon.not.hate.500";
            Assert.IsFalse(rules.RemoveElement(s));
            Assert.IsTrue(rules.TargetsName.Equals("Vernon"));

        }

        [TestMethod]
        public void TestRemoveElementStatic() {
            string s = "social.relationship.npc.trust";
            Assert.IsFalse(rules.RemoveElement(s));
            Assert.IsFalse(rules.TargetsName.Equals("Player"));
            s = "social.relationship.npc.not.trust";
            Assert.IsTrue(rules.RemoveElement(s));
            Assert.IsFalse(rules.TargetsName.Equals("Player"));
            
            s = "social.relationship.npc.loyal to";
            Assert.IsFalse(rules.RemoveElement(s));
            Assert.IsTrue(rules.TargetsName.Equals("Mary"));
            s = "social.relationship.npc.not.loyal to";
            Assert.IsFalse(rules.RemoveElement(s));
            Assert.IsFalse(rules.TargetsName.Equals("Mary"));
            
            s = "social.relationship.player.loyal to";
            Assert.IsTrue(rules.RemoveElement(s));
            Assert.IsTrue(rules.TargetsName.Equals("Player"));
            s = "social.relationship.player.not.loyal to";
            Assert.IsFalse(rules.RemoveElement(s));
            Assert.IsTrue(rules.TargetsName.Equals("Player"));
            s = "social.relationship.player.friends with";
            Assert.IsFalse(rules.RemoveElement(s));
            Assert.IsTrue(rules.TargetsName.Equals("Player"));
            s = "social.relationship.player.not.friends with";
            Assert.IsTrue(rules.RemoveElement(s));
            Assert.IsTrue(rules.TargetsName.Equals("Player"));
            
            s = "social.directed.Vernon.in love with";
            Assert.IsFalse(rules.RemoveElement(s));
            Assert.IsTrue(rules.TargetsName.Equals("Vernon"));
            s = "social.directed.Vernon.not.in love with";
            Assert.IsTrue(rules.RemoveElement(s));
            Assert.IsTrue(rules.TargetsName.Equals("Vernon"));
            s = "social.directed.Vernon.interested in";
            Assert.IsTrue(rules.RemoveElement(s));
            Assert.IsTrue(rules.TargetsName.Equals("Vernon"));
            s = "social.directed.Vernon.not.interested in";
            Assert.IsFalse(rules.RemoveElement(s));
            Assert.IsTrue(rules.TargetsName.Equals("Vernon"));


        }

        [TestMethod]
        public void TestRemoveElementErronious() {
            string s = "social.relationship.npc.ghsd.trust";
            Assert.IsTrue(rules.RemoveElement(s));
            s = "social.relationship.g.npc.trust";
            Assert.IsTrue(rules.RemoveElement(s));
            s = "social.relation.npc.ghsd.trust";
            Assert.IsTrue(rules.RemoveElement(s));
            s = "social.relationship.trust";
            Assert.IsTrue(rules.RemoveElement(s));
            s = "socal.relationship.npc.ghsd.trust";
            Assert.IsFalse(rules.RemoveElement(s));
            s = "social";
            Assert.IsTrue(rules.RemoveElement(s));
            s = "a quarter past Ham";
            Assert.IsFalse(rules.RemoveElement(s));
            s = "";
            Assert.IsFalse(rules.RemoveElement(s));
            s = null;
            Assert.IsTrue(rules.RemoveElement(s));


        }

        [TestMethod]
        public void TestParseSocialRequirements() {
            //correct way to do it.  Will not writeover lib items
            var data = (ctrl.Lib.DeepCopyDictionaryByTopic("sample1", ctrl.Lib.STATEMENT))["friend"];
            var keep = rules.ParseSocialRequirments(data);
            Assert.IsTrue(keep.Count==11);
            Assert.IsTrue(keep.ContainsKey("friend test 1"));
            Assert.IsTrue(keep.ContainsKey("friend test 2"));
            Assert.IsTrue(keep.ContainsKey("friend test 4"));
            Assert.IsTrue(keep.ContainsKey("friend test 6"));
            Assert.IsTrue(keep.ContainsKey("friend test 7"));
            Assert.IsTrue(keep.ContainsKey("friend test 9"));
            Assert.IsTrue(keep.ContainsKey("friend test 10"));
            Assert.IsTrue(keep.ContainsKey("friend test 12"));
            Assert.IsTrue(keep.ContainsKey("friend test 13"));
            Assert.IsTrue(keep.ContainsKey("friend test 15"));
            Assert.IsTrue(keep.ContainsKey("friend test 18"));
        }
    
    }
}
