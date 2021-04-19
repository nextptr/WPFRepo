using CsBase.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CsBase.Class4
{
    class Class4_6 : classBase
    {
        #region codeStart
        public override void RunTest()
        {
            RegexTest("只能输入数字", @"^[0-9]*$", "123xs", "48566");
            RegexTest("只能输入N位数字", @"^\d{3}$", "12", "123", "1234", "123f");
            RegexTest("至少输入N位数字", @"^\d{3,}$", "12", "123","1234", "123f");
            RegexTest("M~N位数字输入", @"^\d{3,4}$", "12", "123", "1234", "12345");
            RegexTest("0~3位数字输入", @"^\d{0,3}$", "12", "123", "1234", "12345");
            RegexTest("只能有两位小数的数字", @"^[0-9]+(.[0-9]{2})?$", "12.3", "12.34", "12.345");
            RegexTest("只能有2~3位小数的数字", @"^[0-9]+(.[0-9]{2,3})?$", "12.3", "12.34", "12.345");
            RegexTest("只能输入非零正整数", @"^\+?[1-9][0-9]*$", "012", "123", "123d");
            RegexTest("只能输入非零负整数", @"^\-[1-9][0-9]*$", "12", "-123");
            RegexTest("只能输入长度为3的字符", @"^.{3}$",",xs", "1s2", "fae5");
            RegexTest("只能输入字母字符串", @"^[A-Za-z]+$",",xs", "1s2", "afsdAFF");
            RegexTest("匹配指定ip", @"127.0.0.1","127.0.0.1", "1127.0.0.1", "192.168.250.250");
            RegexTest("不匹配数字", @"[^0-9.-]+","123","0.123","1.23","12.3","-23","xsa");

            RegexTest("匹配数字1~200的数字", @"^\+?([1-9]{1,2}$)","-1","1","1234","12.3","xsa");
            RegexTest("匹配数字1~200的数字", @"^\+?(1\d{2}$)", "-1","1","1234","12.3","xsa");
            RegexTest("匹配数字1~200的数字", @"^\+?(200)", "-1","1","1234","12.3","xsa");
            RegexTest("匹配数字1~200的数字", @"^\+?([1-9]\d?|1\d{2}|200)", "-1","1","1234","12.3","xsa");
            RegexTest("匹配数字1~200的数字", @"^\+?([1-9]\d?$|1\d{2}$|200)", "-1","1","1234","12.3","12","xsa");

            RegexTest("波长", "PWC:([^\r\n]+)\r\n", "PWC: 1064\r\n");
            RegexTest("波长", @"^PWC:([0-9])*(\r\n)$", "PWC: 1064\r\n");
        }

        //可变参数列表函数 params关键字
        public void RegexTest(string testName, string reg, params string []test)
        {
            List<string> rets = new List<string>();
            string val = "";
            foreach (string ts in test)
            {
                val= Regex.Match(ts, reg).Value;
                if (val == "")
                {
                    val = "Nul";
                }
                rets.Add(val);
            }
            val = null;
            for (int i = 0; i < test.Length; i++)
            {
                val += $"({test[i]}={rets[i]})";
            }
            ddr($"{testName}: ({reg})匹配{val}");
        }
        public void IsRegexTest(string testName, string reg, params string[] test)
        {
            List<string> rets = new List<string>();
            string val = "";
            foreach (string ts in test)
            {
                val = Regex.IsMatch(ts, reg, RegexOptions.RightToLeft).ToString();
                if (val == "")
                {
                    val = "Nul";
                }
                rets.Add(val);
            }
            val = null;
            for (int i = 0; i < test.Length; i++)
            {
                val += $"({test[i]}={rets[i]})";
            }
            ddr($"{testName}: ({reg})匹配{val}");
        }
        #endregion codeEnd
    }
}
