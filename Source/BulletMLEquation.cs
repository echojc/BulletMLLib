using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace BulletMLLib
{
	/// <summary>
	/// This is an equation used in BulletML nodes.
	/// </summary>
	public class BulletMLEquation
	{
        private static readonly Regex wsRegex = new Regex(@"\s+");

        private float v = 0f;

        public void Parse(string text)
        {
            string stripped = wsRegex.Replace(text, "");
            float.TryParse(text, out v);
        }

        public float Solve(Func<int, float> paramResolver)
        {
            return v;
        }
	}
}

