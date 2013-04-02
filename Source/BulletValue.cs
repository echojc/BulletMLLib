
namespace BulletMLLib
{
	//TODO: figure out a better name for this thing
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