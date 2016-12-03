/**
 * HVH.Service - Service that can manage client computers
 * Copyright (c) Dorian Stoll 2016
 * Licensed under the terms of the MIT License (Dorian Stoll)
 */

using System;
using System.DirectoryServices.AccountManagement;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace HVH.Service.Lock
{
    /// <summary>
    /// The main class of the program.
    /// </summary>
    public class Program
    {
        private static Int32 counter;
        private static DateTime time;
        private static Boolean emergencyMode = false;
        private static String token;

        public static void Main(String[] args)
        {
            // Create a blocking form
            Form blockForm = new Form();
            blockForm.FormBorderStyle = FormBorderStyle.None;
            blockForm.Size = blockForm.MinimumSize = blockForm.MaximumSize = new Size(Int32.MaxValue, Int32.MaxValue);
            blockForm.BackColor = Color.Black;
            blockForm.ForeColor = Color.Black;
            //blockForm.TopMost = true;
            //blockForm.ShowInTaskbar = false;
            Timer t = new Timer(5000);
            t.AutoReset = false;
            t.Elapsed += delegate
            {
                emergencyMode = false;
                token = null;
                counter = 0;
                blockForm.Invoke(new Action(() => { blockForm.BackColor = blockForm.ForeColor = Color.Black; }));
            };
            blockForm.KeyPress += delegate(Object sender, KeyPressEventArgs e)
            {
                if (e.KeyChar == '0' && counter != 4 && ((DateTime.Now - time).TotalMilliseconds < 2000 || counter == 0) && !emergencyMode)
                {
                    counter++;
                    time = DateTime.Now;

                    if (counter == 4)
                    {
                        // enter emergency mode - request admin password
                        blockForm.BackColor = blockForm.ForeColor = Color.IndianRed;
                        emergencyMode = true;
                        token = "";
                        t.Start();
                    }
                }
                else if (emergencyMode)
                {
                    if (e.KeyChar != (Int32) Keys.Enter)
                    {
                        token += e.KeyChar;
                        t.Stop();
                        t.Start();
                    }
                    else
                    {
                        String[] split = token.Split(new[] {"::"}, StringSplitOptions.None);
                        t.Stop();

                        // Try to login as a user
                        PrincipalContext pc = new PrincipalContext(ContextType.Machine);
                        if (split.Length == 2 && pc.ValidateCredentials(split[0], split[1]))
                        {
                            File.Delete(Directory.GetCurrentDirectory() + "/screen.lock");
                            Environment.Exit(0); // Kill the lock
                        }
                        else
                        {
                            blockForm.BackColor = blockForm.ForeColor = Color.Black;
                            counter = 0;
                            emergencyMode = false;
                            token = null;
                        }
                    }
                }
                else
                {
                    blockForm.BackColor = blockForm.ForeColor = Color.Black;
                    counter = 0;
                    emergencyMode = false;
                    token = null;
                }
            };
            Cursor.Hide();
#if !DEBUG
            using (KeyboardHook hook = new KeyboardHook(KeyboardHook.Parameters.None))
#endif
                Application.Run(blockForm);
        }
    }
}
