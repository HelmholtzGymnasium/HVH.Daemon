/**
 * HVH.Service - Service that can manage client computers
 * Copyright (c) Dorian Stoll 2016
 * Licensed under the terms of the MIT License (Dorian Stoll)
 */

using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace HVH.Service.Lock
{
    /// <summary>
    /// The main class of the program.
    /// </summary>
    public class Program
    {
        private static Int32 counter;
        private static DateTime time;

        public static void Main(String[] args)
        {
            // Create a blocking form
            Form blockForm = new Form();
            blockForm.FormBorderStyle = FormBorderStyle.None;
            blockForm.Size = blockForm.MinimumSize = blockForm.MaximumSize = new Size(Int32.MaxValue, Int32.MaxValue);
            blockForm.BackColor = Color.Black;
            blockForm.ForeColor = Color.Black;
            blockForm.TopMost = true;
            blockForm.ShowInTaskbar = false;
            blockForm.KeyDown += delegate(Object sender, KeyEventArgs e)
            {
                if ((e.KeyCode == Keys.D0 || e.KeyCode == Keys.NumPad0) && counter != 4 && ((DateTime.Now - time).TotalMilliseconds < 2000 || counter == 0))
                {
                    counter++;
                    time = DateTime.Now;

                    if (counter == 4)
                    {
                        // enter emergency mode - request admin password
                        blockForm.BackColor = blockForm.ForeColor = Color.IndianRed;
                        Thread.Sleep(1000);
                        blockForm.TopMost = false;
                        ShowConsoleWindow();
                        DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_CLOSE, MF_BYCOMMAND);
                        DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MAXIMIZE, MF_BYCOMMAND);
                        DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MINIMIZE, MF_BYCOMMAND);
                        Console.Title = "Emergency Mode";
                        while (true)
                        {
                            Console.WriteLine();
                            Console.Write("Username: ");
                            String username = Console.ReadLine();
                            Console.Write("Password: ");
                            String password = ReadPassword();

                            // Try to login as a user
                            PrincipalContext pc = new PrincipalContext(ContextType.Machine);
                            if (pc.ValidateCredentials(username, password))
                            {
                                File.Delete(Directory.GetCurrentDirectory() + "/screen.lock");
                                Environment.Exit(0); // Kill the lock
                            }
                        }
                    }
                }
                else
                {
                    counter = 0;
                }
            };
            Cursor.Hide();
#if !DEBUG
            using (KeyboardHook hook = new KeyboardHook(KeyboardHook.Parameters.None))
#endif
                Application.Run(blockForm);
        }

        public static void ShowConsoleWindow()
        {
            IntPtr handle = GetConsoleWindow();
            if (handle == IntPtr.Zero)
            {
                AllocConsole();
            }
            else
            {
                ShowWindow(handle, SW_SHOW);
            }
        }

        public static void HideConsoleWindow()
        {
            IntPtr handle = GetConsoleWindow();
            ShowWindow(handle, SW_HIDE);
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern Boolean AllocConsole();
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        private static extern Boolean ShowWindow(IntPtr hWnd, Int32 nCmdShow);
        [DllImport("user32.dll")]
        private static extern Int32 DeleteMenu(IntPtr hMenu, Int32 nPosition, Int32 wFlags);
        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, Boolean bRevert);

        internal const Int32 SW_HIDE = 0;
        internal const Int32 SW_SHOW = 5;
        internal const Int32 MF_BYCOMMAND = 0x00000000;
        internal const Int32 SC_CLOSE = 0xF060;
        internal const Int32 SC_MINIMIZE = 0xF020;
        internal const Int32 SC_MAXIMIZE = 0xF030;

        /// <summary>
        /// Like System.Console.ReadLine(), only with a mask.
        /// </summary>
        /// <param name="mask">a <c>char</c> representing your choice of console mask</param>
        /// <returns>the string the user typed in </returns>
        public static String ReadPassword(Char mask)
        {
            const Int32 ENTER = 13, BACKSP = 8, CTRLBACKSP = 127;
            Int32[] FILTERED = { 0, 27, 9, 10 /*, 32 space, if you care */ }; // const

            var pass = new Stack<Char>();
            Char chr = (Char)0;

            while ((chr = Console.ReadKey(true).KeyChar) != ENTER)
            {
                if (chr == BACKSP)
                {
                    if (pass.Count > 0)
                    {
                        Console.Write("\b \b");
                        pass.Pop();
                    }
                }
                else if (chr == CTRLBACKSP)
                {
                    while (pass.Count > 0)
                    {
                        Console.Write("\b \b");
                        pass.Pop();
                    }
                }
                else if (FILTERED.Count(x => chr == x) > 0) { }
                else
                {
                    pass.Push(chr);
                    Console.Write(mask);
                }
            }

            Console.WriteLine();

            return new String(pass.Reverse().ToArray());
        }

        /// <summary>
        /// Like System.Console.ReadLine(), only with a mask.
        /// </summary>
        /// <returns>the string the user typed in </returns>
        public static String ReadPassword()
        {
            return ReadPassword('*');
        }
    }
}
