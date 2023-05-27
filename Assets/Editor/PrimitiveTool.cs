using UnityEditor;
using UnityEngine;

public class SpawnTool : EditorWindow
{
    string[] monsterTypes = new string[] { "Bat Lord", "Shadow Razor", "Spider King", "Vampire Bat", "Troll" };
    string[] levelVariations = new string[] { "Level 1", "Level 2", "Level 3" };

    int selectedMonster = 0;
    int selectedLevel = 0;

    public GameObject[] monsterPrefabs;

    public float health = 0;
    public float power = 0;
    public float movementSpeed = 0;

    float spawnRadius = 3f; // spawn within a 5 unit radius

    [MenuItem("Window/Spawn Tool")]
    public static void ShowWindow()
    {
        GetWindow<SpawnTool>("Spawn Tool");
    }

    void OnGUI()
    {
        GUILayout.Label("Select the obstacle you want to spawn.", EditorStyles.boldLabel);

        GUILayout.BeginVertical("box");
        GUILayout.Label("Select Monster Type", EditorStyles.boldLabel);
        selectedMonster = EditorGUILayout.Popup(selectedMonster, monsterTypes);
        GUILayout.EndVertical();

        GUILayout.Space(10);

        GUILayout.BeginVertical("box");
        GUILayout.Label("Select Monster Level", EditorStyles.boldLabel);
        selectedLevel = EditorGUILayout.Popup(selectedLevel, levelVariations);
        GUILayout.EndVertical();

        GUILayout.Space(10);

        // Update monster prefabs field
        SerializedObject serializedObject = new SerializedObject(this);
        SerializedProperty serializedProperty = serializedObject.FindProperty("monsterPrefabs");
        EditorGUILayout.PropertyField(serializedProperty, true);
        serializedObject.ApplyModifiedProperties();

        // Sliders for Health, Power, and Movement Speed
        GUILayout.BeginVertical("box");
        GUILayout.Label("Adjust Monster Attributes", EditorStyles.boldLabel);
        AdjustAttribute(ref health, "Health");
        AdjustAttribute(ref power, "Power");
        AdjustAttribute(ref movementSpeed, "Movement Speed");
        GUILayout.EndVertical();

        if (GUILayout.Button("Spawn Monster", GUILayout.Height(50)))
        {
            SpawnMonster();
        }
    }

    // Slider adjustment method
    void AdjustAttribute(ref float attribute, string label)
    {
        float oldValue = attribute;
        attribute = EditorGUILayout.Slider(label, attribute, 0, 100);
        float total = health + power + movementSpeed;

        if (total > 175)
        {
            float diff = total - 175;

            if (attribute != health)
                health = Mathf.Max(0, Mathf.RoundToInt(health - diff * (health / total)));
            if (attribute != power)
                power = Mathf.Max(0, Mathf.RoundToInt(power - diff * (power / total)));
            if (attribute != movementSpeed)
                movementSpeed = Mathf.Max(0, Mathf.RoundToInt(movementSpeed - diff * (movementSpeed / total)));
        }
    }



    void SpawnMonster()
    {
        GameObject monsterPrefab = monsterPrefabs[selectedMonster];
        GameObject newMonster = Instantiate(monsterPrefab);

        // Randomly place the new monster within spawnRadius
        newMonster.transform.position = new Vector3(Random.Range(-spawnRadius, spawnRadius), 0, Random.Range(-spawnRadius, spawnRadius));

        // Set monster's name
        newMonster.name = $"{monsterTypes[selectedMonster]} - {levelVariations[selectedLevel]}";
    }
}
