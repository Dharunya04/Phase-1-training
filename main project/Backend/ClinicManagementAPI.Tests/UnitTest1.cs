namespace ClinicManagementAPI.Tests;
using Moq;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using ClinicManagementAPI.Context;
using ClinicManagementAPI.Models;
using ClinicManagementAPI.Service;
using ClinicManagementAPI.DTOs;
using System;
using System.Linq;

public class Tests
{[TestFixture]
    public class DoctorRepoSimpleTests
    {
        private Mock<IDoctorRepo> _doctorRepoMock;

        [SetUp]
        public void Setup()
        {
            _doctorRepoMock = new Mock<IDoctorRepo>();

            _doctorRepoMock.Setup(repo => repo.GetDoctorById(1)).Returns(new Doctor
            {
                DoctorID = 1,
                DoctorName = "Dr. John",
                Specialization = "Cardiologist",
                Age = 40,
                Email = "john@example.com",
                PhoneNumber = "9876543210",
                ConsultantFee = 500
            });

            _doctorRepoMock.Setup(repo => repo.GetDoctorById(It.Is<int>(id => id != 1)))
                           .Returns((Doctor)null);
            _doctorRepoMock.Setup(repo => repo.CompleteAppointment(1)).Returns(true);
            _doctorRepoMock.Setup(repo => repo.CompleteAppointment(99)).Returns(false);
        }

        [Test]
        public void GetDoctorById_ShouldReturnDoctor_WhenDoctorExists()
        {
            var doctor = _doctorRepoMock.Object.GetDoctorById(1);
            Assert.That(doctor, Is.Not.Null);
            Assert.That(doctor.DoctorName, Is.EqualTo("Dr. John"));
        }

        [Test]
        public void GetDoctorById_ShouldReturnNull_WhenDoctorDoesNotExist()
        {
            var doctor = _doctorRepoMock.Object.GetDoctorById(99);
            Assert.That(doctor, Is.Null);
        }

        [Test]
        public void CompleteAppointment_ShouldReturnTrue_WhenAppointmentExists()
        {
            var result = _doctorRepoMock.Object.CompleteAppointment(1);
            Assert.That(result, Is.True);
        }

        [Test]
        public void CompleteAppointment_ShouldReturnFalse_WhenAppointmentDoesNotExist()
        {
            var result = _doctorRepoMock.Object.CompleteAppointment(99);
            Assert.That(result, Is.False);
        }
    }
}


