using System;

namespace Trains.Tests.Trains.Services
{
    [TestClass]
    public class CheckTrainTest
    {
        [TestMethod]
        public void CheckDate_ShouldReturnTrue()
        {
            // arrange
            var check = new CheckTrain();
            var date = DateTimeOffset.Now.AddDays(50);
            const bool expected = true;

            // act
            var actual = check.CheckDate(date);

            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckDate_ShouldReturnFalse()
        {
            // arrange
            var check = new CheckTrain();
            var date = DateTimeOffset.Now;
            const bool expected = false;

            // act
            var actual = check.CheckDate(date);

            // assert
            Assert.AreEqual(expected, actual);
        }
    }
}
