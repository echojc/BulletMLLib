using System;

namespace BulletMLLib
{
	/// <summary>
	/// Theese are used for tasks during runtime...
	/// </summary>
	internal enum ERunStatus
	{
		Continue, //keep parsing this task
		End, //this task is finished parsing
		Stop //this task is paused
	}
}

