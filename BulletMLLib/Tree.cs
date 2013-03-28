using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace BulletMLLib
{
	public class BulletMLTree
	{
		#region Members

		public BLName name;
		public BLType type;
		public string label;
		public BulletMLTree parent;
		public BulletMLTree next;
		public List<BulletValue> values = new List<BulletValue>();
		public List<BulletMLTree> children = new List<BulletMLTree>();

		static private Random g_Random = new Random(DateTime.Now.Millisecond);

		#endregion //Members

		#region Methods

		public BulletMLTree()
		{
			name = BLName.bulletml;
			type = BLType.Absolute;
			parent = null;
			next = null;
		}

		public BulletMLTree GetLabelNode(string label, BLName name)
		{
			BulletMLTree rootNode = this; //先頭までさかのぼる
			while (rootNode.parent != null)
			{
				rootNode = rootNode.parent;
			}

			foreach (BulletMLTree tree in rootNode.children)
			{
				if (tree.label == label && tree.name == name)
				{
					return tree;
				}
			}
			return null;
		}

		public float GetChildValue(BLName name, BulletMLTask task)
		{
			foreach (BulletMLTree tree in children)
			{
				if (tree.name == name)
				{
					return tree.GetValue(task);
				}
			}
			return 0;
		}

		public BulletMLTree GetChild(BLName name)
		{
			foreach (BulletMLTree node in children)
			{
				if (node.name == name)
				{
					return node;
				}
			}
			return null;
		}

		public float GetValue(BulletMLTask task)
		{
			int startIndex = 0;

			return GetValue(0, ref startIndex, task);
		}

		public float GetValue(float v, ref int i, BulletMLTask task)
		{
			for (; i < values.Count; i++)
			{
				if (values[i].valueType == BLValueType.Operator)
				{
					if (values[i].value == '+')
					{
						i++;
						if (IsNextNum(i))
						{
							v += GetNumValue(values[i], task);
						}
						else
						{
							v += GetValue(v, ref i, task);
						}
					}
					else if (values[i].value == '-')
					{
						i++;
						if (IsNextNum(i))
						{
							v -= GetNumValue(values[i], task);
						}
						else
						{
							v -= GetValue(v, ref i, task);
						}
					}
					else if (values[i].value == '*')
					{
						i++;
						if (IsNextNum(i))
						{
							v *= GetNumValue(values[i], task);
						}
						else
						{
							v *= GetValue(v, ref i, task);
						}
					}
					else if (values[i].value == '/')
					{
						i++;
						if (IsNextNum(i))
						{
							v /= GetNumValue(values[i], task);
						}
						else
						{
							v /= GetValue(v, ref i, task);
						}
					}
					else if (values[i].value == '(')
					{
						i++;
						float res = GetValue(v, ref i, task);
						if ((i < values.Count - 1 && values[i + 1].valueType == BLValueType.Operator)
							   && (values[i + 1].value == '*' || values[i + 1].value == '/'))
						{
							return GetValue(res, ref i, task);
						}
						else
						{
							return res;
						}
					}
					else if (values[i].value == ')')
					{
						//Debug.WriteLine(" ）の戻り値:" + v);
						return v;
					}
				}
				else if (i < values.Count - 1 && values[i + 1].valueType == BLValueType.Operator && values[i + 1].value == '*')
				{
					// 次が掛け算のとき
					float val = GetNumValue(values[i], task);
					i += 2;
					if (IsNextNum(i))
					{
						return val * GetNumValue(values[i], task);
					}
					else
					{
						return val * GetValue(v, ref i, task);
					}
				}
				else if (i < values.Count - 1 && values[i + 1].valueType == BLValueType.Operator && values[i + 1].value == '/')
				{
					// 次が割り算のとき
					float val = GetNumValue(values[i], task);
					i += 2;
					if (IsNextNum(i))
					{
						return val / GetNumValue(values[i], task);
					}
					else
					{
						return val / GetValue(v, ref i, task);
					}
				}
				else
				{
					v = GetNumValue(values[i], task);
				}
			}

			return v;
		}

		bool IsNextNum(int i)
		{
			if ((i < values.Count - 1 && values[i + 1].valueType == BLValueType.Operator) && (values[i + 1].value == '*' || values[i + 1].value == '/'))
			{
				return false;
			}
			else if (values[i].value == ')' || values[i].value == '(')
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		float GetNumValue(BulletValue v, BulletMLTask task)
		{
			if (v.valueType == BLValueType.Number)
			{
				return v.value;
			}
			else if (v.valueType == BLValueType.Rand)
			{
				return (float)g_Random.NextDouble();
			}
			else if (v.valueType == BLValueType.Rank)
			{
				Debug.Assert(null != BulletMLManager.GameDifficulty);
				return BulletMLManager.GameDifficulty();
			}
			else if (v.valueType == BLValueType.Param)
			{
				BulletMLTask ownerTask = task;
				while (ownerTask.paramList.Count == 0)
				{
					ownerTask = ownerTask.owner;
				}
				float val = ownerTask.paramList[(int)v.value - 1];

				return val;
			}
			else
			{
				return 0;
			}
		}

		internal float GetParam(int p, BulletMLTask task)
		{
			return children[p].GetValue(task); //<param>以外のタグは持っていないので
		}

		#endregion //Methods
	}
}
