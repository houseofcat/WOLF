using System;
using System.Threading;
using System.Windows.Forms;

namespace Wolf
{
    static class MainProgram
    {
        //[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoOptimization)]
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            //Cool information about handling the unhandlable!
            //http://tech.pro/tutorial/668/csharp-tutorial-dealing-with-unhandled-exceptions
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException +=
                new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Application.ThreadException +=
                new ThreadExceptionEventHandler(Application_ThreadException);
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new GUI());
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                Exception ex = (Exception)e.ExceptionObject;

                MessageBox.Show("Congratulations, you have broken this program like no other user before you!\n\n" +
                      "Exception : " + ex.Message + "\n\n" + "Stack : " + ex.StackTrace,
                      "Ridonculous Exception Occurred", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                Application.Exit();
            }
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            try
            {
                MessageBox.Show("This is likely an OS incompatibility exception! Report this to me so I " +
                      "can fix/workaround Microsoft's mistake.\n\n" +
                      "Exception : " + e.Exception.Message + "\n\n" + "Stack : " + e.Exception.StackTrace,
                      "Ridonculous Exception Occurred", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                Application.Exit();
            }
        }
    }
}
