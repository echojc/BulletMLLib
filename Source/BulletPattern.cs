using System;
using System.Diagnostics;
using System.Xml;

namespace BulletMLLib
{
	/// <summary>
	/// This is a complete document that describes a bullet pattern.
	/// </summary>
	public class BulletPattern
	{
		#region Members

		/// <summary>
		/// The root node of a tree structure that describes the bullet pattern
		/// </summary>
		public BulletMLNode tree;

		#endregion //Members

		#region Methods

		//TODO: refactor this shit to use enum.parse 

		/// <summary>
		/// Convert a string to it's ENodeType enum equivalent
		/// </summary>
		/// <returns>ENodeType: the nuem value of that string</returns>
		/// <param name="str">The string to convert to an enum</param>
		static ENodeType StringToType(string str)
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
		static ENodeName StringToName(string str)
		{
			return (ENodeName)Enum.Parse(typeof(ENodeName), str);
		}

		public void ParseXML(string xmlFileName)
		{
			XmlReaderSettings settings = new XmlReaderSettings();
			settings.DtdProcessing = DtdProcessing.Ignore;

			try
			{
				using (XmlReader reader = XmlReader.Create(xmlFileName, settings))
				{
					while (reader.Read())
					{
						switch (reader.NodeType)
						{
							case XmlNodeType.Element:
								{
									// The node is an element.
									BulletMLNode element = new BulletMLNode();
									element.name = BulletPattern.StringToName(reader.Name);
									if (reader.HasAttributes)
									{
										element.type = BulletPattern.StringToType(reader.GetAttribute("type"));
										element.label = reader.GetAttribute("label");
									}

									if (tree == null)
										tree = element;
									else
									{
										tree.children.Add(element);
										if (tree.children.Count > 1)
											tree.children[tree.children.Count - 2].next = tree.children[tree.children.Count - 1];

										element.parent = tree;
										if (!reader.IsEmptyElement)
											tree = element;
									}
								}
								break;

							case XmlNodeType.Text:
								{
									//Display the text in each element.
									string line = reader.Value;
									string word = "";
									for (int i = 0; i < line.Length; i++)
									{
										float num;
										if (('0' <= line[i] && line[i] <= '9') || line[i] == '.')
										{
											word = word + line[i];
											if (i < line.Length - 1) //まだ続きがあれば
											{
												continue;
											}
										}

										if (word != "")
										{
											if (float.TryParse(word, out num))
											{
												tree.values.Add(new BulletValue(BLValueType.Number, num));
												word = "";
												//Debug.WriteLine("数値を代入" + num);
											}
											else
											{
												//Debug.WriteLine("構文にエラーがあります : " + line[i]);
											}
										}

										if (line[i] == '$')
										{
											if (line[i + 1] >= '0' && line[i + 1] <= '9')
											{
												tree.values.Add(new BulletValue(BLValueType.Param, Convert.ToInt32(line[i + 1].ToString())));
												i++;
												//Debug.WriteLine("パラメータを代入");
											}
											else if (line.Substring(i, 5) == "$rank")
											{
												//Debug.WriteLine("ランクを代入");
												i += 4;
												tree.values.Add(new BulletValue(BLValueType.Rank, 0));
											}
											else if (line.Substring(i, 5) == "$rand")
											{
												//Debug.WriteLine("Randを代入");
												i += 4;
												tree.values.Add(new BulletValue(BLValueType.Rand, 0));
											}
										}
										else if (line[i] == '*' || line[i] == '/' || line[i] == '+' || line[i] == '-' || line[i] == '(' || line[i] == ')')
										{
											tree.values.Add(new BulletValue(BLValueType.Operator, line[i]));
											//Debug.WriteLine("演算子を代入 " + line[i]);
										}
										else if (line[i] == ' ' || line[i] == '\n')
										{
										}
										else
										{
											//Debug.WriteLine("構文にエラーがあります : " + line[i]);
										}
									}
								}
								break;

							case XmlNodeType.EndElement:
								{
									//Display the end of the element.
									if (tree.parent != null)
									{
										tree = tree.parent;
									}
								}
								break;
						}
					}
				}
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		#endregion //Methods
	}
}
