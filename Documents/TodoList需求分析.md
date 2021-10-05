# TodoList需求分析

## Version1.0

**1.0版本只需要能够实现基本的功能即可，暂时不需要太多的样式美化设计，保证能够进行正常的交互**

### 需求

* 基本功能实现：

  * 可以在桌面锁定显示，动态直接添加项目到列表（后续如果有需要可以做一个侧边放置的功能，可以选定显示方式）
  * 可以将文件备份到OneDrive网盘（暂不考虑加密问题）
  * 可以读取备份文件（反序列化）

  > 在动态添加的实现中需要同时持久化放到运行目录下，避免系统出现异常导致丢失信息

  * 对于不同类型、不同程度的分类放到后续版本实现

* 设置面板需求：

  * 设置在桌面显示的字体、字体大小等

    > 归类为外观设定:
    >
    > * 字体、字体大小
    > * 背景颜色
    > * 透明度
    > * 点击关闭是最小化到任务栏或直接关闭
  
  * 在设置中设置网盘备份信息
  
    > 归类为备份设定，在Version1.0中仅需要设置好OneDriver相关的信息，以及导出、导入功能
  
  * 设置开机默认启动：开关

### 功能实现调研

Windows UI框架：WPF（Windows Presentation Foundation），框架文档：[Windows Presentation Foundation](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/?view=netframeworkdesktop-4.8)

实现要点：

* 锁定在桌面，以及最小化到任务栏

> 桌面的主窗口需要能够被设置锁定、以及能够被拖动，窗口需要关闭标题栏

* 显示窗口的样式动态设定
* 数据存储的数据结构
* 备份功能需要的序列号和反序列化
* 桌面样式的动态功能

> 多窗口下，子窗口调节数值让父窗口产生变化

#### 窗口相关

##### 窗口大小设定

XAML窗体定义中可以设置最小宽度和最小高度

```xaml
<Window
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation" 
        MinHeight = "xxx" Height="xxx">
</Window>
```

[Overview of WPF windows (WPF .NET)](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/windows/?view=netdesktop-5.0)

##### 窗体绘制、重绘

窗口需要可以在缩放的时候Item固定在左边，此外在项目数量过多的时候

WPF实现动画相关：[Graphics and Multimedia](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/graphics-multimedia)



##### 桌面固定

##### 任务栏托盘及菜单

~~默认的wpf中没有最小化的托盘的功能，需要自己实现~~，参考代码[NotifyIconWpf](https://bitbucket.org/hardcodet/notifyicon-wpf/src/master/Hardcodet.NotifyIcon.Wpf/Source/NotifyIconWpf/)
在.NET中存在控件可以用于实现托盘图标[NotifyIcon](https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.notifyicon?redirectedfrom=MSDN&view=net-5.0)

##### 样式设定

在WPF中存在类似于CSS的样式，可以在资源中写好样式绑定到窗口以及其他的控件中，这里可以重用样式相关的代码

[Styles and templates (WPF .NET)](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/controls/styles-templates-overview?view=netdesktop-5.0)

##### 窗体动画实现

> 需要实现的动画：鼠标hover时按钮的阴影、颜色变化，窗体初始化时Item动画

##### 子窗口

#### 数据相关

> 在初始设计时使用C++语言来设计，后续实现视语言情况进行实现

`Item`

```
Item
  params
	- Index: int
	- Desc: string
	- Done: bool
	- Tag: string
	- Color: string  // 留给后面的版本，1.0版本先实现基本的功能
	- Deadline： datetime // 后续实现集成到日程表
	- Starttime： datetime
	- Endtime： datetime
	- Priority: int        // 冗余，内部排序关键字，考虑用于后续操作
	- DoneTime： datetime  // 用于在Done列表中显示日期
  function
  	- sort_cmp: bool
  	
hashmap[index]->Item（可遍历，可直接查询）
doneMap\todoMap
```

`Runtime`

Runtime用于保存、持久化用户运行时的一些基础设置，需要保存的信息有：

* 主窗口宽度、长度、位置，用于下一次启动时加载
* 窗口锁定状态
* 当前的Index，每次自增，不保证唯一性，删除后的item不用进行处理，程序通过同步得到todolist、donelist时，根据两者取最大来得到

`Setting`

```
Setting
  params
    - Appearance:
    - Backup:
```

> Setting中分类出来的项目考虑可以单独作为一个类来实现，后续为其余的相关操作单独作为类来进行
>
> 数据之间的交互，需要考虑到对原始文件修改的时候出现冲突，需要一个子线程去同步避免阻塞主窗口进程

##### 数据持久化

###### Json文件持久化

###### 二进制文件持久化

设计持久化方式，将项目列表放入到一个二进制体中，为了保证不影响到后续添加属性时的还需要修改对应的序列化器，需要通过遍历的方式来进行处理

**读取配置文件得到当前版本信息，需要可以兼容旧版本，新版本的软件可以兼容老版本的序列化文件**



#### 具体实现

主界面：

* 主界面初始化时，根据链表结构得到List，作为对应的选项

* 在点击添加按钮后，在窗口下填入对应的数据后，将数据添加到列表结构中

* 主界面点击一个列表下的“完成”按钮后，将对应的事项设置为完成状态，移入到完成列表的最后

  > 要点：点击后能够得到对应的事项，对该事项进行修改；需要考察一下c#中的引用、指针等的实现

* 点击“删除”按钮后，移出列表，并且在内存中析构

* 点击“编辑”后，弹出对应的提示框，并且进行对应的修改，保存后再写入对应的事项中

* 点击“顶置”（仅Todo列表下会显示），将对应的事项移到顶部，需要考虑数据结构的设计

[^]: 对于**顶置**的实现，可以考虑两种方式，也对应不同的存储结构：1、使用链表，每次顶置将其放到链表前；2、使用数组，每次顶置修改排序关键字用于内部排序，每一次排序后重置优先级；

#### 同步相关

##### OneDrive接口

通过Microsoft Graph可以访问用户在OneDrive中的数据，数据需要考虑并发修改的问题， 避免出现冲突

[通过导航 Microsoft Graph 访问数据和方法](https://docs.microsoft.com/zh-cn/graph/traverse-the-graph)

#### 测试单元

C#中提供了用于编写测试单元的方法：[Walkthrough: Create and run unit tests for managed code](https://docs.microsoft.com/en-us/visualstudio/test/walkthrough-creating-and-running-unit-tests-for-managed-code?view=vs-2019)

单元测试统一放在test文件夹，**测试单元代码文件格式待定**

### UML设计

所需窗口：主窗口（列表），设置窗口（菜单），外观设置窗口、备份设置窗口

Setting：设置相关作为单例，程序初始化反序列化设置信息载入到内存，窗体绘制时根据Setting中的信息来进行绘制。进入设置窗口的时候copy一份单例，在应用之后再将应用设置替换，否则原有的设置替换当前的单例。

### 代码文档生成



### 部分规范

* 代码、文档文件编码：`UTF-8`，避免出现乱码问题



### 实现规划

* 全部窗口的样式设计、以及类之间的交互逻辑

* 在程序中绘制出大体的全部窗口
* 绑定窗口、控件之间的交互关系
* 实现数据的存储、交互逻辑，保证程序能够正常运行
* 添加api等实现备份的功能，**注意要保证可拓展性，可以留出可以用于选择不同网盘进行备份的方法**
* 在完成全部的基本功能之后，再对窗口进行样式的美化，以及相关的过渡动画（放到后续版本）

## Version 1.1

**对1.0版本的样式美化以及附加功能的添加**



