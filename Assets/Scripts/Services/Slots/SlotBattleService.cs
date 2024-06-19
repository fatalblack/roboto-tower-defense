using System.Linq;
using UnityEngine;

public class SlotBattleService : MonoBehaviour
{
    // private variables
    private bool isEmptySlot;
    private GameObject tower;

    // Start is called before the first frame update
    private void Start()
    {
        // initializa as empty slot and no tower
        isEmptySlot = true;
        tower = null;
    }

	private void OnTriggerEnter(Collider other)
	{
		// verify if collides with a tower and slot is empty
		if (other.gameObject.CompareTag(Tags.Tower) && isEmptySlot)
		{
            // sets actual tower
            tower = other.gameObject;

            // marks as filled slot
            isEmptySlot = false;

            // reposition the tower to the center of the slot
            tower.transform.position = GetTowerNewPosition();
        }
	}

    public bool GetIsEmptySlot()
	{
        // refresh isEmptySlot value
        isEmptySlot = !gameObject.GetComponentsInChildren<Transform>().Any(component => component.CompareTag(Tags.Tower));

        // return isEmptySlot
        return isEmptySlot;
	}

    private Vector3 GetTowerNewPosition()
	{
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y + 0.01f, transform.position.z);

        return newPosition;
	}
}