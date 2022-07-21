using SurvivalTemplatePro.CameraSystem;
using UnityEngine;

namespace SurvivalTemplatePro
{
    public interface ICameraMotionHandler : ICharacterModule
    {
        void AddRotationForce(SpringForce force);
        void AddRotationForce(Vector3 recoilForce, int distribution = 1);

        void SetCustomForceSpringSettings(Spring.Settings settings); 
        void ClearCustomForceSpringSettings();

        void SetCustomHeadbob(CameraBob headbob, float speed = 1f);

        void SetCustomState(CameraMotionState state);
        void ClearCustomState();

        void DoShake(ShakeSettings shake, float scale = 1);
    }
}