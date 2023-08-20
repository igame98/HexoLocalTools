# HexoLocalTools
使用Hexo + node.js + git 创建个人博客网站后，所有指令都是使用CMD指令不是很方便。而且我这也是直接上传静态网页，所以未了方便便花了一天时间构建了一个Hexo工具，此工具本质上也执行CMD指令，只不过有界面方便操作。

使用工具前的准备：
1.安装node.js(npm一般随node.js 就安装上了)
2.安装git
3.安装hexo(使用npm安装)


支持功能：
1.新建站点
2.根据模版新建文件
3.可以简单的编辑文件内容
4.支持本地笔记数据
5.支持清楚数据、生成页面、本地预览，发布
6.支持一键预览(clean、generate、server指令依次执行)
7.支持一键发布(clean、generate、deploy指令依次执行，需要配置好deoloy)

deoloy 配置(hexo一键部署)：https://hexo.io/zh-cn/docs/one-command-deployment
