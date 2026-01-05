using UnityEngine;
using System.Collections.Generic;

public class ManagerMarineros : MonoBehaviour
{
    
// [Header("Datos de Compra")]
//     public string marineroName;
//     public string strongWith;
//     public string weakWith;
//     public string description;
//     public int price;

[Header("Datos de Combate")]
    public  bool enemiesInArea = false;
    protected List<GameObject> enemiesInTrigger = new List<GameObject>();



    protected virtual void Start()
    {
        enemiesInTrigger.Clear();
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger con: " + other.name + " (tag: " + other.tag + ")");
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Entro un enemigo");
            if (!enemiesInTrigger.Contains(other.gameObject))
            {
                enemiesInTrigger.Add(other.gameObject);
                enemiesInArea = true;
            }
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Salió un enemigo");
            if (enemiesInTrigger.Contains(other.gameObject))
            {
                enemiesInTrigger.Remove(other.gameObject);

                // Si ya no quedan enemigos, marcar el área como libre
                if (enemiesInTrigger.Count == 0)
                {
                    enemiesInArea = false;
                }
            }
        }
    }



    public GameObject FindClosestEnemy()
    {
        if (enemiesInTrigger.Count == 0)
            return null;
        
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        
        foreach (GameObject enemy in enemiesInTrigger)
        {
            if (enemy != null)
            {
                float distance = Vector3.Distance(currentPosition, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy;
                }
            }
        }
        
        return closestEnemy;
    }
}