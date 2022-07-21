using SurvivalTemplatePro.CameraSystem;
using UnityEngine;

namespace SurvivalTemplatePro.BuildingSystem
{
    [HelpURL("https://polymindgames.gitbook.io/welcome-to-gitbook/qgUktTCVlUDA7CAODZfe/player/modules-and-behaviours/building#character-build-effects-behaviour")]
    public class CharacterBuildEffects : CharacterBehaviour
    {
        [SerializeField]
        private ShakeSettings m_CameraShake;

        [SerializeField]
        private CameraEffectSettings m_CameraEffect;

        private ICameraMotionHandler m_CameraMotion;
        private ICameraEffectsHandler m_CameraEffects;
        private IBuildingController m_BuildingController;


        public override void OnInitialized()
        {
            GetModule(out m_CameraMotion);
            GetModule(out m_CameraEffects);
            GetModule(out m_BuildingController);

            m_BuildingController.onObjectPlaced += PlayEffect;
        }

        public void PlayEffect()
        {
            m_CameraMotion.DoShake(m_CameraShake);
            m_CameraEffects.DoAnimationEffect(m_CameraEffect);
        }
    }
}