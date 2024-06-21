using UnityEngine;

public static class RotationCalculations
{
	public static float CalculateTowerAngleByEnemyPosition(Vector3 towerPosition, Vector3 enemyPosition)
	{
		// calculates oppositeCathetus with base on x axis
		float oppositeCathetus = towerPosition.x - enemyPosition.x;
		// calculates oppositeCathetus with base on z axis
		float adyacentCathetus = towerPosition.z - enemyPosition.z;
		// calculates radians using Atan (ca/co)
		float radians = Mathf.Atan2(oppositeCathetus, adyacentCathetus);
		// calculates degrees converting radians
		float degrees = radians * Mathf.Rad2Deg;

		// return degrees
		return degrees;
	}

	public static float CalculateTowerAngleByDirection(Directions direction)
	{
		// usually towers angle 0 is aiming to the right, so is important to start with -90 degrees to emulate a forward aiming as 0
		float defaultFactor = GameDefaults.defaultTowerInitAngle;

		// set the angle to the related direction
		float angle = CalculateAngleByDirection(direction);

		// return the angle
		return defaultFactor + angle;
	}

	public static float CalculateAngleByDirection(Directions direction)
	{
		float angle = 0f;

		// set the angle to the related direction
		switch (direction)
		{
			case Directions.FORWARD:
				angle = 0f;
				break;
			case Directions.RIGHT:
				angle = 90f;
				break;
			case Directions.BACKWARD:
				angle = 180f;
				break;
			case Directions.LEFT:
				angle = 270f;
				break;
		}

		// return the angle
		return angle;
	}
}