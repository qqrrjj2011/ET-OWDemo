namespace ET.Server
{
    public static partial class RoomMessageHelper
    {
        public static void BroadCast(this ET.Room room, IMessage message)
        {
            // 广播的消息不能被池回收
            (message as MessageObject).IsFromPool = false;

            MessageLocationSenderComponent messageLocationSenderComponent = room.Root().GetComponent<MessageLocationSenderComponent>();
            foreach (var id in room.PlayerIds)
            {
                //if (!pair.Value) continue;
                messageLocationSenderComponent.Get(LocationType.GateSession).Send(id, message);
            }
        }
    }
}