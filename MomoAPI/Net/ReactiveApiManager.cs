using MomoAPI.Converter;
using MomoAPI.Entities;
using MomoAPI.Enumeration.ApiType;
using MomoAPI.Extensions;
using MomoAPI.Model.API;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace MomoAPI.Net;

/// <summary>
/// 用于管理和发送API请求
/// </summary>
internal static class ReactiveApiManager
{
    #region Buffer

    /// <summary>
    /// API响应被观察对象
    /// 结构:Tuple[echo id,响应json]
    /// </summary>
    private static readonly Subject<(Guid id, JsonObject data)> ApiSubject = new();

    #endregion

    #region 通信

    /// <summary>
    /// 获取到API响应
    /// </summary>
    /// <param name="echo">标识符</param>
    /// <param name="response">响应json</param>
    internal static void GetResponse(Guid echo, JsonObject response)
    {
        ApiSubject.OnNext((echo, response));
    }

    /// <summary>
    /// 向API客户端发送请求数据
    /// </summary>
    /// <param name="apiRequest">请求信息</param>
    /// <param name="connectionId">服务器连接标识符</param>
    /// <param name="timeout">覆盖原有超时,在不为空时有效</param>
    /// <returns>API返回</returns>
    internal static async ValueTask<(ApiStatus, JsonObject)> SendApiRequest(ApiRequest request, TimeSpan? timeout = null)
    {
        if (timeout == null)
            timeout = TimeSpan.FromSeconds(15);
        var task = ApiSubject.Where(x => x.id == request.Echo)
            .Select(x => x.data)
            .Timeout((TimeSpan)timeout)
            .Take(1)
            .ToTask()
            .RunCatch(e =>
            {
                return [];
            });
        ConnectMananger.SendMessage(request.ToJson());
        var obj = await task;
        return (GetApiStatus(EnumConverter<ActionType>.GetFieldDesc(request.ApiRequestType), obj ?? []), obj ?? []);



    }

    private static ApiStatus GetApiStatus(string apiName, JsonObject msg)
    {
        if (!msg.TryGetPropertyValue("retcode", out var val) || val == null)
        {
            return new ApiStatus
            {
                RetCode = ApiStatusType.UnknownStatus,
                ApiMessage = "这可能是超时导致的",
                ApiStatusStr = ""
            };
        }
        string retCode = int.TryParse(val.ToString(), out int ret) switch
        {
            true when ret < 0 => "100",
            false => "-5",
            _ => ret.ToString()
        };

        ApiStatusType apiStatus = Enum.TryParse(retCode, out ApiStatusType messageCode)
            ? messageCode
            : ApiStatusType.UnknownStatus;
        string message = msg["msg"] == null && msg["wording"] == null
            ? string.Empty
            : $"{msg["msg"] ?? string.Empty}({msg["wording"] ?? string.Empty})";
        string statusStr = msg["status"]?.ToString() ?? "failed";



        return new ApiStatus
        {
            RetCode = apiStatus,
            ApiMessage = message,
            ApiStatusStr = statusStr
        };
    }

    #endregion
}