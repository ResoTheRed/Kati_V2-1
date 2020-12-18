using Kati;
using Kati.GenericModule;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace KatiUnitTest.Module_Tests.GlobalModuleTest {

    [TestClass]
    public class TestDialogueWeights {

        Controller ctrl;
        DialogueWeigthRules w;

        [TestInitialize]
        public void Start() {
            ctrl = new Controller(Constants.TestJson);
            w = ctrl.Parser.Weight;
        }

        public List<string> SetList() {
            List<string> temp = new List<string>();
            temp.Add("game.weather.humid");//0
            temp.Add("game.publicEvent.art_fest");//1
            temp.Add("personal.trait.adventurous");//2
            temp.Add("personal.interest.likes music");//3
            temp.Add("personal.status.injured");//4
            temp.Add("personal.physicalFeature.is petite");//5
            temp.Add("personal.scalarTrait.charm.1000");//6
            temp.Add("social.attribute.player.romance.500");//7
            temp.Add("social.directed.player.curious about");//8
            return temp;
        }

        [TestMethod]
        public void TestRuleDirectory() {
            var t = SetList();
            Assert.IsTrue(w.RuleDirectoryTop(t[0])==0);
            Assert.IsTrue(w.RuleDirectoryTop(t[1])==20);
            Assert.IsTrue(w.RuleDirectoryTop(t[2])==40);
            Assert.IsTrue(w.RuleDirectoryTop(t[3])==20);
            Assert.IsTrue(w.RuleDirectoryTop(t[4])==80);
            Assert.IsTrue(w.RuleDirectoryTop(t[5])==30);
            Assert.IsTrue(w.RuleDirectoryTop(t[6])>=99);
            Assert.IsTrue(w.RuleDirectoryTop(t[7])>=49);
            Assert.IsTrue(w.RuleDirectoryTop(t[8])==50);
        }

        [TestMethod]
        public void TestConvertToBaseWeights() {
            Dictionary<string, double> weights = new Dictionary<string, double>();
            List<string> keys = new List<string>();
            for (int i = 1; i < 16; ++i) {
                keys.Add("respect test " + i);
            }
            w.Data = ctrl.Lib.Data["sample1_statement"]["respect"];
            w.ConvertToBaseWeights(ref weights);
            Assert.IsTrue(weights[keys[0]]==50);
            Assert.IsTrue(weights[keys[1]]==70);
            Assert.IsTrue(weights[keys[2]]==90);
            Assert.IsTrue(weights[keys[3]]==70);
            Assert.IsTrue(weights[keys[4]]==130);
            Assert.IsTrue(weights[keys[5]]==80);
            Assert.IsTrue(weights[keys[6]]>=149);
            Assert.IsTrue(weights[keys[7]]>=99);
            Assert.IsTrue(weights[keys[8]]==100);
            Assert.IsTrue(weights[keys[9]]==75);
            Assert.IsTrue(weights[keys[10]]>=138);
            Assert.IsTrue(weights[keys[11]]==50);
            Assert.IsTrue(weights[keys[12]]==50);
            Assert.IsTrue(weights[keys[13]]==50);
            Assert.IsTrue(weights[keys[14]]==50);
        }

        [TestMethod]
        public void TestGetDialogue() {
            var package = w.GetDialogue(ctrl.Lib.Data["sample1_statement"]["respect"]);
            Assert.IsTrue(package.Count==1);
            //visual test
            ctrl = new Controller(Constants.TestJson);
            int[] values = new int[15];
            List<string> keys = new List<string>();
            for (int i = 1; i < 16; ++i) {
                keys.Add("respect test " + i);
            }
            for (int j = 0; j < 10000; j++) {
                var d = ctrl.Parser.Weight.GetDialogue(ctrl.Lib.Data["sample1_statement"]["respect"]);
                for (int k = 0; k < keys.Count; k++) {
                    if (d.ContainsKey(keys[k])) {
                        values[k] += 1;
                    }
                }
            }
            for (int l = 0; l < keys.Count; l++) {
                Console.WriteLine(keys[l] + " " + (values[l] / 100.0));
            }
        }
    }
}
