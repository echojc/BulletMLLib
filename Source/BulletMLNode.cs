using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Xml;

namespace BulletMLLib
{
	/// <summary>
	/// This is a single node from a BulletML document.
	/// </summary>
	public class BulletMLNode
	{
		#region Members

		/// <summary>
		/// The XML node name of this item
		/// </summary>
		public ENodeName Name { get; private set; }

		/// <summary>
		/// The type modifie of this node... like is it a sequence, or whatver
		/// idunno, this is really poorly thought out on the part of Kento Cho
		/// </summary>
		public ENodeType NodeType { get; private set; }

		/// <summary>
		/// The label of this node
		/// This can be used by other nodes to reference this node
		/// </summary>
		public string Label { get; private set; }

		/// <summary>
		/// A list of all the values for this dude...
		/// </summary>
		public List<BulletValue> Values = new List<BulletValue>();

		/// <summary>
		/// A list of all the child nodes for this dude
		/// </summary>
		public List<BulletMLNode> ChildNodes = new List<BulletMLNode>();

		/// <summary>
		/// A randomizer for doing some shit
		/// </summary>
		static private Random g_Random = new Random(DateTime.Now.Millisecond);

		#endregion //Members

		#region Methods

		public BulletMLNode()
		{
			Name = ENodeName.bulletml;
			NodeType = ENodeType.absolute;
		}

		/// <summary>
		/// Convert a string to it's ENodeType enum equivalent
		/// </summary>
		/// <returns>ENodeType: the nuem value of that string</returns>
		/// <param name="str">The string to convert to an enum</param>
		private static ENodeType StringToType(string str)
		{
			//make sure there is something there
			if (string.IsNullOrEmpty(str))
			{
				return ENodeType.none;
			}
			else
			{
				return (ENodeType)Enum.Parse(typeof(ENodeType), str);
			}
		}
		
		/// <summary>
		/// Convert a string to it's ENodeName enum equivalent
		/// </summary>
		/// <returns>ENodeName: the nuem value of that string</returns>
		/// <param name="str">The string to convert to an enum</param>
		private static ENodeName StringToName(string str)
		{
			return (ENodeName)Enum.Parse(typeof(ENodeName), str);
		}

		/// <summary>
		/// Find a node of a specific type and label
		/// Recurse into the xml tree until we find it!
		/// </summary>
		/// <returns>The label node.</returns>
		/// <param name="label">Label of the node we are looking for</param>
		/// <param name="name">name of the node we are looking for</param>
		public BulletMLNode FindLabelNode(string strLabel, ENodeName eName)
		{
			//this uses breadth first search, since labelled nodes are usually top level

			//Check if any of our child nodes match the request
			for (int i = 0; i < ChildNodes.Count; i++)
			{
				if ((eName == ChildNodes[i].Name) && (strLabel == ChildNodes[i].Label))
				{
					return ChildNodes[i];
				}
			}

			//recurse into the child nodes and see if we find any matches
			for (int i = 0; i < ChildNodes.Count; i++)
			{
				BulletMLNode foundNode = ChildNodes[i].FindLabelNode(strLabel, eName);
				if (null != foundNode)
				{
					return foundNode;
				}
			}

			//didnt find a BulletMLNode with that name :(
			return null;
		}
		
		/// <summary>
		/// Parse the specified bulletNodeElement.
		/// Read all the data from the xml node into this dude.
		/// </summary>
		/// <param name="bulletNodeElement">Bullet node element.</param>
		public bool Parse(XmlNode bulletNodeElement)
		{
			Debug.Assert(null != bulletNodeElement);

			if (XmlNodeType.Text == bulletNodeElement.NodeType)
			{
				//Get the text out of this dude
				ParseText(bulletNodeElement.Name);
			}
			else
			{
				//get the node type
				Name = BulletMLNode.StringToName(bulletNodeElement.Name);

				//Parse all our attributes
				XmlNamedNodeMap mapAttributes = bulletNodeElement.Attributes;
				for (int i = 0; i < mapAttributes.Count; i++)
				{
					string strName = mapAttributes.Item(i).Name;
					string strValue = mapAttributes.Item(i).Value;

					if ("type" == strName)
					{
						//skip the type attribute in top level nodes
						if (ENodeName.bulletml == Name)
						{
							continue;
						}

						//get the bullet node type
						NodeType = BulletMLNode.StringToType(strValue);
					}
					else if ("label" == strName)
					{
						//label is just a text value
						Label = strValue;
					}
				}

				//parse all the child nodes
				if (bulletNodeElement.HasChildNodes)
				{
					for (XmlNode childNode = bulletNodeElement.FirstChild;
					     null != childNode;
					     childNode = childNode.NextSibling)
					{
						//create a new node
						BulletMLNode childBulletNode = new BulletMLNode();

						//read in the node
						if (!childBulletNode.Parse(childNode))
						{
							return false;
						}

						//store the node
						ChildNodes.Add(childBulletNode);
					}
				}
			}

			return true;
		}

		/// <summary>
		/// Parses the inner text of an xml node into this dude.
		/// There are a bunch of stupid rules about how this works...
		/// </summary>
		/// <param name="bulletNodeText">Bullet node text.</param>
		private void ParseText(string bulletNodeText)
		{
			//Walk through the text and try to parse it out into an expression
			StringBuilder word = new StringBuilder();
			for (int i = 0; i < bulletNodeText.Length; i++)
			{
				//First check if we are reading in a number
				if (('0' <= bulletNodeText[i] && bulletNodeText[i] <= '9') || bulletNodeText[i] == '.')
				{
					//Add the digit/decimal to the end of the number
					word.Append(bulletNodeText[i]);

					//If we haven't reached the end of the text, keep reading
					if (i < bulletNodeText.Length - 1)
					{
						continue;
					}
				}

				//If we have a string in there, it has to be a number that has been parsed up above
				if (!string.IsNullOrEmpty(word.ToString()))
				{
					//Try to parse the string as a floating point value
					float num;
					if (float.TryParse(word.ToString(), out num))
					{
						//Add the number as a bullet value to this node
						Values.Add(new BulletValue(BLValueType.Number, num));
						word.Clear();
					}
				}

				//We aren't reading a string, and if we had a number it was already stored up above... check what the current character is
				if (bulletNodeText[i] == '$')
				{
					//we found a variable, check what sort of valuetype we got
					if (bulletNodeText[i + 1] >= '0' && bulletNodeText[i + 1] <= '9')
					{
						//TODO: bulletml only supports up to 9 params cuz of this chunk of code

						//We have a param value, parse it out and store in the list of values
						int iValue = Convert.ToInt32(bulletNodeText[i + 1].ToString());
						Values.Add(new BulletValue(BLValueType.Param, iValue));

						//since we consumed the $ followed by param number, increment the index by 1
						i++;
					}
					else if (bulletNodeText.Substring(i, 5) == "$rank")
					{
						//We found a value that is using the difficulty of the game
						Values.Add(new BulletValue(BLValueType.Rank, 0));

						//since we consumed the $ followed by 4 characters, increment the index by 4
						i += 4;
					}
					else if (bulletNodeText.Substring(i, 5) == "$rand")
					{
						//we found a random value
						Values.Add(new BulletValue(BLValueType.Rand, 0));

						//since we consumed the $ followed by 4 characters, increment the index by 4
						i += 4;
					}
				}
				else if (bulletNodeText[i] == '*' || 
				         bulletNodeText[i] == '/' || 
				         bulletNodeText[i] == '+' || 
				         bulletNodeText[i] == '-' || 
				         bulletNodeText[i] == '(' || 
				         bulletNodeText[i] == ')')
				{
					//We found an operator value... is this shit seriously storing an ascii character in a float???
					Values.Add(new BulletValue(BLValueType.Operator, bulletNodeText[i]));
				}
			}
		}

		//TODO: sort all these shitty functions out

		public float GetChildValue(ENodeName name, BulletMLTask task)
		{
			foreach (BulletMLNode tree in ChildNodes)
			{
				if (tree.Name == name)
				{
					return tree.GetValue(task);
				}
			}
			return 0.0f;
		}
		
		public BulletMLNode GetChild(ENodeName name)
		{
			foreach (BulletMLNode node in ChildNodes)
			{
				if (node.Name == name)
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
			for (; i < Values.Count; i++)
			{
				if (Values[i].valueType == BLValueType.Operator)
				{
					if (Values[i].value == '+')
					{
						i++;
						if (IsNextNum(i))
						{
							v += GetNumValue(Values[i], task);
						}
						else
						{
							v += GetValue(v, ref i, task);
						}
					}
					else if (Values[i].value == '-')
					{
						i++;
						if (IsNextNum(i))
						{
							v -= GetNumValue(Values[i], task);
						}
						else
						{
							v -= GetValue(v, ref i, task);
						}
					}
					else if (Values[i].value == '*')
					{
						i++;
						if (IsNextNum(i))
						{
							v *= GetNumValue(Values[i], task);
						}
						else
						{
							v *= GetValue(v, ref i, task);
						}
					}
					else if (Values[i].value == '/')
					{
						i++;
						if (IsNextNum(i))
						{
							v /= GetNumValue(Values[i], task);
						}
						else
						{
							v /= GetValue(v, ref i, task);
						}
					}
					else if (Values[i].value == '(')
					{
						i++;
						float res = GetValue(v, ref i, task);
						if ((i < Values.Count - 1 && Values[i + 1].valueType == BLValueType.Operator)
						    && (Values[i + 1].value == '*' || Values[i + 1].value == '/'))
						{
							return GetValue(res, ref i, task);
						}
						else
						{
							return res;
						}
					}
					else if (Values[i].value == ')')
					{
						//Debug.WriteLine(" ）の戻り値:" + v);
						return v;
					}
				}
				else if (i < Values.Count - 1 && Values[i + 1].valueType == BLValueType.Operator && Values[i + 1].value == '*')
				{
					// 次が掛け算のとき
					float val = GetNumValue(Values[i], task);
					i += 2;
					if (IsNextNum(i))
					{
						return val * GetNumValue(Values[i], task);
					}
					else
					{
						return val * GetValue(v, ref i, task);
					}
				}
				else if (i < Values.Count - 1 && Values[i + 1].valueType == BLValueType.Operator && Values[i + 1].value == '/')
				{
					// 次が割り算のとき
					float val = GetNumValue(Values[i], task);
					i += 2;
					if (IsNextNum(i))
					{
						return val / GetNumValue(Values[i], task);
					}
					else
					{
						return val / GetValue(v, ref i, task);
					}
				}
				else
				{
					v = GetNumValue(Values[i], task);
				}
			}
			
			return v;
		}
		
		bool IsNextNum(int i)
		{
			if ((i < Values.Count - 1 && Values[i + 1].valueType == BLValueType.Operator) && (Values[i + 1].value == '*' || Values[i + 1].value == '/'))
			{
				return false;
			}
			else if (Values[i].value == ')' || Values[i].value == '(')
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
				Debug.Assert(null != GameManager.GameDifficulty);
				return GameManager.GameDifficulty();
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
			return ChildNodes[p].GetValue(task); //<param>以外のタグは持っていないので
		}

		#endregion //Methods
	}
}
