using Microsoft.Xna.Framework;

namespace App1.Source.Engine.Player.Earl
{
    public class Earl : ICharacterFrames
    {
        private Rectangle[] playerFramesS, playerFramesA, playerFramesW, playerFramesD, 
            playerFramesSR, playerFramesAR, playerFramesWR, playerFramesDR, 
            playerFramesSS, playerFramesAS, playerFramesWS, playerFramesDS;

        public Earl()
        {
            EarlFrames();
        }

        private void EarlFrames()
        {
            playerFramesS = new Rectangle[6];
            playerFramesS[0] = new Rectangle(5, 87, 22, 34);
            playerFramesS[1] = new Rectangle(37, 85, 22, 35);
            playerFramesS[2] = new Rectangle(70, 82, 22, 39);
            playerFramesS[3] = new Rectangle(105, 86, 21, 34);
            playerFramesS[4] = new Rectangle(137, 85, 22, 34);
            playerFramesS[5] = new Rectangle(173, 85, 19, 35);
            
            playerFramesA = new Rectangle[7];
            playerFramesA[0] = new Rectangle(208, 85, 26, 34);
            playerFramesA[1] = new Rectangle(239, 85, 32, 34);
            playerFramesA[2] = new Rectangle(274, 87, 32, 32);
            playerFramesA[3] = new Rectangle(317, 84, 22, 35);
            playerFramesA[4] = new Rectangle(346, 81, 25, 39);
            playerFramesA[5] = new Rectangle(373, 84, 37, 34);
            playerFramesA[6] = new Rectangle(413, 84, 30, 33);
            
            playerFramesW = new Rectangle[7];
            playerFramesW[0] = new Rectangle(5, 127, 21, 36);
            playerFramesW[1] = new Rectangle(38, 124, 21, 39);
            playerFramesW[2] = new Rectangle(68, 128, 21, 35);
            playerFramesW[3] = new Rectangle(95, 130, 24, 36);
            playerFramesW[4] = new Rectangle(125, 130, 19, 33);
            playerFramesW[5] = new Rectangle(149, 129, 23, 34);
            playerFramesW[6] = new Rectangle(178, 129, 21, 34);
            
            playerFramesD = new Rectangle[7];
            playerFramesD[0] = new Rectangle(208, 127, 25, 34);
            playerFramesD[1] = new Rectangle(238, 127, 32, 34);
            playerFramesD[2] = new Rectangle(274, 129, 32, 32);
            playerFramesD[3] = new Rectangle(316, 126, 22, 35);
            playerFramesD[4] = new Rectangle(343, 123, 22, 39);
            playerFramesD[5] = new Rectangle(373, 126, 37, 34);
            playerFramesD[6] = new Rectangle(412, 126, 32, 33);
            
            playerFramesSR = new Rectangle[4];
            playerFramesSR[0] = new Rectangle(7, 692, 29, 36);
            playerFramesSR[1] = new Rectangle(40, 693, 31, 36);
            playerFramesSR[2] = new Rectangle(70, 692, 35, 37);
            playerFramesSR[3] = new Rectangle(110, 692, 31, 45);
            
            playerFramesAR = new Rectangle[4];
            playerFramesAR[0] = new Rectangle(163, 691, 38, 37);
            playerFramesAR[1] = new Rectangle(200, 692, 42, 36);
            playerFramesAR[2] = new Rectangle(252, 690, 35, 38);
            playerFramesAR[3] = new Rectangle(291, 690, 34, 36);
            
            playerFramesWR = new Rectangle[4];
            playerFramesWR[0] = new Rectangle(6, 735, 30, 37);
            playerFramesWR[1] = new Rectangle(40, 738, 31, 35);
            playerFramesWR[2] = new Rectangle(74, 737, 29, 36);
            playerFramesWR[3] = new Rectangle(79, 736, 29, 37);
            
            playerFramesDR = new Rectangle[4];
            playerFramesDR[0] = new Rectangle(164, 737, 38, 37);
            playerFramesDR[1] = new Rectangle(208, 738, 35, 36);
            playerFramesDR[2] = new Rectangle(252, 738, 40, 38);
            playerFramesDR[3] = new Rectangle(291, 736, 34, 36);
            
            playerFramesSS = new Rectangle[4];
            playerFramesSS[0] = new Rectangle(2, 201, 30, 39);
            playerFramesSS[1] = new Rectangle(35, 206, 26, 34);
            playerFramesSS[2] = new Rectangle(65, 205, 32, 35);
            playerFramesSS[3] = new Rectangle(101, 211, 30, 29);
            
            playerFramesAS = new Rectangle[4];
            playerFramesAS[0] = new Rectangle(133, 255, 40, 25);
            playerFramesAS[1] = new Rectangle(176, 244, 24, 36);
            playerFramesAS[2] = new Rectangle(202, 246, 31, 34);
            playerFramesAS[3] = new Rectangle(235, 250, 32, 30);
            
            playerFramesWS = new Rectangle[4];
            playerFramesWS[0] = new Rectangle(3, 246, 28, 39);
            playerFramesWS[1] = new Rectangle(35, 251, 26, 32);
            playerFramesWS[2] = new Rectangle(66, 250, 32, 32);
            playerFramesWS[3] = new Rectangle(102, 253, 28, 29);
            
            playerFramesDS = new Rectangle[4];
            playerFramesDS[0] = new Rectangle(134, 215, 40, 25);
            playerFramesDS[1] = new Rectangle(176, 204, 24, 36);
            playerFramesDS[2] = new Rectangle(204, 206, 31, 34);
            playerFramesDS[3] = new Rectangle(236, 210, 32, 30);
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
}
