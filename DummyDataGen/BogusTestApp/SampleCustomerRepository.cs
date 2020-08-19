using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BogusTestApp
{
    public class SampleCustomerRepository
    {
        public IEnumerable<Customer> GetCustomers()
        {
            Randomizer.Seed = new Random(123456);
            var genOrder = new Faker<Order>()
                .RuleFor(o => o.Id, Guid.NewGuid)
                .RuleFor(o => o.Date, f => f.Date.Past(3)) // 년도 기준으로 (?)년전부터 현재까지의 날짜 중에서
                .RuleFor(o => o.OrderValue, f => f.Finance.Amount(5000, 150000))
                .RuleFor(o => o.Shipped, f => f.Random.Bool(0.9f)); // 10개 중에서 9개는 true

            var genCustomer = new Faker<Customer>()
                .RuleFor(c => c.Id, Guid.NewGuid)
                .RuleFor(c => c.Name, f => f.Company.CompanyName())
                .RuleFor(c => c.Address, f => f.Address.FullAddress())
                .RuleFor(c => c.Phone, f => f.Phone.PhoneNumber("010-####-####")) // 010으로 된 랜덤번호
                .RuleFor(c => c.ContactName, f => f.Name.FullName())
                .RuleFor(c => c.Orders, f => genOrder.Generate(f.Random.Number(1, 5)).ToList());

            return genCustomer.Generate(10);
        }
    }
}
