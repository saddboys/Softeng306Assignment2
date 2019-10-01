using Game;
using Game.CityMap;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = System.Random;

namespace Game
{
    public class Tower : Structure
    {
        public Tower(GameObject canvas, Vector3 vector) : base(canvas, vector)
        {
            Random random = new Random();
            Cost = random.Next(1, 50);
            Create(44, new Vector2(1, 1.5f));
        }

        public override Stats GetStatsContribution()
        {
            throw new System.NotImplementedException();
        }
    }
}
