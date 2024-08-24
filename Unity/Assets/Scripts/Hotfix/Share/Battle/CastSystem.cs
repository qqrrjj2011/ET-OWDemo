using BulletSharp;
using TrueSync;

namespace ET
{
    [EntitySystemOf(typeof(Cast))]
    [FriendOf(typeof(Cast))]
    public static partial class CastSystem
    {
        [EntitySystem]
        private static void Awake(this Cast self, int configId)
        {
            LSUnit player = self.GetParent<CastComponent>().GetParent<LSUnit>();
            CastConfig config = CastConfigCategory.Instance.Get(configId);
            
            // 创建Unit
            LSUnitComponent unitComponent = (self.IScene as Entity).GetComponent<LSUnitComponent>();
            LSUnit castUnit = unitComponent.Creat();
            castUnit.Owner = player;
            
            self.ConfigId = configId;
            self.Unit = castUnit;

            // 添加行为机
            self.AddComponent<ActionComponent, int>(config.ActionGroup);

            // 发布事件: 创建技能
            EventSystem.Instance.Publish(self.IScene as LSWorld, new UnitUseCast() { Unit = player, Cast = self });
            
            // 设置Unit位置 这个必须要在创建Cast消息发送后再移动
            TSVector offset = new TSVector(config.X, config.Y, config.Z);
            castUnit.Position = player.Position + offset;
            castUnit.Rotation = player.Rotation;
            
            // 添加碰撞
            if (config.RigidBody == 0) return;
            castUnit.AddComponent<B3CollisionComponent, int>(config.RigidBody);
        }

        [EntitySystem]
        private static void Destroy(this Cast self)
        {
            // 发布事件: 销毁技能
            LSUnit player = self.GetParent<CastComponent>().GetParent<LSUnit>();
            EventSystem.Instance.Publish(self.IScene as LSWorld, new UnitRemoveCast() { Unit = player, Cast = self });
            
            // 移除CastUnit
            LSUnitComponent unitComponent = player.GetParent<LSUnitComponent>();
            LSUnit castUnit = self.Unit;
            if (castUnit == null) return;
            unitComponent.RemoveChild(castUnit.Id);
        }
    }
}