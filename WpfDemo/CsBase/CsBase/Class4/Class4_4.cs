using CsBase.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace CsBase.Class4
{
    #region codeStart
    //IEnumerator和IEnumerable接口
    public class ColorEnumerator:IEnumerator
    {
        string[] _colors;
        int _position = -1;
        public ColorEnumerator(string[] theColors)
        {
            _colors = new string[theColors.Length];
            for (int i = 0; i < theColors.Length; i++)
            {
                _colors[i] = theColors[i];
            }
        }

        public object Current
        {
            get
            {
                if (_position == -1)
                {
                    throw new InvalidOperationException();
                }
                if (_position >= _colors.Length)
                {
                    throw new InvalidOperationException();
                }
                return _colors[_position];
            }
        }
        public bool MoveNext()
        {
            if (_position < _colors.Length - 1)
            {
                _position++;
                return true;
            }
            else
            {
                return false;
            }
        }
        public void Reset()
        {
            _position = -1;
        }
    }
    public class Spectrum:IEnumerable
    {
        string[] Colos = { "violet", "blue", "cyan", "green", "yellow", "orange", "red" };
        public IEnumerator GetEnumerator()
        {
            return new ColorEnumerator(Colos);
        }
    }

    //迭代器作为属性
    public class SpectrumProperty
    {
        bool flag;
        string[] Colos = { "violet", "blue", "cyan", "green", "yellow", "orange", "red" };
        public SpectrumProperty(bool f)
        {
            flag = f;
        }
        public IEnumerator<string> GetEnumerator()
        {
            return flag ? UvtoIR : IRtoUv;
        }

        public IEnumerator<string> UvtoIR
        {
            get
            {
                for (int i = 0; i < Colos.Length; i++)
                {
                    yield return Colos[i];
                }
            }
        }
        public IEnumerator<string> IRtoUv
        {
            get
            {
                for (int i = Colos.Length-1; i >=0; i--)
                {
                    yield return Colos[i];
                }
            }
        }
    }
    //迭代器
    public class MyClass1
    {
        public IEnumerator<string> GetEnumerator()
        {
            return BalckAndWhite();
        }
        public IEnumerator<string> BalckAndWhite()
        {
            yield return "black";
            yield return "gray";
            yield return "white";
        }
    }

    public class MyClass2
    {
        public IEnumerator<string> GetEnumerator()
        {
            return BalckAndWhite();
        }
        public IEnumerator<string> BalckAndWhite()
        {
            String[] _colors = { "black", "gray", "white" };
            for (int i = 0; i < _colors.Length; i++)
            {
                yield return _colors[i];
            }
        }
    }

    public class MyClass3
    {
        //public IEnumerator<string> GetEnumerator()//使类可以枚举
        //{ 
        //    IEnumerable<string> myEnumerable = BalckAndWhite();
        //    return myEnumerable.GetEnumerator();
        //}
        public IEnumerable<string> BalckAndWhite() //使方法可以枚举
        {
            String[] _colors = { "black", "gray", "white" };
            for (int i = 0; i < _colors.Length; i++)
            {
                yield return _colors[i];
            }
        }
    }

    #endregion codeEnd
    public class Class4_4:classBase
    {
        #region codeStart
        public override void RunTest()
        {
            ddr("IEnumerator 迭代器接口实现");
            ddr("IEnumerable 可枚举接口实现");
            Spectrum spectrum = new Spectrum();
            foreach (string val in spectrum)
            {
                ddr($"foreach枚举 {val}");
            }
            ddr(" ");
            ddr("yield return 实现迭代器");
            MyClass1 ca1 = new MyClass1();
            MyClass2 ca2 = new MyClass2();
            foreach (string val in ca1)
            {
                ddr($"yield return 迭代器 {val}");
            }
            foreach (string val in ca2)
            {
                ddr($"循环 yield return 迭代器 {val}");
            }
            ddr(" ");
            ddr("可以使类可枚举，也可以使类不可枚举，使用类的迭代器方法枚举");
            MyClass3 ca3 = new MyClass3();
            foreach (string val in ca3.BalckAndWhite())
            {
                ddr($"使用迭代器方法枚举 {val}");
            }
            ddr(" ");
            ddr("迭代器作为属性");
            SpectrumProperty SppT = new SpectrumProperty(true);
            SpectrumProperty SppF = new SpectrumProperty(false);
            foreach (string val in SppT)
            {
                ddr($"正向迭代 {val}");
            }
            foreach (string val in SppF)
            {
                ddr($"逆向迭代 {val}");
            }
        }
        #endregion codeEnd
    }
}
