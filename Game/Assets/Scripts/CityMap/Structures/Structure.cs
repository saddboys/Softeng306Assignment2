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
        public virtual Stats GetStatsContribution()
        {
            return new Stats();
        }

        /// <summary>
        /// Calculate how much the stats will change when the structure is demolished.
        /// </summary>
        /// <returns></returns>
        public virtual Stats GetStatsChangeOnDemolish()
        {
            return new Stats();
        }

        private GameObject gameObject;

        /// <summary>
        /// Helper for Structure implementations to generate the required game object and image
        /// to render the structure onto the map.
        /// </summary>
        /// <param name="spriteNumber"></param>
        /// <param name="imageSize"></param>
        protected void RenderOnto(GameObject canvas, Vector3 position, int spriteNumber, Vector2 imageSize)
        {
            Unrender();
            gameObject = new GameObject();
            SpriteRenderer renderer = gameObject.AddComponent<SpriteRenderer>();
            Sprite sprite = Resources.LoadAll<Sprite>("Textures/structures/hexagonObjects_sheet")[spriteNumber];
            renderer.sprite = sprite;
            renderer.sortingLayerName = "Structure";

            // Sort. Higher = further behind, regardless of the order
            // in which the Structure was added.
            renderer.sortingOrder = -(int)position.y;

            float scale = 80 / sprite.rect.width;
            gameObject.transform.position = position;
            gameObject.transform.localScale = new Vector3(scale, scale, scale);
            gameObject.transform.SetParent(canvas.transform);
            gameObject.SetActive(true);
        }
        
        protected void RenderOntoSprite(GameObject canvas, Vector3 position, string spritePath, Vector2 imageSize)
        {
            Unrender();
            gameObject = new GameObject();
            
            SpriteRenderer renderer = gameObject.AddComponent<SpriteRenderer>();
            Sprite sprite = Resources.Load<Sprite>(spritePath);
            renderer.sprite = sprite;
            renderer.sortingLayerName = "Structure";
            
            // Sort. Higher = further behind, regardless of the order
            // in which the Structure was added.
            renderer.sortingOrder = -(int)position.y;

            float scale = 80 / sprite.rect.width;
            gameObject.transform.position = position;
            gameObject.transform.localScale = new Vector3(scale, scale, scale);
            gameObject.transform.SetParent(canvas.transform);
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Add the game objects to draw this structure onto the screen.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="position"></param>
        public abstract void RenderOnto(GameObject canvas, Vector3 position);

        /// <summary>
        /// Remove the structure from the screen.
        /// </summary>
        public void Unrender()
        {
            if (gameObject != null)
            {
                Object.Destroy(gameObject);
            }
        }
    }
}
