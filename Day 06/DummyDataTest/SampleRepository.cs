﻿using Bogus;
using System;
using System.Collections.Generic;

namespace DummyDataTest
{
    public class SampleRepository
    {
        //가짜 데이터
        public IEnumerable<Customer> GetCustomers()
        {
            Randomizer.Seed = new Random(123456);
            var genCustomer = new Faker<Customer>()
                .RuleFor(r => r.Id, Guid.NewGuid)
                .RuleFor(r => r.Name, f => f.Company.CompanyName())
                .RuleFor(r => r.Address, f => f.Address.FullAddress())
                .RuleFor(r => r.Phone, f => f.Phone.PhoneNumber("010-####-####"))
                .RuleFor(r => r.ContactName, f => f.Name.FullName());

            return genCustomer.Generate(10);        //10개의 가짜 데이터 만들기
        }
    }
}
