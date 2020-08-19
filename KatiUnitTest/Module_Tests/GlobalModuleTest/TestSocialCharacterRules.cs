using Kati.Data_Modules.GlobalClasses;
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
            string[] names = { "Player","Dudley","Vernon","Mary" };
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
            temp["Player"]["trust"] = SocialCharacterRules.RELATIONSHIP;
            temp["Player"]["friends with"] = SocialCharacterRules.RELATIONSHIP;
            temp["Mary"]["trust"] = SocialCharacterRules.RELATIONSHIP;
            temp["Mary"]["loyal to"] = SocialCharacterRules.RELATIONSHIP;
            temp["Dudley"]["trust"] = SocialCharacterRules.RELATIONSHIP;
            temp["Dudley"]["related to"] = SocialCharacterRules.RELATIONSHIP;
            temp["Vernon"]["trust"] = SocialCharacterRules.RELATIONSHIP;
            temp["Vernon"]["works with"] = SocialCharacterRules.RELATIONSHIP;
            arr = new string[]{"interested in","attracted to","curious about","in love with","betrayed by","worried about", "snubbed by" };
            int max = 3;
            int min = 0;
            foreach (string name in names) {
                for (int i = min; i < max; ++i) {
                    temp[name][arr[i]] = SocialCharacterRules.DIRECTED_STATUS;
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
            string[] arr = { "attribute", "npc", "romance", "300" };
            Assert.IsFalse(rules.RuleDirectory(arr));
            Assert.IsFalse(rules.TargetsName.Equals("Mary"));
            arr = new string[] { "attribute", "npc", "not", "romance", "300" };
            Assert.IsFalse(rules.RuleDirectory(arr));
            Assert.IsFalse(rules.TargetsName.Equals("Mary"));
            
        }
    
    }


}
