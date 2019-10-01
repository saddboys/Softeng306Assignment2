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

        private GameObject gameObject;

        public int Cost { get; set; }

        /// <summary>
        /// Adds the created structure to the canvas via an Image.
        /// </summary>
        /// <param name="spriteNumber"></param>
        /// <param name="imageSize"></param>
        protected void RenderOnto(GameObject canvas, Vector3 position, int spriteNumber, Vector2 imageSize)
        {
            Unrender();
            gameObject = new GameObject();
            Image image = gameObject.AddComponent<Image>();
            Sprite[] sprites = Resources.LoadAll<Sprite>("Textures/structures");
            image.sprite = sprites[spriteNumber];
            Vector2 structureSize = imageSize;
            // Determines the size of the structure
            gameObject.GetComponent<RectTransform>().sizeDelta = structureSize;
            Vector2 xy = new Vector2(position.x + structureSize.x - 1, position.y + structureSize.y - 1);
            // Determines where the structure will be placed
            gameObject.GetComponent<RectTransform>().anchoredPosition = xy;
            gameObject.GetComponent<RectTransform>().SetParent(canvas.transform);
            gameObject.SetActive(true);
        }

        public abstract void RenderOnto(GameObject canvas, Vector3 position);

        public void Unrender()
        {
            if (gameObject != null)
            {
                Object.Destroy(gameObject);
            }
        }
    }
}
