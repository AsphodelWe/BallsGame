using UnityEngine;
using System.Collections;

namespace EpicToonFX
{
    public class ETFXLightFade : MonoBehaviour
    {
        [Header("Seconds to dim the light")]
        public float life = 0.2f;
        public bool killAfterLife = true;

        private float initIntensity;

        private Light lighting;

        void Start()
        {
            lighting = GetComponent<Light>();

            if (lighting)
            {
                initIntensity = lighting.intensity;
            }
            else
                print("No light object found on " + gameObject.name);
        }
        void Update()
        {
            if (lighting)
            {
                lighting.intensity -= initIntensity * (Time.deltaTime / life);
                if (killAfterLife && lighting.intensity <= 0)
					Destroy(gameObject.GetComponent<Light>());
            }
        }
    }
}