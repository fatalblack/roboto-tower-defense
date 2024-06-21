using System.Linq;
using UnityEngine;

public class SlotTowerMovementService : MonoBehaviour
{
    // private variables
    private GameObject towerGo;
    private bool directionSetted;

    // Start is called before the first frame update
    private void Start()
    {
        towerGo = GetTower();
    }

    // Update is called once per frame
    private void Update()
    {
        // if a tower was removed must refresh this on the towerGo variable
		if (GetTower() == null && directionSetted)
		{
            directionSetted = false;
        }

        // if a tower was not setted up yet, check if exists one on the slot and assign it
		if (towerGo == null && GetTower() != null)
		{
            // gets tower
            towerGo = GetTower();

            // updates tower angle
            UpdateTowerAngle();
        }
    }

    private GameObject GetTower()
	{
        return gameObject.GetComponentsInChildren<Transform>().FirstOrDefault(component => component.CompareTag(Tags.Tower))?.gameObject;
    }

    private void UpdateTowerAngle()
	{
        // validates if is a tower on the slot and was not setted up yet
        if (towerGo != null && !directionSetted)
        {
            // set directionSetted as true
            directionSetted = true;

            // change the tower y angle
            towerGo.transform.Rotate(Vector3.up, RotationCalculations.CalculateTowerAngleByDirection(Directions.LEFT), Space.World);
        }
    }
}