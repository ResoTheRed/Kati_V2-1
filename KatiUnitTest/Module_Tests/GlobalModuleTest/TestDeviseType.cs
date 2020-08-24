﻿using Kati.GenericModule;
using Kati.SourceFiles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace KatiUnitTest.Module_Tests.GlobalModuleTest {

    [TestClass]
    public class TestDeviseType {

        Controller ctrl;
        DeviseType type;

        [TestInitialize]
        public void Start() {
            ctrl = new Controller(Constants.TestJson);
            type = ctrl.Type;
        }

        [TestMethod]
        public void TestGetTopicTypes() {
            string t = type.GetTopicType();
            Assert.IsTrue(t.Equals(Constants.STATEMENT) || t.Equals(Constants.QUESTION));
            type.SetWeights(0,10000);
            t = type.GetTopicType();
            Assert.IsTrue(t.Equals(Constants.QUESTION));
            type.SetWeights(10000,0);
            t = type.GetTopicType();
            Assert.IsTrue(t.Equals(Constants.STATEMENT));

        }

    }
}