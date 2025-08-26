using UnityEngine;
using Unity.Entities;

#if UNITY_EDITOR
using UnityEditor;

public class PlayerSetupHelper : MonoBehaviour
{
    [MenuItem("GameObject/2D Object/Player Entity (2D Top-Down)")]
    public static void CreatePlayerEntity()
    {
        // Create the player GameObject
        GameObject player = new GameObject("Player");
        
        // Add the PlayerAuthoring component
        player.AddComponent<PlayerAuthoring>();
        
        // Add a visual representation with 2D sprite
        var visual = new GameObject("Visual");
        visual.transform.SetParent(player.transform);
        visual.transform.localPosition = Vector3.zero;
        
        // Add SpriteRenderer component
        var spriteRenderer = visual.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = CreateDefaultPlayerSprite();
        spriteRenderer.sortingOrder = 1; // Ensure player renders above ground
        
        // Add a camera for top-down view
        var camera = new GameObject("GameCamera");
        Camera cam = camera.AddComponent<Camera>();
        camera.transform.SetParent(player.transform);
        camera.transform.localPosition = new Vector3(0, 0, -10); // Behind the player for top-down view
        cam.orthographic = true; // Use orthographic camera for 2D
        cam.orthographicSize = 5f; // Adjust this for zoom level
        
        // Select the created player
        Selection.activeGameObject = player;
        
        // Position in scene
        player.transform.position = Vector3.zero;
        
        Debug.Log("2D Top-Down Player entity created! Make sure to place it in a SubScene for ECS conversion.");
    }
    
    [MenuItem("GameObject/2D Object/Player Entity (Simple 2D)")]
    public static void CreateSimplePlayerEntity()
    {
        // Create the player GameObject
        GameObject player = new GameObject("Player");
        
        // Add the PlayerAuthoring component
        player.AddComponent<PlayerAuthoring>();
        
        // Add a simple visual representation
        var visual = new GameObject("Visual");
        visual.transform.SetParent(player.transform);
        visual.transform.localPosition = Vector3.zero;
        
        // Add SpriteRenderer component
        var spriteRenderer = visual.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = CreateDefaultPlayerSprite();
        spriteRenderer.sortingOrder = 1;
        
        // Select the created player
        Selection.activeGameObject = player;
        
        // Position in scene
        player.transform.position = Vector3.zero;
        
        Debug.Log("Simple 2D Player entity created! Make sure to place it in a SubScene for ECS conversion.");
    }
    
    private static Sprite CreateDefaultPlayerSprite()
    {
        // Create a simple colored square sprite as placeholder
        Texture2D texture = new Texture2D(32, 32);
        Color[] pixels = new Color[32 * 32];
        
        // Create a simple player-like pattern (colored square with border)
        for (int x = 0; x < 32; x++)
        {
            for (int y = 0; y < 32; y++)
            {
                if (x == 0 || x == 31 || y == 0 || y == 31)
                {
                    // Border
                    pixels[y * 32 + x] = Color.black;
                }
                else if (x >= 8 && x <= 23 && y >= 8 && y <= 23)
                {
                    // Center area (player body)
                    pixels[y * 32 + x] = Color.blue;
                }
                else
                {
                    // Background
                    pixels[y * 32 + x] = Color.white;
                }
            }
        }
        
        texture.SetPixels(pixels);
        texture.Apply();
        
        // Create sprite from texture
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f), 32);
        sprite.name = "DefaultPlayerSprite";
        
        return sprite;
    }
}
#endif 