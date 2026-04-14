using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelController))]
public class LevelControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LevelController controller = (LevelController)target;

        // Music Section

        EditorGUILayout.LabelField("Music", EditorStyles.boldLabel);
        controller.playMusic = EditorGUILayout.Toggle("Play Music", controller.playMusic);

        if (controller.playMusic)
        {
            EditorGUI.indentLevel++;
            controller.musicToPlay = EditorGUILayout.TextField("Music To Play", controller.musicToPlay);
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space(10);

        // Storytelling Section

        EditorGUILayout.LabelField("Storytelling", EditorStyles.boldLabel);
        controller.enableStorytelling = EditorGUILayout.Toggle("Enable Storytelling", controller.enableStorytelling);

        if (controller.enableStorytelling)
        {
            EditorGUI.indentLevel++;
            controller.storytellingText = EditorGUILayout.TextArea(controller.storytellingText, GUILayout.Height(100));
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space(10);

        // Notifications Section

        EditorGUILayout.LabelField("Notifications", EditorStyles.boldLabel);
        controller.enableNotifications = EditorGUILayout.Toggle("Enable Notifications", controller.enableNotifications);

        if (controller.enableNotifications)
        {
            EditorGUI.indentLevel++;
            controller.notificationText = EditorGUILayout.TextArea(controller.notificationText, GUILayout.Height(50));
            controller.notificationDuration = EditorGUILayout.FloatField("Duration", controller.notificationDuration);
            controller.notificationSound = EditorGUILayout.TextField("Notification Sound", controller.notificationSound);
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space(10);

        // Knowledge Book Section

        EditorGUILayout.LabelField("Knowledge Book", EditorStyles.boldLabel);
        controller.enableKnowledgeUpdate = EditorGUILayout.Toggle("Enable Update", controller.enableKnowledgeUpdate);

        if (controller.enableKnowledgeUpdate)
        {
            EditorGUI.indentLevel++;
            controller.contentToAdd = EditorGUILayout.TextArea(controller.contentToAdd, GUILayout.Height(100));
            EditorGUI.indentLevel--;
        }

        if (GUI.changed)
            EditorUtility.SetDirty(controller);
    }
}
