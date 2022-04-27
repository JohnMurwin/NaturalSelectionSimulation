using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using NaturalSelectionSimulation;

namespace NSS.EditTests
{
    public class GeneTests
    {
        /*
        [Test]
        public void ValidSpeedType()
        {
            var geneCollection = new RabbitGeneCollection();
            Assert.AreEqual(expected: typeof(float), actual: geneCollection.Speed.GetType());
        }

        [Test]
        public void ValidSizeType()
        {
            var geneCollection = new RabbitGeneCollection();
            Assert.AreEqual(expected: typeof(float), actual: geneCollection.Size.GetType());
        }

        [Test]
        public void ValidHearingDistanceType()
        {
            var geneCollection = new RabbitGeneCollection();
            Assert.AreEqual(expected: typeof(float), actual: geneCollection.HearingDistance.GetType());
        }

        [Test]
        public void ValidSightDistanceType()
        {
            var geneCollection = new RabbitGeneCollection();
            Assert.AreEqual(expected: typeof(float), actual: geneCollection.SightDistance.GetType());
        }

        [Test]
        public void ValidLifeExpectancyType()
        {
            var geneCollection = new RabbitGeneCollection();
            Assert.AreEqual(expected: typeof(float), actual: geneCollection.LifeExpectancy.GetType());
        }

        [Test]
        public void ValidGestationType()
        {
            var geneCollection = new RabbitGeneCollection();
            Assert.AreEqual(expected: typeof(float), actual: geneCollection.Gestation.GetType());
        }

        [Test]
        public void ValidReproductiveUrgeType()
        {
            var geneCollection = new RabbitGeneCollection();
            Assert.AreEqual(expected: typeof(float), actual: geneCollection.ReproductiveUrge.GetType());
        }

        [Test]
        public void ValidGrowthTimeType()
        {
            var geneCollection = new RabbitGeneCollection();
            Assert.AreEqual(expected: typeof(float), actual: geneCollection.GrowthTime.GetType());
        }

        [Test]
        public void ValidDesirabilityType()
        {
            var geneCollection = new RabbitGeneCollection();
            Assert.AreEqual(expected: typeof(float), actual: geneCollection.Desirability.GetType());
        }

        [Test]
        public void ValidSexType()
        {
            var geneCollection = new RabbitGeneCollection();
            Assert.AreEqual(expected: typeof(bool), actual: geneCollection.IsMale.GetType());
        }

        [Test]
        public void ValidGeneInheritenceType()
        {
            var geneCollection1 = new RabbitGeneCollection();
            var geneCollection2 = new RabbitGeneCollection();
            var newGeneCollection = geneCollection1.GenerateNewGeneCollection(geneCollection1, geneCollection2);
            Assert.AreEqual(expected: typeof(RabbitGeneCollection), actual: newGeneCollection.GetType());
        }

        [Test]
        public void ValidMutatedSpeedValue()
        {
            var geneCollection1 = new RabbitGeneCollection(7f, 6f, 8f, 7f, 4f, 5f, 8f, 2f, 6f, true);
            var geneCollection2 = new RabbitGeneCollection(9f, 4f, 5f, 6f, 2f, 6f, 9f, 3f, 5f, false);
            var newGeneCollection = geneCollection1.GenerateNewGeneCollection(geneCollection1, geneCollection2);
            Assert.IsTrue(newGeneCollection.Speed >= 0f);
        }

        [Test]
        public void ValidMutatedSizeValue()
        {
            var geneCollection1 = new RabbitGeneCollection(7f, 6f, 8f, 7f, 4f, 5f, 8f, 2f, 6f, true);
            var geneCollection2 = new RabbitGeneCollection(9f, 4f, 5f, 6f, 2f, 6f, 9f, 3f, 5f, false);
            var newGeneCollection = geneCollection1.GenerateNewGeneCollection(geneCollection1, geneCollection2);
            Assert.IsTrue(newGeneCollection.Size >= 0f);
        }

        [Test]
        public void ValidMutatedHearingDistanceValue()
        {
            var geneCollection1 = new RabbitGeneCollection(7f, 6f, 8f, 7f, 4f, 5f, 8f, 2f, 6f, true);
            var geneCollection2 = new RabbitGeneCollection(9f, 4f, 5f, 6f, 2f, 6f, 9f, 3f, 5f, false);
            var newGeneCollection = geneCollection1.GenerateNewGeneCollection(geneCollection1, geneCollection2);
            Assert.IsTrue(newGeneCollection.HearingDistance >= 0f);
        }

        [Test]
        public void ValidMutatedSightDistanceValue()
        {
            var geneCollection1 = new RabbitGeneCollection(7f, 6f, 8f, 7f, 4f, 5f, 8f, 2f, 6f, true);
            var geneCollection2 = new RabbitGeneCollection(9f, 4f, 5f, 6f, 2f, 6f, 9f, 3f, 5f, false);
            var newGeneCollection = geneCollection1.GenerateNewGeneCollection(geneCollection1, geneCollection2);
            Assert.IsTrue(newGeneCollection.SightDistance >= 0f);
        }

        [Test]
        public void ValidMutatedLifeExpectancyValue()
        {
            var geneCollection1 = new RabbitGeneCollection(7f, 6f, 8f, 7f, 4f, 5f, 8f, 2f, 6f, true);
            var geneCollection2 = new RabbitGeneCollection(9f, 4f, 5f, 6f, 2f, 6f, 9f, 3f, 5f, false);
            var newGeneCollection = geneCollection1.GenerateNewGeneCollection(geneCollection1, geneCollection2);
            Assert.IsTrue(newGeneCollection.LifeExpectancy >= 0f);
        }

        [Test]
        public void ValidMutatedGestationValue()
        {
            var geneCollection1 = new RabbitGeneCollection(7f, 6f, 8f, 7f, 4f, 5f, 8f, 2f, 6f, true);
            var geneCollection2 = new RabbitGeneCollection(9f, 4f, 5f, 6f, 2f, 6f, 9f, 3f, 5f, false);
            var newGeneCollection = geneCollection1.GenerateNewGeneCollection(geneCollection1, geneCollection2);
            Assert.IsTrue(newGeneCollection.Gestation >= 0f);
        }

        [Test]
        public void ValidMutatedReproductiveUrgeValue()
        {
            var geneCollection1 = new RabbitGeneCollection(7f, 6f, 8f, 7f, 4f, 5f, 8f, 2f, 6f, true);
            var geneCollection2 = new RabbitGeneCollection(9f, 4f, 5f, 6f, 2f, 6f, 9f, 3f, 5f, false);
            var newGeneCollection = geneCollection1.GenerateNewGeneCollection(geneCollection1, geneCollection2);
            Assert.IsTrue(newGeneCollection.ReproductiveUrge >= 0f);
        }

        [Test]
        public void ValidMutatedGrowthTimeValue()
        {
            var geneCollection1 = new RabbitGeneCollection(7f, 6f, 8f, 7f, 4f, 5f, 8f, 2f, 6f, true);
            var geneCollection2 = new RabbitGeneCollection(9f, 4f, 5f, 6f, 2f, 6f, 9f, 3f, 5f, false);
            var newGeneCollection = geneCollection1.GenerateNewGeneCollection(geneCollection1, geneCollection2);
            Assert.IsTrue(newGeneCollection.GrowthTime >= 0f);
        }

        [Test]
        public void ValidMutatedDesirabilityValue()
        {
            var geneCollection1 = new RabbitGeneCollection(7f, 6f, 8f, 7f, 4f, 5f, 8f, 2f, 6f, true);
            var geneCollection2 = new RabbitGeneCollection(9f, 4f, 5f, 6f, 2f, 6f, 9f, 3f, 5f, false);
            var newGeneCollection = geneCollection1.GenerateNewGeneCollection(geneCollection1, geneCollection2);
            Assert.IsTrue(newGeneCollection.Desirability >= 0f);
        }
        */
    }
}
