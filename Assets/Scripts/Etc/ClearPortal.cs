using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearPortal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StageManager.instance.LastStageUp();
            GameManager.instance.NextGame();
        }
    }
}
