using System.Collections;
using UnityEngine;

namespace NaturalSelectionSimulation
{
    public class Grass: MonoBehaviour
    {
        #region Public Variables

        public bool IsAlive = true;
        public Vector3 Position;
        public Vector3 originalScale;

        #endregion

        #region Private Variables


        #endregion

        #region Unity Methods

        private void Awake()
        {
            Position = transform.position;
            originalScale = transform.localScale;
        }

        #endregion

        #region Public Methods

        public void KillPlant()
        {
            StartCoroutine(GrassDeathSimulation());
        }

        public void SpawnPlant()
        {
            StartCoroutine(GrassSpawnSimulation());
        }

        #endregion

        #region Private Methods

        private IEnumerator GrassDeathSimulation()
        {
            //Kill Plant
            Debug.Log("Killing Plant...");
            yield return ScaleDownToTarget(new Vector3(0, 0, 0), 2f);
            IsAlive = false;

            //Wait for the duration
            float counter = 0;
            while (counter < 60f)
            {
                counter += Time.deltaTime;
                yield return null;
            }

            //Spawn Plant
            Debug.Log("Plant re-spawning...");
            yield return ScaleUpToTarget(originalScale, 2f);
            IsAlive = true;
        }

        private IEnumerator GrassSpawnSimulation()
        {
            yield return ScaleUpToTarget(originalScale, 2f);
            IsAlive = true;
        }

        private IEnumerator ScaleUpToTarget(Vector3 targetScale, float scaleDuration)
        {
            while (transform.localScale != targetScale)
            {
                float counter = 0f;

                while (counter < scaleDuration)
                {
                    counter += Time.deltaTime;
                    yield return null;
                }

                if (targetScale.x - transform.localScale.x > .5f)
                {
                    transform.localScale = new Vector3(transform.localScale.x + .5f, transform.localScale.y + .5f, transform.localScale.z + .5f);
                }
                else
                {
                    transform.localScale = targetScale;
                }
            }
        }

        private IEnumerator ScaleDownToTarget(Vector3 targetScale, float scaleDuration)
        {
            while (transform.localScale != targetScale)
            {
                float counter = 0f;

                while (counter < scaleDuration)
                {
                    counter += Time.deltaTime;
                    yield return null;
                }

                if (transform.localScale.x - targetScale.x > .5f)
                {
                    transform.localScale = new Vector3(transform.localScale.x - .5f, transform.localScale.y - .5f, transform.localScale.z - .5f);
                }
                else
                {
                    transform.localScale = targetScale;
                }
            }
        }

        #endregion
    }
}