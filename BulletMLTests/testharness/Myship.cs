using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BulletMLTests
{
    class Myship
    {
        public Vector2 pos;

		public Myship()
		{
			pos = new Vector2(0, 100);
		}

		public Vector2 Position()
		{
			return pos;
		}
    }
}
