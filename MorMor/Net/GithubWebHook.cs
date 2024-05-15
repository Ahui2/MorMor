﻿using MomoAPI.Entities;
using MorMor.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Octokit.Webhooks;
using Octokit.Webhooks.Events;
using Octokit.Webhooks.Events.Release;
using Octokit.Webhooks.Events.Star;
using System.Text;
namespace MorMor.Net;

internal class GithubWebHook : WebhookEventProcessor
{
    public List<long> Groups = [994731943, 232109072];
    public async Task SendGroupMsg(MessageBody body)
    {
        var (_, groups) = await MomoAPI.Net.OneBotAPI.Instance.GetGroupList();
        await SendGroupMsg(body, groups.Select(x => x.ID).ToList());
    }

    public async Task SendGroupMsg(MessageBody body, List<long> groups)
    {
        foreach (var group in groups)
        {
            await MomoAPI.Net.OneBotAPI.Instance.SendGroupMessage(group, body, TimeSpan.FromSeconds(10));
        }
    }

    protected override async Task ProcessReleaseWebhookAsync(WebhookHeaders headers, ReleaseEvent releaseEvent, ReleaseAction action)
    {
        if (action == "edited")
        {
            try
            {
                var repName = releaseEvent.Repository?.FullName;
                var sb = new StringBuilder($"# Release Github 仓库 {repName}");
                sb.AppendLine();
                sb.AppendLine("## 有新的版本更新");
                sb.AppendLine($"- 当前时间: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                sb.AppendLine(releaseEvent.Release.Body);
                HttpClient client = new();
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Linux; Android 13; 22127RK46C Build/TKQ1.220905.001) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.5112.97 Mobile Safari/537.36");
                var result = await client.GetStringAsync(releaseEvent.Release.AssetsUrl);
                var jobj = JsonConvert.DeserializeObject<JArray>(result);
                var download = jobj?[0]?["browser_download_url"]?.ToString();
                var update = jobj?[0]?["updated_at"]?.ToString();
                if (!string.IsNullOrEmpty(download))
                {
                    var file = await client.GetByteArrayAsync("https://gitdl.cn/" + download);
                    await SendGroupMsg(new MessageBody().MarkdownImage(sb.ToString()), Groups);
                    await SendGroupMsg(new MessageBody().File("base64://" + Convert.ToBase64String(file), $"({Guid.NewGuid().ToString()[..8]})Plugins.zip"), Groups);
                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }

        }
    }

    protected override async Task ProcessStarWebhookAsync(WebhookHeaders headers, StarEvent starEvent, StarAction action)
    {
        var msg = $"用户 {starEvent.Sender?.Login} Stared 仓库 {starEvent.Repository?.FullName} 共计({starEvent.Repository?.StargazersCount})个Star";
        await SendGroupMsg(msg);
    }
    //protected override async Task ProcessPushWebhookAsync(WebhookHeaders headers, PushEvent pushEvent)
    //{
    //    if (pushEvent.Pusher.Name != "github-actions[bot]")
    //    {
    //        var repName = pushEvent.Repository?.FullName;
    //        var sb = new StringBuilder($"# Push Github 仓库 {repName}");
    //        foreach (var commit in pushEvent.Commits)
    //        {
    //            sb.AppendLine();
    //            sb.AppendLine($"### {commit.Message}");
    //            sb.AppendLine($"- 用户名: `{commit.Author.Username}`");
    //            sb.AppendLine($"- 添加文件: {(commit.Added.Any() ? string.Join(" ", commit.Added.Select(x => $"`{x}`")) : "无")}");
    //            sb.AppendLine($"- 删除文件: {(commit.Removed.Any() ? string.Join(" ", commit.Removed.Select(x => $"`{x}`")) : "无")}");
    //            sb.AppendLine($"- 更改文件: {(commit.Modified.Any() ? string.Join(" ", commit.Modified.Select(x => $"`{x}`")) : "无")}");
    //        }
    //        await SendGroupMsg(new MessageBody().MarkdownImage(sb.ToString()));
    //    }
    //}

    //protected override async Task ProcessPullRequestWebhookAsync(WebhookHeaders headers, PullRequestEvent pullRequestEvent, PullRequestAction action)
    //{
    //    if (action == PullRequestAction.Opened)
    //    { 
    //        var title = pullRequestEvent.PullRequest.Title;
    //        var userName = pullRequestEvent.PullRequest.User.Login;
    //        var repName = pullRequestEvent.Repository?.FullName;
    //        var sb = new StringBuilder($"# Pull Request Github 仓库 {repName}");
    //        sb.AppendLine();
    //        sb.AppendLine($"## {title}");
    //        sb.AppendLine($"- 发起者: `{userName}`");
    //        sb.AppendLine($"```");
    //        sb.AppendLine(pullRequestEvent.PullRequest.Body);
    //        sb.AppendLine($"```");
    //        await SendGroupMsg(new MessageBody().MarkdownImage(sb.ToString()));
    //    }

    //}
}
