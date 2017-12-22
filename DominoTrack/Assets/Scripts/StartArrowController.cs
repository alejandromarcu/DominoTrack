using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartArrowController : MonoBehaviour
{
    void Update()
    {
        gameObject.SetActive(Game.isBuilding);
    }
}
