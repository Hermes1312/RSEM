using System;
using System.Threading;
using System.Collections.Generic;

using RSEM.Other;

namespace RSEM.Managers
{
    internal class ThreadManager
    {
        public static Form1 form1;
        public static Dictionary<string, Thread> threads = new Dictionary<string, Thread>();
        public static Dictionary<string, Thread> activeThreads = new Dictionary<string, Thread>();
        public static Dictionary<string, Thread> pausedThreads = new Dictionary<string, Thread>();

        public static void Add(string name, ThreadStart function)
        {
            if (threads.TryGetValue(name, out Thread temp)) return;

            threads.Add(name, new Thread(function));
        }

        public static void ToggleThread(string name)
        {
            form1.ToggleCheat(name);

            

            if (activeThreads.TryGetValue(name, out Thread temp))
            {
                #pragma warning disable CS0618 // Typ oder Element ist veraltet
                temp.Suspend();
                #pragma warning restore CS0618 // Typ oder Element ist veraltet

                activeThreads.Remove(name);
                pausedThreads.Add(name, temp);
            }
            else
            {
                if (!threads.TryGetValue(name, out temp))
                {
                    Extensions.Error("Could not start { name }", 1500, false);
                    return;
                }

                if (pausedThreads.TryGetValue(name, out Thread temp2))
                {
                    #pragma warning disable CS0618 // Typ oder Element ist veraltet
                    temp2.Resume();
                    #pragma warning restore CS0618 // Typ oder Element ist veraltet

                    pausedThreads.Remove(name);
                }
                else
                {
                    temp.Start();
                }

                activeThreads.Add(name, temp);
            }
        }
    }
}
