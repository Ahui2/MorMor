<div align="center">
  
# MorMor

# 基于[LLOneBot](https://github.com/LLOneBot/LLOneBot) 开发的.NET TerrariaServerBot

</div>

## 说明

- MomoAPI 基于[Sora](https://github.com/Hoshikawa-Kaguya/Sora)更改而来。
- MorMor 项目中数据库与分页工具部使用了[TShock](https://github.com/Pryaxis/TShock)代码
- 该机器人依然采用 tshock 的权限管理模式，使用 RABQ 模型，指令设计上不会与 tshock 有多少区别，另外也采用了插件加载模式，可以自写插件实现更多功能。

## 使用注意事项

- 如何部署 LLOneBot？请[点击此处](https://llonebot.github.io/zh-CN/guide/getting-started)查看教程!
- 使用前你可能需要配置 Mail 服务，机器人会用到，如发送注册密码等服务！
- TShock 管理功能需要插件，机器人用到的插件可在[TshockAdapter](https://github.com/dalaoshus/TShockAdapter)仓库下载使用

# 预计支持功能(部分功能可能需要插件支持)

## 群功能

- [x] 群禁言
- [ ] 群点歌(看框架是否支持)
- [x] 上下管理
- [x] wiki 查询
- [ ] reply(自定义回复)
- [x] 群签到
- [x] 查缩写
- [x] 设置群名
- [ ] 加群请求自定义处理

## 服务器管理

- [x] 执行服务器命令
- [x] 查看玩家背包
- [x] 查看服务器进度
- [x] 自动重置服务器
- [x] 查看服务器地图
- [x] 在线排行
- [x] 死亡排行
- [x] 用户注册管理
- [ ] 泰拉商店
- [ ] 泰拉奖池
- [x] 查询用户详细

## 其他功能

- [x] 服务器消息与群互相转发(需插件)

## 指令列表

| 名称      | 是否需要 TShock 插件 |          描述          |
| --------- | :------------------: | :--------------------: |
| /help     |          否          |      查看指令列表      |
| /签到     |          否          |        每日签到        |
| /reload   |          否          |        重读配置        |
| /group    |          否          |       权限组管理       |
| /account  |          否          |       账户组管理       |
| /星币     |          否          |        货币管理        |
| /scmdperm |          否          |      查询指令权限      |
| /缩写     |          否          |      查询中文缩写      |
| /禁       |          否          |          禁言          |
| /解       |          否          |          解禁          |
| /生成地图 |          是          | 生成 Tshock 服务器地图 |
| /进度查询 |          是          |     查询服务器进度     |
| /user     |          否          |      注册用户管理      |
