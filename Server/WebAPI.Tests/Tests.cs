using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebAPI.Server.Models;
using WebAPI.Server.Controllers;

namespace WebAPI.Tests
{
    [TestClass]
    public class Tests
    {
        private static string key = "y8fN9sLekaKFNvi2apo409MxBv0e";

        [TestMethod]
        public void Get_ValidToken_ReturnsList()
        {
            var pController = new PartsController();

            IEnumerable<Part> parts = pController.Get(key);

            Assert.IsTrue(parts.Count() > 0);
        }

        [TestMethod]
        public void Get_InvalidToken_ReturnsEmptyList()
        {
            var pController = new PartsController();

            IEnumerable<Part> parts = pController.Get("badkey");

            Assert.IsTrue(parts.Count() == 0);
        }

        [TestMethod]
        public void Get_WithParams_ValidToken_MakeHonda_ReturnsList()
        {
            var pController = new PartsController();

            IEnumerable<Part> parts = pController.Get("1978", "Honda", "Frame", key);

            Assert.IsTrue(parts.Count() > 0);
        }

        [TestMethod]
        public void Get_WithParams_ValidToken_MakeKawasaki_ReturnsList()
        {
            var pController = new PartsController();

            IEnumerable<Part> parts = pController.Get("1978", "Kawasaki", "Frame", key);

            Assert.IsTrue(parts.Count() > 0);
        }

        [TestMethod]
        public void Get_WithParams_ValidToken_MakeSuzuki_ReturnsList()
        {
            var pController = new PartsController();

            IEnumerable<Part> parts = pController.Get("1978", "Suzuki", "Frame", key);

            Assert.IsTrue(parts.Count() > 0);
        }

        [TestMethod]
        public void Get_WithParams_ValidToken_MakeYamaha_ReturnsList()
        {
            var pController = new PartsController();

            IEnumerable<Part> parts = pController.Get("1978", "Yamaha", "Frame", key);

            Assert.IsTrue(parts.Count() > 0);
        }

        [TestMethod]
        public void Get_WithParams_InvalidValidToken_ReturnsEmptyList()
        {
            var pController = new PartsController();

            IEnumerable<Part> parts = pController.Get("1978", "Honda", "Frame", "badkey");

            Assert.IsTrue(parts.Count() == 0);
        }

        [TestMethod]
        public void Get_WithParams_YearOutOfRange_ReturnsList()
        {
            var pController = new PartsController();

            IEnumerable<Part> parts = pController.Get("1800", "Honda", "Frame", key);

            Assert.IsTrue(parts.Count() == 0);
        }

        [TestMethod]
        public void Get_WithParams_InvalidMake_ReturnsList()
        {
            var pController = new PartsController();

            IEnumerable<Part> parts = pController.Get("1978", "Invalid", "Frame", key);

            Assert.IsTrue(parts.Count() == 0);
        }

        [TestMethod]
        public void Get_WithParams_InvalidPartName_ReturnsList()
        {
            var pController = new PartsController();

            IEnumerable<Part> parts = pController.Get("1978", "Honda", "Invalid", key);

            Assert.IsTrue(parts.Count() == 0);
        }

        [TestMethod]
        public void GetYearSpinner_ValidToken_MakeHonda_ReturnsList()
        {
            var pController = new PartsController();

            IEnumerable<string> years = pController.GetYearSpinner("Honda", key);

            Assert.IsTrue(years.Count() > 0);
        }

        [TestMethod]
        public void GetYearSpinner_ValidToken_MakeKawasaki_ReturnsList()
        {
            var pController = new PartsController();

            IEnumerable<string> years = pController.GetYearSpinner("Kawasaki", key);

            Assert.IsTrue(years.Count() > 0);
        }

        [TestMethod]
        public void GetYearSpinner_ValidToken_MakeSuzuki_ReturnsList()
        {
            var pController = new PartsController();

            IEnumerable<string> years = pController.GetYearSpinner("Suzuki", key);

            Assert.IsTrue(years.Count() > 0);
        }

        [TestMethod]
        public void GetYearSpinner_ValidToken_MakeYamaha_ReturnsList()
        {
            var pController = new PartsController();

            IEnumerable<string> years = pController.GetYearSpinner("Yamaha", key);

            Assert.IsTrue(years.Count() > 0);
        }

        [TestMethod]
        public void GetYearSpinner_InvalidToken_ReturnsEmptyList()
        {
            var pController = new PartsController();

            IEnumerable<string> years = pController.GetYearSpinner("Honda", "badkey");

            Assert.IsTrue(years.Count() == 0);
        }

        [TestMethod]
        public void GetYearSpinner_InvalidMake_ReturnsEmptyList()
        {
            var pController = new PartsController();

            IEnumerable<string> years = pController.GetYearSpinner("Invalid", key);

            Assert.IsTrue(years.Count() == 0);
        }

        [TestMethod]
        public void GetPartNameSpinner_ValidToken_MakeHonda_ReturnsList()
        {
            var pController = new PartsController();

            IEnumerable<string> partNames = pController.GetPartNameSpinner("1978", "Honda", key);

            Assert.IsTrue(partNames.Count() > 0);
        }

        [TestMethod]
        public void GetPartNameSpinner_ValidToken_MakeKawasaki_ReturnsList()
        {
            var pController = new PartsController();

            IEnumerable<string> partNames = pController.GetPartNameSpinner("1978", "Kawasaki", key);

            Assert.IsTrue(partNames.Count() > 0);
        }

        [TestMethod]
        public void GetPartNameSpinner_ValidToken_MakeSuzuki_ReturnsList()
        {
            var pController = new PartsController();

            IEnumerable<string> partNames = pController.GetPartNameSpinner("1978", "Suzuki", key);

            Assert.IsTrue(partNames.Count() > 0);
        }

        [TestMethod]
        public void GetPartNameSpinner_ValidToken_MakeYamaha_ReturnsList()
        {
            var pController = new PartsController();

            IEnumerable<string> partNames = pController.GetPartNameSpinner("1978", "Yamaha", key);

            Assert.IsTrue(partNames.Count() > 0);
        }

        [TestMethod]
        public void GetPartNameSpinner_InvalidToken_ReturnsEmptyList()
        {
            var pController = new PartsController();

            IEnumerable<string> partNames = pController.GetPartNameSpinner("1978", "Honda", "badkey");

            Assert.IsTrue(partNames.Count() == 0);
        }

        [TestMethod]
        public void GetPartNameSpinner_InvalidYear_ReturnsEmptyList()
        {
            var pController = new PartsController();

            IEnumerable<string> partNames = pController.GetPartNameSpinner("1800", "Honda", key);

            Assert.IsTrue(partNames.Count() == 0);
        }

        [TestMethod]
        public void GetPartNameSpinner_InvalidMake_ReturnsEmptyList()
        {
            var pController = new PartsController();

            IEnumerable<string> partNames = pController.GetPartNameSpinner("1978", "Invalid", key);

            Assert.IsTrue(partNames.Count() == 0);
        }
    }
}
