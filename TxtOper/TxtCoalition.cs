using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;

namespace TxtOper
{
    public partial class TxtCoalition : Form
    {
        public string DirPath = "";
        public string TitleStr = "";
        public string FilterStr = "";
        public string FileFilters = "";
        public ByteEncode be = new ByteEncode();

        public int czxh = 1;
        public long inCount = 0;
        public DateTime OperDate = DateTime.Now;
        public long hs = 0;
        public long rtxdqhs = 0;//监视器当前行数
        public long rtxhs = 1000;//监视器最大行数
        public StringBuilder savehs = new StringBuilder();//保留监视信息

        public TxtCoalition()
        {
            InitializeComponent();
        }

        private void btyl_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = System.Environment.CurrentDirectory;
            if(fbd.ShowDialog()==DialogResult.OK)
            {
                tBdir.Text = fbd.SelectedPath;
            }
        }

        private void btcz_Click(object sender, EventArgs e)
        {
            if (bkWk.IsBusy)
            {
                MessShow("操作正在进行中！",1);
                return;
            }
            DirPath = tBdir.Text;
            FileFilters = tBsp.Text;
            start();
        }

        private void start()
        {
            tBdir.Enabled = false;
            tBsp.Enabled = false;
            rTBFilter.Enabled = false;
            btcz.Enabled = false;
            savehs = new StringBuilder();
            richText.Text = "";
            FilterStr = rTBFilter.Text.Trim();
            TitleStr = rTBTitle.Text.Trim();
            bkWk.RunWorkerAsync();
        }

        private void Rest()
        {
            tBdir.Enabled = true;
            tBsp.Enabled = true;
            rTBFilter.Enabled = true;
            btcz.Enabled = true;
            savehs = new StringBuilder();
            rtxdqhs = 0;
            czxh = 1;
            inCount = 0;
            OperDate = DateTime.Now;
            hs = 0;
        }

        public void czfiles() 
        {
            string outxx = "";
            int cgyes = 0;
            string[] subPaths = Directory.GetDirectories(DirPath);
            OT("[" + czxh.ToString() + "] >>> 参数读取完毕，共计搜索到[" + subPaths.Length.ToString() + "]个需要操作的文件目录。操作开始！ 操作时间： " + DateTime.Now.ToString() + "\r\n");
            for (int ii = 0; ii < subPaths.Length;ii++)
            {
                string dirname = "";
                string spath = subPaths[ii];
                string[] darr = spath.Split('\\');
                dirname = darr[darr.Length - 1].Trim() + ".txt";
                DirectoryInfo pdir = new DirectoryInfo(spath);
                FileInfo[] finfos = pdir.GetFiles(FileFilters);
                Array.Sort(finfos, new MyDateSorter());
                StringBuilder csb = new StringBuilder();
                for (int i = 0; i < finfos.Length; i++)
                {
                    FileInfo fi = finfos[i];
                    string fstr = fi.FullName.Trim();
                    if (fstr != "")
                    {
                        try
                        {
                            FileStream fs = File.Open(fstr, FileMode.Open);
                            Encoding en = Encoding.Default;
                            string enst = be.GetByCode(fs);
                            string ens = "";
                            if (enst == "Big5" || enst == "EUC-JP")
                            {
                                ens = "gb2312";
                            }
                            else
                            {
                                ens = enst;
                            }
                            if (ens.Trim() != "")
                            {
                                en = Encoding.GetEncoding(ens);
                            }
                            fs.Close();
                            string fpath = Path.GetDirectoryName(fstr);
                            string fname = Path.GetFileName(fstr);
                            string[] fexta = fname.Split('.');
                            string fext = "";
                            if (fexta.Length > 1)
                            {
                                fext = "." + fexta[fexta.Length - 1];
                            }
                            string fileName = fname;
                            if (fext != "")
                            {
                                fileName = fileName.Remove(fileName.Length - fext.Length);
                            }
                            fileName = FilterText(fileName);
                            fileName = fileName + fext;
                            string lstr = File.ReadAllText(fstr, en);
                            string lstrv = FilterHtml(lstr);
                            csb.Append(lstrv + "\r\n");
                            File.Delete(fstr);
                            OT("[" + czxh.ToString() + "] >>> 操作文件路径：[" + fstr + "]、文件名：[" + fname + "]、字节数：[" + lstr.Length.ToString() + "]，合并操作完成。操作时间： " + DateTime.Now.ToString() + "\r\n");
                            cgyes++;
                        }
                        catch (Exception e)
                        {
                            OT("[" + czxh.ToString() + "] >>> 文件：[" + fstr + "]操作发生错误！错误信息：[" + e.Message + "]。操作时间： " + DateTime.Now.ToString() + "\r\n");
                        }
                    }
                }
                string str = csb.ToString();
                string newpath = spath + "\\" + dirname;
                FileStream fc = File.Create(newpath);
                byte[] bContent = Encoding.Default.GetBytes(str);
                fc.Write(bContent, 0, bContent.Length);
                fc.Close();
                OT("[" + czxh.ToString() + "] >>> 保存文件路径：[" + newpath + "]、文件名：[" + dirname + "]、字节数：[" + str.Length.ToString() + "]，保存操作成功。操作时间： " + DateTime.Now.ToString() + "\r\n");
            }
            OT("[" + czxh.ToString() + "] >>> 操作完毕，共计操作成功[" + cgyes.ToString() + "]个文件。 操作时间： " + DateTime.Now.ToString() + "\r\n");
        }

        //过滤标题输出存文本
        public string FilterText(string str)
        {
            StringBuilder sb = new StringBuilder(GetStrNarrow(str));
            sb.Replace(" ", "");
            sb.Replace("　", "");
            sb.Replace("", "");
            sb.Replace("<", "");
            sb.Replace(">", "");
            sb.Replace(",", "");
            sb.Replace(".", "");
            sb.Replace("@", "");
            sb.Replace("#", "");
            sb.Replace("$", "");
            sb.Replace("%", "");
            sb.Replace("^", "");
            sb.Replace("&", "");
            sb.Replace("*", "");
            sb.Replace("/", "");
            sb.Replace("\\", "");
            sb.Replace("\"", "");
            sb.Replace("'", "");
            sb.Replace(";", "");
            sb.Replace(":", "");
            sb.Replace("(", "");
            sb.Replace(")", "");
            sb.Replace("[", "");
            sb.Replace("]", "");
            sb.Replace("{", "");
            sb.Replace("}", "");
            sb.Replace("+", "");
            sb.Replace("=", "");
            sb.Replace("!", "");
            sb.Replace("`", "");
            sb.Replace("~", "");
            if (TitleStr != "")
            {
                string[] sR = Split(TitleStr, "\n");
                for (int n = 0; n < sR.Length; n++)
                {
                    string rs = sR[n].Trim();
                    if (rs != "")
                    {
                        sb.Replace(rs, "");
                    }
                }
            }
            return sb.ToString();
        }

        //过滤内容输出纯文本
        public string FilterHtml(string str)
        {
            StringBuilder sb = new StringBuilder(str);
            sb.Replace("\n", "");
            sb.Replace(((Char)(160)).ToString(), "");
            sb.Replace("—", "");
            sb.Replace("*", "");
            sb.Replace("＊", "");
            sb.Replace("=", "");
            sb.Replace("-", "");
            sb.Replace("&raquo;", "");
            sb.Replace("&copy;", "");
            sb.Replace("&lt;", "");
            sb.Replace("&gt;", "");
            sb.Replace("&amp;", "");
            sb.Replace("&apos;", "");
            sb.Replace("&quot;", "");
            sb.Replace("&#13;", "");
            sb.Replace("&#10;", "");
            sb.Replace("&nbsp;", "");
            sb.Replace("&reg;", "");
            sb.Replace("&trade;", "");
            sb.Replace("&emsp;", "");
            sb.Replace("&ensp;", "");
            sb.Replace("※", "");
            sb.Replace("\t", "");
            sb.Replace("\0", "");
            sb.Replace("\a", "");
            sb.Replace("\b", "");
            sb.Replace("\f", "");
            sb.Replace("\v", "");
            sb.Replace("　", " ");
            while (sb.ToString().IndexOf("\r\r") > -1)
            {
                sb.Replace("\r\r", "\r");
            }
            sb.Replace("\r", "\r\n");
            if (FilterStr != "")
            {
                string[] sR = Split(FilterStr, "\n");
                for (int n = 0; n < sR.Length; n++)
                {
                    string rs = sR[n].Trim();
                    if (rs != "")
                    {
                        sb.Replace(rs, "");
                    }
                }
            }
            while (sb.ToString().IndexOf("  ") > -1)
            {
                sb.Replace("  ", " ");
            }
            return sb.ToString();
        }

        /// <summary>返回由指定String分隔的String类型一维数组。</summary>
        /// <param name="str">包含子字符串和分隔符的 String 表达式</param>
        /// <param name="spStr">分隔此实例中子字符串的Unicode字符串、不包含分隔符的空字符串或空引用</param>
        /// <returns>返回String类型一维数组</returns>
        public string[] Split(string str, string spStr)
        {
            return Regex.Split(str, GetRegStr(spStr));
        }

        /// <summary>返回字符串的正则表达式格式</summary>
        /// <param name="inStr">输入的字符串</param>
        /// <returns>返回字符串的正则表达式格式</returns>
        public string GetRegStr(string inStr)
        {
            inStr = inStr.Replace("\\", "\\\\");
            inStr = inStr.Replace(".", "\\.");
            inStr = inStr.Replace("$", "\\$");
            inStr = inStr.Replace("^", "\\^");
            inStr = inStr.Replace("{", "\\{");
            inStr = inStr.Replace("[", "\\[");
            inStr = inStr.Replace("(", "\\(");
            inStr = inStr.Replace("|", "\\|");
            inStr = inStr.Replace(")", "\\)");
            inStr = inStr.Replace("*", "\\*");
            inStr = inStr.Replace("+", "\\+");
            inStr = inStr.Replace("?", "\\?");
            return inStr;
        }

        /// <summary>获取指定目录下所有文件路径及文件夹路径，分别返回到files及paths</summary>
        /// <param name="path">指定目录(物理路径)</param>
        /// <param name="sPattern">要与path中的文件名匹配的搜索字符串</param>
        /// <param name="dirlv">子目录最大深度</param>
        /// <param name="files">返回所有文件路径列表</param>
        /// <param name="paths">返回所有空目录路径列表</param>
        /// <param name="lv">当前目录深度</param>
        private void pGetAllDirFiles(string path, string sPattern, int dirlv, ref List<string> files, ref List<string> paths, ref int lv)
        {
            lv++;
            if (lv > dirlv)
            {
                return;
            }
            string[] subPaths = Directory.GetDirectories(path);
            foreach (string ph in subPaths)
            {
                pGetAllDirFiles(ph, sPattern, dirlv, ref files, ref paths, ref lv);
            }
            string[] fs = Directory.GetFiles(path, sPattern);
            DirectoryInfo pdir = new DirectoryInfo(path);
            FileInfo[] finfos = pdir.GetFiles(sPattern);
            Array.Sort(finfos, new MyDateSorter());
            for (int i = 0; i < finfos.Length; i++ ) 
            {
                FileInfo fi = finfos[i];
                string fstr = fi.FullName.Replace("/", "\\");
                if (files.IndexOf(fstr) == -1)
                {
                    files.Add(fstr);
                }
            }
            if (subPaths.Length == fs.Length && fs.Length == 0)
            {
                string pstr = path.Replace("/", "\\");
                if (paths.IndexOf(pstr) == -1)
                {
                    paths.Add(pstr);
                }
            }
        }

        /// <summary>WinForm弹出信息提示框</summary>
        /// <param name="MesStr">提示语句</param>
        /// <returns></returns>
        public void MessShow(string MesStr)
        {
            MessShow(MesStr, 0);
        }

        /// <summary>WinForm弹出提示框</summary>
        /// <param name="MesStr">提示语句</param>
        /// <param name="infoicon">左侧图标，成功为0 失败为1 提示输入格式不符之类 用2，0为信息、1为错误、2为警告</param>
        /// <returns></returns>
        public void MessShow(string MesStr, int infoicon)
        {
            switch (infoicon)
            {
                //图标为信息
                case 0:
                    MessageBox.Show("\r\n" + MesStr + "    \r\n  ", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                //图标为错误
                case 1:
                    MessageBox.Show("\r\n" + MesStr + "    \r\n  ", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                //图标为警告
                case 2:
                    MessageBox.Show("\r\n" + MesStr + "    \r\n  ", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
            }
        }

        /// <summary>将输入字符串中的全角数字转换成半角数字并返回</summary>
        /// <param name="inStr">输入字符串</param>
        /// <returns>将输入字符串中的全角数字转换成半角数字并返回</returns>
        public static string GetNumNarrow(string inStr)
        {
            StringBuilder sb = new StringBuilder(inStr);
            sb.Replace("１", "1");
            sb.Replace("２", "2");
            sb.Replace("３", "3");
            sb.Replace("４", "4");
            sb.Replace("５", "5");
            sb.Replace("６", "6");
            sb.Replace("７", "7");
            sb.Replace("８", "8");
            sb.Replace("９", "9");
            sb.Replace("０", "0");
            sb.Replace("－", "-");
            sb.Replace("＝", "=");
            sb.Replace("＋", "+");
            sb.Replace("，", ",");
            sb.Replace("。", ".");
            sb.Replace("；", ";");
            sb.Replace("：", ":");
            sb.Replace("‘", "'");
            sb.Replace("“", "\"");
            sb.Replace("”", "\"");
            sb.Replace("’", "'");
            sb.Replace("（", "(");
            sb.Replace("）", ")");
            sb.Replace("＊", "*");
            sb.Replace("％", "%");
            sb.Replace("￥", "$");
            sb.Replace("＃", "#");
            sb.Replace("！", "!");
            sb.Replace("～", "~");
            sb.Replace("、", "|");
            sb.Replace("？", "?");
            sb.Replace("《", "<");
            sb.Replace("》", ">");
            return sb.ToString();
        }

        /// <summary>将输入字符串中的全角数字及英文转换成半角数字及英文并返回</summary>
        /// <param name="inStr">输入字符串</param>
        /// <returns>将输入字符串中的全角数字及英文转换成半角数字及英文并返回</returns>
        public static string GetStrNarrow(string inStr)
        {
            StringBuilder sb = new StringBuilder(GetNumNarrow(inStr));
            sb.Replace("ａ", "a");
            sb.Replace("ｂ", "b");
            sb.Replace("ｃ", "c");
            sb.Replace("ｄ", "d");
            sb.Replace("ｅ", "e");
            sb.Replace("ｆ", "f");
            sb.Replace("ｇ", "g");
            sb.Replace("ｈ", "h");
            sb.Replace("ｉ", "i");
            sb.Replace("ｊ", "j");
            sb.Replace("ｋ", "k");
            sb.Replace("ｌ", "l");
            sb.Replace("ｍ", "m");
            sb.Replace("ｎ", "n");
            sb.Replace("ｏ", "o");
            sb.Replace("ｐ", "p");
            sb.Replace("ｑ", "q");
            sb.Replace("ｒ", "r");
            sb.Replace("ｓ", "s");
            sb.Replace("ｔ", "t");
            sb.Replace("ｕ", "u");
            sb.Replace("ｖ", "v");
            sb.Replace("ｗ", "w");
            sb.Replace("ｘ", "x");
            sb.Replace("ｙ", "y");
            sb.Replace("ｚ", "z");
            sb.Replace("Ａ", "A");
            sb.Replace("Ｂ", "B");
            sb.Replace("Ｃ", "C");
            sb.Replace("Ｄ", "D");
            sb.Replace("Ｅ", "E");
            sb.Replace("Ｆ", "F");
            sb.Replace("Ｇ", "G");
            sb.Replace("Ｈ", "H");
            sb.Replace("Ｉ", "I");
            sb.Replace("Ｊ", "J");
            sb.Replace("Ｋ", "K");
            sb.Replace("Ｌ", "L");
            sb.Replace("Ｍ", "M");
            sb.Replace("Ｎ", "N");
            sb.Replace("Ｏ", "O");
            sb.Replace("Ｐ", "P");
            sb.Replace("Ｑ", "Q");
            sb.Replace("Ｒ", "R");
            sb.Replace("Ｓ", "S");
            sb.Replace("Ｔ", "T");
            sb.Replace("Ｕ", "U");
            sb.Replace("Ｖ", "V");
            sb.Replace("Ｗ", "W");
            sb.Replace("Ｘ", "X");
            sb.Replace("Ｙ", "Y");
            sb.Replace("Ｚ", "Z");
            return sb.ToString();
        }

        /// <summary>文本窗显示委托方法</summary>
        /// <param name="myControl">文本对象</param>
        /// <param name="myCaption">需要插入的文本</param>
        /// <param name="isqc">是否清空文本对象true 是 false 否</param>
        /// <param name="blxx">基础文本</param>
        /// <returns>返回已被截取后的字符串</returns>
        public static void RTB(RichTextBox myControl, string myCaption, bool isqc, string blxx)
        {
            if (isqc)
            {
                myControl.Text = "";
                myControl.AppendText(blxx);
            }
            myControl.AppendText(myCaption);
            myControl.ScrollToCaret();
        }

        /// <summary>输出信息</summary>
        /// <param name="Str">信息</param>
        /// <returns></returns>
        public void OT(string Str)
        {
            bool isqc = false;
            if (rtxdqhs > rtxhs)
            {
                isqc = true;
                rtxdqhs = 0;
            }
            OutOperDate();
            object[] myArray = new object[4];
            myArray[0] = richText;
            myArray[1] = Str;
            myArray[2] = isqc;
            myArray[3] = savehs.ToString();
            try
            {
                richText.BeginInvoke(new RTBDel(RTB), myArray);
            }
            catch
            {
            }
            czxh += 1;
            rtxdqhs += 1;
        }

        /// <summary>设置当前日期并获取与上次操作时间间隔</summary>
        /// <returns></returns>
        public void OutOperDate()
        {
            DateTime rq = DateTime.Now;
            TimeSpan df = rq - OperDate;
            OperDate = rq;
            hs = Convert.ToInt64(df.TotalSeconds);
        }

        private void bkWk_DoWork(object sender, DoWorkEventArgs e)
        {
            czfiles();
        }

        private void bkWk_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Rest();
        }
    }

    public class MyDateSorter : IComparer
    {
        public int Compare(object x, object y)
        {
            if (x == null && y == null)
            {
                return 0;
            }
            if (x == null)
            {
                return -1;
            }
            if (y == null)
            {
                return 1;
            }
            FileInfo xInfo = (FileInfo)x;
            FileInfo yInfo = (FileInfo)y;
            //依名稱排序    
            return xInfo.FullName.CompareTo(yInfo.FullName);//递增    
            //return yInfo.FullName.CompareTo(xInfo.FullName);//递減    
            //依修改日期排序    
            //return xInfo.LastWriteTime.CompareTo(yInfo.LastWriteTime);//递增    
            //return yInfo.LastWriteTime.CompareTo(xInfo.LastWriteTime);//递減    
        }
    }
}
