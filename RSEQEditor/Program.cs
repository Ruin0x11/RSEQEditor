using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RSEQEditor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(MainForm.Instance);
        }

        internal static ResourceNode _rootNode;
        public static ResourceNode RootNode { get { return _rootNode; } set { _rootNode = value; MainForm.Instance.Reset(); } }

        internal static string _rootPath;

        internal static bool Open(string path)
        {
            if((_rootNode = NodeFactory.FromFile(null, _rootPath = path)) != null) {
                MainForm.Instance.Reset();
                return true;
            }

            return false;
        }
    }
}
