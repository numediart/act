using UnityEngine;

namespace _Scripts._Version_1._0
{
    public abstract class AbstractRoom : MonoBehaviour
    {
        protected abstract string roomOwner { get; set; }
        protected abstract string Name { get; set; }
        protected abstract string Password { get; set; }
        
        protected abstract string Id { get; set; }
             
        public string RoomName => Name;
        public string RoomOwner => roomOwner;
        public string RoomPassword => Password;
        
        public string RoomId => Id;
    }
}