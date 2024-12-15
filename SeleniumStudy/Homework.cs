using System.Globalization;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace SeleniumStudy;

public class Homework
{
    private IWebDriver driver;
    private WebDriverWait wait;

    [SetUp]
    public void Setup()
    {
        driver = new ChromeDriver();
        driver.Manage().Window.Maximize();
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
    }

    [TearDown]
    public void TearDown()
    {
        driver.Quit();
        driver.Dispose();
        driver = null;
    }

    [Test]
    public void DiadocOrderFromWidgetSuccess()
    {
        var tryButton = By.CssSelector("a#try-button");
        var sendOrderButton = By.CssSelector("[data-event-name='event-buy-diadoc']");
        var widgetFormSelector = By.CssSelector("section#order");
        var surnameInput = By.CssSelector("input[data-field-role='surname']");
        var nameInput = By.CssSelector("input[data-field-role='name']");
        var regionSelect = By.CssSelector("select[autocomplete='address-level1']");
        var emailInput = By.CssSelector("input[type='email']");
        var emailInputValidationError = By.CssSelector("div.field-validation-error");
        var phoneInput = By.CssSelector("input[type='tel']");
        var companyNameInput = By.CssSelector("input[data-field-role='company-name']");
        var organizationSuggest = By.CssSelector("div.organization-suggest-container");
        var organizationSuggestFirstItem = By.CssSelector("div.autocomplete-suggestion");
        var contragentsFileUploaderInput = By.CssSelector("input[type='file'][data-role='fileUploader']");
        var fileNameLabel = By.CssSelector("span.file-loader__filename");
        var customDayCheckbox = By.CssSelector(".form-checkbox_custom label.form-label");
        var datePicker = By.CssSelector("input.hasDatepicker");
        var submitButton = By.CssSelector("button[type='submit'][data-tid='FWSubmitButton']");
        var successMessageTitle = By.CssSelector("div[data-role='success-message-title']");

        driver.Navigate()
            .GoToUrl(
                "https://konturru-master.ws.testkontur.ru/private/landing?domain=kontur-selenium-qa.ru&path=/diadocselenium");
        driver.FindElement(tryButton).Click();
        wait.Until(ExpectedConditions.ElementIsVisible(sendOrderButton));
        driver.FindElement(sendOrderButton).Click();
        new Actions(driver)
            .MoveToElement(driver.FindElement(widgetFormSelector)).Build().Perform();
        wait.Until(ExpectedConditions.ElementIsVisible(widgetFormSelector));
        driver.FindElement(surnameInput).SendKeys("Иванов");
        driver.FindElement(nameInput).SendKeys("Иван");
        var selectRegion = new SelectElement(driver.FindElement(regionSelect));
        selectRegion.SelectByText("Свердловская область");
        driver.FindElement(emailInput).SendKeys("блаблабла");
        driver.FindElement(emailInput).SendKeys(Keys.Tab);
        Assert.IsTrue(
            driver.FindElement(emailInputValidationError).Text.Contains("Некорректный адрес электронной почты"),
            "Некорретный текст ошибки валидации email");
        driver.FindElement(phoneInput).SendKeys("+3591234567");
        driver.FindElement(emailInput).Clear();
        driver.FindElement(emailInput).SendKeys("a@a.com");
        driver.FindElement(companyNameInput).SendKeys("Контур");
        wait.Until(ExpectedConditions.ElementIsVisible(organizationSuggest));
        driver.FindElement(organizationSuggestFirstItem).Click();
        driver.FindElement(contragentsFileUploaderInput)
            .SendKeys(Environment.CurrentDirectory + @"\Test.docx");
        wait.Until(ExpectedConditions.ElementIsVisible(fileNameLabel));
        driver.FindElement(customDayCheckbox).Click();
        wait.Until(ExpectedConditions.ElementIsVisible(datePicker));
        var date = DateTime.Now.AddDays(8);
        var formatedDate = date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
        driver.ExecuteJavaScript($@"$(""[data-role='datepicker'] input"").datepicker(""setDate"",""{formatedDate}"");");
        driver.FindElement(submitButton).Click();
        wait.Until(ExpectedConditions.ElementIsVisible(successMessageTitle));
        Assert.IsTrue(driver.FindElement(successMessageTitle).Text.Contains("Заявка отправлена!"),
            "Некорректный текст при успешной отправке");
    }
}