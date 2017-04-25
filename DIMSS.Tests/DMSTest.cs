using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DIMSS.Model;
using System.Collections.Generic;

namespace DIMSS.Tests
{
    [TestClass]
    public class DMSTest
    {
        DMS _dms = new DMS();

        [TestInitialize]
        public void ClearMeasureParams()
        {
            _dms.MeasureParams.Clear();
        }

        [TestMethod]
        public void WhenSpectrumNotFixed_FixSpectrum_ShouldReturnFixedSpectrum()
        {
            // Arrange
            List<int> spectrum = new List<int>() { 1, 0, 4, -5, -32768, 32767, -32768, -32768, -32767, -15, -32767 };
            List<int> expected = new List<int>() { 1, 0, 4, -5, -32768, 32767, 32767, 32767, -32767, -15, -32767 };
            // Act
            _dms.FixSpectrum(spectrum);
            // Assert
            CollectionAssert.AreEqual(expected, spectrum);
        }

        [TestMethod]
        public void WhenSpectrumNotFull_FixSpectrum_ShouldReturnSameSpectrum()
        {
            // Arrange
            List<int> spectrum = new List<int>() { -32768 };
            List<int> expected = new List<int>() { -32768 };
            // Act
            _dms.FixSpectrum(spectrum);
            // Assert
            CollectionAssert.AreEqual(expected, spectrum);
        }
        
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void WhenSpectrumIsNull_FixSpectrum_ShouldThrowException()
        {
            // Arrange, Act, Assert
            _dms.FixSpectrum(null);
        }

        [TestMethod]
        public void WhenParamsAreNormal_ParseMeasureParams_ShouldReturnNormalParams()
        {
            // Arrange
            float fromV = 0.0f, toV = 0.0f;
            _dms.MeasureParams.Add(@"Param1=12;From,V = 0,003; To,V=0,15; Param2=,76");
            // Act
            _dms.ParseMeasureParams(0, ref fromV, ref toV);
            // Assert
            Assert.AreEqual(0.003, fromV, 1e-7);
            Assert.AreEqual(0.15, toV, 1e-7);
        }

        [TestMethod]
        public void WhenParamsInPointFormat_ParseMeasureParams_ShouldParseParamsNormally()
        {
            // Arrange
            float fromV = 0.0f, toV = 0.0f;
            _dms.MeasureParams.Add(@"Param1=12;From,V = 0.03; To,V=0.15; Param2=,76");
            // Act
            _dms.ParseMeasureParams(0, ref fromV, ref toV);
            // Assert
            Assert.AreEqual(0.03, fromV, 1e-7);
            Assert.AreEqual(0.15, toV, 1e-7);
        }

        [TestMethod]
        public void WhenParamIsAbsent_ParseMeasureParams_ShouldLeaveParamUnchanged()
        {
            // Arrange
            float fromV = 2.0f, toV = 0.0f;
            _dms.MeasureParams.Add(@"Param1=12; Param2=0,76");
            // Act
            _dms.ParseMeasureParams(0, ref fromV, ref toV);
            // Assert
            Assert.AreEqual(2.0, fromV, 1e-7);
            Assert.AreEqual(0.0, toV, 1e-7);
        }
    }
}
