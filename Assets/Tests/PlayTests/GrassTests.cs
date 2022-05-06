using System.Collections;
using System.Collections.Generic;
using NaturalSelectionSimulation;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace NSS.PlayTests
{
    public class GrassTests
    {
        [UnityTest]
        public IEnumerator Grass_WillBeSetAsDeadInUnder10Seconds_WhenKillPlantIsCalled()
        {
            var gameObject = new GameObject();
            var grass = gameObject.AddComponent<Grass>();

            grass.KillPlant();

            yield return new WaitForSeconds(10f);

            Assert.AreEqual(grass.IsAlive, false);
        }

        [UnityTest]
        public IEnumerator Grass_WillScaleToZeroInUnder10Seconds_WhenKillPlantIsCalled()
        {
            var gameObject = new GameObject();
            var grass = gameObject.AddComponent<Grass>();

            grass.KillPlant();

            yield return new WaitForSeconds(10f);

            Assert.AreEqual(grass.transform.localScale, new Vector3(0,0,0));
        }

        [UnityTest]
        public IEnumerator Grass_WillBeSetAsAliveInUnder10Seconds_WhenSpawnPlantIsCalled()
        {
            var gameObject = new GameObject();
            var grass = gameObject.AddComponent<Grass>();
            grass.IsAlive = false;

            grass.SpawnPlant();

            yield return new WaitForSeconds(10f);

            Assert.AreEqual(grass.IsAlive, true);
        }

        [UnityTest]
        public IEnumerator Grass_WillScaleToOriginalScaleInUnder10Seconds_WhenSpawnPlantIsCalled()
        {
            var gameObject = new GameObject();
            var grass = gameObject.AddComponent<Grass>();
            grass.transform.localScale = new Vector3(0, 0, 0);

            grass.SpawnPlant();

            yield return new WaitForSeconds(10f);

            Assert.AreEqual(grass.transform.localScale, grass.originalScale);
        }
    }
}
