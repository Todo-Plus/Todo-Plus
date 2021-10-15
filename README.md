# Todo+ —— 一款开源美观的桌面todolist

![.Net Framework](https://img.shields.io/badge/.net-5.0-blue)
[![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)

## 项目背景

Todo+是一个用于放置在桌面进行待办事项管理的工具，起初为一个基本想法，最后在本人大四软工课程中作为课程项目实现

作为一个日常偶尔会出现工作堆积情况的人，需要一款好用的软件来实现待办事项管理

相比于目前常见的备忘录、todolist产品，该软件可以放置在桌面上，可以直接在桌面查看，更便于事项的管理和提醒

## 运行截图

![桌面截图](https://i.loli.net/2021/10/15/MYgCqVct5lWGfEK.png)

<img src="https://i.loli.net/2021/10/15/ysM9coTBJpxedD3.png" alt="设置窗口" style="zoom:67%;" />

## 安装及系统环境

程序基于.Net Framework开发，可能需要安装对应的环境[.net 5.0](https://dotnet.microsoft.com/download/dotnet/5.0)，之后在Release中下载安装包安装进行使用即可

目前1.0.0版本暂未进行测试，可以运行在Window10（或以上） 64位操作

## 使用

目前版本支持：

* 添加、顶置、删除事项，以及添加事项类别，但是事项类别的设置暂未事项
* 修改窗口颜色以及窗口透明度
* 设置开机自启以及关闭是否进行提示

点击右上角设置按钮进入设置，设置分为三种：基本设置、外观设置、备份设置（待实现）

基本设置中可以进行开机自启动、提示开关的设置，以及tab的添加

外观设置中目前可以修改背景颜色以及透明度

## 版本迭代

### v1.0.0 beta

* 实现简单的交互逻辑，可以将程序主窗口锁定在桌面
* 可以进行事项的管理，以及自定义窗口的样式
