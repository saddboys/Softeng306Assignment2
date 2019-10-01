using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Stats : MonoBehaviour
    {
        public double CO2 { get; set; }
        public double Temperature { get; set; }
        public int Population { get; set; }
        public double ElectricCapacity { get; set; }
        public double Reputation { get; set; }
        public double Score { get; set; }
        public double Wealth { get; set; }

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
