using _02_ClaimsClasses;
using _02_ClaimsClasses.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsConsole
{
    public class ProgramUI
    {
        private readonly IClaimRepository _repo;
        public ProgramUI(IClaimRepository repo)
        {
            _repo = repo;
        }
        public void Run()
        {
            while (Menu()) {}
        }
        private bool Menu()
        {
            Console.WriteLine("Komodo Claims Software:\n" +
                "1. Enter a new claim\n" +
                "2. Take care of the next claim\n" +
                "3. See all the claims\n" +
                "4. Exit");
            string userKey = Console.ReadLine();
            switch (userKey)
            {
                case "1":
                    _repo.AddClaim(GetUserClaim());
                    break;
                case "2":
                    NextClaim();
                    break;
                case "3":
                    DisplayClaims();
                    break;
                case "4":
                    return false;
                default:
                    Console.WriteLine("Invalid input, Please try again.");
                    return true;
            }

            return true;
        }
        private Claim GetUserClaim()
        {
            Claim userClaim = new Claim();
            Console.WriteLine("Claim ID:");
            userClaim.ClaimId = int.Parse(Console.ReadLine());
            Console.WriteLine("Which Claim type is it?");
            int enumCounter = 1;
            foreach (ClaimType claimType in Enum.GetValues(typeof(ClaimType)))
            {
                Console.WriteLine($"{enumCounter}: {claimType}");
                enumCounter++;
            }
            string enumCast = Console.ReadLine();
            switch (enumCast)
            {
                case "1":
                    userClaim.TypeOfClaim = ClaimType.Car;
                    break;
                case "2":
                    userClaim.TypeOfClaim = ClaimType.Home;
                    break;
                case "3":
                    userClaim.TypeOfClaim = ClaimType.Theft;
                    break;
                default:
                    Console.WriteLine("Please try again.");
                    return null;
            }
            Console.WriteLine("What's the description of the claim?");
            userClaim.Description = Console.ReadLine();
            Console.WriteLine("What's the claim amount?");
            userClaim.ClaimAmount = decimal.Parse(Console.ReadLine());
            Console.WriteLine("What was the date of the incident? (ex: 08/18/2018)");
            userClaim.DateOfIncident = DateTime.Parse(Console.ReadLine());
            Console.WriteLine("What is the date of the claim? (ex: 08/18/2018)");
            userClaim.DateOfClaim = DateTime.Parse(Console.ReadLine());
            userClaim.IsValid = IsClaimValid(userClaim);
            return userClaim;
        }
        private bool IsClaimValid(Claim claimToTest)
        {
            TimeSpan claimTimeSpan = claimToTest.DateOfClaim - claimToTest.DateOfIncident;
            if (claimTimeSpan.Days > 30)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private bool NextClaim()
        {
            
            Claim peekClaim = _repo.NextClaim();
            Console.WriteLine($"\nClaimID: {peekClaim.ClaimId}\n" +
               $"Type: {peekClaim.TypeOfClaim}\n" +
               $"Description: {peekClaim.Description}\n" +
               $"Amount: {peekClaim.ClaimAmount}\n" +
               $"DateOfAccident: {peekClaim.DateOfIncident}\n" +
               $"DateOfClaim: {peekClaim.DateOfClaim}\n" +
               $"IsValid: {peekClaim.IsValid}\n");
            Console.WriteLine("Do you want to deal with this claim now?(y/n)");
            string dealWithClaim = Console.ReadLine();
            if (dealWithClaim == "y")
            {
                _repo.RemoveClaim(peekClaim);
                return true;
            }
            else
            {
                return false;
            }
        }
        private void DisplayClaims()
        {
            Console.WriteLine("ClaimID\tType\tDescription\t\tAmount\tDateOfAccidnet\tDateOfClaim\tIsValid");
            foreach (Claim claim in _repo.GetClaims())
            {
                Console.WriteLine($"{claim.ClaimId}\t{claim.TypeOfClaim}\t{claim.Description}\t\t{claim.ClaimAmount}\t{claim.DateOfIncident.ToString("MM/dd/yyyy")}\t{claim.DateOfClaim.ToString("MM/dd/yyyy")}\t{claim.IsValid}");
            }
        }
    }
}