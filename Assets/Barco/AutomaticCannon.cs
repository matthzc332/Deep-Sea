using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
public class AutomaticCannon : Guns
{
    public GameObject bulletPrefab;

    public float timeShoot = 2;
    void Start()
    {
        ammunition = 20000;
        speed = 5f;
        power = 5;
        StartCoroutine(shoot());

    }


    IEnumerator shoot()
    {
        while (!empty())
        {
            Vector3 enemy = findEnemy();

            if (enemy == Vector3.zero)
            {
                yield return null;
                continue;
            }

            var b = Instantiate(bulletPrefab, transform.position, Quaternion.identity).GetComponent<Bullet>();
            b.Initialize(enemy, initialSpeed: 18f, power: 2); 
        
            yield return new WaitForSeconds(timeShoot);
            ammunition -= 1;
        }
    }

    public Vector3 findEnemy()
    {
        Entity[] enemies = UnityEngine.Object.FindObjectsByType<Entity>(FindObjectsSortMode.None);
        if (enemies.Length == 0) return Vector3.zero;

        Entity near = null;
        float minDistance = Mathf.Infinity;

        foreach (Entity e in enemies)
        {
            float dist = Vector3.Distance(transform.position, e.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                near = e;
            }
        }

        return near.transform.position;

    }
}