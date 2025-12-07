using Microsoft.Xna.Framework;

namespace App1.Source.Engine.Player
{
    public interface ICharacterFrames
    {
        Rectangle[] GetFrames(bool isRunning, bool isSneaking, int direction);
    }
}

