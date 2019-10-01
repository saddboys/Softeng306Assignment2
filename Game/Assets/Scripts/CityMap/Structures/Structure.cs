using UnityEngine;
using UnityEngine.UI;

namespace Game.CityMap
{
    public abstract class Structure
    {
        /// <summary>
        /// Calculate how much this structure will contribute to the stats, such
        /// as CO2 generated, profits/losses, etc.
        /// </summary>
        /// <returns>The structure's stats contribution.</returns>
        public abstract Stats GetStatsContribution();

        private Image structureImage;
        private GameObject canvas;
        private Vector3 vector;

        public int Cost { get; set; }

        public Structure(GameObject canvas, Vector3 vector)
        {
            this.canvas = canvas;
            this.vector = vector;
        }

        /// <summary>
        /// Adds the created structure to the canvas via an Image.
        /// </summary>
        /// <param name="spriteNumber"></param>
        /// <param name="imageSize"></param>
        protected void Create(int spriteNumber, Vector2 imageSize)
        {
            GameObject structure = new GameObject();
            Image image = structure.AddComponent<Image>();
            Sprite[] sprites = Resources.LoadAll<Sprite>("Textures/structures");
            image.sprite = sprites[spriteNumber];
            Vector2 structureSize = imageSize;
            // Determines the size of the structure
            structure.GetComponent<RectTransform>().sizeDelta = structureSize;
            Vector2 xy = new Vector2(vector.x + structureSize.x - 1, vector.y + structureSize.y - 1);
            // Determines where the structure will be placed
            structure.GetComponent<RectTransform>().anchoredPosition = xy;
            structure.GetComponent<RectTransform>().SetParent(canvas.transform);
            structure.SetActive(true);
        }
    }
}
