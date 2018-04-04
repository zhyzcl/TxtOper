using System;
using System.IO;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using NChardet;

namespace TxtOper
{
    /// <summary>�ı�����ʾί��</summary>
    /// <param name="myControl">�ı�����</param>
    /// <param name="myCaption">��Ҫ������ı�</param>
    /// <param name="isqc">�Ƿ�����ı�����true �� false ��</param>
    /// <param name="blxx">�����ı�</param>
    /// <returns>�����ѱ���ȡ����ַ���</returns>
    public delegate void RTBDel(RichTextBox myControl, string myCaption, bool isqc, string blxx);

    /// <summary>�ֽ����������Ͳ�����</summary> 
    [Serializable]
    public class ByteEncode
    {
        #region ˽������

        /// <summary>���û��ȡ�Ƿ����ASCII false������ true����</summary> 
        private bool _IsAscii = false;

        /// <summary>���û��ȡ��ӽ��ı���֮һ</summary> 
        private string _EnStr = "";

        /// <summary>���û��ȡ��ȷ�ϵı���</summary> 
        private string _EnAbsStr = "";

        /// <summary>���û��ȡ�Ƿ��ж�����ܵı��� 0 û�� 1 Ψһ  2 ���</summary> 
        private byte _IsEnArr = 0;

        /// <summary>Ĭ������ 1 => Japanese 2 => Chinese 3 => Simplified Chinese 4 => Traditional Chinese 5 => Korean 6 => Dont know (default)</summary> 
        private byte _Lang = 2;

        #endregion

        #region ��������

        /// <summary>���û��ȡ���п��ܵı�������</summary> 
        public List<string> EnArr = new List<string>();

        /// <summary>���û��ȡ�Ƿ����ASCII false������ true����</summary> 
        [Description("���û��ȡ�Ƿ����ASCII false������ true����")]
        public bool IsAscii
        {
            get { return _IsAscii; }
            set { _IsAscii = value; }
        }

        /// <summary>���û��ȡ��ӽ��ı���֮һ</summary> 
        [Description("���û��ȡ��ӽ��ı���֮һ")]
        public string EnStr
        {
            get { return _EnStr; }
            set { _EnStr = value; }
        }

        /// <summary>���û��ȡ��ȷ�ϵı���</summary> 
        [Description("���û��ȡ��ȷ�ϵı���")]
        public string EnAbsStr
        {
            get { return _EnAbsStr; }
            set { _EnAbsStr = value; }
        }

        /// <summary>���û��ȡ�Ƿ��ж�����ܵı��� 0 û�� 1 Ψһ  2 ���</summary> 
        [Description("���û��ȡ�Ƿ��ж�����ܵı��� 0 û�� 1 Ψһ  2 ���")]
        public byte IsEnArr
        {
            get { return _IsEnArr; }
            set { _IsEnArr = value; }
        }

        /// <summary>Ĭ������ 1 => Japanese 2 => Chinese 3 => Simplified Chinese 4 => Traditional Chinese 5 => Korean 6 => Dont know (default)</summary> 
        [Description("Ĭ������ 1 => Japanese 2 => Chinese 3 => Simplified Chinese 4 => Traditional Chinese 5 => Korean 6 => Dont know (default)")]
        public byte Lang
        {
            get { return _Lang; }
            set { _Lang = value; }
        }

        /// <summary>��ȡ�����÷�������ʱ���صĴ�����Ϣ</summary>
        public Exception ExErr = null;

        /// <summary>��ȡ�������Ƿ������� ֵ�� false û�д���  true ��������</summary>
        public bool ExBol = false;

        #endregion

        #region ��ȡ�ֽ�����������

        /// <summary>���ô�������</summary>
        /// <param name="vals">Exception �������</param>
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

        /// <summary>�����أ�����Stream������ӽ��ı���֮һ</summary>
        /// <param name="sr">һ��Stream����</param>
        /// <returns>����Stream������ӽ��ı���֮һ</returns>
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

        /// <summary>�����أ�����MemoryStream������ӽ��ı���֮һ</summary>
        /// <param name="sr">һ��MemoryStream����</param>
        /// <returns>����MemoryStream������ӽ��ı���֮һ</returns>
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

        /// <summary>�����أ�����FileStream������ӽ��ı���֮һ</summary>
        /// <param name="sr">һ��FileStream����</param>
        /// <returns>����FileStream������ӽ��ı���֮һ</returns>
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

        /// <summary>�����أ������ֽ����������ӽ��ı���֮һ</summary>
        /// <param name="sr">һ���ֽ��������</param>
        /// <returns>�����ֽ����������ӽ��ı���֮һ</returns>
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

    /// <summary>CharsetDetectionObserver�ӿ�ʵ����</summary>
    class CDOClass : NChardet.ICharsetDetectionObserver
    {
        #region CharsetDetectionObserver�ӿ�

        /// <summary>��Ҫ��������ַ���</summary> 
        public string Charset = null;

        /// <summary>�ӿ�ʵ������</summary> 
        /// <param name="charset">��Ҫ��������ַ���</param>
        public void Notify(string charset)
        {
            Charset = charset;
        }

        #endregion
    }
}
