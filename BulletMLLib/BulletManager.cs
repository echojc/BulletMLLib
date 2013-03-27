
namespace BulletMLLib
{
	public static class BulletMLManager
	{
		static IBulletMLManager ib;
		static public void Init(IBulletMLManager ib1)
		{
			ib = ib1;
		}

		static public float GetRandom() { return ib.GetRandom(); }

		static public float GetRank() { return ib.GetRank(); }

		static public float GetShipPosX() { return ib.GetShipPosX(); }

		static public float GetShipPosY() { return ib.GetShipPosY(); }

	}
}