using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Game
{
    public class InfoBox 
    {
        private const float X = 0;
        private const float Y = 0;
        private const float WIDTH = 240;
        private const float HEIGHT = 150;
        private const float PAD = 10;
        private const float HEADER_HEIGHT = 40;

        private Text title;
        private Text meta;
        private Image image;
        private Text details;

        public InfoBox(GameObject canvas)
        {
            var font = Resources.Load<Font>("Fonts/visitor1");
            var fontMaterial = Resources.Load<Material>("Fonts/visitor1");
            var boxObject = new GameObject();
            var transform = boxObject.AddComponent<RectTransform>();
            transform.SetParent(canvas.transform);
            transform.anchorMin = new Vector2(1, 0);
            transform.anchorMax = new Vector2(1, 0);
            transform.pivot = new Vector2(1, 0);
            transform.offsetMin = new Vector2(X - WIDTH, Y);
            transform.offsetMax = new Vector2(X, Y + HEIGHT);
            var background = boxObject.AddComponent<Image>();
            background.sprite = canvas.GetComponentInChildren<Image>().sprite;
            background.type = Image.Type.Sliced;
            background.color = new Color(0,0,0,(float) 0.60);

            const float TITLE_WIDTH = WIDTH - PAD - PAD - PAD - HEADER_HEIGHT;

            var titleObject = new GameObject();
            transform = titleObject.AddComponent<RectTransform>();
            transform.SetParent(boxObject.transform);
            transform.anchorMin = new Vector2(0, 1);
            transform.anchorMax = new Vector2(0, 1);
            transform.pivot = new Vector2(0, 1);
            transform.offsetMin = new Vector2(PAD, -PAD - HEADER_HEIGHT * 0.6f);
            transform.offsetMax = new Vector2(PAD + TITLE_WIDTH, -PAD);
            title = titleObject.AddComponent<Text>();
            title.text = "Title";
            title.font = font;
            title.material = fontMaterial;
            title.fontSize = 14;
            title.color = Color.white;
            title.alignment = TextAnchor.MiddleLeft;
            Shadow shadow = titleObject.AddComponent<Shadow>();
            shadow.effectColor = new Color(0, 0, 0);

            var metaObject = new GameObject();
            transform = metaObject.AddComponent<RectTransform>();
            transform.SetParent(boxObject.transform);
            transform.anchorMin = new Vector2(0, 1);
            transform.anchorMax = new Vector2(0, 1);
            transform.pivot = new Vector2(0, 1);
            transform.offsetMin = new Vector2(PAD, -PAD - HEADER_HEIGHT);
            transform.offsetMax = new Vector2(PAD + TITLE_WIDTH, -PAD - HEADER_HEIGHT * 0.4f);
            meta = metaObject.AddComponent<Text>();
            meta.text = "Cost";
            meta.font = Resources.GetBuiltinResource<Font>("Arial.ttf"); 
            meta.fontSize = 10;
            meta.color = Color.green;
            meta.alignment = TextAnchor.MiddleLeft;

            var imageObject = new GameObject();
            transform = imageObject.AddComponent<RectTransform>();
            transform.SetParent(boxObject.transform);
            transform.anchorMin = new Vector2(1, 1);
            transform.anchorMax = new Vector2(1, 1);
            transform.pivot = new Vector2(1, 1);
            transform.offsetMin = new Vector2(-PAD - HEADER_HEIGHT, -PAD - HEADER_HEIGHT);
            transform.offsetMax = new Vector2(-PAD, -PAD);
            image = imageObject.AddComponent<Image>();
            image.preserveAspect = true;

            var detailsObject = new GameObject();
            transform = detailsObject.AddComponent<RectTransform>();
            transform.SetParent(boxObject.transform);
            transform.anchorMin = new Vector2(0, 0);
            transform.anchorMax = new Vector2(1, 0);
            transform.pivot = new Vector2(0, 0);
            transform.offsetMin = new Vector2(PAD, PAD);
            transform.offsetMax = new Vector2(-PAD, HEIGHT - HEADER_HEIGHT - PAD - PAD);
            details = detailsObject.AddComponent<Text>();
            details.text = "Details";
            details.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            details.fontSize = 12;
            details.color = Color.white;

            SetInfo(null);
        }

        public void SetInfo(InfoBoxSource source)
        {
            if (source == null)
            {
                title.text = "Nothing selected";
                meta.text = "";
                image.sprite = null;
                image.color = Color.clear;
                details.text = "Select something to build to get started, or click a tile to learn more about it.";
            }
            else
            {
                source.GetInfoBoxData(out string title, out string meta, out Sprite sprite, out string details);
                this.title.text = title;
                this.meta.text = meta;
                this.image.sprite = sprite;
                this.image.color = Color.white;
                this.details.text = details;
            }
        }
    }

    public interface InfoBoxSource
    {
        void GetInfoBoxData(out string title, out string meta, out Sprite sprite, out string details);
    }
}
