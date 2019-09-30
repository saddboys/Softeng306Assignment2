using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ToolBar : MonoBehaviour
    {
        public ToolBar(City city)
        {
            // E.g.
            city.Map.TileClickedEvent += HandleTileClick;
        }

        private void HandleTileClick(object sender, CityMap.TileClickArgs e)
        {
            throw new System.NotImplementedException();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}