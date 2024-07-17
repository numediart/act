
    using System.Collections.Generic;

    public class AU
    {
        public string Name { get; set; }
        public List<BlendShapeList> BlendShapeList { get; set; }
        
        public AU(string name, List<BlendShapeList> blendShapeList)
        {
            Name = name;
            BlendShapeList = blendShapeList;
        }
    }