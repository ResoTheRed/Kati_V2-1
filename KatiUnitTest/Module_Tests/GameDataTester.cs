using Kati.Module_Hub;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KatiUnitTest.Module_Tests{

    /// <summary>
    /// Test GameData.cs in Module_Hub directory
    /// </summary>
    [TestClass()]
    public class GameDataTester{

        

    }

    [TestClass()]
    public class CharacterDataTester {

        private CharacterData data;
        private readonly string[] stats = { "romance","friends","professional","respect",
            "admiration","disgust","hate","rivalry"};
        private Random dice = new Random();

        [TestInitialize]
        public void Start() {
            data = CharacterData.GetCharacterData();
        }
        /*
        [TestMethod]
        public void TestInitiatorNameString() {
            data.InitiatorsName = "Stephen";
            Assert.IsTrue(data.InitiatorsName.Equals("Stephen"));
        }

        [TestMethod]
        public void TestRespondersNameString() {
            data.RespondersName = "Kati";
            Assert.IsTrue(data.RespondersName.Equals("Kati"));
        }

        [TestMethod]
        public void TestInitiatorsGender() {
            data.InitialorsGender = "Male";
            Assert.IsTrue(data.InitialorsGender.Equals("Male"));
        }

        [TestMethod]
        public void TestRespondersGender() {
            data.RespondersGender = "Female";
            Assert.IsTrue(data.RespondersGender.Equals("Female"));
        }

        [TestMethod]
        public void TestInteracitonTone() {
            Dictionary<string, double> tones = new Dictionary<string, double>();
            for (int i = 0; i < stats.Length; i++) {
                tones[stats[i]] = dice.NextDouble();
            }
            data.InteractionTone = tones;
            for (int i = 0; i < stats.Length; i++) {
                Assert.IsNotNull(data.InteractionTone[stats[i]]);
            }
        }

        [TestMethod]
        public void TestInitiatorsTone() {
            Dictionary<string, double> tones = new Dictionary<string, double>();
            for (int i = 0; i < stats.Length; i++) {
                tones[stats[i]] = dice.NextDouble();
            }
            data.InitiatorsTone = tones;
            for (int i = 0; i < stats.Length; i++) {
                Assert.IsNotNull(data.InitiatorsTone[stats[i]]);
            }
        }

        [TestMethod]
        public void TestRespondersTone() {
            Dictionary<string, double> tones = new Dictionary<string, double>();
            for (int i = 0; i < stats.Length; i++) {
                tones[stats[i]] = dice.NextDouble();
            }
            data.RespondersTone = tones;
            for (int i = 0; i < stats.Length; i++) {
                Assert.IsNotNull(data.RespondersTone[stats[i]]);
            }
        }

        [TestMethod]
        public void TestIniatorsAttributeList() {
            Dictionary<string, string> list = new Dictionary<string, string>();
            list["lucky"] = "characterTrait";
            data.InitiatorPersonalList = list;
            Assert.AreEqual(data.InitiatorPersonalList["lucky"], "characterTrait");
        }

        [TestMethod]
        public void TestRespondersAttributeList() {
            Dictionary<string, string> list = new Dictionary<string, string>();
            list["lucky"] = "characterTrait";
            data.ResponderAttributeList = list;
            Assert.AreEqual(data.ResponderAttributeList["lucky"], "characterTrait");
        }

        [TestMethod]
        public void TestInitiatorsScalarList() {
            Dictionary<string, int> list = new Dictionary<string, int>();
            list["charm"] = 200;
            data.InitiatorScalarList = list;
            Assert.AreEqual(data.InitiatorScalarList["charm"],200);
        }

        [TestMethod]
        public void TestRespondersScalarList() {
            Dictionary<string, int> list = new Dictionary<string, int>();
            list["charm"] = 200;
            data.ResponderScalarList = list;
            Assert.AreEqual(data.ResponderScalarList["charm"], 200);
        }
        */
    }

}
