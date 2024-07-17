using System.Collections.Generic;
using act_server.Enum;

namespace act_server.Utils;

public class ActionUnit
{
   public string Name { get; set; }
   public List<BlendShapeData> BlendShapeList { get; set; }
}

public class BlendShapeData
{
   public string Key { get; set; }
   public float Value { get; set; }
}
