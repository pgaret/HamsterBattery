using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets {
    class Battery {
        private float power;
        private int capacity;

        private const int RATE_COUNT = 64;

        private Queue<float> rates;
        private float rateTotal;
        private float currentRate;

        public Battery(int capacity) {
            this.capacity = capacity;
            power = 0;

            rates = new Queue<float>();
            for (int i = 0; i < RATE_COUNT; i++)
                rates.Enqueue(0);
            rateTotal = 0;
            currentRate = 0;
        }

        public void Add(float power) {
            power = Math.Min(capacity - this.power, power);
            currentRate += power;
            this.power += power;
        }

        // call at the end of update, after adding power
        public void ResetRate() {
            rates.Enqueue(currentRate);
            rateTotal -= rates.Dequeue();
            rateTotal += currentRate;
            currentRate = 0;
        }

        // average power gained per frame in the last RATE_COUNT frames
        public float GetAverageRate() {
            return rateTotal / RATE_COUNT;
        }

        public int GetCapacity() {
            return capacity;
        }

        public float GetAmountFilled() {
            return power;
        }

        public bool IsFilled() {
            return power == capacity;
        }

        public bool Sell(float cost) {
            if (cost <= power) {
                power -= cost;
                return true;
            }
            return false;
        }
    }
}
