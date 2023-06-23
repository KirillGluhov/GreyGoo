using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]

public class ChunkGenerator : MonoBehaviour
{
    public const int ChunkWidth = 16;
    public const int ChunkHeight = 128;
    public const float BlockScale = 0.25f;
    public ChunkData ChunkData;
    public GameWorld ParentWorld;

    private Mesh chunkMesh;

    private List<Vector3> verticies = new List<Vector3>();
    private List<Vector2> uvs = new List<Vector2>();
    private List<int> triangles = new List<int>();

    private void Start()
    {
        chunkMesh = new Mesh();

        RegenerateMesh();

        GetComponent<MeshFilter>().mesh = chunkMesh;
        GetComponent<MeshCollider>().sharedMesh = chunkMesh;
    }

    private void RegenerateMesh()
    {
        verticies.Clear();
        uvs.Clear();
        triangles.Clear();

        for (int y = 0; y < ChunkHeight; y++)
        {
            for (int x = 0; x < ChunkWidth; x++)
            {
                for (int z = 0; z < ChunkWidth; z++)
                {
                    GenerateBlock(x, y, z);
                }
            }
        }

        chunkMesh.triangles = System.Array.Empty<int>();

        chunkMesh.vertices = verticies.ToArray();
        chunkMesh.uv = uvs.ToArray();
        chunkMesh.triangles = triangles.ToArray();

        chunkMesh.Optimize();
        chunkMesh.RecalculateBounds();
        chunkMesh.RecalculateNormals();

        GetComponent<MeshCollider>().sharedMesh = chunkMesh;
    }

    private void GenerateBlock(int x, int y, int z)
    {
        var blockPosition = new Vector3Int(x, y, z);

        if (GetBlockAtPosition(blockPosition) == 0) return;

            if (GetBlockAtPosition(blockPosition + Vector3Int.right) == 0)
            {
                GenerateRightSide(blockPosition, GetBlockAtPosition(blockPosition));
            }
            if (GetBlockAtPosition(blockPosition + Vector3Int.left) == 0)
            {
                GenerateLeftSide(blockPosition, GetBlockAtPosition(blockPosition));
            }
            if (GetBlockAtPosition(blockPosition + Vector3Int.forward) == 0)
            {
                GenerateFrontSide(blockPosition, GetBlockAtPosition(blockPosition));
            }
            if (GetBlockAtPosition(blockPosition + Vector3Int.back) == 0)
            {
                GenerateBackSide(blockPosition, GetBlockAtPosition(blockPosition));
            }
            if (GetBlockAtPosition(blockPosition + Vector3Int.up) == 0)
            {
                GenerateTopSide(blockPosition, GetBlockAtPosition(blockPosition));
            }
            if (GetBlockAtPosition(blockPosition + Vector3Int.down) == 0)
            {
                GenerateBottomSide(blockPosition, GetBlockAtPosition(blockPosition));
            }

        
    }

    private BlockType GetBlockAtPosition(Vector3Int blockPosition)
    {
        if (blockPosition.x >= 0 && blockPosition.x < ChunkWidth &&
            blockPosition.y >= 0 && blockPosition.y < ChunkHeight &&
            blockPosition.z >= 0 && blockPosition.z < ChunkWidth)
        {
            return ChunkData.infoAboutChunk.Blocks[blockPosition.x, blockPosition.y, blockPosition.z];
        }
        else
        {
            if (blockPosition.y < 0 || blockPosition.y > ChunkHeight) return BlockType.Air;

            Vector2Int adjacentChunkPosition = ChunkData.Chunkposition;


            if (blockPosition.x < 0)
            { 
                adjacentChunkPosition.x--;
                blockPosition.x += ChunkWidth;
            }
            else if (blockPosition.x >= ChunkWidth)
            {
                adjacentChunkPosition.x++;
                blockPosition.x -= ChunkWidth;
            }


            if (blockPosition.z < 0)
            {
                adjacentChunkPosition.y--;
                blockPosition.z += ChunkWidth;
            }
            else if (blockPosition.z >= ChunkWidth)
            {
                adjacentChunkPosition.y++;
                blockPosition.z -= ChunkWidth;
            }

            if (ParentWorld.ChunkDatas.TryGetValue(adjacentChunkPosition, out ChunkData adjacentChunk))
            {
                return adjacentChunk.infoAboutChunk.Blocks[blockPosition.x,blockPosition.y, blockPosition.z];
            }
            else
            {
                return BlockType.Air;
            }

            
        }
    }

    private void GenerateRightSide(Vector3Int blockPosition, BlockType blockType)
    {
        verticies.Add((new Vector3(1, 0, 0) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(1, 1, 0) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(1, 0, 1) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(1, 1, 1) + blockPosition) * BlockScale);

        AddLastVerticiesSquare(1, blockType);
    }

    private void GenerateLeftSide(Vector3Int blockPosition, BlockType blockType)
    {
        verticies.Add((new Vector3(0, 0, 0) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(0, 0, 1) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(0, 1, 0) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(0, 1, 1) + blockPosition) * BlockScale);
        

        AddLastVerticiesSquare(3, blockType);
    }

    private void AddLastVerticiesSquare(int side, BlockType blockType)
    {
        AddUvs(side, blockType);

        triangles.Add(verticies.Count - 4);
        triangles.Add(verticies.Count - 3);
        triangles.Add(verticies.Count - 2);

        triangles.Add(verticies.Count - 3);
        triangles.Add(verticies.Count - 1);
        triangles.Add(verticies.Count - 2);
    }

    private void GenerateFrontSide(Vector3Int blockPosition, BlockType blockType)
    {
        verticies.Add((new Vector3(0, 0, 1) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(1, 0, 1) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(0, 1, 1) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(1, 1, 1) + blockPosition) * BlockScale);
        

        AddLastVerticiesSquare(3, blockType);
    }

    private void GenerateBackSide(Vector3Int blockPosition, BlockType blockType)
    {
        verticies.Add((new Vector3(0, 0, 0) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(0, 1, 0) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(1, 0, 0) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(1, 1, 0) + blockPosition) * BlockScale);

        AddLastVerticiesSquare(1, blockType);
    }

    private void GenerateTopSide(Vector3Int blockPosition, BlockType blockType)
    {
        verticies.Add((new Vector3(0, 1, 0) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(0, 1, 1) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(1, 1, 0) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(1, 1, 1) + blockPosition) * BlockScale);

        AddLastVerticiesSquare(0, blockType);
    }

    private void GenerateBottomSide(Vector3Int blockPosition, BlockType blockType)
    {
        verticies.Add((new Vector3(0, 0, 0) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(1, 0, 0) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(0, 0, 1) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(1, 0, 1) + blockPosition) * BlockScale);

        AddLastVerticiesSquare(4, blockType);
    }

    private void AddUvs(int side, BlockType blockType)
    {
        Vector2 uv;
        int lengthOfImage = 3750;
        int lengthOfSide = 750;
        int height = 1500;

        if (blockType == BlockType.Grass)
        {
            if (side == 0)
            {
                uvs.Add(new Vector2(10f / (float)(lengthOfImage + 0), (float)(lengthOfSide + 10) / (float)(height)));
                uvs.Add(new Vector2(10f / (float)(lengthOfImage + 0), (float)(height - 10) / (float)(height)));
                uvs.Add(new Vector2((float)(lengthOfSide - 10) / (float)(lengthOfImage), (float)(lengthOfSide + 10) / (float)(height)));
                uvs.Add(new Vector2((float)(lengthOfSide - 10) / (float)(lengthOfImage), (float)(height - 10) / (float)(height)));

            }
            else
            {
                uvs.Add(new Vector2((float)(100 + lengthOfSide * 2) / (float)(lengthOfImage), (float)(lengthOfSide + 100) / (float)(height)));
                uvs.Add(new Vector2((float)(100 + lengthOfSide * 2) / (float)(lengthOfImage), (float)(height - 100) / (float)(height)));
                uvs.Add(new Vector2((float)(lengthOfSide * 3 - 100) / (float)(lengthOfImage), (float)(lengthOfSide + 100) / (float)(height)));
                uvs.Add(new Vector2((float)(lengthOfSide * 3 - 100) / (float)(lengthOfImage), (float)(height - 100) / (float)(height)));
            }
        }
        else if (blockType == BlockType.Stone)
        {
            uvs.Add(new Vector2((float)(100 + lengthOfSide * 3) / (float)(lengthOfImage), (float)(lengthOfSide + 100) / (float)(height)));
            uvs.Add(new Vector2((float)(100 + lengthOfSide * 3) / (float)(lengthOfImage), (float)(height - 100) / (float)(height)));
            uvs.Add(new Vector2((float)(lengthOfSide * 4 - 100) / (float)(lengthOfImage), (float)(lengthOfSide + 100) / (float)(height)));
            uvs.Add(new Vector2((float)(lengthOfSide * 4 - 100) / (float)(lengthOfImage), (float)(height - 100) / (float)(height)));
        }
        else if (blockType == BlockType.Wood)
        {
            uvs.Add(new Vector2(20f / lengthOfImage, 20f / height));
            uvs.Add(new Vector2(20f / lengthOfImage, (float)(lengthOfSide - 20) / (float)(height)));
            uvs.Add(new Vector2((float)(lengthOfSide - 20) / (float)(lengthOfImage), 20f / height));
            uvs.Add(new Vector2((float)(lengthOfSide - 20) / (float)(lengthOfImage), (float)(lengthOfSide - 20) / (float)(height)));
        }
        else if (blockType == BlockType.Leafs)
        {
            uvs.Add(new Vector2((float)(20 + lengthOfSide) / lengthOfImage, 20f / height));
            uvs.Add(new Vector2((float)(20 + lengthOfSide) / lengthOfImage, (float)(lengthOfSide - 20) / (float)(height)));
            uvs.Add(new Vector2((float)(lengthOfSide * 2 - 20) / (float)(lengthOfImage), 20f / height));
            uvs.Add(new Vector2((float)(lengthOfSide * 2 - 20) / (float)(lengthOfImage), (float)(lengthOfSide - 20) / (float)(height)));
        }
        else if (blockType == BlockType.Water)
        {
            {
                uvs.Add(new Vector2((float)(100 + lengthOfSide * 4) / (float)(lengthOfImage), (float)(lengthOfSide + 100) / (float)(height)));
                uvs.Add(new Vector2((float)(100 + lengthOfSide * 4) / (float)(lengthOfImage), (float)(height - 100) / (float)(height)));
                uvs.Add(new Vector2((float)(lengthOfSide * 5 - 100) / (float)(lengthOfImage), (float)(lengthOfSide + 100) / (float)(height)));
                uvs.Add(new Vector2((float)(lengthOfSide * 5 - 100) / (float)(lengthOfImage), (float)(height - 100) / (float)(height)));
            }

        }

    }
}
