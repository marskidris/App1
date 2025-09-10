#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App1.Source.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace App1.Source;

public class World
{
    public Basic2D hero;

    public World()
    {
        hero = new Basic2D("2D\\Earl_Transparent", new Vector2(9, 13), new Vector2(150, 150));
        hero.sourceRect = new Rectangle(9, 13, 23, 33); 
    }

    public virtual void Update()
    {
        hero.Update();
    }

    public virtual void Draw()
    {
        hero.Draw();
    }
}