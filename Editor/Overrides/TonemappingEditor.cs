using UnityEngine.Rendering.Universal;

namespace UnityEditor.Rendering.Universal
{
    [CustomEditor(typeof(Tonemapping))]
    sealed class TonemappingEditor : VolumeComponentEditor
    {
        SerializedDataParameter m_Mode;
        
        /////////////////UE_ACES_BEGIN/////////////////
        SerializedDataParameter m_Slope;
        SerializedDataParameter m_Toe;
        SerializedDataParameter m_Shoulder;
        SerializedDataParameter m_BlackClip;
        SerializedDataParameter m_WhiteClip;
        /////////////////UE4_ACES_END/////////////////

        // HDR Mode.
        SerializedDataParameter m_NeutralHDRRangeReductionMode;
        SerializedDataParameter m_HueShiftAmount;
        SerializedDataParameter m_HDRDetectPaperWhite;
        SerializedDataParameter m_HDRPaperwhite;
        SerializedDataParameter m_HDRDetectNitLimits;
        SerializedDataParameter m_HDRMinNits;
        SerializedDataParameter m_HDRMaxNits;
        SerializedDataParameter m_HDRAcesPreset;

        public override bool hasAdditionalProperties => true;

        public override void OnEnable()
        {
            var o = new PropertyFetcher<Tonemapping>(serializedObject);

            m_Mode = Unpack(o.Find(x => x.mode));
            /////////////////UE_ACES_BEGIN/////////////////
            m_Slope = Unpack(o.Find(x => x.slope));
            m_Toe = Unpack(o.Find(x => x.toe));
            m_Shoulder = Unpack(o.Find(x => x.shoulder));
            m_BlackClip = Unpack(o.Find(x => x.blackClip));
            m_WhiteClip = Unpack(o.Find(x => x.whiteClip));
            /////////////////UE4_ACES_END///////////////////
            
            m_NeutralHDRRangeReductionMode = Unpack(o.Find(x => x.neutralHDRRangeReductionMode));
            m_HueShiftAmount = Unpack(o.Find(x => x.hueShiftAmount));
            m_HDRDetectPaperWhite = Unpack(o.Find(x => x.detectPaperWhite));
            m_HDRPaperwhite = Unpack(o.Find(x => x.paperWhite));
            m_HDRDetectNitLimits = Unpack(o.Find(x => x.detectBrightnessLimits));
            m_HDRMinNits = Unpack(o.Find(x => x.minNits));
            m_HDRMaxNits = Unpack(o.Find(x => x.maxNits));
            m_HDRAcesPreset = Unpack(o.Find(x => x.acesPreset));
        }

        public override void OnInspectorGUI()
        {
            PropertyField(m_Mode);
            /////////////////UE_ACES_BEGIN/////////////////
            if ( m_Mode.value.intValue == (int)TonemappingMode.ACES_UE5)
            {
                UnityEngine.GUILayout.BeginVertical("box");
                UnityEngine.GUILayout.BeginHorizontal();

                PropertyField(m_Slope);
                if (UnityEngine.GUILayout.Button("Reset"))
                {
                    m_Slope.value.floatValue = 0.88f;
                }
                UnityEngine.GUILayout.EndHorizontal();

                UnityEngine.GUILayout.BeginHorizontal();
                PropertyField(m_Toe);
                if (UnityEngine.GUILayout.Button("Reset"))
                {
                    m_Toe.value.floatValue = 0.55f;
                }
                UnityEngine.GUILayout.EndHorizontal();

                UnityEngine.GUILayout.BeginHorizontal();
                PropertyField(m_Shoulder);
                if (UnityEngine.GUILayout.Button("Reset"))
                {
                    m_Shoulder.value.floatValue = 0.26f;
                }
                UnityEngine.GUILayout.EndHorizontal();

                UnityEngine.GUILayout.BeginHorizontal();
                PropertyField(m_BlackClip);
                if (UnityEngine.GUILayout.Button("Reset"))
                {
                    m_BlackClip.value.floatValue = 0.0f;
                }
                UnityEngine.GUILayout.EndHorizontal();

                UnityEngine.GUILayout.BeginHorizontal();
                PropertyField(m_WhiteClip);
                if (UnityEngine.GUILayout.Button("Reset"))
                {
                    m_WhiteClip.value.floatValue = 0.04f;
                }
                UnityEngine.GUILayout.EndHorizontal();
                UnityEngine.GUILayout.EndVertical();

            }
            /////////////////UE4_ACES_END////////////////////

            // Display a warning if the user is trying to use a tonemap while rendering in LDR
            var asset = UniversalRenderPipeline.asset;
            int hdrTonemapMode = m_Mode.value.intValue;
            if (asset != null && !asset.supportsHDR && hdrTonemapMode != (int)TonemappingMode.None)
            {
                EditorGUILayout.HelpBox("Tonemapping should only be used when working with High Dynamic Range (HDR). Please enable HDR through the active Render Pipeline Asset.", MessageType.Warning);
                return;
            }

            if (PlayerSettings.allowHDRDisplaySupport && hdrTonemapMode != (int)TonemappingMode.None)
            {
                EditorGUILayout.LabelField("HDR Output");

                if (hdrTonemapMode == (int)TonemappingMode.Neutral)
                {
                    PropertyField(m_NeutralHDRRangeReductionMode);
                    PropertyField(m_HueShiftAmount);

                    PropertyField(m_HDRDetectPaperWhite);
                    EditorGUI.indentLevel++;
                    using (new EditorGUI.DisabledScope(m_HDRDetectPaperWhite.value.boolValue))
                    {
                        PropertyField(m_HDRPaperwhite);
                    }
                    EditorGUI.indentLevel--;

                    PropertyField(m_HDRDetectNitLimits);
                    EditorGUI.indentLevel++;
                    using (new EditorGUI.DisabledScope(m_HDRDetectNitLimits.value.boolValue))
                    {
                        PropertyField(m_HDRMinNits);
                        PropertyField(m_HDRMaxNits);
                    }
                    EditorGUI.indentLevel--;
                }
                if (hdrTonemapMode == (int)TonemappingMode.ACES)
                {
                    PropertyField(m_HDRAcesPreset);

                    PropertyField(m_HDRDetectPaperWhite);
                    EditorGUI.indentLevel++;
                    using (new EditorGUI.DisabledScope(m_HDRDetectPaperWhite.value.boolValue))
                    {
                        PropertyField(m_HDRPaperwhite);
                    }
                    EditorGUI.indentLevel--;
                }
            }
        }
    }
}
