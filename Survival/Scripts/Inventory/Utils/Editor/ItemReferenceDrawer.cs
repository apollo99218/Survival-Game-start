using UnityEditor;
using UnityEngine;

namespace SurvivalTemplatePro
{
    [CustomPropertyDrawer(typeof(ItemReference))]
    public class ItemReferenceDrawer : PopupIdElementsDrawer
    {
        private static string[] m_AllNames;
        private static string[] m_AllNamesFullPath;

        private bool m_IsInitialized;


        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            int databaseCount = 1;

            if (!m_IsInitialized)
                databaseCount = ItemDatabase.GetInstanceCount();

            if (databaseCount == 1)
            {
                base.OnGUI(position, property, label);
                m_IsInitialized = true;
                return;
            }

            string errorMessage = databaseCount == 0 ? "No item databases found in the resources folder" : "Multiple item databases found in the resources folder, make sure there's only one.";
            EditorGUI.HelpBox(position, errorMessage, MessageType.Error);
        }

        protected override string[] GetElementNames()
        {
            if (m_AllNames == null)
                m_AllNames = ItemDatabase.GetItemNames().ToArray();

            return m_AllNames;
        }

        protected override string[] GetElementNamesFullPath()
        {
            if (m_AllNamesFullPath == null)
                m_AllNamesFullPath = ItemDatabase.GetItemNamesFullPath().ToArray();
            
            return m_AllNamesFullPath;
        }

        protected override int IdOfElement(int index) => ItemDatabase.GetItemAtIndex(index).Id;
        protected override int IndexOfElement(int id) => ItemDatabase.GetIndexOfItem(id);
    }
}