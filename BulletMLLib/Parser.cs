using System;
using System.Diagnostics;
using System.Xml;

namespace BulletMLLib
{
	public class BulletMLParser
	{
		#region Members

		public BulletMLTree tree;

		//string[] name2string = {
		//    "bullet", "action", "fire", "changeDirection", "changeSpeed", "accel",
		//    "wait", "repeat", "bulletRef", "actionRef", "fireRef", "vanish",
		//    "horizontal", "vertical", "term", "times", "direction", "speed", "param",
		//    "bulletml"
		//                           };

		#endregion //Members

		#region Methods

		BLType StringToType(string str)
		{
			if (str == "aim") return BLType.Aim;
			else if (str == "absolute") return BLType.Absolute;
			else if (str == "relative") return BLType.Relative;
			else if (str == "sequence") return BLType.Sequence;
			else if (str == null) return BLType.None;
			//else Debug.WriteLine("BulletML parser: unknown type " + str);

			return BLType.None;
		}

		//タグ文字列をBLNameに変換する
		BLName StringToName(string str)
		{
			Debug.WriteLine(" tag " + str);
			return (BLName)Enum.Parse(typeof(BLName), str);
		}

		public void ParseXML(string xmlFileName)
		{
			XmlReaderSettings settings = new XmlReaderSettings();

			settings.DtdProcessing = DtdProcessing.Ignore;

#if WINDOWS
			settings.ValidationType = ValidationType.DTD;
#endif

			BulletMLParser parser = new BulletMLParser();

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
									BulletMLTree element = new BulletMLTree();
									element.name = parser.StringToName(reader.Name);
									if (reader.HasAttributes)
									{
										element.type = parser.StringToType(reader.GetAttribute("type"));
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
