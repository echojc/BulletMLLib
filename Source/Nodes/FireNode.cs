using System;
using System.Xml;

namespace BulletMLLib
{
	public class FireNode : BulletMLNode
	{
		#region Members

		/// <summary>
		/// The node we are going to use to set the direction of any bullets shot with this task
		/// </summary>
		/// <value>The dir node.</value>
		public DirectionNode InitialDirectionNode { get; set; }

		/// <summary>
		/// The node we are going to use to set the speed of any bullets shot with this task
		/// </summary>
		/// <value>The speed node.</value>
		public SpeedNode InitialSpeedNode { get; set; }

		/// <summary>
		/// If there is a sequence direction node used to increment the direction of each successive bullet that is fired
		/// </summary>
		/// <value>The sequence direction node.</value>
		public DirectionNode SequenceDirectionNode { get; set; }

		/// <summary>
		/// If there is a sequence direction node used to increment the direction of each successive bullet that is fired
		/// </summary>
		/// <value>The sequence direction node.</value>
		public SpeedNode SequenceSpeedNode { get; set; }

		/// <summary>
		/// A bullet node this task will use to set any bullets shot from this task
		/// </summary>
		/// <value>The bullet node.</value>
		public BulletNode BulletDescriptionNode { get; set; }

		#endregion //Members

		#region Methods

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

		/// <summary>
		/// Validates the node.
		/// Overloaded in child classes to validate that each type of node follows the correct business logic.
		/// This checks stuff that isn't validated by the XML validation
		/// </summary>
		public override void ValidateNode()
		{
			//check for a bullet node
			BulletDescriptionNode = GetChild(ENodeName.bullet) as BulletNode;

			//if it didn't find one, check for the bulletref node
			if (null == BulletDescriptionNode)
			{
				BulletRefNode refNode = GetChild(ENodeName.bulletRef) as BulletRefNode;
				BulletDescriptionNode = refNode.ReferencedBulletNode;
			}

			//Setup all the direction nodes
			GetDirectionNodes(this);
			GetDirectionNodes(BulletDescriptionNode);

			//setup all the speed nodes
			GetSpeedNodes(this);
			GetSpeedNodes(BulletDescriptionNode);
		}

		/// <summary>
		/// Given a node, pull the direction nodes out from underneath it and store them if necessary
		/// </summary>
		/// <param name="nodeToCheck">Node to check.</param>
		private void GetDirectionNodes(BulletMLNode nodeToCheck)
		{
			if (null == nodeToCheck)
			{
				return;
			}

			//check if the dude has a direction node
			DirectionNode dirNode = nodeToCheck.GetChild(ENodeName.direction) as DirectionNode;
			if (null != dirNode)
			{
				//check if it is a sequence type of node
				if (ENodeType.sequence == dirNode.NodeType)
				{
					//do we need a sequence node?
					if (null == SequenceDirectionNode)
					{
						//store it in the sequence direction node
						SequenceDirectionNode = dirNode;
					}
				}
				else
				{
					//else do we need an initial node?
					if (null == InitialDirectionNode)
					{
						//store it in the initial direction node
						InitialDirectionNode = dirNode;
					}
				}
			}
		}

		/// <summary>
		/// Given a node, pull the speed nodes out from underneath it and store them if necessary
		/// </summary>
		/// <param name="nodeToCheck">Node to check.</param>
		private void GetSpeedNodes(BulletMLNode nodeToCheck)
		{
			if (null == nodeToCheck)
			{
				return;
			}

			//check if the dude has a speed node
			SpeedNode spdNode = nodeToCheck.GetChild(ENodeName.speed) as SpeedNode;
			if (null != spdNode)
			{
				//check if it is a sequence type of node
				if (ENodeType.sequence == spdNode.NodeType)
				{
					//do we need a sequence node?
					if (null == SequenceSpeedNode)
					{
						//store it in the sequence speed node
						SequenceSpeedNode = spdNode;
					}
				}
				else
				{
					//else do we need an initial node?
					if (null == InitialSpeedNode)
					{
						//store it in the initial speed node
						InitialSpeedNode = spdNode;
					}
				}
			}
		}

		#endregion Methods
	}
}
