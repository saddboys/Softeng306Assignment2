using System.Collections.Generic;
using Game.CityMap;
using UnityEngine;
using UnityEngine.Tilemaps;
using Terrain = Game.CityMap.Terrain;

namespace Game.Story.Events.RandomEvent
{
    public class FloodEvent : StoryEvent
    {
        public override string Title
        {
            get { return TITLE; }
        }

        public override string Description
        {
            get { return DESCRIPTION; }
        }

        public override Sprite EventImage
        {
            get { return SPRITE; }
        }

        public override Queue<string> Dialogues { get; }

        private const string TITLE = "FLOOD";
        private const string DESCRIPTION = "Flooding happened bad luck :(";
        private const Sprite SPRITE = null;
        public override void OnYesClick()
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>("Textures/terrain");
            // do something with the tiles here
            // This is currently just a placeholder
            MapTile[] tiles = StoryManager.city.Map.Tiles;
            for (int i = 0; i < 10; i++)
            {
                tiles[i].Structure = null;
                tiles[i].Terrain = new Terrain(Terrain.TerrainTypes.Ocean, sprites);
            }

            Destroy(StoryManager.storyManagerGameObject.GetComponent<CircusEvent>());
        }
    }
}