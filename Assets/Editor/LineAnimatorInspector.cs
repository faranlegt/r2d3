using Ld50.Animations;
using UnityEditor;

namespace Ld50.Editor
{
    [CustomEditor(typeof(LineAnimator))]
    public class LineAnimatorInspector: UnityEditor.Editor
    {
        private SerializedProperty _spritesProperty;
        private SerializedProperty _animationFrameProperty;
        private SerializedProperty _loopProperty;
        private SerializedProperty _frameLengthProperty;
        private SerializedProperty _animateProperty;

        private void OnEnable()
        {
            _spritesProperty = serializedObject.FindProperty("sprites");
            _animationFrameProperty = serializedObject.FindProperty("animationFrame");
            _loopProperty = serializedObject.FindProperty("loop");
            _animateProperty = serializedObject.FindProperty("animate");
            _frameLengthProperty = serializedObject.FindProperty("frameLength");
        }

        public override void OnInspectorGUI()
        {
            var animator = target as LineAnimator;
            
            serializedObject.Update();
            EditorGUILayout.PropertyField(_spritesProperty);
            if (animator && animator.sprites != null)
            {
                animator.animationFrame =
                    EditorGUILayout.IntSlider(animator.animationFrame, 0, animator.sprites.sprites.Length - 1);
            }
            EditorGUILayout.PropertyField(_loopProperty);
            EditorGUILayout.PropertyField(_frameLengthProperty);
            EditorGUILayout.PropertyField(_animateProperty);

            serializedObject.ApplyModifiedProperties();
        }
    }
}