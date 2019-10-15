// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using CodeMonkey.Utils;

// public class GameHandler : MonoBehaviour {

//     [SerializeField] private HealthBar healthBar;

// 	private void Start () {
//         float health = 1f;
//         FunctionPeriodic.Create(() => {
//             if (health > .01f) {
//                 health -= .01f;
//                 healthBar.SetSize(health);

//                 if (health < .3f) {
//                     // Under 30% health
//                     if ((int)(health * 100f) % 3 == 0) {
//                         healthBar.SetColor(Color.white);
//                     } else {
//                         healthBar.SetColor(Color.red);
//                     }
//                 }
//             } else {
//                 health = 1f;
//                 healthBar.SetColor(Color.red);
//             }
//         }, .05f);
// 	}
// }
