using System;
using NUnit.Framework;
using WebReports.LiveQueueReport;
using WebReports.LiveQueueReport.Interfaces;
using WebReports.LiveQueueReport.Implementation;

namespace LiveQueueReportTests.LiveQueueHtmlParserTests
{
    [TestFixture]
    public class HtmlParserTests
    {
        private IHtmlDataExtractor GetExtractor()
        {
            return new HtmlDataExtractor();
        }

        [Test]
        public void GetAuthenticityToken_GetsCorrectPageContent_ReturnsAuthenticityToken()
        {
            const string pageContent = @"<html>" +
                                       @"<div>" +
                                       @"<input name=""authenticity_token"" type=""hidden"" value=""gcL7 + swbHFR9DuXlwOmzWEQPO3YpRrCD6s7pJMfNLuw = "" />" +
                                       @"</div>" +
                                       @"</html>";

            var extractor = GetExtractor();
            var actualAuthenticityToken = extractor.GetAuthenticityToken(pageContent);

            Assert.AreEqual(@"gcL7 + swbHFR9DuXlwOmzWEQPO3YpRrCD6s7pJMfNLuw = ", actualAuthenticityToken);
        }

        [Test]
        public void GetAuthenticityToken_GetsIcorrectPageContent_RetunsNull()
        {
            const string pageContent = @"<html>" +
                @"<div>" +
                @"<table>" +
                @"</table>" +
                @"</div>" +
                @"</html>";

            var extractor = GetExtractor();
            var expectedAuthenticityToken = extractor.GetAuthenticityToken(pageContent);

            Assert.IsNull(expectedAuthenticityToken);
        }

        [Test]
        public void GetAuthenticityToken_GetsNullAsPageContent_ThrowsAnArgumentException()
        {
            var extractor = GetExtractor();
            var expectedException = Assert.Catch<ArgumentException>(() => extractor.GetAuthenticityToken(null));

            StringAssert.Contains("отсутствует содержание html-страницы.", expectedException.Message);
        }
    }
}