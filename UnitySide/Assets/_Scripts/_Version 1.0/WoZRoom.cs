using System;
using _Scripts._Version_1._0.Controllers.RoomController;
using _Scripts._Version_1._0.Services.RoomServices.WoZRoomService;
using UnityEngine;

namespace _Scripts._Version_1._0
{
    public class WoZRoom : AbstractRoom
    {
        protected override string roomOwner { get; set; }
        protected override string Name { get; set; }
        protected override string Password { get; set; }
        
        protected override string Id { get; set; }

        private WoZRoomController _woZRoomController;
        private void Start()
        {
            gameObject.AddComponent<WoZRoomService>();
            _woZRoomController = new WoZRoomController(gameObject.GetComponent<WoZRoomService>());
        }

        public void Init(string roomOwner, string name, string password, string id)
        {
            this.roomOwner = roomOwner;
            this.Name = name;
            this.Password = password;
            this.Id = id;
            
        }
        
        public void SetRoomName(string name)
        {
            this.Name = name;
        }
        
        public void SetRoomOwner(string owner)
        {
            this.roomOwner = owner;
        }
        
        public void SetRoomPassword(string password)
        {
            this.Password = password;
        }
        
        public void SetRoomId(string id)
        {
            this.Id = id;
        }
        
    }
}