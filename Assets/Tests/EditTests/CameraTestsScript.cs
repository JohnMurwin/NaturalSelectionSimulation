using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using NaturalSelectionSimulation;

namespace NSS.EditTests
{
    public class CameraTests
    {
        [Test]
        public void Up()
        {
            Assert.AreEqual(expected: new Vector3(x: 0, y: 1, z: 0), actual: Vector3.up);
        }

        [Test]
        public void Down()
        {
            Assert.AreEqual(expected: new Vector3(x: 0, y: -1, z: 0), actual: Vector3.down);
        }

        [Test]
        public void Left()
        {
            Assert.AreEqual(expected: new Vector3(x: -1, y: 0, z: 0), actual: Vector3.left);
        }

        [Test]
        public void Right()
        {
            Assert.AreEqual(expected: new Vector3(x: 1, y: 0, z: 0), actual: Vector3.right);
        }

        [Test]
        public void Forward()
        {
            Assert.AreEqual(expected: new Vector3(x: 0, y: 0, z: 1), actual: Vector3.forward);
        }

        [Test]
        public void Back()
        {
            Assert.AreEqual(expected: new Vector3(x: 0, y: 0, z: -1), actual: Vector3.back);
        }
    }
}