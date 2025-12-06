#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App1.Source.Engine;
using App1.Source.Engine.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace App1.Source;

public class World
{
    public Player hero;
    public PlayerMovement heroMovement;

    public World()
    {
        hero = new Player("2D\\Earl_Transparent", new Vector2(150, 150), new Vector2(150, 150));
        heroMovement = new PlayerMovement(hero);
    }

    public virtual void Update()
    {
        heroMovement.Update();
    }

    public virtual void Draw()
    {
        hero.Draw();
    }
}