namespace ET.Client
{
    [MessageHandler(SceneType.LockStep)]
    public class Room2C_AdjustUpdateTimeHandler: MessageHandler<Scene, Room2C_AdjustUpdateTime>
    {
        protected override async ETTask Run(Scene root, Room2C_AdjustUpdateTime message)
        {
            ET.Room room = root.GetComponent<ET.Room>();
            int newInterval = (1000 + (message.DiffTime - LSConstValue.UpdateInterval)) * LSConstValue.UpdateInterval / 1000;

            if (newInterval < 40)
            {
                newInterval = 40;
            }

            if (newInterval > 66)
            {
                newInterval = 66;
            }
            
            
            Log.Warning("Change FixedUpdate Rate");
            room.FixedTimeCounter.ChangeInterval(newInterval, room.PredictionFrame);
            await ETTask.CompletedTask;
        }
    }
}