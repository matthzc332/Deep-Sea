using UnityEngine;
using System.Collections;
using System;
public class Guns : MonoBehaviour
{
    public int ammunition;
    public int Level;
    public int power;
    public float speed;

    public bool empty()
    {
        return ammunition <= 0;
    }

    public void recharge(int municion)
    {
        ammunition += municion;
    }
}