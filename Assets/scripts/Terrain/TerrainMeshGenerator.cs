using UnityEngine;

public class TerrainMeshGenerator : MonoBehaviour
{
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private Mesh terrainMesh;


    private void Awake()
    {
        // Ensure the GameObject has a MeshFilter and MeshRenderer
        meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            meshFilter = gameObject.AddComponent<MeshFilter>();
        }
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
        }
    }

    public void GenerateMesh(TerrainData terrainData)
    {
        if (terrainData == null || terrainData.heightMap == null)
        {
            Debug.LogError("Invalid terrain data provided.");
            return;
        }

        terrainMesh = new Mesh();
        terrainMesh.name = "Terrain Mesh";

        int width = terrainData.width;
        int height = terrainData.height;

        // Create vertices and UVs
        Vector3[] vertices = new Vector3[(width + 1) * (height + 1)];
        Vector2[] uvs = new Vector2[vertices.Length];

        for (int y = 0; y <= height; y++)
        {
            for (int x = 0; x <= width; x++)
            {
                int index = x + y * (width + 1);


                // Get height value from heightmap 
                int heightmapX = Mathf.Clamp(x, 0, width - 1);
                int heightmapY = Mathf.Clamp(y, 0, height - 1);
                float heightValue = terrainData.GetHeight(heightmapX, heightmapY) * terrainData.verticalScale;

                vertices[index] = new Vector3(x * terrainData.horizontalScale, heightValue, y * terrainData.horizontalScale);

                // UVs coordinates
                uvs[index] = new Vector2((float)x / width, (float)y / height);
            }
        }

        // Triangles: 2 triangles per quad = 6 indices per quad
        // Total quads = width * height
        int[] triangles = new int[width * height * 6];
        int triIndex = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int bottomLeft = x + y * (width + 1);
                int bottomRight = (x + 1) + y * (width + 1);
                int topLeft = x + (y + 1) * (width + 1);
                int topRight = (x + 1) + (y + 1) * (width + 1);

                // First triangle (bottom-left to top-left to top-right)
                triangles[triIndex++] = bottomLeft;
                triangles[triIndex++] = topLeft;
                triangles[triIndex++] = topRight;

                // Second triangle (bottom-left to top-right to bottom-right)
                triangles[triIndex++] = bottomLeft;
                triangles[triIndex++] = topRight;
                triangles[triIndex++] = bottomRight;
            }
        }

        // Assign data to mesh
        terrainMesh.vertices = vertices;
        terrainMesh.triangles = triangles;
        terrainMesh.uv = uvs;

        // Recalculate normals for lighting
        terrainMesh.RecalculateNormals();

        // Recalculate bounds
        terrainMesh.RecalculateBounds();

        // Assign mesh to MeshFilter
        meshFilter.mesh = terrainMesh;

        // Set default material if none exists
        if (meshRenderer.sharedMaterial == null)
        {
            Material defaultMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            meshRenderer.sharedMaterial = defaultMaterial;
        }

        Debug.Log($"Generated terrain mesh: {width}x{height} vertices");
    }

    public void Updatemesh(TerrainData terrainData)
    {
        if (terrainMesh == null)
        {
            GenerateMesh(terrainData);
            return;
        }

        int width = terrainData.width;
        int height = terrainData.height;

        Vector3[] vertices = terrainMesh.vertices;

        for (int y = 0; y <= height; y++)
        {
            for (int x = 0; x <= width; x++)
            {
                int index = x + y * (width + 1);

                if (index >= vertices.Length)
                    continue;

                int heightmapX = Mathf.Min(x, width - 1);
                int heightmapY = Mathf.Min(y, height - 1);
                float heightValue = terrainData.GetHeight(heightmapX, heightmapY) * terrainData.verticalScale;

                vertices[index].y = heightValue;
            }
        }

        terrainMesh.vertices = vertices;
        terrainMesh.RecalculateNormals();
        terrainMesh.RecalculateBounds();
    }

}
