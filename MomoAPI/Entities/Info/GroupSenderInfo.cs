﻿using MomoAPI.Enumeration.EventParamType;
using MomoAPI.Utils;
using Newtonsoft.Json;

namespace MomoAPI.Entities.Info;

public class GroupSenderInfo
{
    /// <summary>
    /// 账号
    /// </summary>
    [JsonProperty("user_id")]
    public long QQ { get; internal set; }

    /// <summary>
    /// 昵称
    /// </summary>
    [JsonProperty("nickname")]
    public string Name { get; internal set; }

    /// <summary>
    /// 群头
    /// </summary>
    [JsonProperty("card")]
    public string Card { get; internal set; }

    /// <summary>
    /// 权限
    /// </summary>
    [JsonProperty("role")]
    [JsonConverter(typeof(EnumConverter))]
    public MemberRoleType Role { get; internal set; }
}