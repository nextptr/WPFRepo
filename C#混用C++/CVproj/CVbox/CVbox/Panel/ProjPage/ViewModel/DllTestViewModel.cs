using CVbox.Common;

namespace CVbox.Panel.ProjPage.ViewModel
{
    public class DllTestViewModel : IPage
    {
        public string Name { get; set; } = "WPF与CPP调用测试";
        public void btnTest(string arg)
        {
            switch (arg)
            {
                case "1":
                    //CVAlgorithm.CVProxy.CVProxyInstance.Test();
                    break;
                case "2":
                    CVWrapper.CVproxy.CVproxyInstance.CVTest1(@"D:\GitWell\WPFRepo\C#混用C++\CVproj\CVbox\IMG\lena.jpg");
                    break;
                default:
                    break;
            }
        }

    }
}
