﻿using ProtoBuf;

namespace MorMor.Model.Socket.Action.Response;

[ProtoContract]
[ProtoInclude(401, typeof(DeadRank))]
[ProtoInclude(402, typeof(GameProgress))]
[ProtoInclude(403, typeof(MapImage))]
[ProtoInclude(404, typeof(PlayerInventory))]
[ProtoInclude(405, typeof(PlayerOnlineRank))]
[ProtoInclude(406, typeof(ServerCommand))]
[ProtoInclude(407, typeof(ServerOnline))]
[ProtoInclude(408, typeof(UpLoadWorldFile))]
public class BaseActionResponse : BaseAction
{
    [ProtoMember(6)] public string Message { get; set; }

    [ProtoMember(7)] public bool Status { get; set; }

    public BaseActionResponse() : base()
    {
    }

}