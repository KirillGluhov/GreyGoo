using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWorld : MonoBehaviour
{
    private const int ViewRadius = 5;

    public Dictionary<Vector2Int, ChunkData> ChunkDatas = new Dictionary<Vector2Int, ChunkData>();
    public ChunkGenerator Chunkprefab;
    public TerrainGenerator Generator;

    public bool IsStartFinished = false;

    public GameObject player;

    private Camera mainCamera;
    private Vector2Int currentPlayerChunk;
    public void Start()
    {
        mainCamera = Camera.main;

        Generator.Init();
        Generat(false, ViewRadius);

        IsStartFinished = true;
    }

    private IEnumerator Generate(bool wait, int radius)
    {
        for (int x = currentPlayerChunk.x - radius; x < currentPlayerChunk.x + radius; x++)
        {
            for (int y = currentPlayerChunk.y - radius; y < currentPlayerChunk.y + radius; y++)
            {

                Vector2Int chunkPosition = new Vector2Int(x, y);

                if (ChunkDatas.ContainsKey(chunkPosition))
                {
                    var value = ChunkDatas[chunkPosition];

                    for (int i = 0; i < value.infoAboutChunk.AnimalsAndTrees.Count; i++)
                    {
                        value.infoAboutChunk.AnimalsAndTrees[i].SetActive(true);
                    }
                    value.Renderer.gameObject.SetActive(true);
                    continue;
                }

                LoadChunk(chunkPosition);

                if (wait) yield return new WaitForSecondsRealtime(0.2f);
            }
        }
    }

    private void Generat(bool wait, int radius)
    {
        for (int x = currentPlayerChunk.x - radius; x < currentPlayerChunk.x + radius; x++)
        {
            for (int y = currentPlayerChunk.y - radius; y < currentPlayerChunk.y + radius; y++)
            {

                Vector2Int chunkPosition = new Vector2Int(x, y);

                if (ChunkDatas.ContainsKey(chunkPosition))
                {
                    var value = ChunkDatas[chunkPosition];

                    for (int i = 0; i < value.infoAboutChunk.AnimalsAndTrees.Count; i++)
                    {
                        value.infoAboutChunk.AnimalsAndTrees[i].SetActive(true);
                    }
                    value.Renderer.gameObject.SetActive(true);
                    continue;
                }

                LoadChunk(chunkPosition);
            }
        }

        IsStartFinished = true;

    }

    public void LoadChunk(Vector2Int chunkPosition)
    {
        float xPos = chunkPosition.x * ChunkGenerator.ChunkWidth * ChunkGenerator.BlockScale;
        float zPos = chunkPosition.y * ChunkGenerator.ChunkWidth * ChunkGenerator.BlockScale;

        ChunkData chunkData = new ChunkData();

        chunkData.Chunkposition = chunkPosition;
        chunkData.infoAboutChunk = Generator.GenerateTerrain(xPos, zPos);
        ChunkDatas.Add(chunkPosition, chunkData);

        var chunk = Instantiate(Chunkprefab, new Vector3(xPos, 0, zPos), Quaternion.identity, transform);
        chunk.ChunkData = chunkData;
        chunk.ParentWorld = this;

        chunkData.Renderer = chunk;
    }

    private void Update()
    {
        Vector3Int playerWorldPos = Vector3Int.FloorToInt(player.transform.position / ChunkGenerator.BlockScale);
        Vector2Int playerChunk = GetChunkContainingBlock(playerWorldPos);

        if (playerChunk != currentPlayerChunk)
        {
            currentPlayerChunk = playerChunk;
            StartCoroutine(Generate(true, ViewRadius));
        }

        List<Vector2Int> chunksToRemove = new List<Vector2Int>();
        foreach (var chunkPos in ChunkDatas.Keys)
        {
            float distance = Vector2Int.Distance(chunkPos, playerChunk);
            if (distance > 2*ViewRadius)
            {
                chunksToRemove.Add(chunkPos);
            }
        }

        foreach (var chunkPos in chunksToRemove)
        {
            UnloadChunk(chunkPos);
        }
    }

    public Vector2Int GetChunkContainingBlock(Vector3Int blockWorldPos)
    {
        Vector2Int chunkPosition = new Vector2Int(blockWorldPos.x/ChunkGenerator.ChunkWidth, blockWorldPos.z/ChunkGenerator.ChunkWidth);

        if (blockWorldPos.x < 0)
        {
            chunkPosition.x--;
        }

        if (blockWorldPos.y < 0)
        {
            chunkPosition.y--;
        }

        return chunkPosition;
    }

    private void UnloadChunk(Vector2Int chunkPosition)
    {
        if (ChunkDatas.ContainsKey(chunkPosition))
        {
            ChunkData chunkData = ChunkDatas[chunkPosition];

            if (chunkData.Renderer != null)
            {
                for (int i = 0; i < chunkData.infoAboutChunk.AnimalsAndTrees.Count; i++)
                {
                    chunkData.infoAboutChunk.AnimalsAndTrees[i].SetActive(false);
                }

                chunkData.Renderer.gameObject.SetActive(false);
            }
        }
    }
}
