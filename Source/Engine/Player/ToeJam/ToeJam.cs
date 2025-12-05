using Microsoft.Xna.Framework;

public class ToeJam
{
    private Rectangle[] playerFramesS, playerFramesA, playerFramesW, playerFramesD, 
        playerFramesSR, playerFramesAR, playerFramesWR, playerFramesDR, 
        playerFramesSS, playerFramesAS, playerFramesWS, playerFramesDS;
    // reminder: S = sneak, J = jump
    public ToeJam()
    {
        ToeJamFrames();
    }

    private void ToeJamFrames()
    {
        playerFramesS = new Rectangle[6];
        playerFramesS[0] = new Rectangle(20, 81, 24, 32);
        playerFramesS[1] = new Rectangle(55, 82, 21, 30);
        playerFramesS[2] = new Rectangle(86, 82, 27, 31);
        playerFramesS[3] = new Rectangle(123, 82, 22, 30);
        playerFramesS[4] = new Rectangle(157, 82, 21, 30);
        playerFramesS[5] = new Rectangle(186, 81, 27, 32);
        
        playerFramesA = new Rectangle[6];
        playerFramesA[0] = new Rectangle(239, 84, 20, 28);
        playerFramesA[1] = new Rectangle(271, 83, 25, 29);
        playerFramesA[2] = new Rectangle(304, 82, 24, 30);
        playerFramesA[3] = new Rectangle(336, 83, 23, 29);
        playerFramesA[4] = new Rectangle(372, 85, 20, 27);
        playerFramesA[5] = new Rectangle(403, 85, 22, 27);
        
        playerFramesW = new Rectangle[6];
        playerFramesD[0] = new Rectangle(22, 132, 27, 31);
        playerFramesD[1] = new Rectangle(62, 133, 21, 30);
        playerFramesD[2] = new Rectangle(93, 131, 24, 32);
        playerFramesD[3] = new Rectangle(125, 131, 27, 32);
        playerFramesD[4] = new Rectangle(160, 133, 22, 30);
        playerFramesD[5] = new Rectangle(190, 133, 22, 30);
        
        playerFramesD = new Rectangle[6];
        playerFramesD[0] = new Rectangle(241, 133, 22, 27);
        playerFramesD[1] = new Rectangle(274, 133, 20, 27);
        playerFramesD[2] = new Rectangle(307, 131, 23, 29);
        playerFramesD[3] = new Rectangle(338, 130, 24, 30);
        playerFramesD[4] = new Rectangle(370, 131, 25, 39);
        playerFramesD[5] = new Rectangle(407, 132, 20, 28);
        
        playerFramesSR = new Rectangle[3];
        playerFramesSR[0] = new Rectangle(37, 691, 31, 37);
        playerFramesSR[1] = new Rectangle(80, 691, 31, 36);
        playerFramesSR[2] = new Rectangle(120, 690, 32, 39);
        
        playerFramesAR = new Rectangle[3];
        playerFramesAR[0] = new Rectangle(169, 690, 32, 36);
        playerFramesAR[1] = new Rectangle(212, 689, 33, 37);
        playerFramesAR[2] = new Rectangle(256, 690, 33, 36);
        
        playerFramesWR = new Rectangle[3];
        playerFramesWR[0] = new Rectangle(38, 737, 31, 38);
        playerFramesWR[1] = new Rectangle(82, 735, 32, 38);
        playerFramesWR[2] = new Rectangle(121, 735, 32, 37);
        
        playerFramesDR = new Rectangle[3];
        playerFramesDR[0] = new Rectangle(164, 738, 33, 36);
        playerFramesDR[1] = new Rectangle(208, 737, 33, 37);
        playerFramesDR[2] = new Rectangle(252, 738, 32, 36);
        
        playerFramesSS = new Rectangle[3];
        playerFramesSS[0] = new Rectangle(26, 204, 18, 28);
        playerFramesSS[1] = new Rectangle(54, 204, 18, 25);
        playerFramesSS[2] = new Rectangle(85, 204, 18, 24);
        
        playerFramesAS = new Rectangle[3];
        playerFramesAS[0] = new Rectangle(133, 202, 14, 25);
        playerFramesAS[1] = new Rectangle(158, 204, 15, 23);
        playerFramesAS[2] = new Rectangle(184, 205, 15, 21);
        
        playerFramesWS = new Rectangle[3];
        playerFramesWS[0] = new Rectangle(24, 249, 17, 24);
        playerFramesWS[1] = new Rectangle(52, 247, 18, 28);
        playerFramesWS[2] = new Rectangle(82, 247, 17, 25);
        
        playerFramesDS = new Rectangle[3];
        playerFramesDS[0] = new Rectangle(131, 247, 15, 21);
        playerFramesDS[1] = new Rectangle(157, 246, 15, 23);
        playerFramesDS[2] = new Rectangle(183, 244, 14, 25);
    }

    public Rectangle[] GetFrames(bool isRunning, bool isSneaking, int direction)
    {
        return (isRunning, isSneaking, direction) switch
        {
            (false, false, 0) => playerFramesW,
            (false, false, 1) => playerFramesS,
            (false, false, 2) => playerFramesA,
            (false, false, 3) => playerFramesD,
            (true, false, 0) => playerFramesWR,
            (true, false, 1) => playerFramesSR,
            (true, false, 2) => playerFramesAR,
            (true, false, 3) => playerFramesDR,
            (_, true, 0) => playerFramesWS,
            (_, true, 1) => playerFramesSS,
            (_, true, 2) => playerFramesAS,
            (_, true, 3) => playerFramesDS,
            _ => playerFramesS
        };
    }
}
