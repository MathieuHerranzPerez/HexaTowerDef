using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MapBuilder : EditorWindow
{
    [SerializeField]
    private List<GameObject> listPrefabTileContent = new List<GameObject>(1);

    // ---- INTERN ----
    Event currentEvent;
    private int selectedLayer;

    private Vector3 currentMousePosition = Vector3.zero;
    private Vector3 previousMousePosition = Vector3.zero;

    private GameObject objectToBuild;


    [MenuItem("Window/HexaTileMapBuilder")]
    public static void ShowWindow()
    {
        GetWindow<MapBuilder>();
    }

    void OnGUI()
    {
        selectedLayer = EditorGUILayout.LayerField("Tile layer", selectedLayer);
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("List TileContent prefabs");
        int newSize = Mathf.Max(0, EditorGUILayout.IntField("size", listPrefabTileContent.Count));

        while (newSize < listPrefabTileContent.Count)
            listPrefabTileContent.RemoveAt(listPrefabTileContent.Count - 1);
        while (newSize > listPrefabTileContent.Count)
            listPrefabTileContent.Add(null);

        for (int i = 0; i < listPrefabTileContent.Count; i++)
        {
            if (GUILayout.Button("select"))
            {
                objectToBuild = listPrefabTileContent[i];
            }
            listPrefabTileContent[i] = (GameObject)EditorGUILayout.ObjectField("", listPrefabTileContent[i], typeof(GameObject), false);
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("Stop editing"))
        {
            objectToBuild = null;
        }
    }

    void SceneGUI(SceneView sceneView)
    {
        currentEvent = Event.current;
        //UpdateMousePos(sceneView);
        SceneInput(sceneView);
    }

    private void SceneInput(SceneView sceneView)
    {
        if ((currentEvent.button == 0 && currentEvent.type == EventType.MouseDown))
        {
            Vector3 mousePos = currentEvent.mousePosition;
            float ppp = EditorGUIUtility.pixelsPerPoint;
            mousePos.y = sceneView.camera.pixelHeight - mousePos.y * ppp;
            mousePos.x *= ppp;

            Ray ray = sceneView.camera.ScreenPointToRay(mousePos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000, 1 << selectedLayer))
            {
                currentMousePosition = hit.point;
                TryToPlaceTileContent(hit.collider);
            }
        }
    }

    private void TryToPlaceTileContent(Collider collider)
    {
        if (objectToBuild != null)
        {
            Tile tile = collider.gameObject.GetComponent<Tile>();
            if (tile != null && tile.content == null)
            {
                GameObject tileContentGO = (GameObject)Instantiate(objectToBuild, tile.center, tile.transform.rotation, tile.transform);
                TileContent tileContent = tileContentGO.GetComponent<TileContent>();
                tile.content = tileContent;
            }
        }
        else
        {
            Debug.LogWarning("No Tile content selected");
        }
    }


    void OnEnable()
    {
        SceneView.duringSceneGui += SceneGUI;
    }

    void OnDisable()
    {
        SceneView.duringSceneGui -= SceneGUI;
    }
}
