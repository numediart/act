using System.Collections.Generic;

namespace act_server.Enum
{
    public class BlendShape : Enumeration<BlendShape>
    {
        private static readonly Dictionary<string, BlendShape> _blendShapes = new();

        static BlendShape()
        {
            var blendShapeNames = new[]
            {
                "Expressions_abdomExpansion_max",
                "Expressions_abdomExpansion_min",
                "Expressions_browOutVertL_max",
                "Expressions_browOutVertL_min",
                "Expressions_browOutVertR_max",
                "Expressions_browOutVertR_min",
                "Expressions_browSqueezeL_max",
                "Expressions_browSqueezeL_min",
                "Expressions_browSqueezeR_max",
                "Expressions_browSqueezeR_min",
                "Expressions_browsMidVert_max",
                "Expressions_browsMidVert_min",
                "Expressions_cheekSneerL_max",
                "Expressions_cheekSneerR_max",
                "Expressions_chestExpansion_max",
                "Expressions_chestExpansion_min",
                "Expressions_eyeClosedL_max",
                "Expressions_deglutition_max",
                "Expressions_deglutition_min",
                "Expressions_eyeClosedL_max",
                "Expressions_eyeClosedL_min",
                "Expressions_eyeClosedPressureL_max",
                "Expressions_eyeClosedPressureL_min",
                "Expressions_eyeClosedPressureR_max",
                "Expressions_eyeClosedPressureR_min",
                "Expressions_eyeClosedR_max",
                "Expressions_eyeClosedR_min",
                "Expressions_eyeSquintL_max",
                "Expressions_eyeSquintL_min",
                "Expressions_eyeSquintR_max",
                "Expressions_eyeSquintR_min",
                "Expressions_eyesHoriz_max",
                "Expressions_eyesHoriz_min",
                "Expressions_eyesVert_max",
                "Expressions_eyesVert_min",
                "Expressions_jawHoriz_max",
                "Expressions_jawHoriz_min",
                "Expressions_jawOut_max",
                "Expressions_jawOut_min",
                "Expressions_mouthBite_max",
                "Expressions_mouthBite_min",
                "Expressions_mouthChew_max",
                "Expressions_mouthChew_min",
                "Expressions_mouthClosed_max",
                "Expressions_mouthClosed_min",
                "Expressions_mouthHoriz_max",
                "Expressions_mouthHoriz_min",
                "Expressions_mouthInflated_max",
                "Expressions_mouthInflated_min",
                "Expressions_mouthLowerOut_max",
                "Expressions_mouthLowerOut_min",
                "Expressions_mouthOpenAggr_max",
                "Expressions_mouthOpenAggr_min",
                "Expressions_mouthOpenHalf_max",
                "Expressions_mouthOpenLarge_max",
                "Expressions_mouthOpenLarge_min",
                "Expressions_mouthOpenO_max",
                "Expressions_mouthOpenO_min",
                "Expressions_mouthOpenTeethClosed_max",
                "Expressions_mouthOpenTeethClosed_min",
                "Expressions_mouthOpen_max",
                "Expressions_mouthOpen_min",
                "Expressions_mouthSmileL_max",
                "Expressions_mouthSmileOpen2_max",
                "Expressions_mouthSmileOpen2_min",
                "Expressions_mouthSmileOpen_max",
                "Expressions_mouthSmileOpen_min",
                "Expressions_mouthSmileR_max",
                "Expressions_mouthSmile_max",
                "Expressions_mouthSmile_min",
                "Expressions_nostrilsExpansion_max",
                "Expressions_nostrilsExpansion_min",
                "Expressions_pupilsDilatation_max",
                "Expressions_pupilsDilatation_min",
                "Expressions_tongueHoriz_max",
                "Expressions_tongueHoriz_min",
                "Expressions_tongueOutPressure_max",
                "Expressions_tongueOut_max",
                "Expressions_tongueOut_min",
                "Expressions_tongueTipUp_max",
                "Expressions_tongueVert_max",
                "Expressions_tongueVert_min"
            };

            int value = 0;
            foreach (var name in blendShapeNames)
            {
                _blendShapes[name + "_max"] = new BlendShape(value++, name + "_max");
                _blendShapes[name + "_min"] = new BlendShape(value++, name + "_min");
            }
        }

        public static IReadOnlyDictionary<string, BlendShape> BlendShapes => _blendShapes;

        private BlendShape(int value, string name) : base(value, name)
        {
        }
    }
}