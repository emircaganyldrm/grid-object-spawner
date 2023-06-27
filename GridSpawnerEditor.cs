using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class GridSpawnerEditor : EditorWindow
{
    private GameObject _gridPrefab;
    
    private Vector2Int _gridSize = new Vector2Int(2,2);
    
    private Vector2 _cellSize = new Vector2(1,1);

    private float _heightOffset = 0.01f;
    
    private static List<GameObject> _lastSpawnedObjects = new List<GameObject>();

    [MenuItem("Tools/Grid Spawner")]
    private static void Init()
    {
        GridSpawnerEditor gridSpawnerWindow = (GridSpawnerEditor)GetWindow(typeof(GridSpawnerEditor));
        
        gridSpawnerWindow.titleContent = new GUIContent("Grid Spawner");
        gridSpawnerWindow.maxSize = new Vector2(300, 250);
        gridSpawnerWindow.minSize = new Vector2(300, 250);
        gridSpawnerWindow.Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.Space();
        
        GUILayout.Label("Grid Object");
        _gridPrefab = (GameObject)EditorGUILayout.ObjectField(_gridPrefab, typeof(GameObject), true,
            GUILayout.MinHeight(50));
        
        EditorGUILayout.Space();
        
        _gridSize = EditorGUILayout.Vector2IntField("Grid Size" ,_gridSize);
        
        EditorGUILayout.Space();
        
        _cellSize = EditorGUILayout.Vector2Field("Cell Size", _cellSize);
        _heightOffset = EditorGUILayout.FloatField("Height Offset", _heightOffset);
        
        EditorGUILayout.Space();

        
        if (GUILayout.Button("Spawn Grid"))
        {
            SpawnGrid();
        }

        EditorGUILayout.BeginHorizontal();
        
        if (GUILayout.Button("Delete Last"))
        {
            if (_lastSpawnedObjects != null)
            {
                DestroyImmediate(_lastSpawnedObjects[^1]);
                _lastSpawnedObjects.Remove(_lastSpawnedObjects[^1]);
            }
        }

        if (GUILayout.Button("Delete All"))
        {
            if (_lastSpawnedObjects != null)
            {
                foreach (var lastSpawnedObject in _lastSpawnedObjects.ToList())
                {
                    DestroyImmediate(lastSpawnedObject);
                    _lastSpawnedObjects.Remove(lastSpawnedObject);
                }
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    private void SpawnGrid()
    {
        if (_gridPrefab == null)
        {
            ShowNotification(new GUIContent("No Grid Prefab Selected"));
            return;
        }
        
        GameObject gridParent = new GameObject("Grid");
        
        
        for (int x = 0; x < _gridSize.x; x++)
        {
            for (int y = 0; y < _gridSize.y; y++)
            {
                var go = Instantiate(_gridPrefab, gridParent.transform);
                go.transform.position = new Vector3(x * _cellSize.x, _heightOffset, y * _cellSize.y);
            }
        }
        
        _lastSpawnedObjects.Add(gridParent);
    }
}
