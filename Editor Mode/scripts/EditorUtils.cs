using UnityEditor;
using UnityEngine.Events;

namespace GameDevUtils.EditorMode
{
    public static class EditorUtils
    {
        /// <summary>
        /// Safely invoke an action that should throw a warning in the editor if invoked while outside of play mode.
        /// </summary>
        /// <param name="action">The action to invoke</param>
        public static void InvokeEditorSafeAction(UnityAction action) {
#if !UNITY_EDITOR
            action?.Invoke();
#else
            EditorApplication.delayCall += delegate {
                action?.Invoke();
            };
#endif
        }
    }
}