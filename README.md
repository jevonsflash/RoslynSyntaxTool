# Roslyn Syntax Tool

## 基础概念
Syntax Api:

Roslyn 是微软开源的 .NET 编译平台。编译平台支持 C# 和 Visual Basic 代码编译，并提供丰富的语法分析 API。

语法树(SyntaxTree)是一种由编译器 API 公开的基础数据结构。这些树描述了C#源代码的词法和语法结构。

利用语法分析 API可以将一段C#代码翻译成等效的语法树代码。
关于语法分析请查看官方文档
https://docs.microsoft.com/zh-cn/dotnet/csharp/roslyn-sdk/get-started/syntax-analysis

可以通过 [Roslyn 入门系列文章](https://blog.csdn.net/WPwalter/article/details/79616402) 学习Roslyn相关知识 

## 应用场景

* 需要动态编译的，如在开发微服务中动态生成代理类，项目的插件化改造等
* 需要动态生成C#代码脚本的，如项目模板生成器，C#脚本生成工具等
* 需要分析C#使用场景，如代码安全性审查等
* ...
## 介绍

RoslynSyntaxTool利用语法分析 API，提供以下功能：

* 将指定的C#代码转为等效的语法树代码
* 将语法树代码还原为C#代码
* 图形化查看语法树结构
* 查看语法树节点属性详情

这是独立开发者的一个开源项目, 希望得到您的意见反馈，请将Bugs汇报至我的邮箱

 ![avatar](https://github.com/MatoApps/RoslynSyntaxTool/blob/master/RST/screenshot.png)

## 感谢

KirillOsenkov的RoslynOuter项目，链接: https://github.com/KirillOsenkov/RoslynQuoter

语法树代码生成器代码借鉴自此项目

## 更新内容：


| Date  |  Version  | Content                                                             |
| :---: | :-------: | :------------------------------------------------------------------ |
| V1.0  | 2021-3-16 | 初始版本                                                            |
| V2.0  | 2022-5-16 | 1. 升级项目框架至.Net 6.0  2. 增加ConvertToCSharp页面 3. 更新README |
| V2.1  | 2024-8-24 | 修复语法树代码生成器代码生成错误的问题                              |



## 安装说明：

1. 下载安装包 https://raw.githubusercontent.com/MatoApps/RoslynSyntaxTool/master/RST/rst.zip

2. 解压并双击 setup.exe 安装

## 运行环境

* Microsoft Windows 7 及以上



## 已知问题：


## 作者信息：

作者：林小

邮箱：jevonsflash@qq.com


## License

The MIT License (MIT)

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
