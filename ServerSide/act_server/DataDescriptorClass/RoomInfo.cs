namespace act_server.DataDescriptorClass
{

    public class RoomInfo(string roomName, string roomOwner, string roomId, bool hasPassword, int clientCount)
    {
        string RoomName { get; set; } = roomName;
        string RoomOwner { get; set; } = roomOwner; // RoomOwner is a client id
        string RoomId { get; set; } = roomId;
        bool HasPassword { get; set; } = hasPassword;
        int ClientCount { get; set; } = clientCount;

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