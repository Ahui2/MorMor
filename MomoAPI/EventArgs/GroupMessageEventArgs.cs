﻿using MomoAPI.Entities;
using MomoAPI.Entities.Info;
using MomoAPI.Entities.Segment;
using MomoAPI.Model.Event.MessageEvent;

namespace MomoAPI.EventArgs;

public class GroupMessageEventArgs : MessageEventArgs
{
    internal GroupMessageEventArgs(OnebotGroupMsgEventArgs args) : base(args)
    {
        Group = new(args.GroupId);
        IsSuperAdmin = args.SenderInfo.Role == Enumeration.EventParamType.MemberRoleType.Admin || args.SenderInfo.Role == Enumeration.EventParamType.MemberRoleType.Owner;
        SenderInfo = args.SenderInfo;
    }

    public Group Group { get; }

    public GroupSenderInfo SenderInfo { get; }

    public bool IsSuperAdmin { get; }

    public async ValueTask<ApiStatus> SetMemberNick(string nick)
    {
        return await Group.SetMemberCard(SenderInfo.UserId, nick);
    }

    public async ValueTask<(ApiStatus, long)> Reply(MessageBody body, bool Cite = false)
    {
        if (Cite)
            body.Insert(0, MomoSegment.Reply(MessageContext.MessageID));
        return await Group.Reply(body);
    }
}
