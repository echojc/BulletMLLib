

namespace BulletMLLib
{
	/// <summary>
	/// This is the interface that outisde assemblies will use to supply data for bullets.
    /// And maybe creating/destroying bullets? 
	/// </summary>
	public interface IBulletManager
	{
		#region Methods

		/// <summary>
		/// Gets the current X position of the player.
		/// This is used to target bullets at that position
		/// </summary>
		/// <returns>The X position to aim the bullet at</returns>
        float PlayerX { get; }

		/// <summary>
		/// Gets the current Y position of the player.
		/// This is used to target bullets at that position
		/// </summary>
		/// <returns>The Y position to aim the bullet at</returns>
        float PlayerY { get; }

		/// <summary>
		/// Gets the difficulty value used to populate the $rank variable (0.0 to 1.0 inclusive).
		/// </summary>
		/// <returns>The difficulty of the game</returns>
        float GameDifficulty { get; }

		/// <summary>
		/// Gets a random value used to populate the $rand variable (0.0 to 1.0 inclusive).
		/// </summary>
		/// <returns>The difficulty of the game</returns>
        float Random { get; }

		/// <summary>
		/// A bullet is done being used, do something to get rid of it.
		/// </summary>
		/// <param name="deadBullet">the Dead bullet.</param>
		void RemoveBullet(Bullet deadBullet);

		/// <summary>
		/// Create a new bullet.
		/// </summary>
		/// <returns>A shiny new bullet</returns>
		Bullet CreateBullet();

		#endregion //Methods
	}
}
