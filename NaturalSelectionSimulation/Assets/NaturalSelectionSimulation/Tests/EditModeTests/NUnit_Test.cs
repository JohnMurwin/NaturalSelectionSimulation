using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class NUnit_Test
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void PassingTest()
    {
        Assert.Pass();
    }

    [Test]
    public void FailingTest()
    {
        Assert.Fail("This test should fail");
    }

    [Test]
    public void TestWithException()
    {
        throw new InvalidOperationException("Some error happened");
    }
}
