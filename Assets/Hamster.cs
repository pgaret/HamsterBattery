using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets {
    class Hamster {
        private const float ENERGY_OUTPUT = 5/16f;
        
        public GameObject Object {
            get; private set;
        }

        public Hamster(GameObject blueprint, Vector3 position) {
            Object = GameObject.Instantiate(blueprint, position, Quaternion.identity) as GameObject;
        }

        public Hamster() { }

        public bool Tapped(Touch touch) {
            return touch.phase == TouchPhase.Began && Object.collider2D.OverlapPoint(Camera.main.ScreenToWorldPoint(touch.position));
        }

        public void Remove() {
            GameObject.Destroy(Object);
        }

        public float GetEnergyOutput() {
            return ENERGY_OUTPUT;
        }
    }
}
