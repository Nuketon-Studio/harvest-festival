using HarvestFestival.SO;
using HarvestFestival.Types;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CharacterSO))]
public class CharacterDropdownEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CharacterSO script = (CharacterSO) target;

        GUIContent skinLabel = new GUIContent("Skin");
        var skinList = SkinPathTypes.ToArray();

        script.skinIndex = EditorGUILayout.Popup(skinLabel, script.skinIndex, skinList);
        script.skin = skinList[script.skinIndex];

        GUIContent projectileLabel = new GUIContent("Projectile");
        var projectileList = ProjectilePathTypes.ToArray();

        script.projectileIndex = EditorGUILayout.Popup(projectileLabel, script.projectileIndex, projectileList);
        script.projectile = projectileList[script.projectileIndex];
    }
}