
namespace BulletMLLib
{
	public class BulletValue
	{
		#region Members

		public BLValueType valueType;
		public float value;

		#endregion //Members

		#region Methods

		public BulletValue(BLValueType type, float value)
		{
			this.valueType = type;
			this.value = value;
		}

		#endregion //Methods
	}
}