using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.CityMap;

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
        //load structure items dynamically, assume static here
        //Populate();
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
        //check if the button is clickable 
        StructureFactory sf = new RockFactory();
        
        if(sf.CanBuild(out string reason)){
        //only one component now for testing use
        GameObject rockBtn = GameObject.FindWithTag("RockButton");
        string tag = "RockButton";
        Debug.Log("find the object with tag" + tag);
        
        }else{
        //deselect the buttons 
        GameObject btn = GameObject.Find("RockButton").GetComponent<Button>();
        
        }
        }

        //call method in StructureFactory create a specified strcuture component
        // void CreateStructure(string tag){
        
        // }

        //add the strcuture component to the grid at a specified position
        void ClickGrid(){
        
        }


    }
}
