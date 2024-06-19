public static class PlayerTowerCalculations
{
	public static int CalculateUpgradeCost(int toLevel, int upgradeCost, int upgradeCostMultiplierByLevel)
	{
		// set the initial value for upgrade cost
		int totalUpgradeCost = 0;

		// if level is not valid return 0
		if (toLevel < 1)
		{
			return totalUpgradeCost;
		}

		// set the given upgrade cost as initial value
		totalUpgradeCost = upgradeCost;

		// calculate the upgrade cost for each level and add to the total
		for (int i = 0; i < toLevel - 1; i++)
		{
			totalUpgradeCost *= upgradeCostMultiplierByLevel;
		}

		// return total
		return totalUpgradeCost;
	}

	public static int CalculateSellingPrice(int level, int buyCost, int upgradeCost, int upgradeCostMultiplierByLevel)
	{
		// set the initial values for total selling price and upgrade cost
		int totalSellingPrice = 0;
		int totalUpgradeCost = 0;

		// if level is not valid return 0
		if (level < 1)
		{
			return totalSellingPrice;
		}

		// set the given buy cost as initial value
		totalSellingPrice += buyCost;

		// if some upgrade were made calculates upgradeCost
		if (level > 1)
		{
			// set the given upgrade cost as initial value
			totalUpgradeCost = upgradeCost;

			// calculate the upgrade cost for each level and add to the upgrade total
			for (int i = 0; i < level - 1; i++)
			{
				totalUpgradeCost *= upgradeCostMultiplierByLevel;
			}
		}

		// sum the upgrade total to the total selling price
		totalSellingPrice += totalUpgradeCost;

		// return total
		return totalSellingPrice;
	}
}