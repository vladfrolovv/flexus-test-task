using Cameras;
using UnityEditor;
using UnityEngine;
namespace Editor.Tools
{
    [CustomEditor(typeof(CameraShakeEffect))]
    public class CameraShakeEffectEditor : UnityEditor.Editor
    {

        private CameraShakeEffect _cameraShakeEffect;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Shake"))
            {
                _cameraShakeEffect.Shake();
            }

            GUILayout.EndHorizontal();
        }

        protected void OnEnable()
        {
            _cameraShakeEffect = target as CameraShakeEffect;
        }

    }
}
