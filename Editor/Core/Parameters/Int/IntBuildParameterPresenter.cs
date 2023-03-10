using UnityEditor;

namespace UniTools.Build
{
    public sealed class IntBuildParameterPresenter : BuildParameterPresenter
    {
        private readonly IntBuildParameter m_target = default;
        private readonly SerializedObject m_serializedObject = default;
        private readonly SerializedProperty m_value = default;
        private readonly SerializedProperty m_options = default;

        public IntBuildParameterPresenter(IntBuildParameter target)
        {
            m_target = target;
            m_serializedObject = new SerializedObject(m_target);
            m_value = m_serializedObject.FindProperty(nameof(m_value));
            m_options = m_serializedObject.FindProperty(nameof(m_options));
        }

        public override void Draw()
        {
            IntBuildParameterEditor.Draw(m_target, m_serializedObject, m_value, m_options);
        }
    }
}