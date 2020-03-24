using ApplicantTrackingSystem.controller;
using ApplicantTrackingSystem.dto;
using System;

namespace ApplicantTrackingSystem.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            Person p = new Person();

            p.FirstName = "Andrew";
            p.MiddleName = "Michael";
            p.LastName = "Carson";
            p.Email = "andrewcarson@yahoo.com";
            p.SocialSecurityNumber = "111223333";

            ApplicantTrackingSystemController a = new ApplicantTrackingSystemController();
            a.PlaceApplicantOnQueue(p);
        }
    }
}
