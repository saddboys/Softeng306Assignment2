using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.CityMap;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Linq;

namespace Game
/**
 * This class handles the cheat codes when the "`" tilde key is pressed
 */
{
    public class Cheats : MonoBehaviour
    {
        private City city;
        String editStr;
        private Boolean isOn;

        void OnGUI()
        {
            
            if((Event.current.Equals(Event.KeyboardEvent ("`"))))
            {
                if (!isOn)
                {
                    Debug.Log("on");
                    isOn = true;
                }
                else
                {
                    Debug.Log("off");
                    isOn = false;
                }
            }
            
            if(isOn)
            {
                GUI.SetNextControlName("cheatConsole");
                GUI.FocusControl("cheatConsole");
                if(!Event.current.Equals(Event.KeyboardEvent ("`")))
                {
                    editStr = editStr.Replace("`", "");
                    editStr = GUI.TextField(new Rect(15, 50, 300, 20), editStr, 200);
                    if (Event.current.type == EventType.KeyDown && Event.current.character == "\n"[0])
                    {
                        Cheat(editStr);
                        editStr = "";
                    }

                }

            }
        }

        /**
         * Cheat codes:
         *     <name> <number>
         *
         *     names:
         *         pops        increase population
         *         cash        increase money
         *         happiness   increase temperature
         *         energy      increase electricity
         *         co2         increase CO2 output per turn
         *         temp        increase city temperature
         *
         *     number:
         *         increase the stat specified by <name> by amount specified here. Negative numbers allowed.
         */
        private void Cheat(String str)
        {
            String[] parameters = str.Split(' ');

            
            if (parameters.Length > 1) 
            {
                Boolean isNum = int.TryParse(parameters[1], out int number);

                if (isNum)
                {
                    switch (parameters[0])
                    {
                        case ("pops"):
                            Debug.Log("Increase pops by " + parameters[1]);
                            city.Stats.Population += number;
                            break;
                        case ("cash"):
                            Debug.Log("Increase cash by " + parameters[1]);
                            city.Stats.Wealth += number;
                            break;
                        case ("happiness"):
                            Debug.Log("Increase reputation by " + parameters[1]);
                            city.Stats.Reputation += number;
                            break;
                        case ("energy"):
                            Debug.Log("Increase electricity by " + parameters[1]);
                            city.Stats.ElectricCapacity += number;
                            break;
                        case ("co2"):
                            Debug.Log("Increase co2 output by " + parameters[1]);
                            city.Stats.CO2 += number;
                            break;
                        case ("temp"):
                            Debug.Log("Increase temperature by " + parameters[1]);
                            city.Stats.Temperature += number;
                            break;
                    }
                }
            }

        }

        private void Start()
        {
            city = GameObject.FindObjectOfType<City>();
            editStr = "";
            isOn = false;
        }
        
    }
}