using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kortge
{
    public class Damage : MonoBehaviour
    {
        public bool player;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if ((player && other.CompareTag("Player"))||(!player && other.CompareTag("Boss")))
            {
                Health health = other.GetComponent<Health>();
                health.Damage();
                print("Ow!");
            }
        }
    }
}