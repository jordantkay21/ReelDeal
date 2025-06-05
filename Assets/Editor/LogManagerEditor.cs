using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using KayosTech.ReelDeal.Prototype.LogSystem;
using KayosTech.ReelDeal.Prototype.LogSystem.Bridge.Manager;
using KayosTech.ReelDeal.Prototype.LogSystem.Settings; // update if needed

[CustomEditor(typeof(LogSettingsManager))]
public class LogManagerEditor : Editor
{
    private SerializedProperty soList;
    private Dictionary<AppLogType, bool> foldoutStates = new();

    private void OnEnable()
    {
        soList = serializedObject.FindProperty("logTypeAssets");

        foreach (AppLogType type in System.Enum.GetValues(typeof(AppLogType)))
        {
            if (!foldoutStates.ContainsKey(type))
                foldoutStates[type] = false;
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("🛠️ Log Type Settings (ScriptableObjects)", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("Each log type configuration is stored in a separate .asset file.", MessageType.Info);

        for (int i = 0; i < soList.arraySize; i++)
        {
            var element = soList.GetArrayElementAtIndex(i);
            var logAsset = element.objectReferenceValue as LogTypeSettingsSO;

            if (logAsset == null)
            {
                EditorGUILayout.HelpBox("⚠️ Missing or null LogTypeSettingsSO reference.", MessageType.Warning);
                continue;
            }

            AppLogType type = logAsset.type;
            Color backgroundColor = logAsset.primaryColor;

            GUI.backgroundColor = backgroundColor;
            EditorGUILayout.BeginVertical("box");

            foldoutStates[type] = EditorGUILayout.Foldout(
                foldoutStates[type],
                $"{type} Log Settings",
                true,
                EditorStyles.foldoutHeader
            );

            if (foldoutStates[type])
            {
                EditorGUI.BeginChangeCheck();

                EditorGUILayout.PropertyField(element, new GUIContent("ScriptableObject Reference"));
                logAsset.showInUI = EditorGUILayout.Toggle("Show In UI", logAsset.showInUI);

                EditorGUILayout.Space(4);
                logAsset.duration = EditorGUILayout.Slider("Show Duration (sec)", logAsset.duration, 1f, 30f);
                logAsset.fadeDuration = EditorGUILayout.Slider("Fade Duration (sec)", logAsset.fadeDuration, 0f, 5f);

                EditorGUILayout.Space(4);
                logAsset.primaryColor = EditorGUILayout.ColorField("Primary Color", logAsset.primaryColor);
                logAsset.secondaryColor = EditorGUILayout.ColorField("Secondary Color", logAsset.secondaryColor);

                EditorGUILayout.Space(6);

                GUIContent buttonContent = new GUIContent(
                    $"  Test {type} Log",
                    GetIconForLogType(type),
                    $"Emit a test {type} log using DevLog"
                );

                if (GUILayout.Button(buttonContent, GUILayout.Height(24)))
                {
                    EmitTestLog(type);
                }

                GUI.enabled = true;

                if (EditorGUI.EndChangeCheck())
                {
                    EditorUtility.SetDirty(logAsset);
                }
            }

            EditorGUILayout.EndVertical();
            GUI.backgroundColor = Color.white;
            EditorGUILayout.Space(8);
        }

        serializedObject.ApplyModifiedProperties();

        DrawValidationUI();
        DrawAutoAssignButton();
    }

    private void DrawValidationUI()
    {
        EditorGUILayout.Space(8);
        EditorGUILayout.LabelField("📋 Validation", EditorStyles.boldLabel);

        if (GUILayout.Button("Check for Missing AppLogTypes"))
        {
            var manager = (LogSettingsManager)target;
            var definedTypes = manager.LogTypeAssets?.Where(x => x != null).Select(x => x.type).ToList() ?? new List<AppLogType>();
            var allTypes = System.Enum.GetValues(typeof(AppLogType)).Cast<AppLogType>().ToList();
            var missing = allTypes.Except(definedTypes).ToList();

            if (missing.Count == 0)
                Debug.Log("✅ All AppLogTypes are covered.");
            else
                Debug.LogWarning("⚠️ Missing AppLogTypes: " + string.Join(", ", missing));
        }
    }

    private void EmitTestLog(AppLogType type)
    {
        string message = $"[TEST] This is a {type} log from the LogManagerEditor.";

        switch (type)
        {
            case AppLogType.Info: DevLog.Info(message); break;
            case AppLogType.Success: DevLog.Success(message); break;
            case AppLogType.Alert: DevLog.Warning(message); break;
            case AppLogType.Error: DevLog.Error(message); break;
            case AppLogType.Internal: DevLog.Info("[INTERNAL] " + message, "InternalTest"); break;
            case AppLogType.Urgent: DevLog.Info("[URGENT] " + message, "UrgentTest"); break;
        }
    }

    private Texture2D GetIconForLogType(AppLogType type)
    {
        string iconName = type switch
        {
            AppLogType.Info => "console.infoicon",
            AppLogType.Success => "TestPassed",
            AppLogType.Alert => "console.warnicon",
            AppLogType.Error => "console.erroricon",
            AppLogType.Internal => "UnityEditor.ConsoleWindow",
            AppLogType.Urgent => "console.erroricon.sml",
            _ => "console.infoicon"
        };

        return EditorGUIUtility.IconContent(iconName).image as Texture2D;
    }

    private void DrawAutoAssignButton()
    {
        EditorGUILayout.Space(8);
        EditorGUILayout.LabelField("⚙️ Tools", EditorStyles.boldLabel);

        if (GUILayout.Button("🧠 Auto-Assign All Log Type Settings"))
        {
            string[] guids = AssetDatabase.FindAssets("t:LogTypeSettingsSO");
            var assets = guids
                .Select(guid => AssetDatabase.LoadAssetAtPath<LogTypeSettingsSO>(AssetDatabase.GUIDToAssetPath(guid)))
                .Where(asset => asset != null)
                .GroupBy(asset => asset.type)
                .Select(g => g.First()) // prevent duplicates per AppLogType
                .OrderBy(a => a.type)
                .ToList();

            var manager = (LogSettingsManager)target;
            Undo.RecordObject(manager, "Auto-Assign Log Type Settings");
            SerializedProperty listProp = serializedObject.FindProperty("logTypeAssets");

            listProp.ClearArray();
            for (int i = 0; i < assets.Count; i++)
            {
                listProp.InsertArrayElementAtIndex(i);
                listProp.GetArrayElementAtIndex(i).objectReferenceValue = assets[i];
            }

            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(manager);
            Debug.Log($"✅ Assigned {assets.Count} LogTypeSettingsSO assets to LogSettingsManager.");
        }
    }
}
