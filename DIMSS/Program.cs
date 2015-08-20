using DIMSS.Presenter;
using DIMSS.View;
using System;
using System.Windows.Forms;

namespace DIMSS
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var mainView = new DMSView();
            var presenter = new DMSPresenter(mainView);
            
            Application.Run(mainView);
        }
    }
}
