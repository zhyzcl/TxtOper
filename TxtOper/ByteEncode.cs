using System;
using System.IO;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using NChardet;

namespace TxtOper
{
    /// <summary>文本窗显示委托</summary>
    /// <param name="myControl">文本对象</param>
    /// <param name="myCaption">需要插入的文本</param>
    /// <param name="isqc">是否清空文本对象true 是 false 否</param>
    /// <param name="blxx">基础文本</param>
    /// <returns>返回已被截取后的字符串</returns>
    public delegate void RTBDel(RichTextBox myControl, string myCaption, bool isqc, string blxx);

    /// <summary>字节流编码类型操作类</summary> 
    [Serializable]
    public class ByteEncode
    {
        #region 私有属性

        /// <summary>设置或获取是否检验ASCII false不检验 true检验</summary> 
        private bool _IsAscii = false;

        /// <summary>设置或获取最接近的编码之一</summary> 
        private string _EnStr = "";

        /// <summary>设置或获取已确认的编码</summary> 
        private string _EnAbsStr = "";

        /// <summary>设置或获取是否有多个可能的编码 0 没有 1 唯一  2 多个</summary> 
        private byte _IsEnArr = 0;

        /// <summary>默认语言 1 => Japanese 2 => Chinese 3 => Simplified Chinese 4 => Traditional Chinese 5 => Korean 6 => Dont know (default)</summary> 
        private byte _Lang = 2;

        #endregion

        #region 公共属性

        /// <summary>设置或获取所有可能的编码数组</summary> 
        public List<string> EnArr = new List<string>();

        /// <summary>设置或获取是否检验ASCII false不检验 true检验</summary> 
        [Description("设置或获取是否检验ASCII false不检验 true检验")]
        public bool IsAscii
        {
            get { return _IsAscii; }
            set { _IsAscii = value; }
        }

        /// <summary>设置或获取最接近的编码之一</summary> 
        [Description("设置或获取最接近的编码之一")]
        public string EnStr
        {
            get { return _EnStr; }
            set { _EnStr = value; }
        }

        /// <summary>设置或获取已确认的编码</summary> 
        [Description("设置或获取已确认的编码")]
        public string EnAbsStr
        {
            get { return _EnAbsStr; }
            set { _EnAbsStr = value; }
        }

        /// <summary>设置或获取是否有多个可能的编码 0 没有 1 唯一  2 多个</summary> 
        [Description("设置或获取是否有多个可能的编码 0 没有 1 唯一  2 多个")]
        public byte IsEnArr
        {
            get { return _IsEnArr; }
            set { _IsEnArr = value; }
        }

        /// <summary>默认语言 1 => Japanese 2 => Chinese 3 => Simplified Chinese 4 => Traditional Chinese 5 => Korean 6 => Dont know (default)</summary> 
        [Description("默认语言 1 => Japanese 2 => Chinese 3 => Simplified Chinese 4 => Traditional Chinese 5 => Korean 6 => Dont know (default)")]
        public byte Lang
        {
            get { return _Lang; }
            set { _Lang = value; }
        }

        /// <summary>获取或设置发生错误时返回的错误信息</summary>
        public Exception ExErr = null;

        /// <summary>获取或设置是否发生错误 值： false 没有错误  true 发生错误</summary>
        public bool ExBol = false;

        #endregion

        #region 获取字节流编码类型

        /// <summary>设置错误属性</summary>
        /// <param name="vals">Exception 错误对象</param>
        public virtual void SetErr(Exception vals)
        {
            if (vals == null)
            {
                ExErr = null;
                ExBol = false;
            }
            else
            {
                ExErr = vals;
                ExBol = true;
            }
        }

        /// <summary>已重载，返回Stream对象最接近的编码之一</summary>
        /// <param name="sr">一个Stream对象</param>
        /// <returns>返回Stream对象最接近的编码之一</returns>
        public virtual string GetByCode(Stream sr)
        {
            SetErr(null);
            try
            {
                EnArr.Clear();
                _EnStr = "";
                _EnAbsStr = "";
                _IsEnArr = 0;
                Detector det = new Detector(Lang);
                CDOClass cdo = new CDOClass();
                det.Init(cdo);
                byte[] buf = new byte[1024];
                bool done = false;
                int lent;
                while ((lent = sr.Read(buf, 0, buf.Length)) != 0)
                {
                    if (_IsAscii)
                    {
                        IsAscii = det.isAscii(buf, lent);
                        if (IsAscii) { break; }
                    }
                    if (_IsAscii == false && done == false)
                    {
                        done = det.DoIt(buf, lent, false);
                        if (done) { break; }
                    }
                }
                sr.Close();
                det.DataEnd();
                if (_IsAscii)
                {
                    _EnAbsStr = "ASCII";
                    _EnStr = EnAbsStr;
                    _IsEnArr = 1;
                }
                else if (cdo.Charset != null)
                {
                    _EnAbsStr = cdo.Charset.ToString();
                    _EnStr = EnAbsStr;
                    _IsEnArr = 1;
                }
                if (_IsEnArr == 0)
                {
                    EnArr = new List<string>(det.getProbableCharsets());
                    if (EnArr.Count > 0)
                    {
                        _EnStr = EnArr[0].ToString();
                        if (EnArr.Count > 1)
                        { _IsEnArr = 2; }
                        else
                        { _IsEnArr = 1; }
                    }
                }
                return _EnStr;
            }
            catch (Exception ex)
            {
                SetErr(ex);
                return "";
            }
        }

        /// <summary>已重载，返回MemoryStream对象最接近的编码之一</summary>
        /// <param name="sr">一个MemoryStream对象</param>
        /// <returns>返回MemoryStream对象最接近的编码之一</returns>
        public virtual string GetByCode(MemoryStream sr)
        {
            SetErr(null);
            try
            {
                EnArr.Clear();
                _EnStr = "";
                _EnAbsStr = "";
                _IsEnArr = 0;
                Detector det = new Detector(Lang);
                CDOClass cdo = new CDOClass();
                det.Init(cdo);
                byte[] buf = new byte[1024];
                bool done = false;
                int lent;
                while ((lent = sr.Read(buf, 0, buf.Length)) != 0)
                {
                    if (_IsAscii)
                    {
                        IsAscii = det.isAscii(buf, lent);
                        if (IsAscii) { break; }
                    }
                    if (_IsAscii == false && done == false)
                    {
                        done = det.DoIt(buf, lent, false);
                        if (done) { break; }
                    }
                }
                sr.Close();
                det.DataEnd();
                if (_IsAscii)
                {
                    _EnAbsStr = "ASCII";
                    _EnStr = EnAbsStr;
                    _IsEnArr = 1;
                }
                else if (cdo.Charset != null)
                {
                    _EnAbsStr = cdo.Charset.ToString();
                    _EnStr = EnAbsStr;
                    _IsEnArr = 1;
                }
                if (_IsEnArr == 0)
                {
                    EnArr = new List<string>(det.getProbableCharsets());
                    if (EnArr.Count > 0)
                    {
                        _EnStr = EnArr[0].ToString();
                        if (EnArr.Count > 1)
                        { _IsEnArr = 2; }
                        else
                        { _IsEnArr = 1; }
                    }
                }
                return _EnStr;
            }
            catch (Exception ex)
            {
                SetErr(ex);
                return "";
            }
        }

        /// <summary>已重载，返回FileStream对象最接近的编码之一</summary>
        /// <param name="sr">一个FileStream对象</param>
        /// <returns>返回FileStream对象最接近的编码之一</returns>
        public virtual string GetByCode(FileStream sr)
        {
            SetErr(null);
            try
            {
                EnArr.Clear();
                _EnStr = "";
                _EnAbsStr = "";
                _IsEnArr = 0;
                Detector det = new Detector(Lang);
                CDOClass cdo = new CDOClass();
                det.Init(cdo);
                byte[] buf = new byte[1024];
                bool done = false;
                int lent;
                while ((lent = sr.Read(buf, 0, buf.Length)) != 0)
                {
                    if (_IsAscii)
                    {
                        IsAscii = det.isAscii(buf, lent);
                        if (IsAscii) { break; }
                    }
                    if (_IsAscii == false && done == false)
                    {
                        done = det.DoIt(buf, lent, false);
                        if (done) { break; }
                    }
                }
                sr.Close();
                det.DataEnd();
                if (_IsAscii)
                {
                    _EnAbsStr = "ASCII";
                    _EnStr = EnAbsStr;
                    _IsEnArr = 1;
                }
                else if (cdo.Charset != null)
                {
                    _EnAbsStr = cdo.Charset.ToString();
                    _EnStr = EnAbsStr;
                    _IsEnArr = 1;
                }
                if (_IsEnArr == 0)
                {
                    EnArr = new List<string>(det.getProbableCharsets());
                    if (EnArr.Count > 0)
                    {
                        _EnStr = EnArr[0].ToString();
                        if (EnArr.Count > 1)
                        { _IsEnArr = 2; }
                        else
                        { _IsEnArr = 1; }
                    }
                }
                return _EnStr;
            }
            catch (Exception ex)
            {
                SetErr(ex);
                return "";
            }
        }

        /// <summary>已重载，返回字节数组对象最接近的编码之一</summary>
        /// <param name="sr">一个字节数组对象</param>
        /// <returns>返回字节数组对象最接近的编码之一</returns>
        public virtual string GetByCode(byte[] sr)
        {
            SetErr(null);
            try
            {
                EnArr.Clear();
                _EnStr = "";
                _EnAbsStr = "";
                _IsEnArr = 0;
                Detector det = new Detector(Lang);
                CDOClass cdo = new CDOClass();
                det.Init(cdo);
                byte[] buf = new byte[1024];
                bool done = false;
                int lent = 0;
                int leni = 0;
                while (lent < sr.Length)
                {
                    leni = sr.Length - lent;
                    if (leni > 1024)
                    {
                        Array.Copy(sr, lent, buf, 0, 1024);
                        leni = 1024;
                    }
                    else
                    {
                        Array.Copy(sr, lent, buf, 0, leni);
                    }
                    lent += 1024;
                    if (_IsAscii)
                    {
                        _IsAscii = det.isAscii(buf, leni);
                        if (IsAscii) { break; }
                    }
                    if (_IsAscii == false && done == false)
                    {
                        done = det.DoIt(buf, leni, false);
                        if (done) { break; }
                    }
                }
                det.DataEnd();
                if (_IsAscii)
                {
                    _EnAbsStr = "ASCII";
                    _EnStr = EnAbsStr;
                    _IsEnArr = 1;
                }
                else if (cdo.Charset != null)
                {
                    _EnAbsStr = cdo.Charset.ToString();
                    _EnStr = EnAbsStr;
                    _IsEnArr = 1;
                }
                if (_IsEnArr == 0)
                {
                    EnArr = new List<string>(det.getProbableCharsets());
                    if (EnArr.Count > 0)
                    {
                        _EnStr = EnArr[0].ToString();
                        if (EnArr.Count > 1)
                        { _IsEnArr = 2; }
                        else
                        { _IsEnArr = 1; }
                    }
                }
                return _EnStr;
            }
            catch (Exception ex)
            {
                SetErr(ex);
                return "";
            }
        }

        #endregion
    }

    /// <summary>CharsetDetectionObserver接口实例类</summary>
    class CDOClass : NChardet.ICharsetDetectionObserver
    {
        #region CharsetDetectionObserver接口

        /// <summary>需要检测编码的字符串</summary> 
        public string Charset = null;

        /// <summary>接口实例方法</summary> 
        /// <param name="charset">需要检测编码的字符串</param>
        public void Notify(string charset)
        {
            Charset = charset;
        }

        #endregion
    }
}
