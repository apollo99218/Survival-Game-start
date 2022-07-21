using UnityEditor;
using UnityEngine;

namespace SurvivalTemplatePro.WieldableSystem
{
    [CustomEditor(typeof(STPEventsListenerBehaviour), true)]
    public class STPEventsListenerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            STPEditorGUI.Separator();
            GUI.color = new Color(0.85f, 0.85f, 0.85f, 1f);

            EditorGUILayout.HelpBox($"For speeding up the productivity you can copy-paste values from other ''{target.GetType().Name.DoUnityLikeNameFormat()}'s'' (i.e. knife to axe etc..)", MessageType.Info);

            GUI.color = Color.white;
        }
    }
}