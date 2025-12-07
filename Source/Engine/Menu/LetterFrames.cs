namespace App1.Source.Engine.Menu;
using Microsoft.Xna.Framework;

public class LetterFrames
{
    private Rectangle[] lettersframes;
    
    public LetterFrames()
    {
        lettersframes = new Rectangle[26];
        lettersframes[0] = new Rectangle(3, 70, 6, 7);
        lettersframes[1] = new Rectangle(10, 70, 8, 8);
        lettersframes[2] = new Rectangle(20, 70, 7, 7);
        lettersframes[3] = new Rectangle(29, 70, 8, 7);
        lettersframes[4] = new Rectangle(38, 70, 7, 8);
        lettersframes[5] = new Rectangle(47, 70, 7, 7);
        lettersframes[6] = new Rectangle(56, 70, 7, 8);
        lettersframes[7] = new Rectangle(65, 70, 8, 8);
        lettersframes[8] = new Rectangle(74, 70, 6, 7);
        lettersframes[9] = new Rectangle(84, 70, 6, 7);
        lettersframes[10] = new Rectangle(93, 70, 7, 7);
        lettersframes[11] = new Rectangle(102, 70, 7, 7);
        lettersframes[12] = new Rectangle(110, 70, 7, 8);
        lettersframes[13] = new Rectangle(120, 70, 7, 8);
        lettersframes[14] = new Rectangle(129, 70, 6, 6);
        lettersframes[15] = new Rectangle(2, 78, 7, 9);
        lettersframes[16] = new Rectangle(12, 79, 7, 7);
        lettersframes[17] = new Rectangle(21, 81, 7, 5);
        lettersframes[18] = new Rectangle(29, 79, 8, 7);
        lettersframes[19] = new Rectangle(38, 80, 8, 7);
        lettersframes[20] = new Rectangle(48, 79, 7, 7);
        lettersframes[21] = new Rectangle(56, 80, 7, 6);
        lettersframes[22] = new Rectangle(64, 79, 9, 7);
        lettersframes[23] = new Rectangle(74, 79, 8, 7);
        lettersframes[24] = new Rectangle(83, 79, 7, 7);
        lettersframes[25] = new Rectangle(92, 79, 9, 7);
    }
    
    // Get the frame for a letter (A=0, B=1, ..., Z=25)
    public Rectangle GetFrame(char letter)
    {
        char upper = char.ToUpper(letter);
        if (upper >= 'A' && upper <= 'Z')
        {
            int index = upper - 'A';
            return lettersframes[index];
        }
        // Return empty rectangle for non-letter characters (space, etc.)
        return Rectangle.Empty;
    }
    
    public Rectangle[] GetAllFrames()
    {
        return lettersframes;
    }
}