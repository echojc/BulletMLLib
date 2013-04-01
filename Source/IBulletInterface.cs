
namespace BulletMLLib
{
	/// <summary>
	/// BulletMLを使用する場合は、このインタフェースを継承した弾クラスを作成すること
	/// </summary>
	public interface IBulletMLBulletInterface
	{
		#region Properties

		//仮想プロパティ・メソッド
		float X { get; set; }
		float Y { get; set; }
		float Speed { get; set; }
		float Dir { get; set; }

		#endregion //Properties

		#region Methods

		void Vanish();

		BulletMLBullet GetNewBullet();

		#endregion //Methods
	}
}