using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using NaturalSelectionSimulation;

namespace NSS.EditTests
{
    public class ET_DevJohnCoverageTest
    {

        // A Test behaves as an ordinary method
        [Test]
        public void TestNumberAdder()
        {
            // this should pass
            Assert.AreEqual(DEV_John.NumberAdder(2, 3), 5);
        }
        
        [Test]
        public void TestNumberMultiplier()
        {
            // this should pass
            Assert.AreEqual(DEV_John.NumberMultiplier(2, 3), 6);
        }
        
        [Test]
        public void TestNumberSquarer()
        {
            // this should fail
            Assert.AreEqual(DEV_John.NumberSquarer(2), 5);
        }
    }
}