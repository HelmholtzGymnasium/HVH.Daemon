/**
 * HVH.Service - Service that can manage client computers
 * Copyright (c) Dorian Stoll 2016
 * Licensed under the terms of the MIT License (Dorian Stoll)
 */

using System;
using System.ComponentModel;
using System.DirectoryServices.AccountManagement;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using log4net;
using MjpegProcessor;
using Timer = System.Timers.Timer;

namespace HVH.Service.Lock
{
    /// <summary>
    /// The main class of the program.
    /// </summary>
    public class Program
    {        
        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static Int32 counter;
        private static DateTime time;
        private static Boolean emergencyMode = false;
        private static String token;

        public static void Main(String[] args)
        {
            Form form = new Form();
            log.InfoFormat("Locking screen. MediaStream: {0}", args.Length != 0);
            if (args.Length == 0)
            {
                // Create a blocking form
                form.FormBorderStyle = FormBorderStyle.None;
                form.Size = form.MinimumSize = form.MaximumSize = new Size(Int32.MaxValue, Int32.MaxValue);
                form.BackColor = Color.Black;
                form.ForeColor = Color.Black;
#if !DEBUG
                form.ShowInTaskbar = false;
                form.TopMost = true;
#endif
            }
            else
            {
                // Create the picture box
                PictureBox image = new PictureBox();
                ((ISupportInitialize) image).BeginInit();
                form.SuspendLayout();
                image.Location = new Point(0, 0);
                image.Size = Screen.PrimaryScreen.Bounds.Size; 
                image.TabIndex = 0;
                image.TabStop = false;
                image.SizeMode = PictureBoxSizeMode.CenterImage;

                // The rest of the form
                form.BackColor = Color.Black;
                form.Location = new Point(0, 0);
                form.Size = form.MinimumSize = form.MaximumSize = Screen.PrimaryScreen.WorkingArea.Size;
                form.Controls.Add(image);
                form.FormBorderStyle = FormBorderStyle.None;
                form.StartPosition = FormStartPosition.Manual;
#if !DEBUG
                form.ShowInTaskbar = false;
                form.TopMost = true;
#endif
                ((ISupportInitialize) image).EndInit();
                form.ResumeLayout(false);

                // Listen to the mjpeg stream
                MessageBox.Show(new Uri(args[0]).AbsoluteUri);
                MjpegDecoder decoder = new MjpegDecoder();
                try
                {
                    decoder.ParseStream(new Uri(args[0]));
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
                decoder.FrameReady += delegate(Object sender, FrameReadyEventArgs eventArgs)
                {
                    image.Image = !emergencyMode ? eventArgs.Bitmap : null;
                };
            }
            Timer t = new Timer(5000);
            t.AutoReset = false;
            t.Elapsed += delegate
            {
                emergencyMode = false;
                token = null;
                counter = 0;
                form.Invoke(new Action(() => { form.BackColor = form.ForeColor = Color.Black; }));
                log.Info("Exiting emergency mode.");
            };
            form.KeyPress += delegate(Object sender, KeyPressEventArgs e)
            {
                if (e.KeyChar == '0' && counter != 4 && ((DateTime.Now - time).TotalMilliseconds < 2000 || counter == 0) && !emergencyMode)
                {
                    counter++;
                    time = DateTime.Now;

                    if (counter == 4)
                    {
                        // enter emergency mode - request admin password
                        log.Info("Emergency Mode!");
                        form.BackColor = form.ForeColor = Color.IndianRed;
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
                            log.Info("Account data is correct, unlocking client computer");
                            File.Delete(Directory.GetCurrentDirectory() + "/screen.lock");
                            Environment.Exit(0); // Kill the lock
                        }
                        else
                        {
                            log.Info("Account data is wrong, computer stays locked");
                            form.BackColor = form.ForeColor = Color.Black;
                            counter = 0;
                            emergencyMode = false;
                            token = null;
                        }
                    }
                }
                else
                {
                    form.BackColor = form.ForeColor = Color.Black;
                    counter = 0;
                    emergencyMode = false;
                    token = null;
                }
            };
            Cursor.Hide();
#if !DEBUG
            using (KeyboardHook hook = new KeyboardHook(KeyboardHook.Parameters.None))
#endif
                Application.Run(form);
        }
    }
}
