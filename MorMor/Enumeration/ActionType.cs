﻿

namespace MorMor.Enumeration;

public enum ActionType
{
    /// <summary>
    /// 查询进度
    /// </summary>
    GameProgress,

    /// <summary>
    /// 生成地图
    /// </summary>
    WorldMap,

    /// <summary>
    /// 公共消息
    /// </summary>
    PluginMsg,

    /// <summary>
    /// 似有消息
    /// </summary>
    PrivateMsg,

    /// <summary>
    /// 执行指令
    /// </summary>
    Command,
    /// <summary>
    /// 在线排行
    /// </summary>
    OnlineRank,

    /// <summary>
    /// 死亡排行
    /// </summary>
    DeadRank,

    /// <summary>
    /// 背包查询
    /// </summary>
    Inventory,

    /// <summary>
    /// 服务器在线玩家查询
    /// </summary>
    ServerOnline,

    /// <summary>
    /// 注册账户
    /// </summary>
    RegisterAccount,

    /// <summary>
    /// 服务器重置}
    /// </summary>
    RestServer,

    /// <summary>
    /// 上传世界地图
    /// </summary>
    UpLoadWorld
}