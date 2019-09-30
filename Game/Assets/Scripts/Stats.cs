using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Stats : MonoBehaviour
    {
        public double CO2 { get; private set; }
        public double Temperature { get; private set; }
        public int Population { get; private set; }
        public double ElectricCapacity { get; private set; }
        public double Reputation { get; private set; }
        public double Score { get; private set; }
        public double Wealth { get; private set; }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public static Stats operator +(Stats a, Stats b)
        {
            Stats sum = new Stats();
            sum.AddContribution(a);
            sum.AddContribution(b);
            return sum;
        }

        public void AddContribution(Stats stats)
        {
            CO2 += stats.CO2;
            // etc.
            throw new System.NotImplementedException();
        }
    }
}