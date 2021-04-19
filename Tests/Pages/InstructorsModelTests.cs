using ContosoUniversityWithRazor.Pages.Instructors;
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
        public void OnGetDeleteAsyncTest()
        {
            dynamic r = m.OnGetDeleteAsync(null).GetAwaiter().GetResult();
            Assert.IsNotNull(r);
        }

    }
}
