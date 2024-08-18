using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _StarShip
{
    public class SpeedUptoItem : MonoBehaviour
    {
        static public SpeedUptoItem Instance;
        [Header("Speed min: 7")]
        public float speedUpTo;
        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
            }
        }
    }
}
