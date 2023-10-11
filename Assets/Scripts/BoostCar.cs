using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostCar : MonoBehaviour
{
    public CarController player;
    public CarController player2;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.name == "Player")
        {
            player.BoostCar();
            DestroyObject(gameObject);
        }
        if (collision.name == "Player2")
        {
            player2.BoostCar();
            DestroyObject(gameObject);
        }
    }
}
