using System.Collections.Generic;

    public class BlendShapeList
    {
      
        
        public string Key { get; set; }
        public float Value { get; set; }
        
        public BlendShapeList(string key, float value)
        {
            Key = key;
            Value = value;
        }
    }