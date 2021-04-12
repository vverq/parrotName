using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ParrotNameSite
{
    class ParrotNameSiteTests
    {
        public ChromeDriver driver;
        public const string URL = "https://qa-course.kontur.host/selenium-practice/";

        private readonly string incorrectEmail = "1@";
        private readonly string correctEmail = "1@test.ru";

        private readonly By buttonLocator = By.Id("sendMe");
        private readonly By emailInputLocator = By.Name("email");
        private readonly By emailErrorLocator = By.ClassName("form-error"); 
        private readonly By yourEmailLocator = By.ClassName("your-email");
        private readonly By anotherEmailLinkLocator = By.Id("anotherEmail");
        private readonly By resultTextLocator = By.ClassName("result-text");
        private readonly By radioButtonBoyLocator = By.Id("boy");
        private readonly By radioButtonGirlLocator = By.Id("girl");

        [SetUp]
        public void SetUp()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximize");
            driver = new ChromeDriver(options);
        }

        [TearDown]
        public void TearDriver()
        {
            driver.Quit();
        }

        [Test]
        public void ParrotNameSite_FillFromWithEmail_EmptyEmail()
        {
            driver.Navigate().GoToUrl(URL);
            driver.FindElement(buttonLocator).Click();

            Assert.IsTrue(driver.FindElement(emailErrorLocator).Displayed, "No error message");
            Assert.AreEqual("Введите email", driver.FindElement(emailErrorLocator).Text, "Incorrect error message");
        }

        [Test]
        public void ParrotNameSite_FillFromWithEmail_IncorrectEmail()
        {
            driver.Navigate().GoToUrl(URL);
            driver.FindElement(emailInputLocator).SendKeys(incorrectEmail);
            driver.FindElement(buttonLocator).Click();

            Assert.IsTrue(driver.FindElement(emailErrorLocator).Displayed, "No error message");
            Assert.AreEqual("Некорректный email", driver.FindElement(emailErrorLocator).Text, "Incorrect error message");
        }

        [Test]
        public void ParrotNameSite_FillForWithEmail_CorrectEmail()
        {
            driver.Navigate().GoToUrl(URL);
            driver.FindElement(emailInputLocator).SendKeys(correctEmail);
            driver.FindElement(buttonLocator).Click();

            Assert.AreEqual(correctEmail, driver.FindElement(yourEmailLocator).Text, "Result email does not match the entered email");
            Assert.IsFalse(driver.FindElement(buttonLocator).Displayed, "Button is visible after clicking");
            Assert.IsTrue(driver.FindElement(anotherEmailLinkLocator).Displayed, "Another email link is not visible");
        }

        [Test]
        public void ParrotNameSite_FillFormWithEmail_ClickAnotherEmail()
        {
            driver.Navigate().GoToUrl(URL);
            driver.FindElement(emailInputLocator).SendKeys(correctEmail);
            driver.FindElement(buttonLocator).Click();
            driver.FindElement(anotherEmailLinkLocator).Click();

            Assert.AreEqual(string.Empty, driver.FindElement(emailInputLocator).Text, "Input email field is not empty");
            Assert.IsFalse(driver.FindElement(anotherEmailLinkLocator).Displayed, "Another email link is visible");
        }

        [Test]
        public void ParrotNameSite_ClickRadioButtonBoy_CorrectResultText()
        {
            driver.Navigate().GoToUrl(URL);
            driver.FindElement(radioButtonBoyLocator).Click();
            driver.FindElement(emailInputLocator).SendKeys(correctEmail);
            driver.FindElement(buttonLocator).Click();

            StringAssert.Contains("вашего мальчика", driver.FindElement(resultTextLocator).Text, "Incorrect result text after selecting boy");
        }

        [Test]
        public void ParrotNameSite_ClickRadioButtonGirl_CorrectResultText()
        {
            driver.Navigate().GoToUrl(URL);
            driver.FindElement(radioButtonGirlLocator).Click();
            driver.FindElement(emailInputLocator).SendKeys(correctEmail);
            driver.FindElement(buttonLocator).Click();

            StringAssert.Contains("вашей девочки", driver.FindElement(resultTextLocator).Text, "Incorrect result text after selecting girl");
        }

        [Test]
        public void ParrotNameSite_ClickRadioButton_SaveStateAfterClickAnotherEmailLink()
        {
            driver.Navigate().GoToUrl(URL);
            driver.FindElement(radioButtonGirlLocator).Click();
            driver.FindElement(emailInputLocator).SendKeys(correctEmail);
            driver.FindElement(buttonLocator).Click();
            driver.FindElement(anotherEmailLinkLocator).Click();

            Assert.IsTrue(driver.FindElement(radioButtonGirlLocator).Selected, "After click another email link state of radio button is not saved");
        }

        [Test]
        public void ParrotNameSite_ClickTwoRadioButtons_OnlyOneRadioButtonSelected()
        {
            driver.Navigate().GoToUrl(URL);
            driver.FindElement(radioButtonGirlLocator).Click();
            driver.FindElement(radioButtonBoyLocator).Click();

            Assert.IsTrue(driver.FindElement(radioButtonBoyLocator).Selected && !driver.FindElement(radioButtonGirlLocator).Selected, "Selected two radio buttons at the same time");
        }
    }
}
