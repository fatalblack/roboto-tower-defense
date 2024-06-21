using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// for shootingArea
public class TowerMovementService : MonoBehaviour
{
    // private variables
    private GameObject towerGo;

    // Start is called before the first frame update
    void Start()
    {
        towerGo = gameObject.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
