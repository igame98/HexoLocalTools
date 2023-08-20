using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HexoLocalTools.Tools
{
    internal static class GloalTools
    {
        public static string currentSite = "";

        public static void CreateSite(string path,Action<string> onCallback)
        {
            currentSite = path;
            string cmd = $"hexo init {path}&cd {path} &npm install&exit";
            ExeCmd(cmd,onCallback);
        }

        /// <summary>
        /// 执行 cmd
        /// </summary>
        public static void ExeCmd(string cmd,Action<string> onCallback=null,bool isWait = true)
        {
            Process p = new Process();
            try
            {
                
                //设置要启动的应用程序
                p.StartInfo.FileName = "cmd.exe";
                //是否使用操作系统shell启动
                p.StartInfo.UseShellExecute = false;
                // 接受来自调用程序的输入信息
                p.StartInfo.RedirectStandardInput = true;
                //输出信息
                //p.StartInfo.RedirectStandardOutput = true;
                // 输出错误
                //p.StartInfo.RedirectStandardError = true;
                //不显示程序窗口
                p.StartInfo.CreateNoWindow = false;
                //启动程序
                p.Start();

                p.StandardInput.WriteLine(cmd );
                p.StandardInput.AutoFlush = true;


                //string strOuput = p.StandardOutput.ReadToEnd();
                if (isWait)
                {
                    p.WaitForExit();
                    p.Close();
                }
               
                onCallback?.Invoke("指令执行完成！！");
            }
            catch (Exception e)
            {
                MessageBox.Show("错误："+e.Message);
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                onCallback?.Invoke("指令实行失败！！");
                p.CloseMainWindow();
            }
            
        }


        [DllImport("user32")]
        private static extern long ShowScrollBar(long hwnd, long wBar, long bShow);
        private static long SB_HORZ = 0;
        private static long SB_VERT = 1;
        private static long SB_BOTH = 3;

        public static void HideHorizontalScrollBar(ListView view)
        {
            ShowScrollBar(view.Handle.ToInt64(), SB_HORZ, 0);
        }



    }
}
