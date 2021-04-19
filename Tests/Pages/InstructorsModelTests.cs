using ContosoUniversityWithRazor.Pages.Instructors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Pages
{
    [TestClass] public class InstructorsModelTests
    {
        private InstructorsModel m;

        [TestInitialize]
        public void TestInitialize()
        {
            m = new InstructorsModel(null);
        }

        [TestMethod]
        public void OnGetDeleteAsyncTestNotFound()
        {
            var r = m.OnGetDeleteAsync(null).GetAwaiter().GetResult();
            Assert.IsInstanceOfType(r,typeof(NotFoundResult));
        }

    }
}
