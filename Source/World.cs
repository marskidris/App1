#region Includes

using App1.Source.Engine;
using Microsoft.Xna.Framework;

#endregion

namespace App1.Source;

public class World
{
    private Player Hero;
    public PlayerMovement HeroMovement;

    public World()
    {
        Hero = new Player("2D\\Earl_Transparent", new Vector2(150, 150), new Vector2(150, 150));
        HeroMovement = new PlayerMovement(Hero);
    }

    public virtual void Update(GameTime gameTime)
    {
        HeroMovement.Update(gameTime);
    }

    public virtual void Draw()
    {
        Hero.Draw();
    }
}