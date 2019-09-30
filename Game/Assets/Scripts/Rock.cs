
    using UnityEngine;

    public class Rock : Structure
    {
        public Rock(GameObject canvas, Vector3 vector) : base(canvas, vector)
        {
            cost = 20;
            Create(29,new Vector2(1,1.5f));
        }
    }