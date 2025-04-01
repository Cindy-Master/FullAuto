using System;
using System.Collections.Generic;
using ECommons.DalamudServices;
using FFXIVClientStructs.FFXIV.Client.Game.Object;

namespace AutoRaidHelper.Utils
{
    /// <summary>
    /// 处理游戏中连线(Tether)相关的工具类
    /// </summary>
    /// <example>
    /// 使用示例:
    /// <code>
    /// // 获取DataID为11344的对象所连线的目标列表
    /// var targets = TetherUtils.GetTetheredTargets(11344);
    /// 
    /// // 获取格式化的连线信息并在控制台显示
    /// string info = TetherUtils.GetTetheredTargetsInfo(11344);
    /// Svc.Log.Info(info);
    /// 
    /// // 直接将连线信息发送到游戏聊天窗口
    /// TetherUtils.SendTetheredTargetsToChat(11344);
    /// </code>
    /// </example>
    public static class TetherUtils
    {
        /// <summary>
        /// 获取指定DataID对象所连线的目标对象
        /// </summary>
        /// <param name="dataId">要查询的对象DataID</param>
        /// <returns>与该DataID对象连线的目标对象列表</returns>
        public static List<IGameObject> GetTetheredTargets(uint dataId)
        {
            List<IGameObject> results = new List<IGameObject>();
            
            // 获取指定DataID的所有对象
            var sources = Svc.Objects.GetEnumerableByDataId(dataId);
            
            foreach (var source in sources)
            {
                // 确保对象能转换为IBattleChara类型
                if (source is IBattleChara battleChara)
                {
                    // 获取连线目标ID
                    var targetId = battleChara.Struct()->Tethers[0].TargetId;
                    
                    // 如果有有效的目标ID
                    if (targetId != 0)
                    {
                        // 通过ID搜索目标对象
                        var target = Svc.Objects.SearchById(targetId);
                        if (target != null)
                        {
                            results.Add(target);
                        }
                    }
                }
            }
            
            return results;
        }
        
        /// <summary>
        /// 获取指定DataID对象所连线的目标，并格式化输出连线信息
        /// </summary>
        /// <param name="dataId">要查询的对象DataID</param>
        /// <returns>格式化的连线信息</returns>
        public static string GetTetheredTargetsInfo(uint dataId)
        {
            string result = "";
            var sources = Svc.Objects.GetEnumerableByDataId(dataId);
            
            foreach (var source in sources)
            {
                if (source is IBattleChara battleChara)
                {
                    var targetId = battleChara.Struct()->Tethers[0].TargetId;
                    if (targetId != 0)
                    {
                        var target = Svc.Objects.SearchById(targetId);
                        if (target != null)
                        {
                            result += $"{source.DataId}|{source.Name}|{source.GameObjectId} -> {target.DataId}|{target.Name}|{target.GameObjectId}\n";
                        }
                    }
                }
            }
            
            return result;
        }
        
        /// <summary>
        /// 获取指定DataID对象所连线的目标，并将信息发送到聊天窗口
        /// </summary>
        /// <param name="dataId">要查询的对象DataID</param>
        public static void SendTetheredTargetsToChat(uint dataId)
        {
            var sources = Svc.Objects.GetEnumerableByDataId(dataId);
            
            foreach (var source in sources)
            {
                if (source is IBattleChara battleChara)
                {
                    var targetId = battleChara.Struct()->Tethers[0].TargetId;
                    if (targetId != 0)
                    {
                        var target = Svc.Objects.SearchById(targetId);
                        if (target != null)
                        {
                            string str = $"{source.DataId}|{source.Name}|{source.GameObjectId} -> {target.DataId}|{target.Name}|{target.GameObjectId}";
                            Svc.Chat.SendMessage("/e " + str);
                        }
                    }
                }
            }
        }
    }
} 