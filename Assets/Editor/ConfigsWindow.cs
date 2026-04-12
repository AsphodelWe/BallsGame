using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ConfigsWindow : EditorWindow
{
    private Vector2 scrollPosition;
    private Dictionary<string, bool> folderFoldouts = new();
    private Dictionary<string, List<ScriptableObject>> configsByFolder = new();
    private Dictionary<ScriptableObject, bool> configFoldouts = new();

    private readonly string[] _rootFolders = { "Assets/_Project/Configs", "Assets/Configs" };

    [MenuItem("Tools/Configs Window")]
    public static void ShowWindow()
    {
        GetWindow<ConfigsWindow>("Конфиги");
    }

    private void OnEnable()
    {
        LoadConfigs();
    }

    private void LoadConfigs()
    {
        configsByFolder.Clear();

        foreach (string root in _rootFolders)
        {
            if (!Directory.Exists(root)) continue;

            string[] guids = AssetDatabase.FindAssets("t:ScriptableObject", new[] { root });

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
                if (asset == null) continue;

                string folder = Path.GetDirectoryName(path).Replace("\\", "/");

                if (!configsByFolder.ContainsKey(folder))
                    configsByFolder[folder] = new List<ScriptableObject>();

                configsByFolder[folder].Add(asset);
            }
        }

        foreach (var folder in configsByFolder.Keys)
        {
            if (!folderFoldouts.ContainsKey(folder))
                folderFoldouts[folder] = true;
        }
    }

    private void OnGUI()
    {
        int total = configsByFolder.Values.Sum(list => list.Count);

        EditorGUILayout.LabelField($"Конфигов: {total}", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        // Получаем корневые папки
        var rootPaths = GetSortedRoots();

        foreach (string rootPath in rootPaths)
        {
            DrawFolderRecursive(rootPath);
        }

        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("Обновить", GUILayout.Height(25)))
        {
            LoadConfigs();
        }
    }

    private List<string> GetRootPaths()
    {
        var roots = new List<string>();

        foreach (string folder in configsByFolder.Keys)
        {
            string parent = Path.GetDirectoryName(folder).Replace("\\", "/");

            // Если у папки нет родителя в нашем списке — это корень
            if (!configsByFolder.ContainsKey(parent))
                roots.Add(folder);
        }

        return roots;
    }

    private void DrawFolderRecursive(string folderPath, string parentPath = null)
    {
        if (parentPath != null)
        {
            string actualParent = Path.GetDirectoryName(folderPath).Replace("\\", "/");
            if (actualParent != parentPath)
                return;
        }

        string folderName = Path.GetFileName(folderPath);
        int count = configsByFolder.ContainsKey(folderPath) ? configsByFolder[folderPath].Count : 0;

        var childFolders = GetChildFolders(folderPath);

        folderFoldouts[folderPath] = EditorGUILayout.Foldout(folderFoldouts[folderPath], $"📁 {folderName} ({count})", true);

        if (!folderFoldouts[folderPath]) return;

        EditorGUI.indentLevel++;

        if (configsByFolder.ContainsKey(folderPath))
        {
            foreach (var config in configsByFolder[folderPath])
            {
                DrawConfig(config);
            }
        }

        // Подпапки
        foreach (string child in childFolders)
        {
            DrawFolderRecursive(child, folderPath);
        }

        EditorGUI.indentLevel--;
    }

    private List<string> GetChildFolders(string parentPath)
    {
        var children = new List<string>();

        foreach (string folder in configsByFolder.Keys)
        {
            string folderParent = Path.GetDirectoryName(folder).Replace("\\", "/");
            if (folderParent == parentPath)
                children.Add(folder);
        }

        children.Sort();
        return children;
    }

    private void DrawConfig(ScriptableObject config)
    {
        // Инициализируем состояние развёрнутости
        if (!configFoldouts.ContainsKey(config))
            configFoldouts[config] = false;

        // Заголовок конфига
        configFoldouts[config] = EditorGUILayout.Foldout(configFoldouts[config], config.name, true);

        if (!configFoldouts[config]) return;

        EditorGUI.indentLevel++;

        // Рисуем поля конфига
        var editor = Editor.CreateEditor(config);
        editor.OnInspectorGUI();

        EditorGUI.indentLevel--;
        EditorGUILayout.Space(2);
    }

    private List<string> GetSortedRoots()
{
    var roots = GetRootPaths();
    
    var priority = new Dictionary<string, int>
    {
        { "Country", 1 },
        { "Side", 2 },
        { "Weapons", 3 },
        { "Bullet", 4 },
        { "Map", 4 }
    };
    
    roots.Sort((a, b) =>
    {
        string nameA = Path.GetFileName(a);
        string nameB = Path.GetFileName(b);
        
        int priorityA = priority.ContainsKey(nameA) ? priority[nameA] : 999;
        int priorityB = priority.ContainsKey(nameB) ? priority[nameB] : 999;
        
        if (priorityA != priorityB)
            return priorityA.CompareTo(priorityB);
        
        return nameA.CompareTo(nameB); // по алфавиту
    });
    
    return roots;
}

}
