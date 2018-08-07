namespace FrontEnd.Courses
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using Backend;
    using FluentAssertions;
    using OpenQA.Selenium;
    using Xbehave;

    public class FrontEnd : SeleniumSpec
    {
        [Scenario]
        public void KurseAufrufen()
        {
            var newCourseName = "mytest3";
            var newCourse = new Course(
                Guid.Parse("CAECCA78-2706-4E5B-B3D8-FC91C77F62C9"),
                newCourseName);

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

            "Fake: Einzelkurs kann zurückgegeben werden".x(()
                => this.backend.Course = newCourse);

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

            $"wenn als neuer Kurs-Name `{newCourseName}` eingetragen wird".x(()
                => this.browser.FindElement(By.CssSelector("input")).SendKeys(newCourseName));

            "wenn auf den Hinzufügen-Button geklickt wird".x(()
                => this.browser.FindElement(By.CssSelector("button")).Click());

            "soll der Inhalt erneut geladen werden".x(()
                => this.wait.For(
                    () => this.browser.FindElements(By.CssSelector("li")).Should().HaveCount(3)));

            $"soll der neue Kurs `{newCourseName}` heissen".x(()
                => this.wait.For(
                    () => this.browser.FindElements(By.CssSelector("li")).Should().Contain(e => e.Text == newCourseName)));

            "soll das Eingabefeld leer sein".x(()
                => this.wait.For(
                    () => this.browser.FindElement(By.CssSelector("input")).GetAttribute("value").Should().BeEmpty()));

            "wenn auf ein Kurs geklickt wird".x(()
                => this.browser.FindElements(By.CssSelector("a")).Last().Click());

            "soll auf die Admin-Kurs-Seite weitergeleitet werden".x(()
                => this.wait.For(
                    () => this.browser.FindElement(By.CssSelector("h1")).Text.Should().Be(newCourseName)));
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

        [Scenario]
        public void UnbekannteSeite()
        {
            "wenn eine unbekannte Seite aufgerufen wird".x(()
                => this.browser.Open(
                    this.server,
                    "/frontend/#sadfl"));

            "Inhalt".x(()
                => this.wait.For(
                    () => this.browser.FindElement(By.CssSelector("h1")).Text.Should().Contain("404")));
        }

        [Scenario]
        public void SelfieMachen()
        {
            var course = new Course(
                Guid.Parse("CAECCA78-2706-4E5B-B3D8-FC91C77F62C9"),
                "test1");

            var portraitPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Data",
                "portrait.jpg");

            "es existieren Kurse".x(()
                => this.backend.Course = course);

            "wenn die Selfie-Seite aufgerufen wird".x(()
                => this.browser.Open(
                    this.server,
                    $"/frontend/#takeselfie/{course.Id}"));

            "Inhalt".x(()
                => this.wait.For(
                    () => this.browser.FindElement(By.CssSelector("h1")).Text.Should().Contain(course.Name)));

            "wenn ein Vorname gesetzt wird".x(()
                => this.browser.FindElements(By.XPath("//input[@type='text']")).First().SendKeys("MeinVorname"));

            "wenn ein Nachname gesetzt wird".x(()
                => this.browser.FindElements(By.XPath("//input[@type='text']")).Last().SendKeys("MeinNachname"));

            "wenn ein Bild gesetzt wird".x(()
                => this.browser.FindElement(By.XPath("//input[@type='file']")).SendKeys(portraitPath));

            "soll ein Vorschaubild mit der richtigen Grösse erscheinen".x(()
                => this.wait.For(
                    () => this.browser.FindElement(By.CssSelector("img")).Size.Height.Should().Be(800)));

            "_".x(()
                => this.wait.For(
                    () => this.browser.FindElement(By.CssSelector("img")).Size.Width.Should().Be(533)));

            "wenn auf OK geklickt wird".x(()
                => this.browser.FindElement(By.CssSelector("button")).Click());

            "soll der Benutzer verdankt werden".x(()
                => this.wait.For(
                    () => this.browser.FindElement(By.CssSelector("h1")).Text.Should().Contain("Danke")));

            "sollen die Daten gespeichert werden".x(()
                =>
            {
                this.backend.SavedSelfie.CourseId.Should().Be(course.Id);
                this.backend.SavedSelfie.StudentFirstname.Should().Be("MeinVorname");
                this.backend.SavedSelfie.StundetLastname.Should().Be("MeinNachname");
                
                var byteArray = Convert.FromBase64String(this.backend.SavedSelfie.ImageInBase64);
                var memStream = new MemoryStream(byteArray);
                var image = new Bitmap(memStream);
                image.Size.Should().Be(new Size(533, 800));
            });
        }
    }
}