using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DIMSS.Model;
using System.Collections.Generic;

namespace DIMSSTests
{
    [TestClass]
    public class DMSModelTest
    {
        DMSModel model = new DMSModel();

        [TestMethod]
        public void FixSpectrum_WhenSpectrumNotFixed_ShouldReturnFixedSpectrum()
        {
            // Arrange
            List<int> spectrum = new List<int>() { 1,0,4,-5,-32768, 32767, -32768, -32768, -32767, -15, -32767 };
            List<int> expected = new List<int>() { 1, 0, 4, -5, -32768, 32767, 32767, 32767, -32767, -15, -32767 };
            // Act
            model.FixSpectrum( spectrum );
            // Assert
            CollectionAssert.AreEqual(expected, spectrum);
        }

        [TestMethod]
        public void FixSpectrum_WhenSpectrumNotFull_ShouldReturnSameSpectrum()
        {
            // Arrange
            List<int> spectrum = new List<int>() { -32768 };
            List<int> expected = new List<int>() { -32768 };
            // Act
            model.FixSpectrum(spectrum);
            // Assert
            CollectionAssert.AreEqual(expected, spectrum);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void FixSpectrum_WhenSpectrumIsNull_ShouldThrowException()
        {
            // Arrange
            List<int> nullspectrum = null;
            // Act, Assert
            model.FixSpectrum(nullspectrum);
        }

        [TestMethod]
        public void ParseMeasureParams_WhenParamsNormal_ShouldReturnNormalParams()
        {
            // Arrange
            float fromV = 0.0f, toV = 0.0f;
            model.MeasureParams.Add(@"Param1=12;From,V = 0,003; To,V=0,15; Param2=,76");
            // Act
            model.ParseMeasureParams(0, ref fromV, ref toV);
            // Assert
            Assert.AreEqual(0.003, fromV, 1e-7);
            Assert.AreEqual(0.15, toV, 1e-7);
        }

        [TestMethod]
        public void ParseMeasureParams_WhenParamsInPointFormat_ShouldParseParamsNormally()
        {
            // Arrange
            float fromV = 0.0f, toV = 0.0f;
            model.MeasureParams.Add(@"Param1=12;From,V = 0.03; To,V=0.15; Param2=,76");
            // Act
            model.ParseMeasureParams(0, ref fromV, ref toV);
            // Assert
            Assert.AreEqual(0.03, fromV, 1e-7);
            Assert.AreEqual(0.15, toV, 1e-7);
        }

        [TestMethod]
        public void ParseMeasureParams_WhenParamIsAbsent_ShouldLeaveParamUnchanged()
        {
            // Arrange
            float fromV = 2.0f, toV = 0.0f;
            model.MeasureParams.Add(@"Param1=12; Param2=0,76");
            // Act
            model.ParseMeasureParams(0, ref fromV, ref toV);
            // Assert
            Assert.AreEqual(2.0, fromV, 1e-7);
            Assert.AreEqual(0.0, toV, 1e-7);
        }
    }
}
