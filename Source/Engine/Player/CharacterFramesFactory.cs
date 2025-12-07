using System;
using App1.Source.Engine.Player.Earl;
using App1.Source.Engine.Player.ToeJam;

namespace App1.Source.Engine.Player
{
    public static class CharacterFramesFactory
    {
        public static ICharacterFrames Create(CharacterType characterType)
        {
            return characterType switch
            {
                CharacterType.Earl => new Earl.Earl(),
                CharacterType.ToeJam => new ToeJam.ToeJam(),
                _ => throw new ArgumentException($"Unknown character type: {characterType}")
            };
        }

        public static ICharacterFrames Create(string characterName)
        {
            return characterName?.ToLower() switch
            {
                "earl" => new Earl.Earl(),
                "toejam" => new ToeJam.ToeJam(),
                _ => new Earl.Earl()
            };
        }

        public static string GetTexturePath(CharacterType characterType)
        {
            return characterType switch
            {
                CharacterType.Earl => "2D/Earl_Transparent",
                CharacterType.ToeJam => "2D/ToeJam_Transparent",
                _ => "2D/Earl_Transparent"
            };
        }

        public static string GetTexturePath(string characterName)
        {
            return characterName?.ToLower() switch
            {
                "earl" => "2D/Earl_Transparent",
                "toejam" => "2D/ToeJam_Transparent",
                _ => "2D/Earl_Transparent"
            };
        }
    }
}

