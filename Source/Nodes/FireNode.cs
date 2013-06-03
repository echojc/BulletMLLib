using System;
using System.Xml;

namespace BulletMLLib
{
	public class FireNode : BulletMLNode
	{
		//TODO: fire nodes should store the initial direction & speed nodes

		//TODO: fire nodes should store the "sequence" direction & speed nodes also, if they are present.

		//TODO: fire node should store the node describing the bullet that will be shot 

		/// <summary>
		/// Initializes a new instance of the <see cref="BulletMLLib.FireNode"/> class.
		/// </summary>
		public FireNode() : this(ENodeName.fire)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BulletMLLib.FireNode"/> class.
		/// this is the constructor used by sub classes
		/// </summary>
		/// <param name="eNodeType">the node type.</param>
		public FireNode(ENodeName eNodeType) : base(eNodeType)
		{
		}
	}
}

