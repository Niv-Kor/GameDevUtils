using UnityEngine;

namespace GameDevUtils.ObjectManagement
{
    public static class LayerExtensions
    {
        /// <summary>
        /// Check if a certain layer is contained in a layer mask.
        /// </summary>
        /// <param name="layer">The layer to check (as is "gameObject.layer")</param>
        /// <returns>True if the mask contains the layer.</returns>
        public static bool ContainsLayer(this LayerMask mask, int layer) {
            return (mask & 1 << layer) == 1 << layer;
        }

        /// <summary>
        /// Get the layer value of a layer mask.
        /// This function only works well with one layered masks.
        /// </summary>
        /// <returns>
        /// The value of the layer mask,
        /// or the last one if it contains multiple layers.
        /// </returns>
        public static int GetLayerValue(this LayerMask mask) {
            return (int)Mathf.Log(mask.value, 2);
        }

        /// <summary>
        /// Apply a layer to a game object.
        /// </summary>
        /// <param name="obj">The object to which to apply the layer</param>
        /// <param name="applyToChildren">True to also apply the layer to each of the object's children</param>
        public static void ApplyOnObject(this LayerMask layer, GameObject obj, bool applyToChildren = false) {
            int layerVal = GetLayerValue(layer);
            obj.layer = layerVal;

            if (applyToChildren)
                foreach (Transform child in obj.transform)
                    child.gameObject.layer = layerVal;
        }
    }
}