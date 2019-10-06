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
            city.Map.TileClickedEvent += (s, e) =>
            {
                // TODO: handle when the tile e.Tile has been clicked.
                Debug.Log(e.Tile);
                throw new System.NotImplementedException();
            };
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
        
        }

        //initialise list of structures on the toolbar
        void Populate(){
 
        }

        //trigger button select event 
        void OnSelect(){

        }

        //call method in StructureFactory create a specified strcuture component
        void CreateStructure(string tag){
        
        }

        //add the strcuture component to the grid at a specified position
        void AddToGrid(){

        }


    }
}
