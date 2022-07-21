using UnityEngine;
using UnityEngine.InputSystem;

namespace SurvivalTemplatePro.InputSystem
{
    [HelpURL("https://polymindgames.gitbook.io/welcome-to-gitbook/qgUktTCVlUDA7CAODZfe/player/modules-and-behaviours/camera#player-look-input-behaviour")]
    public class PlayerLookInput : CharacterBehaviour
    {
        [SerializeField]
        private bool m_EnableOnStart = true;

        [SerializeField]
        private InputActionReference m_LookInput;

        private ILookHandler m_LookHandler;


        public override void OnInitialized()
        {
            GetModule(out m_LookHandler);

            if (m_EnableOnStart)
                m_LookInput.action.Enable();
        }

        private void LateUpdate()
        {
            if (!IsInitialized)
                return;

            m_LookHandler.UpdateLook(m_LookInput.action.ReadValue<Vector2>());
        }
    }
}