namespace FrontEnd.Courses
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
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

            var portraitPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Data",
                "portrait.jpg");

            var base64 = GetImageAsBase64Jpeg(
                portraitPath,
                533,
                800);

            var students = new[]
            {
                new Student(course.Id, "Max", "Mustermann", base64), 
                new Student(course.Id, "Yvette", "Musterfrau", base64), 
            };

            "es existieren Kurse".x(()
                => this.backend.Course = course);

            "es existieren Schüler".x(()
                => this.backend.Students = students);

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

            "soll ein Link zur Student-Übersicht enthalten sein".x(()
                => this.wait.For(
                    () => this.browser.FindElement(By.CssSelector("a.students")).Text.Should().Contain("Übersicht")));

            "wenn auf den Link geklickt wird".x(()
                => this.browser.FindElement(By.CssSelector("a.students")).Click());

            "soll eine Liste vorhanden sein".x(()
                => this.wait.For(
                    () => this.browser.FindElements(By.CssSelector("li")).Select(x => x.FindElement(By.CssSelector("span")).Text).Should().BeEquivalentTo(students.Select(x => $"{x.Firstname} {x.Lastname}"))));

            "soll eine Liste vorhanden sein".x(()
                => this.wait.For(
                    () => this.browser.FindElements(By.CssSelector("li")).Select(x => x.FindElement(By.CssSelector("img")).Size).Should().AllBeEquivalentTo(new Size(533, 800))));

            "soll der richtige Kurs abgerufen werden".x(()
                => this.backend.GetCourseId.Should().Be(course.Id));

            "sollen die Studenten für den richtigen Kurs abgerufen werden".x(()
                => this.backend.GetStudentsByCourseId.Should().Be(course.Id));

            "soll der Kurs-Name vorhanden sein".x(()
                => this.wait.For(
                    () => this.browser.FindElement(By.CssSelector("h1")).Text.Should().Contain(course.Name)));
        }

        [Scenario]
        public void Lernen()
        {
            var course = new Course(
                Guid.Parse("CAECCA78-2706-4E5B-B3D8-FC91C77F62C9"),
                "test1");

            var portraitPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Data",
                "portrait.jpg");

            var base64 = GetImageAsBase64Jpeg(
                portraitPath,
                533,
                800);

            var student = new Student(course.Id, "Max", "Mustermann", base64);

            "es existieren Kurse".x(()
                => this.backend.Course = course);

            "es existieren Schüler".x(()
                => this.backend.StudentToLearn = student);

            "wenn die Kurs-Seite aufgerufen wird".x(()
                => this.browser.Open(
                    this.server,
                    $"/frontend/#courses/{course.Id}"));

            "_".x(()
                => this.wait.For(
                    () => this.browser.FindElement(By.CssSelector("h1")).Text.Should().Contain(course.Name)));

            "soll ein Link zur Student-Übersicht enthalten sein".x(()
                => this.wait.For(
                    () => this.browser.FindElement(By.CssSelector("a.learning")).Text.Should().Contain("Lernen")));

            "wenn auf den Link geklickt wird".x(()
                => this.browser.FindElement(By.CssSelector("a.learning")).Click());

            "soll ein Bild eines Schülers erscheinen".x(()
                => this.wait.For(
                    () => this.browser.FindElement(By.CssSelector("img")).Size.Should().Be(new Size(533, 800))));

            "soll kein Name vorhanden sein".x(()
                => this.wait.For(
                    () => this.browser.FindElement(By.CssSelector(".name")).Displayed.Should().BeFalse()));

            "wenn auf das Bild geklickt wird".x(()
                => this.wait.For(
                    () => this.browser.FindElement(By.CssSelector("img")).Click()));

            "soll das Bild verschwinden".x(()
                => this.wait.For(
                    () => this.browser.FindElement(By.CssSelector("img")).Displayed.Should().BeFalse()));

            "soll der Name erscheinen".x(()
                => this.wait.For(
                    () => this.browser.FindElement(By.CssSelector(".name")).Displayed.Should().BeTrue()));

            "soll der Name erscheinen (2)".x(()
                => this.wait.For(
                    () => this.browser.FindElement(By.CssSelector(".name")).Text.Should().Be($"{student.Firstname} {student.Lastname}")));

            "wenn auf den Button geklickt wird".x(()
                => this.wait.For(
                    () => this.browser.FindElement(By.CssSelector("button")).Click()));

            "soll ein Bild eines Schülers erscheinen".x(()
                => this.wait.For(
                    () => this.browser.FindElement(By.CssSelector("img")).Displayed.Should().BeTrue()));

            "soll kein Name vorhanden sein".x(()
                => this.wait.For(
                    () => this.browser.FindElement(By.CssSelector(".name")).Displayed.Should().BeFalse()));
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
                this.backend.SavedStudent.CourseId.Should().Be(course.Id);
                this.backend.SavedStudent.Firstname.Should().Be("MeinVorname");
                this.backend.SavedStudent.Lastname.Should().Be("MeinNachname");
                
                var byteArray = Convert.FromBase64String(this.backend.SavedStudent.ImageInBase64);
                var memStream = new MemoryStream(byteArray);
                var image = new Bitmap(memStream);
                image.Size.Should().Be(new Size(533, 800));
            });
        }

        private string GetImageAsBase64Jpeg(
            string path,
            int width,
            int height)
        {
            using (var fileStream = File.OpenRead(path))
            {
                using (var original = new Bitmap(fileStream))
                {
                    using (var resized = new Bitmap(original, width, height))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            resized.Save(memoryStream, ImageFormat.Jpeg);

                            return Convert.ToBase64String(memoryStream.ToArray());
                        }
                    }
                }
            }
        }
    }
}