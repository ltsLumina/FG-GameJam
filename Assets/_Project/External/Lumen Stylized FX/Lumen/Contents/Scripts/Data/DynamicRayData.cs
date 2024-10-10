using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DistantLands.Lumen.Data
{
    [CreateAssetMenu(menuName = "Tools/Lumen/New Dynamic Ray Data", order = 391)]
    public class DynamicRayData : ScriptableObject
    {
        public delegate void SetupRay();
        public static SetupRay setupRay;

        public bool useCameraDepthFade = true;
        public float cameraDepthFadeStart;
        public float cameraDepthFadeEnd;

        public bool useCameraDistanceFade = true;
        public float cameraDistanceFadeStart;
        public float cameraDistanceFadeEnd;

        public bool useSceneDepthFade = false;
        public float sceneDepthFadeStart;
        public float sceneDepthFadeEnd;

        public bool useAngleFade = false;
        public float angleFadeStart;
        public float angleFadeEnd;

        public float angleOpacityEffect;
        public bool useRaylengthFade;
        public float angleRaylengthEffect;

        public bool useVariation;
        public bool useLightColor;

        public List<RayLayer> rayLayers;
        public float globalScale = 1;
        public Material rayMaterial;
        [Range(0, 1)]
        public float globalBrightness = 1;
        [Range(0, 10)]
        public float variationSpeed = 1;
        public float variationScale = 10;
        [ColorUsage(true, true)]
        public Color variationColor = Color.white;
        [Tooltip("Should this ray pull the direction from an outside source or from an internal vector?")]
        public bool autoAssignSun = true;
        [Tooltip("Enable to pull the current ray direction from the global keyword, disbale to pull the current ray direction from the brightest diectional light. Setup the keyword using the Lumen Sun script on an object.")]
        public bool useLumenSun = false;
        public Vector3 sunDirection;
        public bool bidirectional = true;

        [HideInInspector]
        public bool showLayerSettings;
        [HideInInspector]
        public bool showMainSettings;
        [HideInInspector]
        public bool showVariationSettings;
        [HideInInspector]
        public bool showFadingSettings;
        [HideInInspector]
        public bool showDynamicSettings;
        public bool needsToBeUpdated;

        public void OnValidate()
        {

#if UNITY_EDITOR
            EditorApplication.update += ResetRay;
#endif

        }

        public void ResetRay()
        {

            setupRay?.Invoke();

#if UNITY_EDITOR
            EditorApplication.update -= ResetRay;
#endif
        }

        [Serializable]
        public class RayLayer
        {
            public delegate void SetupRay();

            public enum RayShapes
            {
                SqaureScatter,
                HollowSqaureScatter,
                CircleScatter,
                HollowCircleScatter,
                TriangleScatter,
                HollowTriangleScatter,
                LineScatter,
                GradientScatter,

            }

            public static SetupRay setupRay;
            public RayShapes rayShape;
            [HideInInspector]
            public Mesh rayMesh;
            [Range(0, 1)]
            public float brightness = 1;
            [ColorUsage(false, true)]
            public Color colorMultiplier = Color.white;
            public Vector3 position;
            public Vector3 scale = Vector3.one;
            public float raylength;

            [HideInInspector]
            public bool open;
        }
    }
}