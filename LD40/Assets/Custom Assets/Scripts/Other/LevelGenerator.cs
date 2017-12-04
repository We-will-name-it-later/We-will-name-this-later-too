using UnityEngine;

public class LevelGenerator : MonoBehaviour {

	public Texture2D map;

	public float scale = 0.64f;

	public ColorToPrefab[] colorMappings;

	private void Start () {
		GenerateLevel();
	}

	public void GenerateLevel()
	{
		for (int x = 0; x < map.width; x++)
		{
			for (int y = 0; y < map.height; y++)
			{
				GenerateTile(x, y);
			}
		}
	}

	private void GenerateTile(int x, int y)
	{
		Color pixelColor = map.GetPixel(x, y);

		if (pixelColor.a == 0)
		{
			//Pixel is transparent.
			return;
		}

		foreach (ColorToPrefab colorMapping in colorMappings)
		{
			if (colorMapping.color.Equals(pixelColor))
			{
				Vector2 position = new Vector2(x * scale, y * scale);
				Instantiate(colorMapping.prefab, position, colorMapping.prefab.transform.rotation, transform);
			}
		}

	}

    public void DestroyGrid() {
        foreach (Transform child in gameObject.transform)
        {
            GameObject.DestroyImmediate(child.gameObject);
        }
    }

}
