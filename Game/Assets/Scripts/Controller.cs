using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Controller : MonoBehaviour
    {
        [SerializeField]
        private GameObject menu;

        [SerializeField]
        private GameObject statsBar;

        public void Quit()
        {
            Application.Quit();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && statsBar.activeSelf)
            {
                menu.SetActive(!menu.activeSelf);
            }
        }
    }
}
