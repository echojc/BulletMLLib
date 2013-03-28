
namespace BulletMLLib
{
	/// <summary>
	/// A thing for talking to the game
	/// </summary>
	public  class BulletMLManager
	{
		#region Methods

		/// <summary>
		/// A callback method to get the difficulty of the game
		/// </summary>
		static public FloatDelegate GameDifficulty;

		/// <summary>
		/// a callback mathod to get current position of the player
		/// </summary>
		static public PositionDelegate PlayerPosition;

		#endregion //Methods
	}
}
