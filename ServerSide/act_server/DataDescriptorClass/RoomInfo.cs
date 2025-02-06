namespace act_server.DataDescriptorClass
{

    public class RoomInfo
    {
        public RoomInfo(string roomName, string roomOwner, string roomId, bool hasPassword, int clientCount)
        {
            RoomName = roomName;
            RoomOwner = roomOwner;
            RoomId = roomId;
            HasPassword = hasPassword;
            ClientCount = clientCount;
        }

        string RoomName { get; set; }
        string RoomOwner { get; set; } // RoomOwner is a client id
        string RoomId { get; set; }
        bool HasPassword { get; set; }
        int ClientCount { get; set; }

        public override string ToString()
        {
            return "RoomName: " + RoomName + " RoomOwner: " + RoomOwner + " RoomId: " + RoomId + " HasPassword: " + HasPassword + " ClientCount: " + ClientCount;
        }
        public object ToJson()
        {
            return new
            {
                RoomName,
                RoomOwner,
                RoomId,
                HasPassword,
                ClientCount
            };
        }
    }
}