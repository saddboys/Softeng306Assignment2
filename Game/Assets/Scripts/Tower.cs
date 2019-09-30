
    using UnityEngine;
    using UnityEngine.Tilemaps;
    using Random = System.Random;

    public class Tower : Structure
    {
        public Tower(GameObject canvas, Vector3 vector) : base(canvas, vector)
        {
            Random random = new Random();
            cost =  random.Next(1, 50);
            Create(44,new Vector2(1,1.5f));
        }
    }