﻿using Kati;
using Kati.GenericModule;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace KatiUnitTest.Module_Tests.GlobalModuleTest {

    [TestClass]
    public class TestDeviseTopics {

        Controller ctrl;
        DeviseTopic topics;

        [TestInitialize]
        public void Start() {
            ctrl = new Controller(Constants.TestJson);
            topics = ctrl.Topic;
        }

        [TestMethod]
        public void TestConstructor() {
            Assert.IsTrue(topics.TopicWeights.Count ==2);
            Assert.IsTrue(topics.TopicWeights["sample1"] == 20);
            Assert.IsTrue(topics.TopicWeights["sample2"] == 20);
        }

        [TestMethod]
        public void TestSetSingleWeight() {
            topics.SetSingleTopicWeight("sample1", 50);
            topics.SetSingleTopicWeight("sample3", 50);
            Assert.IsTrue(topics.TopicWeights.Count==2);
            Assert.IsTrue(topics.TopicWeights["sample1"] == 50);
            Assert.IsTrue(topics.TopicWeights["sample2"] == 20);
        }

        [TestMethod]
        public void TestSetAllWeights() {
            Dictionary<string, double> temp = new Dictionary<string, double>();
            temp["sample1"] = 25;
            temp["sample2"] = 40;
            temp["sam"] = 60;
            topics.SetMultiWeights(temp);
            Assert.IsTrue(topics.TopicWeights.Count == 2);
            Assert.IsTrue(topics.TopicWeights["sample1"] == 25);
            Assert.IsTrue(topics.TopicWeights["sample2"] == 40);
        }

        [TestMethod]
        public void TestGetTopic() {
            string topic = topics.GetTopic();
            Assert.IsTrue(topic.Equals("sample1")||topic.Equals("sample2"));
            List<string> temp = new List<string>() { "sample1", "scrimp", "scrawberries" };
            topic = topics.GetTopic(temp);
            Assert.IsTrue(topic.Equals("sample1"));
        }
    }
}
