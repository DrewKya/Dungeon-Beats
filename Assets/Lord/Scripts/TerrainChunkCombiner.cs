using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TerrainChunkCombiner : MonoBehaviour
{
    public const int CHUNK_SIZE = 32;
    [SerializeField] private Transform tilesParent; // Parent object containing all tiles
    [SerializeField] private Material defaultMaterial;

    private List<TerrainChunk> chunks = new List<TerrainChunk>();

    public static GameObject CombineTiles(GameObject[] tiles)
    {
        GameObject combined = new GameObject("CombinedTerrain");
        List<CombineInstance> combineInstances = new List<CombineInstance>();
        List<Material> materials = new List<Material>();

        foreach (GameObject tile in tiles)
        {
            // Only get the first child
            if (tile.transform.childCount > 0)
            {
                Transform firstChild = tile.transform.GetChild(0);
                MeshFilter meshFilter = firstChild.GetComponent<MeshFilter>();

                if (meshFilter != null && meshFilter.sharedMesh != null)
                {
                    CombineInstance ci = new CombineInstance();
                    ci.mesh = meshFilter.sharedMesh;
                    // Include the parent tile's transform in the calculation
                    ci.transform = firstChild.transform.localToWorldMatrix;
                    combineInstances.Add(ci);

                    // Get material from the first child
                    MeshRenderer _renderer = firstChild.GetComponent<MeshRenderer>();
                    if (_renderer != null && _renderer.sharedMaterial != null)
                    {
                        materials.Add(_renderer.sharedMaterial);
                    }
                }
            }
        }

        // Create the combined mesh
        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.CombineMeshes(combineInstances.ToArray(), true);

        // Add components
        combined.AddComponent<MeshFilter>().mesh = mesh;
        MeshRenderer renderer = combined.AddComponent<MeshRenderer>();
        if (materials.Count > 0)
        {
            renderer.material = materials[0]; // Use the first material found
        }
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        // Add MeshCollider
        MeshCollider meshCollider = combined.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
        meshCollider.convex = false;

        return combined;
    }

    public class TerrainChunk
    {
        public GameObject chunkObject;
        public Vector2Int chunkPosition;

        public TerrainChunk(Vector2Int position, GameObject[] tiles)
        {
            chunkPosition = position;
            chunkObject = new GameObject($"Chunk_{position.x}_{position.y}");

            // Filter tiles that belong to this chunk
            List<GameObject> tilesInChunk = new List<GameObject>();
            foreach (GameObject tile in tiles)
            {
                Vector3 tilePos = tile.transform.position;
                int chunkX = Mathf.FloorToInt(tilePos.x / CHUNK_SIZE);
                int chunkZ = Mathf.FloorToInt(tilePos.z / CHUNK_SIZE);

                if (chunkX == position.x && chunkZ == position.y)
                {
                    tilesInChunk.Add(tile);
                }
            }

            if (tilesInChunk.Count > 0)
            {
                GameObject combinedTiles = CombineTiles(tilesInChunk.ToArray());
                combinedTiles.transform.parent = chunkObject.transform;
            }
            else
            {
                //Destroy the chunk if it contains no tiles
                #if UNITY_EDITOR
                    DestroyImmediate(chunkObject);
                #else
                    Destroy(chunkObject);
                #endif
                chunkObject = null;
            }
        }
    }

    public void ClearChunks()
    {
        foreach (var chunk in chunks)
        {
            if (chunk.chunkObject != null)
            {
#if UNITY_EDITOR
                DestroyImmediate(chunk.chunkObject);
#else
                Destroy(chunk.chunkObject);
#endif
            }
        }
        chunks.Clear();
    }

    public void CreateChunks()
    {
        if (tilesParent == null)
        {
            Debug.LogError("Tiles Parent is not assigned!");
            return;
        }

        ClearChunks();

        // Get all tiles from the parent
        List<GameObject> allTiles = new List<GameObject>();
        for (int i = 0; i < tilesParent.childCount; i++)
        {
            allTiles.Add(tilesParent.GetChild(i).gameObject);
        }

        if (allTiles.Count == 0)
        {
            Debug.LogWarning("No tiles found in parent object!");
            return;
        }

        // Find bounds
        float minX = float.MaxValue, maxX = float.MinValue;
        float minZ = float.MaxValue, maxZ = float.MinValue;

        foreach (GameObject tile in allTiles)
        {
            Vector3 pos = tile.transform.position;
            minX = Mathf.Min(minX, pos.x);
            maxX = Mathf.Max(maxX, pos.x);
            minZ = Mathf.Min(minZ, pos.z);
            maxZ = Mathf.Max(maxZ, pos.z);
        }

        // Calculate chunk indices
        int startX = Mathf.FloorToInt(minX / CHUNK_SIZE);
        int endX = Mathf.CeilToInt(maxX / CHUNK_SIZE);
        int startZ = Mathf.FloorToInt(minZ / CHUNK_SIZE);
        int endZ = Mathf.CeilToInt(maxZ / CHUNK_SIZE);

        GameObject chunksParent = new GameObject("Chunks");
        chunksParent.transform.parent = transform;

        for (int x = startX; x < endX; x++)
        {
            for (int z = startZ; z < endZ; z++)
            {
                TerrainChunk chunk = new TerrainChunk(new Vector2Int(x, z), allTiles.ToArray());
                if(chunk.chunkObject != null)
                {
                    chunk.chunkObject.transform.parent = chunksParent.transform;
                    chunks.Add(chunk);
                }
            }
        }
    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(TerrainChunkCombiner))]
public class TerrainChunkCombinerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TerrainChunkCombiner combiner = (TerrainChunkCombiner)target;

        EditorGUILayout.Space();
        if (GUILayout.Button("Create Chunks"))
        {
            Undo.RecordObject(combiner.gameObject, "Create Terrain Chunks");
            combiner.CreateChunks();
            EditorUtility.SetDirty(combiner.gameObject);
        }

        if (GUILayout.Button("Clear Chunks"))
        {
            Undo.RecordObject(combiner.gameObject, "Clear Terrain Chunks");
            combiner.ClearChunks();
            EditorUtility.SetDirty(combiner.gameObject);
        }
    }
}
#endif