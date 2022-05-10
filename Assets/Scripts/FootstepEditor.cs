
using UnityEditor;

[CustomEditor(typeof(MaterialSetter))]
public class FootstepEditor : Editor{
    public override void OnInspectorGUI()
    {
        var MS = target as MaterialSetter;
        var FS = FindObjectOfType<PlayerController>();

        MS.materialValue = EditorGUILayout.Popup("Set Material As", MS.materialValue, FS.materialTypes);
    }
}

[CustomEditor(typeof(PlayerController))]
public class FootstepEditorTwo : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var FS = target as PlayerController;
        FS.defaultMaterialValue = EditorGUILayout.Popup("Set Default Material as", FS.defaultMaterialValue, FS.materialTypes);
    }
}
