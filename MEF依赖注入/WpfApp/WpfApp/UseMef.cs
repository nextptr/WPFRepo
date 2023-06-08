using MEFCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    public class UseMef
    {
        [ImportMany]
        public List<IMefBase> MefLs { get; set; }

        private static readonly object LockSync = new object();

        private static UseMef _instance = null;

        public static UseMef Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (LockSync)
                    {
                        if (_instance==null)
                        {
                            _instance = new UseMef();
                            _instance.Initialize();
                        }
                    }
                }
                return _instance;
            }
        }

        private void Initialize()
        {
            var aggregateCatalog = new AggregateCatalog();
            aggregateCatalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));

            //DirectoryInfo dirs = new DirectoryInfo("ImportDlls");
            DirectoryInfo dirs = new DirectoryInfo("ImportDlls");

            foreach (var ddd in dirs.GetDirectories())
            {
                aggregateCatalog.Catalogs.Add(new DirectoryCatalog(ddd.FullName, "MEFTestDll.dll"));
            }

            var container = new CompositionContainer(aggregateCatalog);
            container.ComposeParts(this);
        }
    }
}
