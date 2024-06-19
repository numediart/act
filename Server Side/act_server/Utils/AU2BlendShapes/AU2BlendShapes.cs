using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using act_server.Enum;
using act_server.Service.MainWebSocketService;
using Microsoft.Extensions.Logging;

namespace act_server.Utils.AU2BlendShapes
{
    public class AU2BlendShapes
    {
        public List<ActionUnit> ActionUnits => _actionUnits;
        private List<ActionUnit> _actionUnits;
        private Dictionary<string, Dictionary<BlendShape, float>> _actionUnitDict;
        private Dictionary<BlendShape, float> _blendshapeDictNew;
        private Dictionary<BlendShape, float> _blendshapeDict;
        ILogger<MainWebSocketService> _logger;
        public AU2BlendShapes(IncomingData incomingData)
        {
            _actionUnits = ParsePayload(incomingData);
        }


        private List<ActionUnit> ParsePayload(IncomingData incomingData)
        {
            var actionUnits = new List<ActionUnit>();

            if (incomingData.au_r != null)
            {
                // Convert AuR properties to a list of ActionUnit objects
                PropertyInfo[] properties = typeof(AuR).GetProperties();
         
               
                foreach (var property in properties)
                {
                    double value = (double)property.GetValue(incomingData.au_r);
                    var blendShapeList = GetBlendShapeList(property.Name, (float) value);
                    actionUnits.Add(new ActionUnit { Name = property.Name, BlendShapeList = blendShapeList });
                }
            }

            return actionUnits;
        }




    private static readonly string[] blendShapeNames = new[]
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

    private List<BlendShapeData> GetBlendShapeList(string actionUnitName, float value)
    {
        var blendShapeList = new List<BlendShapeData>();

        void AddBlendShape(string key, float value)
        {
            blendShapeList.Add(new BlendShapeData { Key = key, Value = value });
        }

        switch (actionUnitName)
        {
            case "AU01":
                AddBlendShape("Expressions_browsMidVert_max", value);
                AddBlendShape("Expressions_browsMidVert_min", 0.0f);
                break;
            case "AU02":
                AddBlendShape("Expressions_browOutVertL_max", value * 0.95f);
                AddBlendShape("Expressions_browOutVertR_max", value * 0.95f);
                AddBlendShape("Expressions_browSqueezeL_min", 0.0f);
                AddBlendShape("Expressions_browSqueezeR_min", 0.0f);
                break;
            case "AU04":
                AddBlendShape("Expressions_browOutVertL_max", value * 0.2f);
                AddBlendShape("Expressions_browOutVertR_max", value * 0.2f);
                AddBlendShape("Expressions_browSqueezeL_max", value);
                AddBlendShape("Expressions_browSqueezeR_max", value);
                AddBlendShape("Expressions_browsMidVert_min", 0.0f);
                break;
            case "AU05":
                AddBlendShape("Expressions_eyeClosedL_min", 0.0f);
                AddBlendShape("Expressions_eyeClosedR_min", 0.0f);
                AddBlendShape("Expressions_eyeClosedPressureL_max", value * 0.3f);
                AddBlendShape("Expressions_eyeClosedPressureR_max", value * 0.3f);
                AddBlendShape("Expressions_eyeSquintL_max", value * 0.58f);
                AddBlendShape("Expressions_eyeSquintR_max", value * 0.58f);
                break;
            case "AU06":
                AddBlendShape("Expressions_cheekSneerL_max", value * 0.7f);
                AddBlendShape("Expressions_cheekSneerR_max", value * 0.7f);
                AddBlendShape("Expressions_mouthSmile_max", value * 0.65f);
                AddBlendShape("Expressions_browOutVertL_max", value * 0.35f);
                AddBlendShape("Expressions_browOutVertR_max", value * 0.35f);
                break;
            case "AU07":
                // Add blend shapes for AU07
                break;
            case "AU08":
                // Add blend shapes for AU08
                break;
            case "AU09":
                AddBlendShape("Expressions_cheekSneerL_max", value);
                AddBlendShape("Expressions_cheekSneerR_max", value);
                AddBlendShape("Expressions_eyeClosedPressureL_max", value * 0.6f);
                AddBlendShape("Expressions_eyeClosedPressureR_max", value * 0.6f);
                AddBlendShape("Expressions_mouthOpenAggr_max", value * 0.8f);
                AddBlendShape("Expressions_nostrilsExpansion_max", value * 0.7f);
                break;
            case "AU10":
                AddBlendShape("Expressions_cheekSneerL_max", value);
                AddBlendShape("Expressions_cheekSneerR_max", value);
                AddBlendShape("Expressions_mouthOpenAggr_max", value * 0.8f);
                AddBlendShape("Expressions_nostrilsExpansion_max", value);
                break;
            case "AU12":
                AddBlendShape("Expressions_mouthSmile_max", value);
                break;
            case "AU14":
                AddBlendShape("Expressions_mouthSmileL_max", value * 0.85f);
                AddBlendShape("Expressions_mouthSmileR_max", value * 0.85f);
                AddBlendShape("Expressions_mouthSmileOpen_max", value * 0.8f);
                break;
            case "AU15":
                AddBlendShape("Expressions_mouthSmile_max", value * 0.6f);
                break;
            case "AU17":
                AddBlendShape("Expressions_mouthSmileOpen2_max", value * 0.85f);
                AddBlendShape("Expressions_mouthLowerOut_max", value * 0.7f);
                break;
            case "AU20":
                AddBlendShape("Expressions_mouthClosed_max", value * 0.9f);
                AddBlendShape("Expressions_mouthSmileL_max", value * 0.7f);
                AddBlendShape("Expressions_mouthSmileR_max", value * 0.65f);
                break;
            case "AU23":
                AddBlendShape("Expressions_mouthOpen_max", value * 0.85f);
                break;
            case "AU25":
                AddBlendShape("Expressions_mouthClosed_min", 0.0f);
                AddBlendShape("Expressions_mouthOpenTeethClosed_max", value * 0.55f);
                break;
            case "AU26":
                AddBlendShape("Expressions_mouthOpenLarge_max", value * 0.85f);
                AddBlendShape("Expressions_mouthChew_max", value * 0.6f);
                break;
            case "AU45":
                AddBlendShape("Expressions_eyeClosedL_max", value);
                AddBlendShape("Expressions_eyeClosedR_max", value);
                break;
            case "AU61":
                AddBlendShape("Expressions_eyesHoriz_min", 0.0f);
                break;
            case "AU62":
                AddBlendShape("Expressions_eyesHoriz_max", value);
                break;
            case "AU63":
                AddBlendShape("Expressions_eyesVert_max", value);
                break;
            case "AU64":
                AddBlendShape("Expressions_eyesVert_min", 0.0f);
                break;
            default:
                Console.WriteLine($"Unknown action unit: {actionUnitName}");
                throw new ArgumentException($"Unknown action unit: {actionUnitName}");
        }

        return blendShapeList;
    }


        private Dictionary<BlendShape, float> JsonBlendshapeMatcher(Dictionary<string, float> jsonMB)
        {
            var dictBlendshape = new Dictionary<BlendShape, float>();

            foreach (var item in jsonMB)
            {
                var name = item.Key;
                var value = item.Value;

                if (value < 0.5f)
                {
                    name += "_min";
                    value = (0.5f - value) * 2;
                }
                else
                {
                    name += "_max";
                    value = (value - 0.5f) * 2;
                }

                if (BlendShape.BlendShapes.TryGetValue(name, out var blendShape))
                {
                    dictBlendshape[blendShape] = (float)Math.Round(value, 5);
                }
            }

            return dictBlendshape;
        }

        private void LoadBlendshapeDict()
        {
            IReadOnlyDictionary<string, BlendShape> blendShapes = BlendShape.BlendShapes;
            foreach (var blendShape in blendShapes)
            {
                _blendshapeDictNew[blendShape.Value] = 0;
            }

            _blendshapeDict = new Dictionary<BlendShape, float>(_blendshapeDictNew);
        }

        public void CalcBlendshapes(Dictionary<string, float> facsDict)
        {
            _logger.LogInformation("Loading blendshapes...");
            LoadBlendshapeDict();

            _logger.LogInformation("Calculating blendshapes...");
            foreach (var au in facsDict)
            {
                if (au.Key.StartsWith("AU"))
                {
                    if (_actionUnitDict.ContainsKey(au.Key))
                    {
                        if (au.Value > 0.001f)
                        {
                            foreach (var exp in _actionUnitDict[au.Key])
                            {
                                _blendshapeDict[exp.Key] += (float)Math.Round(exp.Value * au.Value, 5);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"No json file found for {au.Key}");
                    }
                }
            }
        }

        public Dictionary<BlendShape, float> OutputBlendshapes(Dictionary<string, float> facsDict)
        {
            Console.WriteLine($"Calculating blendshapes for provided AU values...");
            CalcBlendshapes(facsDict);
            return _blendshapeDict;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(new {_actionUnits});
        }
    }
}