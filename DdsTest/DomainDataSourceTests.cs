using DdsTest.Web.Services;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenRiaServices.Controls;

namespace DdsTest
{
    [TestClass]
    public class DomainDataSourceTests : SilverlightTest
    {
        private DomainDataSource _domainDataSource;
        private bool _isLoaded;
        private bool _hasError;

        [TestInitialize]
        public void Init()
        {
            _isLoaded = false;
            _hasError = false;
            _domainDataSource = new DomainDataSource
            {
                QueryName = "GetPeopleQuery",
                DomainContext = new PeopleContext()
            };
            _domainDataSource.LoadedData += (s, e) =>
            {
                _hasError = e.HasError;
                if (_hasError)
                    e.MarkErrorAsHandled();
                _isLoaded = true;
            };
            _domainDataSource.Load();
        }

        [TestMethod, Asynchronous]
        [Description("The service should return 2 entities")]
        public void EntitiesCountTest()
        {
            EnqueueConditional(() => _isLoaded);
            EnqueueCallback(
                () => Assert.IsFalse(_hasError),
                () => Assert.AreEqual(2, _domainDataSource.DataView.Count));
            EnqueueTestComplete();
        }

        [TestMethod, Asynchronous] 
        [Description("DomainDataSource.RejectChanges should return the removed entity to the DataView")]
        public void RejectChangesTest()
        {
            EnqueueConditional(() => _isLoaded);
            EnqueueCallback(() =>
            {
                var firstItem = _domainDataSource.DataView[0];
                _domainDataSource.DataView.Remove(firstItem);
                Assert.AreEqual(1, _domainDataSource.DataView.Count, "There should stay only 1 entity in the DataView");
                _domainDataSource.RejectChanges();
                Assert.AreEqual(2, _domainDataSource.DataView.Count, "The removed entity has not been restored");
            });
            EnqueueTestComplete();

        }

        [TestMethod, Asynchronous]
        [Description("DomainDataSource.RejectChanges should return the removed entity to the DataView" +
                     "even after DomainDataSource.SubmitChanges")]
        public void RejectChangesAfterSubmitTest()
        {
            var changesSubmitted = false;
            var hasSubmitError = false;
            EnqueueConditional(() => _isLoaded);
            EnqueueCallback(() =>
            {
                var firstItem = _domainDataSource.DataView[0];
                _domainDataSource.DataView.Remove(firstItem);
                _domainDataSource.SubmittedChanges += (s, e) =>
                {
                    hasSubmitError = e.HasError;
                    if (e.HasError)
                        e.MarkErrorAsHandled();
                    changesSubmitted = true;
                };
                _domainDataSource.SubmitChanges();
            });
            EnqueueConditional(() => changesSubmitted);
            EnqueueCallback(() =>
            {
                Assert.IsFalse(hasSubmitError, "There should be no submit errors");
                Assert.AreEqual(1, _domainDataSource.DataView.Count, "There should be only 1 entity in the dds after successful submit");

                var firstItem = _domainDataSource.DataView[0];
                _domainDataSource.DataView.Remove(firstItem);
                Assert.AreEqual(0, _domainDataSource.DataView.Count, "There should be no entities in the dds");
                _domainDataSource.RejectChanges();
                //((ICollectionView)_domainDataSource.DataView).Refresh();
                Assert.AreEqual(1, _domainDataSource.DataView.Count, "The removed entity has not been restored");
            });
            EnqueueTestComplete();
        }
    }
}
