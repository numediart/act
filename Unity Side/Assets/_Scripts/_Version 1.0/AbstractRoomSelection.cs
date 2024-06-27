using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace _Scripts._Version_1._0
{
    public abstract class AbstractRoomSelection<TRoomList,TRoom>:MonoBehaviour where TRoomList : List<TRoom> where TRoom : AbstractRoom
    {
        public abstract void CreateRoom();
        public abstract void DeleteRoom(string roomName, string roomOwner, string password, string id);
        public abstract void DeleteAllRooms();

        public static void JoinRoom(string password, string id)
        {
            
        }
        public abstract TRoomList GetRooms();

    }
}