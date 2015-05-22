# 工具条按钮说明 #
## 文件工具条 ##
  * ![http://sgsubdotnet.googlecode.com/svn-history/r37/wiki/imgs/openass.png](http://sgsubdotnet.googlecode.com/svn-history/r37/wiki/imgs/openass.png)  打开ass字幕文件
  * ![http://sgsubdotnet.googlecode.com/svn-history/r37/wiki/imgs/opentxt.png](http://sgsubdotnet.googlecode.com/svn-history/r37/wiki/imgs/opentxt.png)  打开txt纯文本翻译稿
  * ![http://sgsubdotnet.googlecode.com/svn-history/r37/wiki/imgs/openvideo.png](http://sgsubdotnet.googlecode.com/svn-history/r37/wiki/imgs/openvideo.png)  打开视频文件
  * ![http://sgsubdotnet.googlecode.com/svn-history/r37/wiki/imgs/saveass.png](http://sgsubdotnet.googlecode.com/svn-history/r37/wiki/imgs/saveass.png)  保存ass字幕文件
  * ![http://sgsubdotnet.googlecode.com/svn/wiki/imgs/initfft.png](http://sgsubdotnet.googlecode.com/svn/wiki/imgs/initfft.png) 载入频谱图

## 编辑工具条 ##
  * ![http://sgsubdotnet.googlecode.com/svn-history/r37/wiki/imgs/play.png](http://sgsubdotnet.googlecode.com/svn-history/r37/wiki/imgs/play.png)  翻放视频
  * ![http://sgsubdotnet.googlecode.com/svn-history/r37/wiki/imgs/pause.png](http://sgsubdotnet.googlecode.com/svn-history/r37/wiki/imgs/pause.png)  暂停播放
  * ![http://sgsubdotnet.googlecode.com/svn-history/r37/wiki/imgs/jmp.png](http://sgsubdotnet.googlecode.com/svn-history/r37/wiki/imgs/jmp.png)  视频跳至当前行的开始时间
  * ![http://sgsubdotnet.googlecode.com/svn-history/r37/wiki/imgs/dup.png](http://sgsubdotnet.googlecode.com/svn-history/r37/wiki/imgs/dup.png)  重复当前行
  * ![http://sgsubdotnet.googlecode.com/svn-history/r37/wiki/imgs/del.png](http://sgsubdotnet.googlecode.com/svn-history/r37/wiki/imgs/del.png)  删除当前行
  * ![http://sgsubdotnet.googlecode.com/svn-history/r37/wiki/imgs/ins.png](http://sgsubdotnet.googlecode.com/svn-history/r37/wiki/imgs/ins.png)  在当前行之后插入空行
  * ![http://sgsubdotnet.googlecode.com/svn/wiki/imgs/insertbefore.png](http://sgsubdotnet.googlecode.com/svn/wiki/imgs/insertbefore.png)  在当前行之前插入空行
  * ![http://sgsubdotnet.googlecode.com/svn/wiki/imgs/timelinecheck.png](http://sgsubdotnet.googlecode.com/svn/wiki/imgs/timelinecheck.png)  检查时间轴，如有异常，用对话框提示并改变相应时间的文字颜色
  * ![http://sgsubdotnet.googlecode.com/svn/wiki/imgs/undo.png](http://sgsubdotnet.googlecode.com/svn/wiki/imgs/undo.png)  撤消，暂定最多20步
  * ![http://sgsubdotnet.googlecode.com/svn/wiki/imgs/markcell.png](http://sgsubdotnet.googlecode.com/svn/wiki/imgs/markcell.png)  标记选定的单元格
  * ![http://sgsubdotnet.googlecode.com/svn/wiki/imgs/unmarkall.png](http://sgsubdotnet.googlecode.com/svn/wiki/imgs/unmarkall.png)  取消所有标记
  * ![http://sgsubdotnet.googlecode.com/svn/wiki/imgs/timeoffset.png](http://sgsubdotnet.googlecode.com/svn/wiki/imgs/timeoffset.png)  对标记的时间单元格进行时间平移，忽略标记的文字单元格



# 按键说明 #
  * 按键设置界面
![http://sgsubdotnet.googlecode.com/svn/wiki/imgs/keycfg.png](http://sgsubdotnet.googlecode.com/svn/wiki/imgs/keycfg.png)

选中按钮后按相应的按键就可以对快捷键进行设置。

起始和终止反应时间是指插入时间点时的修正值，负数表示提前。前进、后退步长指按一次前时（后退）时视频前进（后退）的时间。

自动重叠修正指当说话人句与句之间停顿很短，上一句的插入终止时间和下一句的起始时间会由于其修正值不相等而发生少量重叠。激活该功能则会以下一句的起始时间为准自动修正上一句的终止时间，避免产生重叠。

  * 快捷键说明
    * 后退 视频后退一个步长；
    * 前进 视频前进一个步长；
    * 跳至当前行 视频跳至当前行的开始时间；
    * 跳至上一行 视频跳至当前行的开始时间；
    * 插入时间点 按下时插入开始时间，抬起时插入终止时间，之后移至下一行；
    * 连续插入时间 插入当前行的结束时间同时插入下一行的开始时间，并将当前行移至下一行；
    * 编辑模式 开始编辑当前单元格（功能同等于双击鼠标左键）；
    * 插入起始时间 按下时插入开始时间；
    * 插入终止时间 按下时插入终止时间并移至下一行。
  * 固定快捷键
    * 复制 Ctrl + C
    * 粘贴 Ctrl + V
    * 清空单元格内容 Delete
    * 删除选定行 Ctrl + Delete

# 界面预览 #

![http://sgsubdotnet.googlecode.com/svn/wiki/imgs/screen1_1_6.png](http://sgsubdotnet.googlecode.com/svn/wiki/imgs/screen1_1_6.png)