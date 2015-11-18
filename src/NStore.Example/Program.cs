using System;
using System.IO;
using NStore.Example.Examples;

namespace NStore.Example
{
    internal static class Program
    {
        static Program()
        {
            // Initialize NStore
            NStore.Init(GetConfig());
        }

        private static void Main(string[] args)
        {
            var e = new AdvancedExample();
            e.Go();

            /**********************************
             * All Done
             **********************************/
            Console.WriteLine("All Done. Press Enter to Continue...");
            Console.ReadKey();
        }

        /// <summary>
        ///     Get configuration from file as a string
        /// </summary>
        /// <returns></returns>
        private static string GetConfig()
        {
            var configPath = AppDomain.CurrentDomain.BaseDirectory + "NStoreConfig.xml";
            return File.ReadAllText(configPath);
        }
    }
}