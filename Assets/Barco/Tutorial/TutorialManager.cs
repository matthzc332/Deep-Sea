// using System.Collections;
// using Unity.VisualScripting;
// using UnityEngine;

// public class TutorialManager : MonoBehaviour
// {
//     public GameObject blackBackgroud;
//     public GameObject tutorialShootHand;
//     public GameObject tutorialMoveHand;
//     public GameObject playerTutorial;
//     public GameObject enemyTutorial;
//     public GameObject enemySpawner;
//     public GameObject waveController;
//     private GameObject hand;
//     public Vector3 enemyTutorialPosition;

//     public bool isPaused = false;


//     public void StartShootTutorial(Vector3 handPosition)
//     {
//         blackBackgroud.SetActive(true);
//         hand = Instantiate(tutorialShootHand,handPosition,Quaternion.identity);
//         playerTutorial.GetComponent<TutorialPlayerController>().enabled = true;
//         isPaused = true;
//         Time.timeScale = 0f;
//     }

//     public void AfterShoot()
// {
//     // 1. Primero habilita todos los componentes necesarios
//     if (playerTutorial != null)
//     {
//         var handCannon = playerTutorial.GetComponent<HandCannon>();
//         var ship = playerTutorial.GetComponent<Ship>();
//         if (handCannon != null) handCannon.enabled = true;
//         if (ship != null) ship.enabled = true;
//     }

//     if (enemyTutorial != null)
//     {
//         var collider = enemyTutorial.GetComponent<BoxCollider2D>();
//         var globo = enemyTutorial.GetComponent<Globo>();
//         if (collider != null) collider.enabled = true;
//         if (globo != null) globo.enabled = true;
//     }

//     // 2. Activa los sistemas del juego
//     if (enemySpawner != null) enemySpawner.SetActive(true);
//     if (waveController != null) waveController.SetActive(true);

//     // 3. FINALMENTE destruye los objetos del tutorial
//     if (blackBackgroud != null)
//         Destroy(blackBackgroud);
    
//     if (hand != null)
//         Destroy(hand);

//     // 4. Inicia la siguiente parte del tutorial
//     StartCoroutine(SecondHandMovement());
// }






//     IEnumerator SecondHandMovement()
//     {
//         tutorialMoveHand.SetActive(true);
//         yield return new WaitForSeconds(5f);
//         Destroy(tutorialMoveHand);
//         // Destroy(gameObject);
//         enabled = false;
//     }


// }