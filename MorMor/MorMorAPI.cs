﻿global using MomoAPI.IO;
using System.Data;
using Microsoft.Data.Sqlite;
using MomoAPI;
using MomoAPI.Interface;
using MorMor.Commands;
using MorMor.Configuration;
using MorMor.DB.Manager;
using MorMor.Event;
using MorMor.Net;
using MorMor.Plugin;
using MySql.Data.MySqlClient;

namespace MorMor;

public class MorMorAPI
{
    public static IDbConnection DB { get; internal set; } = null!;

    public static SignManager SignManager { get; internal set; } = null!;

    public static GroupMananger GroupManager { get; internal set; } = null!;

    public static AccountManager AccountManager { get; internal set; } = null!;

    public static CurrencyManager CurrencyManager { get; internal set; } = null!;

    public static TerrariaUserManager TerrariaUserManager { get; internal set; } = null!;

    public static string PATH => Environment.CurrentDirectory;

    public static string SAVE_PATH => Path.Combine(PATH, "Config");

    internal static string ConfigPath => Path.Combine(SAVE_PATH, "MorMor.Json");

    internal static string UserLocationPath => Path.Combine(SAVE_PATH, "UserLocation.Json");

    internal static string TerrariaShopPath => Path.Combine(SAVE_PATH, "Shop.Json");

    internal static string TerrariaPrizePath => Path.Combine(SAVE_PATH, "Prize.Json");

    public static MorMorSetting Setting { get; internal set; } = new();

    public static UserLocation UserLocation { get; internal set; } = new();

    public static TerrariaShop TerrariaShop { get; internal set; } = new();

    public static TerrariaPrize TerrariaPrize { get; internal set; } = new();

    public static IMomoService Service { get; internal set; } = null!;

    public static async Task Star()
    {
        var ConsoleInfo = @" 
 /$$      /$$                     /$$      /$$                     /$$$$$$$              /$$    
| $$$    /$$$                    | $$$    /$$$                    | $$__  $$            | $$    
| $$$$  /$$$$  /$$$$$$   /$$$$$$ | $$$$  /$$$$  /$$$$$$   /$$$$$$ | $$  \ $$  /$$$$$$  /$$$$$$  
| $$ $$/$$ $$ /$$__  $$ /$$__  $$| $$ $$/$$ $$ /$$__  $$ /$$__  $$| $$$$$$$  /$$__  $$|_  $$_/  
| $$  $$$| $$| $$  \ $$| $$  \__/| $$  $$$| $$| $$  \ $$| $$  \__/| $$__  $$| $$  \ $$  | $$    
| $$\  $ | $$| $$  | $$| $$      | $$\  $ | $$| $$  | $$| $$      | $$  \ $$| $$  | $$  | $$ /$$
| $$ \/  | $$|  $$$$$$/| $$      | $$ \/  | $$|  $$$$$$/| $$      | $$$$$$$/|  $$$$$$/  |  $$$$/
|__/     |__/ \______/ |__/      |__/     |__/ \______/ |__/      |_______/  \______/    \___/  ";
        if (!Directory.Exists(SAVE_PATH))
            Directory.CreateDirectory(SAVE_PATH);
        Log.ConsoleInfo(ConsoleInfo, ConsoleColor.Green);
        Console.Title = "MorMor";
        Utils.Utility.KillChrome();
        //读取Config
        LoadConfig();
        //初始化数据库
        InitDb();
        //扩展程序集
        AppDomain.CurrentDomain.AssemblyResolve += PluginLoader.Resolve;
        //启动机器人服务
        Service = await MomoServiceFactory.CreateService(new()
        {
            Host = Setting.Host,
            Port = Setting.Port,
            AccessToken = Setting.AccessToken
        }).Start();
        //加载插件
        PluginLoader.Load();
        //Socket信息适配器
        TShockReceive.SocketMessage += TerrariaMsgReceiveHandler.Adapter;
        //群消息转发适配器
        Service.Event.OnGroupMessage += TerrariaMsgReceiveHandler.GroupMessageForwardAdapter;
        //文件事件
        Service.Event.OnGroupUpLoadFile += TerrariaMsgReceiveHandler.GroupFile;
        //监听指令
        Service.Event.OnGroupMessage += e => CommandManager.Hook.CommandAdapter(e);
        //socket服务器启动
        await TShockReceive.StartService();

    }

    internal static void LoadConfig()
    {
        Setting = ConfigHelpr.LoadConfig(ConfigPath, Setting);
        UserLocation = ConfigHelpr.LoadConfig(UserLocationPath, UserLocation);
        TerrariaShop = ConfigHelpr.LoadConfig(TerrariaShopPath, TerrariaShop);
        TerrariaPrize = ConfigHelpr.LoadConfig(TerrariaPrizePath, TerrariaPrize);
    }

    internal static void ConfigSave()
    {
        ConfigHelpr.Write(ConfigPath, Setting);
        ConfigHelpr.Write(UserLocationPath, UserLocation);
        ConfigHelpr.Write(TerrariaShopPath, TerrariaShop);
        ConfigHelpr.Write(TerrariaPrizePath, TerrariaPrize);
    }

    private static void InitDb()
    {
        switch (Setting.DbType.ToLower())
        {
            case "sqlite":
                {
                    string sql = Path.Combine(PATH, Setting.DbPath);
                    if (Path.GetDirectoryName(sql) is string path)
                    {
                        Directory.CreateDirectory(path);
                        DB = new SqliteConnection(string.Format("Data Source={0}", sql));
                        break;
                    }
                    throw new ArgumentNullException("无法找到数据库路径!");
                }
            case "mysql":
                {
                    DB = new MySqlConnection()
                    {
                        ConnectionString = string.Format("Server={0};Port={1};Database={2};Uid={3};Pwd={4}",
                        Setting.DbHost, Setting.DbPort, Setting.DbName, Setting.DbUserName, Setting.DbPassword)
                    };
                    break;
                }
            default:
                throw new TypeLoadException("无法使用类型:" + Setting.DbType);

        }
        GroupManager = new();
        AccountManager = new();
        CurrencyManager = new();
        SignManager = new();
        TerrariaUserManager = new();
    }
}
