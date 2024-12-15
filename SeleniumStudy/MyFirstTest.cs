using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace SeleniumStudy;

public class MyFirstTest
{
    private IWebDriver driver;

    [SetUp]
    public void Setup()
    {
        driver = new ChromeDriver();
        driver.Manage().Window.Maximize(); // альтернативный вариант
        new WebDriverWait(driver, TimeSpan.FromSeconds(5));
    }

    [TearDown]
    public void TearDown()
    {
        driver.Quit();
        driver.Dispose(); // Без этой строки была ошибка компиляции в строке 11
        driver = null;
    }

    [Test]
    public void CheckingPageTitle()
    {
        driver.Navigate().GoToUrl("https://www.wikipedia.org/");
        IWebElement queryInput = driver.FindElement(By.Name("search"));
        IWebElement searchButton =
            driver.FindElement(
                By.XPath(
                    "//button[@class='pure-button pure-button-primary-progressive']")); // не искалось по By.Name("go), вероятно устаревшая инфа
        queryInput.SendKeys("Selenium");
        searchButton.Click();

        Assert.IsTrue(driver.Title.Contains("Selenium — Википедия"),
            "Неверный заголовок страницы"); // Assert.IsTrue лежит в MSTest.TestFramework * 3.6.4
    }

    [Test]
    public void Locators()
    {
        driver.Navigate().GoToUrl("https://konturru-master.ws.testkontur.ru/private/landing?domain=kontur-selenium-qa.ru&path=/diadocseleniumref");
        // Локатор блока “Зарабатывайте на рекомендациях”
        var orderSection = driver.FindElement(By.Id("order"));
        // Локатор кнопки “Стать партнером” в блоке в конце страницы
        var becomePartner = driver.FindElement(By.CssSelector("[value='Стать партнером']"));
        // Локатор лайтбокса “Заявка на партнерство” (для вызова лайтбокса необходимо нажать кнопку “Стать партнером” в блоке в конце страницы)
        var becomePartnerLightbox = driver.FindElement(By.CssSelector(".lightbox-window__content"));
        // Локатор поля “Фамилия”
        var surnameInput = driver.FindElement(By.CssSelector(".lightbox-window__content input[data-field-role='surname']"));
        // Локатор поля “Имя”
        var nameInput = driver.FindElement(By.CssSelector(".lightbox-window__content input[data-field-role='name']"));
        // Локатор поля “Электронная почта”
        var emailInput = driver.FindElement(By.CssSelector(".lightbox-window__content input[type='email']"));
        // Локатор кнопки “Отправить”
        var submitButton = driver.FindElement(By.CssSelector(".lightbox-window__content button[type='submit']"));
        // Локатор лайтбокса “Заявка отправлена”
        var successLightbox = driver.FindElement(By.CssSelector("[data-tid='FWSuccessBlock']"));
        // Локатор текста в лайтбоксе об успешной отправке 
        var successLightboxText = driver.FindElement(By.CssSelector("[data-role='success-message-text'][data-tid='FWSuccessBlockText']"));
    }
}