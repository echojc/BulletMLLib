using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BulletMLSample
{
    class Myship
    {
        public Vector2 pos;

		public Vector2 Position()
		{
			return pos;
		}

        public void Init()
        {
			pos.X = 0;
			pos.Y = 100;
        }

        public void Update()
        {
        }
    }


}
