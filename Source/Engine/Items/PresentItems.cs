using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using App1.Source.Engine.Audio;

namespace App1.Source.Engine.Items;

public class PresentItems
{
    private Texture2D itemTexture;
    private Rectangle itemFrame;
    private Vector3[] itemPositions;
    private bool[] itemsCollected;
    private float pickupDistance = 100f;

    public PresentItems(Vector3 referencePosition, int itemCount = 3)
    {
        itemFrame = new Rectangle(137, 10, 15, 14);
        
        itemPositions = new Vector3[itemCount];
        itemPositions[0] = new Vector3(referencePosition.X + 150, referencePosition.Y, 0);
        itemPositions[1] = new Vector3(referencePosition.X - 150, referencePosition.Y, 0);
        itemPositions[2] = new Vector3(referencePosition.X, referencePosition.Y + 150, 0);
        
        itemsCollected = new bool[itemCount];
        
        itemTexture = Globals.content.Load<Texture2D>("2D/items_scenery_tranparent");
    }

    public void CheckCollisions(Vector3 playerPosition)
    {
        for (int i = 0; i < itemPositions.Length; i++)
        {
            if (!itemsCollected[i])
            {
                float distanceToItem = Vector3.Distance(playerPosition, itemPositions[i]);
                if (distanceToItem < pickupDistance)
                {
                    itemsCollected[i] = true;
                    // Play the money sound with volume adjustments
                    AudioState.Instance.PlayMoneySoundWithVolumeAdjustment();
                }
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        for (int i = 0; i < itemPositions.Length; i++)
        {
            if (!itemsCollected[i])
            {
                Vector2 itemPosition2D = new Vector2(itemPositions[i].X, itemPositions[i].Y);
                Vector2 itemOrigin = new Vector2(itemFrame.Width / 2f, itemFrame.Height / 2f);
                
                spriteBatch.Draw(
                    itemTexture,
                    itemPosition2D,
                    itemFrame,
                    Color.White,
                    0f,
                    itemOrigin,
                    2f,
                    SpriteEffects.None,
                    0f);
            }
        }
    }

    public float PickupDistance
    {
        get => pickupDistance;
        set => pickupDistance = value;
    }

    public int CollectedCount
    {
        get
        {
            int count = 0;
            foreach (bool collected in itemsCollected)
            {
                if (collected) count++;
            }
            return count;
        }
    }

    public int TotalCount => itemPositions.Length;
}
