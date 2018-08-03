namespace FrontEnd.Courses
{
    using System.Collections.Generic;
    using Backend;
    using FluentAssertions;
    using OpenQA.Selenium;
    using Xbehave;

    public class FrontEnd : SeleniumSpec
    {
        [Scenario]
        public void KurseAufrufen()
        {
            var newItemName = "mytest3";

            "es existieren Kurse".x(()
                => this.backend.Courses = new[]
                    {
                        new Course("test1"),
                        new Course("test2")
                    });

            "_".x(() => this.backend.OnAddCourse(
                c => this.backend.Courses = new List<Course>(this.backend.Courses) { new Course(c) }.ToArray()));

            "wenn die Kurs-Seite aufgerufen wird".x(()
                => this.browser.Open(
                    this.server,
                    "/frontend"));

            "Titel".x(()
                => this.wait.For(
                    () => this.browser.Title.Should().StartWith("Student")));

            "Inhalt".x(()
                => this.wait.For(
                    () => this.browser.FindElement(By.CssSelector("li")).Text.Should().Contain("test1")));

            $"wenn als neuer Kurs-Name `{newItemName}` eingetragen wird".x(()
                => this.browser.FindElement(By.CssSelector("input")).SendKeys(newItemName));

            "wenn auf den Hinzufügen-Button geklickt wird".x(()
                => this.browser.FindElement(By.CssSelector("button")).Click());

            "soll der Inhalt erneut geladen werden".x(()
                => this.wait.For(
                    () => this.browser.FindElements(By.CssSelector("li")).Should().HaveCount(3)));

            $"soll der neue Kurs `{newItemName}` heissen".x(()
                => this.wait.For(
                    () => this.browser.FindElements(By.CssSelector("li")).Should().Contain(e => e.Text == newItemName)));

            "soll das Eingabefeld leer sein".x(()
                => this.wait.For(
                    () => this.browser.FindElement(By.CssSelector("input")).GetAttribute("value").Should().BeEmpty()));
        }
    }
}