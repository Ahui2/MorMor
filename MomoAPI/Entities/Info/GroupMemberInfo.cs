using MomoAPI.Enumeration;
using MomoAPI.Enumeration.EventParamType;
using MomoAPI.Extensions;
using MomoAPI.Utils;
using Newtonsoft.Json;

namespace MomoAPI.Entities.Info;

/// <summary>
/// 群成员信息
/// </summary>
public sealed record GroupMemberInfo
{
    /// <summary>
    /// 群号
    /// </summary>
    [JsonProperty(PropertyName = "group_id")]
    public long GroupId { get; internal init; }

    /// <summary>
    /// 成员UID
    /// </summary>
    [JsonProperty(PropertyName = "user_id")]
    public long UserId { get; internal init; }

    /// <summary>
    /// 昵称
    /// </summary>
    [JsonProperty(PropertyName = "nickname")]
    public string Nick { get; internal init; }

    /// <summary>
    /// 群名片／备注
    /// </summary>
    [JsonProperty(PropertyName = "card")]
    public string Card { get; internal init; }

    /// <summary>
    /// 性别
    /// </summary>
    [JsonProperty(PropertyName = "sex")]
    [JsonConverter(typeof(EnumConverter))]
    private SexType Sex { get; init; }


    /// <summary>
    /// 年龄
    /// </summary>
    [JsonProperty(PropertyName = "age")]
    public int Age { get; internal init; }

    /// <summary>
    /// 地区
    /// </summary>
    [JsonProperty(PropertyName = "area")]
    public string Area { get; internal init; }

    /// <summary>
    /// 加群时间戳
    /// </summary>
    [JsonIgnore]
    public DateTime JoinTime { get; internal init; }

    [JsonProperty(PropertyName = "join_time")]
    private long JoinTimeStamp
    {
        init => JoinTime = value.ToDateTime();
    }

    /// <summary>
    /// 最后发言时间戳
    /// </summary>
    [JsonIgnore]
    public DateTime LastSentTime { get; internal init; }

    [JsonProperty(PropertyName = "last_sent_time")]
    private long LastSentTimeStamp
    {
        init => LastSentTime = value.ToDateTime();
    }

    /// <summary>
    /// 成员等级
    /// </summary>
    [JsonProperty(PropertyName = "level")]
    public string Level { get; internal init; }

    /// <summary>
    /// 角色(权限等级)
    /// </summary>
    [JsonConverter(typeof(EnumConverter))]
    [JsonProperty(PropertyName = "role")]
    public MemberRoleType Role { get; internal init; }

    /// <summary>
    /// 是否为机器人管理员
    /// </summary>
    [JsonIgnore]
    public bool IsSuperUser { get; internal set; } = false;

    /// <summary>
    /// 是否不良记录成员
    /// </summary>
    [JsonProperty(PropertyName = "unfriendly")]
    public bool Unfriendly { get; internal init; }

    /// <summary>
    /// 专属头衔
    /// </summary>
    [JsonProperty(PropertyName = "title")]
    public string Title { get; internal init; }

    /// <summary>
    /// <para>专属头衔过期时间</para>
    /// <para>在<see cref="Title"/>不为空时有效</para>
    /// </summary>
    [JsonIgnore]
    public DateTime? TitleExpireTime { get; internal init; }

    [JsonProperty(PropertyName = "title_expire_time",
                  NullValueHandling = NullValueHandling.Ignore,
                  DefaultValueHandling = DefaultValueHandling.Ignore)]
    private long? TitleExpireTimeStamp
    {
        init => TitleExpireTime = value == 0 ? null : value?.ToDateTime() ?? null;
    }

    /// <summary>
    /// 是否允许修改群名片
    /// </summary>
    [JsonProperty(PropertyName = "card_changeable")]
    public bool CardChangeable { get; internal init; }
}