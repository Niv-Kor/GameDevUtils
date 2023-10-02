using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace GameDevUtils.EditorMode
{
    [CustomEditor(typeof(MonoBehaviour))]
    public abstract class InspectorUI<T> : Editor where T : MonoBehaviour
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();
            ConstructUI((T)target);
        }

        /// <summary>
        /// Add an editor button.
        /// </summary>
        /// <param name="label">The button's label</param>
        /// <param name="action">The action to invoke when the button is clicked</param>
        protected void CreateButton(string label, UnityAction action) {
            if (GUILayout.Button(label)) action?.Invoke();
        }

        /// <summary>
        /// Build the UI's components.
        /// </summary>
        /// <param name="component">The editor's target component</param>
        protected abstract void ConstructUI(T component);
    }
}
