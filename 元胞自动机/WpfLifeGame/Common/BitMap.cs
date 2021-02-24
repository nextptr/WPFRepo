using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfLifeGame.Common
{
    public class BitMap
    {
        private int _length = 0;
        private byte[] ByteArry = null;

        public BitMap(int len)
        {
            _length = len;
            List<bool> ls = new List<bool>();
            for (int i = 0; i < len; i++)
            {
                ls.Add(false);
            }
            SetBitMap(ls);
        }
        public BitMap(List<bool> list)
        {
            SetBitMap(list);
        }
        public BitMap(int len, string str)
        {
            _length = len;
            //ascii字符串转byte数组
            ByteArry = Hex16StringToByteArry(str);
        }
        public bool this[int index]
        {
            get
            {
                if (index < 0 || index > _length)
                {
                    return false;
                }
                if ((ByteArry[index / 8] & (1 << (index % 8))) == (1 << (index % 8)))
                {
                    return true;
                }
                return false;
            }
        }

        public int Length
        {
            get
            {
                return _length;
            }
        }
        public string ToStr
        {
            get
            {
                if (_length == 0)
                {
                    return "";
                }
                else
                {
                    return ByteArryToHex16String(ByteArry);
                }
            }
        }
        public List<bool> ToList
        {
            get
            {
                if (_length == 0)
                {
                    return null;
                }
                else
                {
                    List<bool> ls = new List<bool>();
                    for (int i = 0; i < _length; i++)
                    {
                        ls.Add(this[i]);
                    }
                    return ls;
                }
            }
        }

        private string ByteArryToHex16String(byte[] arr)
        {
            StringBuilder info = new StringBuilder();
            int num = 0;
            int mod = 0;
            foreach (byte bt in arr)
            {
                num = (int)bt;
                //高4位
                mod = (num >> 4) & 15;
                info.AppendFormat("{0:x}", mod);
                //低4位
                mod = num & 15;
                info.AppendFormat("{0:x}", mod);
            }
            return info.ToString();
        }
        private byte[] Hex16StringToByteArry(string str)
        {
            string tmpstr = "";
            List<int> tmpint = new List<int>();
            for (int i = 0; i < str.Length; i+=2)
            {
                tmpstr = "";
                tmpstr = tmpstr + str[i] + str[i + 1];
                tmpint.Add((int)Convert.ToInt32(tmpstr, 16));
            }
            byte[] arry = new byte[tmpint.Count];
            for (int i = 0; i < tmpint.Count; i++)
            {
                arry[i] = (byte)tmpint[i];
            }
            return arry;
        }

        private string SetBitMap(List<bool> list)
        {
            List<bool> localList = new List<bool>();
            _length = list.Count;
            //补齐字节数
            foreach (var tmp in list)
            {
                localList.Add(tmp);
            }
            for (int i = 0; i < (8 - _length % 8); i++)
            {
                localList.Add(false);
            }
            //初始化字节数组
            ByteArry = new byte[localList.Count / 8];
            for (int i = 0; i < ByteArry.Length; i++)
            {
                ByteArry[i] = 0;
            }
            //字节数组赋值
            for (int i = 0; i < localList.Count;)
            {
                byte tmp = 0;
                for (int j = 0; j < 8; j++)
                {
                    if (localList[i + j])
                    {
                        tmp = (byte)(tmp | (1 << j));
                    }
                }
                ByteArry[i / 8] = tmp;
                i += 8;
            }
            //转换成ascii字符串
            return ByteArryToHex16String(ByteArry);
        }
        //private void GetBitMap(ref List<bool> ls)
        //{
        //    if (ByteArry == null)
        //    {
        //        ls.Clear();
        //    }
        //    else
        //    {
        //        for (int i = 0; i < ByteArry.Length - 1; i++)
        //        {
        //            byte tmp = ByteArry[i];
        //            for (int j = 0; j < 8; j++)
        //            {
        //                if ((tmp & (1 << j)) == (1 << j))
        //                {
        //                    ls.Add(true);
        //                }
        //                else
        //                {
        //                    ls.Add(false);
        //                }
        //            }
        //        }
        //        for (int i = 0; i < (_length - (ByteArry.Length - 1) * 8); i++)
        //        {
        //            if ((ByteArry[ByteArry.Length - 1] & (1 << i)) == (1 << i))
        //            {
        //                ls.Add(true);
        //            }
        //            else
        //            {
        //                ls.Add(false);
        //            }
        //        }
        //    }
        //}
    }
}
