
    using Game;
    using Game.CityMap;
    using UnityEngine;

    public class Rock : Structure
    {
        public Rock(GameObject canvas, Vector3 vector) : base(canvas, vector)
        {
            Cost = 20;
            Create(29,new Vector2(1,1.5f));
        }

        public override Stats GetStatsContribution()
        {
            throw new System.NotImplementedException();
        }
    }