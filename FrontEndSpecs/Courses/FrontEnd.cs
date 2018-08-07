namespace FrontEnd.Courses
{
    using System;
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
                        new Course(
                            Guid.Parse("A8A1C1C1-49FC-4DCB-8CD8-F15932904930"),
                            "test1"),
                        new Course(
                            Guid.Parse("A96314FA-6B59-4164-95EB-3F8F955F019A"),
                            "test2")
                    });

            "_".x(() => this.backend.OnAddCourse(
                c => this.backend.Courses = new List<Course>(this.backend.Courses) { new Course(Guid.NewGuid(), c) }.ToArray()));

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

        [Scenario]
        public void KursAufrufen()
        {
            var course = new Course(
                Guid.Parse("CAECCA78-2706-4E5B-B3D8-FC91C77F62C9"),
                "test1");

            "es existieren Kurse".x(()
                => this.backend.Course = course);

            "wenn die Kurs-Seite aufgerufen wird".x(()
                => this.browser.Open(
                    this.server,
                    $"/frontend/#courses/{course.Id}"));

            "Inhalt".x(()
                => this.wait.For(
                    () => this.browser.FindElement(By.CssSelector("h1")).Text.Should().Contain(course.Name)));

            "soll der richtige Kurs abgerufen werden".x(()
                => this.backend.GetCourseId.Should().Be(course.Id));

            "soll Server-URL im QR Code enthalten sein".x(()
                => this.browser.ScanForQrCodeContaining(this.server.RootUri));

            "soll ID des Kurs' enthalten sein".x(()
                => this.browser.ScanForQrCodeContaining(course.Id.ToString()));
        }
    }
}