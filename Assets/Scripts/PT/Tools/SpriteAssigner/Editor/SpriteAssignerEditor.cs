using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PT.Tools.SpriteAssigner
{
    [CustomEditor(typeof(SpriteAssigner))]
    public class SpriteAssignerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            var spriteAssigner = (SpriteAssigner)target;

            if (GUILayout.Button("Create objects", GUILayout.Width(400), GUILayout.Height(20)))
            {
                ClearParent(spriteAssigner.transform);

                CreateObjectsSignSprites(spriteAssigner);
            }

            if (GUILayout.Button("Clear", GUILayout.Width(200), GUILayout.Height(20)))
            {
                ClearParent(spriteAssigner.transform);
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void CreateObjectsSignSprites(SpriteAssigner spriteAssigner)
        {
            var ogName = spriteAssigner.PrefabToAssign.name;

            for (int i = 0; i < spriteAssigner.KVSpriteAssigns[0].SpritesToAssign.Length; i++)
            {
                var mainObj = Instantiate(spriteAssigner.PrefabToAssign, spriteAssigner.transform);

                foreach (Transform childTransform in mainObj.transform)
                {
                    var kvSpriteAssign = spriteAssigner.KVSpriteAssigns.FirstOrDefault(x => x.ObjetName == childTransform.name);

                    if (kvSpriteAssign != null)
                    {
                        var image = childTransform.GetComponent<Image>();

                        image.sprite = kvSpriteAssign.SpritesToAssign[i];
                    }
                }

                mainObj.name = ogName + " " + i;
            }
        }

        private void ClearParent(Transform parent)
        {
            while (parent.childCount > 0)
                foreach (Transform childTransform in parent)
                    DestroyImmediate(childTransform.gameObject);
        }
    }
}
