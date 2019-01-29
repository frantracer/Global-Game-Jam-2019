using UnityEngine;
using UnityEditor;

public class CustomTileManager : MonoBehaviour {

    [MenuItem("Assets/Create/Custom Assets/Custom Tile")]
    public static CustomTile Create()
    {
        CustomTile asset = ScriptableObject.CreateInstance<CustomTile>();

        AssetDatabase.CreateAsset(asset, "Assets/CustomTile.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;

        return asset;
    }
}
