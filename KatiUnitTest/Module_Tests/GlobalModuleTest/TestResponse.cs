using Kati.GenericModule;
using Kati.Module_Hub;
using Kati.SourceFiles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

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

        [TestMethod]
        public void TestPullPositive() {
            double[] d = new double[] { 800, 700, 600, 500, 400, 300, 200, 100 };
            var stuff = SetTone(d);
            DialoguePackage package = new DialoguePackage();
            package.LeadTo.Add("response_tag.tag1");
            List<string> list = res.OrderRelationshipBranches(ref package, stuff);
            var master = ctrl.Lib.Data["sample1_response"];
            var item = res.PullPositive(ref master);
            System.Console.WriteLine(item.Count);
            
            Assert.IsTrue(item.Count == 1);
            Assert.IsTrue(res.Relationship.Equals(Constants.ROMANCE));
            Assert.IsTrue(res.Relationship.Equals(
                item.ContainsKey("positive+ test 1") || item.ContainsKey("positive+ test 2") ||
                item.ContainsKey("positive+ test 3") || item.ContainsKey("positive+ test 10") ||
                item.ContainsKey("positive+ test 11") || item.ContainsKey("positive+ test 12") )||
                item.ContainsKey("positive test 1") || item.ContainsKey("positive test 2") ||
                item.ContainsKey("positive test 3") || item.ContainsKey("positive test 10") ||
                item.ContainsKey("positive test 11") || item.ContainsKey("positive test 12"));
        }

        [TestMethod]
        public void TestPickPositive() {
            double[] d = new double[] { 800, 700, 600, 500, 400, 300, 200, 100 };
            var stuff = SetTone(d);
            DialoguePackage package = new DialoguePackage();
            package.LeadTo.Add("response_tag.tag1");
            List<string> list = res.OrderRelationshipBranches(ref package, stuff);
            var master = ctrl.Lib.Data["sample1_response"];
            for (int i = 0; i < 10; i++) {
                string type = res.PickPositive(ref master);
                Assert.IsTrue(type.Equals(Constants.POSITIVE_PLUS) || type.Equals(Constants.POSITIVE));
            }
        }

        [TestMethod]
        public void TestCheckRequirements() {
            double[] d = new double[] { 800, 700, 600, 500, 400, 300, 200, 100 };
            var stuff = SetTone(d);
            DialoguePackage package = new DialoguePackage();
            package.LeadTo.Add("response_tag.tag1");
            var master = ctrl.Lib.Data["sample1_response"]["positive+"];
            res.OrderRelationshipBranches(ref package, stuff);
            var item = res.CheckRequirements(master);
            
            Assert.IsTrue(item.Count==6);
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

            package.LeadTo = new List<string> {
                "response_tag.tag10"
            };
            master = ctrl.Lib.Data["sample1_response"]["negative+"];
            res.OrderRelationshipBranches(ref package, stuff);
            item = res.CheckRequirements(master);
            Assert.IsTrue(item.Count == 3);
            Assert.IsTrue(item.ContainsKey("negative+ test 10"));
            Assert.IsTrue(item.ContainsKey("negative+ test 11"));
            Assert.IsTrue(item.ContainsKey("negative+ test 12"));

        }

        [TestMethod]
        public void TestRemoveElement() {
            double[] d = new double[] { 800, 700, 600, 500, 400, 300, 200, 100 };
            var stuff = SetTone(d);
            DialoguePackage package = new DialoguePackage();
            package.LeadTo.Add("response_tag.tag1");
            var master = ctrl.Lib.Data["sample1_response"]["positive+"];


        }

        [TestMethod]
        public void TestD() {

        }

        [TestMethod]
        public void TestE() {

        }

    }
}
