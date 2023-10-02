using UnityEngine;
using UnityEditor;

namespace GameDevUtils.EditorMode
{
    [InitializeOnLoad]
    public class ChanneledLoggerEditor : EditorWindow
    {
        private void OnEnable() {
            ChanneledLogger.ResetChannels();
        }

        private void OnGUI() {
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Clear all")) ChanneledLogger.ResetChannels();
            if (GUILayout.Button("Select all")) ChanneledLogger.ClearChannels();
            EditorGUILayout.EndHorizontal();

            GUILayout.Label("Click to toggle logging channels", EditorStyles.boldLabel);

            foreach (LogChannel channel in System.Enum.GetValues(typeof(LogChannel))) {
                EditorGUILayout.BeginHorizontal();

                bool toggleValue = (ChanneledLogger.GetActiveChannels() & channel) == channel;
                GUILayout.Toggle(toggleValue, "", GUILayout.ExpandWidth(false));

                if (GUILayout.Button(channel.ToString())) ChanneledLogger.ToggleChannel(channel);
                EditorGUILayout.EndHorizontal();
            }

            if (EditorApplication.isPlaying && EditorGUI.EndChangeCheck())
                ChanneledLogger.SetChannels(ChanneledLogger.GetActiveChannels());
        }

        [MenuItem("Logging/Logger Window")]
        public static void ShowWindow() {
            GetWindow(typeof(ChanneledLoggerEditor));
        }
    }
}