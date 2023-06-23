using System.Collections.Generic;
using UnityEngine;

public class ChunkData
{
    public Vector2Int Chunkposition;
    public ChunkGenerator Renderer; 
    public infoaboutChunk infoAboutChunk;
}

public class infoaboutChunk
{
    public BlockType[,,] Blocks;
    public List<GameObject> AnimalsAndTrees;
}
