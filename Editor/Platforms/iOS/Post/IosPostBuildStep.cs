using UnityEngine;

namespace UniTools.Build
{
    public abstract class IosPostBuildStep : ScriptableCustomBuildStep
    {
        [SerializeField, Tooltip("Can be found at Apple Developer Console")] private string m_teamId = string.Empty;
        [SerializeField] private string m_provisioningProfileName = default;
        [SerializeField] private string m_provisioningProfileUuid = string.Empty;

        protected string TeamId => m_teamId;

        protected string ProvisioningProfileName => m_provisioningProfileName;

        protected string ProvisioningProfileUuid => m_provisioningProfileUuid;
    }
}