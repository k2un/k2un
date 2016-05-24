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
            System.Diagnostics.Process currentProc = System.Diagnostics.Process.GetCurrentProcess();
            System.Diagnostics.Process[] proc = System.Diagnostics.Process.GetProcessesByName(currentProc.ProcessName);


            //if (proc.Length > 1)
            //{
            //    // 일단 중복 실행 상황이다.

            //    for (int dintLoop = 0; dintLoop < proc.Length; dintLoop++)
            //    {
            //        int dintPID = proc[dintLoop].Id;

            //        if (dintPID != currentProc.Id)
            //        {
            //            if (proc[dintLoop].MainWindowHandle == System.IntPtr.Zero)
            //            {
            //                // 비정상 종료 상태.
            //                // 강제 종료 하고 이 프로그램 진행하믄 될듯...
            //                if (MessageBox.Show(string.Format("STM.exe 프로그램이 종료되지않고, 비정상적으로 실행 중입니다.\n해당 프로세스({0})를 강제로 종료하고 새 프로그램을 실행 하겠습니까?", dintPID), "",
            //                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;
            //                else
            //                {
            //                    try
            //                    {
            //                        proc[dintLoop].Kill();
            //                    }
            //                    catch { continue; }
            //                }
            //            }
            //            else
            //            {
            //                MessageBox.Show("Already program is running.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //                Application.Exit();
            //                return;
            //            }
            //        }
            //    }
            //}
            
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