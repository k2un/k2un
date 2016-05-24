using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace STM
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //프로그램의 중복 실행 체크
            //System.Diagnostics.Process[] proc = System.Diagnostics.Process.GetProcessesByName("MCC");

            if (System.Diagnostics.Process.GetProcessesByName(System.Diagnostics.Process.GetCurrentProcess().ProcessName).Length > 1)
            {
                //MessageBox.Show("Already program is running.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
            }
            
            Application.EnableVisualStyles(); 
            Application.SetCompatibleTextRenderingDefault(false);
            frmLoading dfrmLoading = new frmLoading();

            try
            {
                Application.Run(dfrmLoading);
            }
            catch (Exception ex)
            {
                dfrmLoading.subClose();
                MessageBox.Show("Program Error, will terminate abnormally \n" + ex.ToString(), "Error!",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}