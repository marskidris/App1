using Microsoft.Xna.Framework;
using System;

namespace App1.Source.Engine.Menu;

public class Time
{
    private double totalGameTime = 0.0;
    private double lastPrintTime = 0.0;
    private const double PRINT_INTERVAL = 1.0;
    
    public void Update(GameTime gameTime)
    {
        totalGameTime += gameTime.ElapsedGameTime.TotalSeconds;
        
        if (totalGameTime - lastPrintTime >= PRINT_INTERVAL)
        {
            PrintGameTime();
            lastPrintTime = totalGameTime;
        }
    }
    
    private void PrintGameTime()
    {
        int hours = (int)(totalGameTime / 3600);
        int minutes = (int)((totalGameTime % 3600) / 60);
        int seconds = (int)(totalGameTime % 60);
        
        Console.WriteLine($"{hours:D2}:{minutes:D2}:{seconds:D2}");
    }
    
    public string GetFormattedTime()
    {
        int hours = (int)(totalGameTime / 3600);
        int minutes = (int)((totalGameTime % 3600) / 60);
        int seconds = (int)(totalGameTime % 60);
        
        return $"{hours:D2}:{minutes:D2}:{seconds:D2}";
    }
    
    public double GetTotalGameTime()
    {
        return totalGameTime;
    }
    
    public void Reset()
    {
        totalGameTime = 0.0;
        lastPrintTime = 0.0;
    }
}