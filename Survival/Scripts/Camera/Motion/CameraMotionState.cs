using System;

namespace SurvivalTemplatePro.CameraSystem
{
    [Serializable]
    public class CameraMotionState
    {
        public Spring.Settings PositionSpring = Spring.Settings.Default;
        public Spring.Settings RotationSpring = Spring.Settings.Default;

        public CameraBob Headbob;
        public NoiseMotionModule Noise;

        [BHeader("State Change Forces")]

        public SpringForce EnterForce;
        public SpringForce ExitForce;
    }
}
