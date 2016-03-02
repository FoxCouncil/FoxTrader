using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FoxTrader.UnitTests
{
    [TestClass]
    public class GameUnitTests
    {
        [TestMethod]
        [TestCategory("GeneratorTests")]
        public void NameIsNotNull()
        {
            var a_randomName = Game.Utils.Generator.Name();
            Assert.IsNotNull(a_randomName, "This should never be null.");
        }

        [TestMethod]
        [TestCategory("GeneratorTests")]
        public void NameHasLength()
        {
            var a_randomName = Game.Utils.Generator.Name();
            Assert.IsNotNull(a_randomName, "This should never be null.");
            Assert.IsTrue(a_randomName.Length > 3, "A random name should always be larger than 3 characters.");
        }

        [TestMethod]
        [TestCategory("PlayerTests")]
        public void DefaultPlayerHasName()
        {
            var a_newPlayer = new Game.Player();
            Assert.IsNotNull(a_newPlayer, "This should never be null.");
            Assert.IsNotNull(a_newPlayer.Name, "This should never be null.");
            Assert.IsTrue(a_newPlayer.Name.Length > 0, "A name should always be larger than 0 characters.");
        }

        [TestMethod]
        [TestCategory("PlayerTests")]
        public void PlayerKeepsName()
        {
            var a_randomName = "Quality";
            var a_newPlayer = new Game.Player(a_randomName);
            Assert.IsNotNull(a_newPlayer, "This should never be null.");
            Assert.IsNotNull(a_newPlayer.Name, "This should never be null.");
            Assert.IsTrue(a_newPlayer.Name == a_randomName, a_randomName + " should've been the name, but it wasn't.");
        }
    }
}
