namespace App1.Source.Engine.Enemy;
using Microsoft.Xna.Framework;

public class EnemyFrames
{
    private Rectangle[] tornadoUAFrames, torandoAFrames;

    public EnemyFrames()
    {
        TornadoFrames();
    }

    public Rectangle[] GetTornadoUAFrames()
    {
        return tornadoUAFrames;
    }

    public Rectangle[] GetTornadoAFrames()
    {
        return torandoAFrames;
    }

    private void TornadoFrames()
    {
        tornadoUAFrames = new Rectangle[8];
        tornadoUAFrames[0] = new Rectangle(19, 28, 11, 12);
        tornadoUAFrames[1] = new Rectangle(65, 25, 15, 15);
        tornadoUAFrames[2] = new Rectangle(110, 19, 21, 21);
        tornadoUAFrames[3] = new Rectangle(156, 15, 24, 25);
        tornadoUAFrames[4] = new Rectangle(9, 58, 30, 30);
        tornadoUAFrames[5] = new Rectangle(57, 58, 30, 30);
        tornadoUAFrames[6] = new Rectangle(105, 58, 30, 30);
        tornadoUAFrames[7] = new Rectangle(154, 58, 29, 30);
        
        torandoAFrames  = new Rectangle[8];
        torandoAFrames[0] = new Rectangle(9, 103, 30, 33);
        torandoAFrames[1] = new Rectangle(57, 100, 30, 36);
        torandoAFrames[2] = new Rectangle(105, 103, 30, 33);
        torandoAFrames[3] = new Rectangle(152, 106, 33, 30);
        torandoAFrames[4] = new Rectangle(9, 150, 30, 34);
        torandoAFrames[5] = new Rectangle(57, 149, 30, 35);
        torandoAFrames[6] = new Rectangle(104, 152, 33, 32);
        torandoAFrames[7] = new Rectangle(154, 153, 29, 31);
    }
}