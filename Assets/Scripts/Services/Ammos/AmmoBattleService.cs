using UnityEngine;

public class AmmoBattleService : MonoBehaviour
{
    // public variables
    public Vector3 destinyPosition;

    // private variables
    private bool initiated;
    private Vector3 calculatedDestinyPosition;
    private float ammoMoveSpeedFactor;

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        // if the gameObject was initiated from the shooting area we can continue
		if (initiated)
		{
            // move the ammo to the target
            MoveToTarget();
        }
    }

	private void OnTriggerEnter(Collider other)
	{
        // if we collides with the target or the ground must destroy the ammo
		if (other.gameObject.CompareTag(Tags.Enemy) || other.gameObject.CompareTag(Tags.Ground))
		{
            Destroy(gameObject);
		}
	}

	public void Initiate(Vector3 destinyPosition, float ammoMoveSpeedFactor)
	{
        this.destinyPosition = destinyPosition;
        this.ammoMoveSpeedFactor = ammoMoveSpeedFactor;

        initiated = true;
    }

    private void CalculateDestinyPosition()
	{
        // calculates the destiny position to move the enemy
        calculatedDestinyPosition = destinyPosition - transform.position;
        calculatedDestinyPosition *= ammoMoveSpeedFactor * Time.deltaTime;
    }

    private void MoveToTarget()
	{
        
        // calculates the destiny position to move the enemy
        CalculateDestinyPosition();

        // moves the ammo
        transform.Translate(calculatedDestinyPosition, Space.World);
    }
}