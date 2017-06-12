using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Conver_To_Sbo_Icons
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("de-DE");
            Application.CurrentCulture = cultureInfo;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
