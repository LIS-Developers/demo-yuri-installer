# 尤里的复仇安装器 Yuri's Revenge Installer

&emsp;&emsp;本尤里的复仇安装器灵感来源于[Westwood官方的尤复CD安装包](https://www.uc129.com/xiazai/ra2/10802.html)，但真正开始开发本项目的动机是灵感同样来自CD安装包的[重聚未来](https://www.yra2.com/)安装包。  
&emsp;&emsp;本人很喜欢红警安装界面，曾联系重聚未来制作组获取该安装包模版以用于我自己的MOD项目，但对方似乎并不愿意与我分享，于是无奈自己复刻了一个类似的出来，但是易语言报毒，C++不会写，Python做GUI不方便，平均下来还是C#好。

## 解决方案组成

本解决方案由以下项目组成：
1. 安装器（C#）
    1) 组成
        + 源代码
        + 素材
        + 扩展控件
        + NuGet包及COM组件
    2) 说明
        + 整个解决方案最核心的部分，依赖于卸载程序项目，没了它这个解决方案可以说啥也不是。
        + 源代码是整个项目的核心部分，包含界面、控件、事件等。
        + 素材文件是项目的重要组成部分，也是“红警”风格的重要元素，因为这个才让它看起来像红警，点起来像红警。
        + `MizukiTools`是由我开发的组件扩展，包含`ScrollPanel`、`ScrollPictureBox`、`MizukiLabel`、`SetupLabel`、`ScrollClasses`、`MizukiTools`、`LRXDontLoveMeException`、`RightBarButton`等多个类。
        + 一些NuGet包和COM组件：`7z.lib`用于解压，`Windows Media Player`用于播放视频，`Obfuscar`用于混淆代码。
2. 注册机（C++）
    1) 组成
        + 源代码
    2) 说明
        + 生成符合某一条件的CDKey。
3. 卸载程序（VB）
    1) 组成
        + 源代码
    2) 说明
        + 用于卸载程序，如删除文件、注册表以及快捷方式。
4. 打包器（Python）
    1) 组成
        + 源代码
        + 可执行文件描述文件
    2) 额外模块（import）
        + pypiwin32
        + pywin32
        + pyinstaller
    3) 说明
        + 动态链接安装文件和安装器。
5. 安装文件（7z）
    1) 说明
        + 安装怎么可以连安装什么都没有呢？一般命名为`setup\*.7z`。
6. 打包脚本（Bat）
    1) 说明
        + 引用Python打包器打包安装器和安装文件，生成的exe运行时将安装文件释放到临时目录。

&emsp;&emsp;生成解决方案/安装器的时候会先生成卸载程序，再将混淆后的卸载程序载入到安装器的Resources里，最后生成安装器。生成完解决方案点击方案根目录的pack.bat即可将安装器与安装文件打包。

## 功能及计划

&emsp;&emsp;目前并不考虑跨平台的计划。

已知Bug：

- 许可协议与路径选择页面前进后退按钮连点Bug
- 注册码音效缺失
- 编辑框本身不高亮

总体功能计划：

- [ ] 多语言支持
- [ ] 优化源代码可读性
- [ ] 统一的安装包信息编辑

安装器功能计划：

- [x] 安装程序界面
- [x] 类似红警的目录选择框、滚动框
- [x] 类似红警的按钮点击动画
- [x] 写入注册表
- [x] 创建快捷方式
- [x] 内存优化
- [x] 类似红警的对话框
- [x] 注册码基本支持（在不修改该功能的前提下直接使用该功能可能引发Bug）
- [ ] 注册码完全支持
- [ ] 在线安装功能
- [ ] 灵活且不同版本兼容的自定义安装信息
- [ ] 安装包背景窗口或切换分辨率

卸载器功能计划：

- [x] 删除注册表及快捷方式
- [ ] 允许仅删除安装的文件
- [ ] 允许修复原程序

注册机功能计划：

- [x] 生成注册码
- [ ] 个性化生成
- [ ] 按照某一条件针对性生成
- [ ] 灵活条件设置

打包器功能计划：

- [x] 链接安装器和安装文件
- [ ] 静态链接，以免资源浪费

## 简易生成步骤

在此之前请确保你已安装Python所需的所有依赖项。

1. 点击生成解决方案。
2. 点击pack.bat。
3. 打开Compiled文件夹，得到生成的文件。

## 注意事项

* 本项目使用Visual Studio 2022开发，但不知道其他版本行不行，作者以为2019没用了就给卸了，所以无法测试。
* 项目的资源放置在Resources文件夹，安装文件放在项目根目录。
* 安装选项（比如要安装什么文件）设置在ExtraControls.cs里。

## 需求

Windows .NET 4.8 运行库:

* [.NET Framework 4.8 Runtime](https://dotnet.microsoft.com/zh-cn/download/dotnet-framework/thank-you/net48-web-installer)

## 版权

&emsp;&emsp;本项目非完全开源项目（但demo-yuri-installer开源），未经许可不得盗用，著作权属Enderseven Tina（Shimada Mizuki、Ender7 Tina、Enderseven1），Obfuscar插件之著作权属原作者。
&emsp;&emsp;本仓库不包含版权不属于本人的图片、声音资源文件（如EA版权所有之界面背景），仅包含源代码及引用程序集、必要的图标文件等。不包含的文件可以在[此处](#)免费下载。

## 获取本项目使用权

&emsp;&emsp;若本人在任何平台发布项目开源地址，则访问该项目并下载者默认获得标准使用许可。  
&emsp;&emsp;本项目不强制收费获取，但也请各位喜欢或愿意支持本项目的朋友能打≥20元或者$3.5的赏支持下作者（毕竟本项目最初是他人找我花钱定制的，如果都能免费拿去用对他不公平）。暂时没钱的朋友也可以以后补票，我不会亏待任何一位给了赞助的宝子们。  

Copyright © 2024 Shimada Mizuki. All Rights Reserved.