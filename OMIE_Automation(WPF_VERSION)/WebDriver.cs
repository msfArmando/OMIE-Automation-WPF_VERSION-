using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OMIE_Automation_WPF_VERSION_
{
    internal class WebDriver
    {
        private static WebDriver instance;
        public static IWebDriver Driver { get; private set; }

        public WebDriver() {
            ChromeOptions options = new ChromeOptions();
            //options.AddArgument("--headless");
            options.AddArgument("--start-minimized");
            Driver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), options);
        }

        public static WebDriver GetInstance()
        {
            if (instance == null)
            {
                instance = new WebDriver();
            }
            return instance;
        }

        public static IWebElement TryFindElementWithEx(string xpath, int tries, int time)
        {
            int triesTime = 0;
            do
            {
                try
                {
                    return Driver.FindElement(By.XPath(xpath));
                }
                catch (Exception)
                {
                    triesTime++;
                    Thread.Sleep(time * 1000);
                    continue;
                }
            } while (triesTime <= tries);

            return null;
        }

        public static List<IWebElement> TryFindElementsWithEx(string xpath, int tries, int time)
        {
            int triesTime = 0;
            List<IWebElement> elements = new List<IWebElement>();

            do
            {
                try
                {
                    elements = Driver.FindElements(By.XPath(xpath)).ToList();
                    if (elements.Count > 0)
                    {
                        return elements;
                    }
                }
                catch (Exception)
                {
                    triesTime++;
                    Thread.Sleep(time * 1000);
                    continue;
                }
            } while (triesTime <= tries); // Mudando <= para <

            return null;
        }
    }
}
