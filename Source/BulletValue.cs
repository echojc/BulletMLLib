using System.Diagnostics;
using System;

namespace BulletMLLib
{
	/// <summary>
	/// This is a value that is loaded from a bulletml node. 
	/// These things are used to store a bunch of different types of stuff
	/// </summary>
	public class BulletValue
	{
		#region Members

		/// <summary>
		/// The type of the value.
		/// </summary>
		public EValueType ValueType { get; private set; }

		/// <summary>
		/// The value this this value. LOL
		/// </summary>
		public float Value { get; private set; }

		/// <summary>
		/// A randomizer for getting random values
		/// </summary>
		static private Random g_Random = new Random(DateTime.Now.Millisecond);

		#endregion //Members

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="BulletMLLib.BulletValue"/> class.
		/// </summary>
		/// <param name="type">Type.</param>
		/// <param name="value">Value.</param>
		public BulletValue(EValueType type, float value)
		{
			ValueType = type;
			this.Value = value;
		}

		/// <summary>
		/// Given a task, find the value of this object for it
		/// </summary>
		/// <returns>The value for task.</returns>
		/// <param name="task">Task.</param>
		public float GetValueForTask(BulletMLTask task)
		{
			Debug.Assert(null != task);
			switch (ValueType)
			{
				case EValueType.Number:
				{
					//This item just holds a number... return that
					return Value;
				}

				case EValueType.Rand:
				{
					//this value is "$rand", return a random number
					return (float)g_Random.NextDouble();
				}

				case EValueType.Rank:
				{
					//This number is "$rank" which is the game difficulty.
					Debug.Assert(null != GameManager.GameDifficulty);
					return GameManager.GameDifficulty();
				}

				case EValueType.Param:
				{
					//Pull a param out of the task!  This means the Value member is a parameter index...
					
					//start with the task that was given to us...
					BulletMLTask currentTask = task;

					//if that task doesn't have any params, go up until we find one that does
					while (currentTask.ParamList.Count < (int)Value)
					{
						//the current task doens't have enough params to solve this value
						currentTask = currentTask.Owner;

						if (null == currentTask)
						{
							//got to the top of the list...this means not enough params were passed into the ref
							return 0.0f;
						}
					}

					//the value of that param is the one we want
					return currentTask.ParamList[(int)Value - 1];
				}

				default:
				{
					//somehow an operator got in here?
					Debug.Assert(false);
					return 0;
				}
			}
		}

		#endregion //Methods
	}
}