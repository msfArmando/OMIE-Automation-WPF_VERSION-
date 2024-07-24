using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace OMIE_Automation_WPF_VERSION_
{
    internal class Automations
    {

        IWebDriver Driver;

        public Automations()
        {
            WebDriver.GetInstance();
            Driver = WebDriver.Driver;
        }

        public void GoToWebSite(string url)
        {
            Driver.Navigate().GoToUrl(url);
            Driver.Manage().Window.Maximize();
        }

        public void Login(string username, string password)
        {
            try
            {
                var emailElement = WebDriver.TryFindElementWithEx("//input[contains(@autocomplete, 'username')]", 10, 1);
                emailElement.Click();
                emailElement.SendKeys(username);

                var continueElement = WebDriver.TryFindElementWithEx("//button[contains(@id, 'btn-continue')]", 10, 1);
                continueElement.Click();

                var passwordElement = WebDriver.TryFindElementWithEx("//input[contains(@id, 'current-password')]", 10, 1);
                passwordElement.SendKeys(password);

                var btnLogin = WebDriver.TryFindElementWithEx("//button[contains(@id, 'btn-login')]", 10, 1);
                btnLogin.Click();

                try
                {
                    var btnScale = WebDriver.TryFindElementWithEx("//div[contains(@aria-label, 'Fechar diálogo')]\r\n", 10, 1);
                    if (btnScale != null)
                        btnScale.Click();
                    else
                        throw new Exception();
                }
                catch (Exception)
                {
                    throw new ElementNotVisibleException("Recomendation button not found");
                }
            }
            catch (ElementNotVisibleException)
            {
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Login error");
            }
        }

        public void CadastrarUsuarios(string newUser, string idPermission)
        {
            try
            {
                var showMoreBtn = WebDriver.TryFindElementWithEx("//button[contains(@class, 'MuiButtonBase-root MuiButton-root MuiButton-text MuiButton-textPrimary MuiButton-sizeMedium MuiButton-textSizeMedium MuiButton-root MuiButton-text MuiButton-textPrimary MuiButton-sizeMedium MuiButton-textSizeMedium jss67 css-fnqg9q')]", 10, 1);
                do
                {
                    IJavaScriptExecutor executor = (IJavaScriptExecutor)Driver;
                    executor.ExecuteScript("arguments[0].click();", showMoreBtn);

                    showMoreBtn = WebDriver.TryFindElementWithEx("//button[contains(@class, 'MuiButtonBase-root MuiButton-root MuiButton-text MuiButton-textPrimary MuiButton-sizeMedium MuiButton-textSizeMedium MuiButton-root MuiButton-text MuiButton-textPrimary MuiButton-sizeMedium MuiButton-textSizeMedium jss67 css-fnqg9q')]", 10, 1);
                } while (showMoreBtn != null);

                var coligadasLista = new List<IWebElement>();

                coligadasLista = (WebDriver.TryFindElementsWithEx("//button[contains(@id, 'basic-button')]", 10, 1));

                foreach (var element in coligadasLista)
                {
                    IJavaScriptExecutor executor = (IJavaScriptExecutor)Driver;
                    executor.ExecuteScript("arguments[0].click();", element);
                    
                    var newUserRegister = WebDriver.TryFindElementWithEx("//li[contains(., 'Cadastrar usuários')]", 10, 1);
                    executor.ExecuteScript("arguments[0].click();", newUserRegister);

                    var allWindowHandles = Driver.WindowHandles;
                    Driver.SwitchTo().Window(allWindowHandles[1]);

                    var newUserService = WebDriver.TryFindElementWithEx("//button[contains(@id, 'service-users-new')]", 10, 1);
                    executor.ExecuteScript("arguments[0].click();", newUserService);

                    var inputNewUser = WebDriver.TryFindElementWithEx("//input[contains(@class, 'form-control user-email')]", 10, 1);
                    inputNewUser.SendKeys(newUser);

                    WebDriver.TryFindElementWithEx("//tr[contains(., 'Enviar')]//Button[contains(., 'Administrador')]", 10, 5).Click();

                    Thread.Sleep(1000);

                    var permissionBtn = WebDriver.TryFindElementWithEx($"(//tr[contains(., 'Enviar')]//li//span[contains(@class, 'auth')])[{idPermission}]", 10, 5);

                    Thread.Sleep(2000);
                    
                    if(permissionBtn != null)
                        executor.ExecuteScript("arguments[0].click();", permissionBtn);

                    Thread.Sleep(2000);

                    var sendBtn = WebDriver.TryFindElementWithEx("//button[contains(., 'Enviar o convite')]", 10, 5);

                    executor.ExecuteScript("arguments[0].click();", sendBtn);

                    Thread.Sleep(2000);

                    Driver.Close();
                    Driver.SwitchTo().Window(allWindowHandles[0]);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void Dispose()
        {
            Driver.Quit();
        }
    }
}
