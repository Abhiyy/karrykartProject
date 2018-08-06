using AppBanwao.KarryKart.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KarryKart.API.Models
{
    public class UserModel
    {
        public string user { get; set; }

        public string pwd { get; set; }
    }

    public class UserInformation
    {
        public string Name { get; set; }
        public Guid UserID { get; set; }
        public Guid Token { get; set; }
        public DateTime ExpiryDateTime { get; set; }
        public string Error { get; set; }
    }

    public class UserSignUpModel:UserModel {
        public string Name { get; set; }
        public Guid UserID { get; set; }
        public string Message { get; set; }
        public string Otp { get; set; }
    }

    public class UserAddress {

        public int AddressID { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        //public int CityID { get; set; }
        //public int StateID { get; set; }
        //public int CountryID { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string LandMark { get; set; }
        public string PinCode { get; set; }

        public UserAddress() { }
        public UserAddress(UserAddressDetail address,karrykartEntities context)
        {
            this.AddressID = address.AddressID;
            this.AddressLine1 = address.AddressLine1;
            this.AddressLine2 = address.AddressLine2;
            this.City = context.refCities.Find(address.CityID).Name;
            this.Country = context.Countries.Find(address.CountryID).CountryName;
            this.LandMark = address.Landmark;
            this.PinCode = address.Pincode;
            this.State = context.refStates.Find(address.StateID).Name;
            //this.StateID = address.StateID.Value;
            //this.CountryID = address.CountryID.Value;
            //this.CityID = address.CityID.Value;
        }
    }

    public class UserDetails {

        public Guid UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email {get;set;}
        public string Phone { get; set; }

        
        public List<UserAddress> AddressList { get; set; }

        public UserDetails() { }

        public UserDetails(User user, karrykartEntities context)
        {
            this.UserID = user.UserID;
            this.Email = user.EmailAddress;
            this.FirstName = user.UserDetails.First().FirstName;
            this.LastName = user.UserDetails.First().LastName;
            this.Phone = user.Mobile;

            this.AddressList = new List<UserAddress>();
            if (user.UserAddressDetails.Count > 0)
            {
                foreach (var userAddress in user.UserAddressDetails)
                {
                    if(!string.IsNullOrEmpty(userAddress.AddressLine1))
                        this.AddressList.Add(new UserAddress(userAddress, context));
                }
            }
        }

    }

    public class AddUserAddressModel {
        public Guid UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public int CityID { get; set; }
        public int StateID { get; set; }
        public int CountryID { get; set; }
        public string LandMark { get; set; }
        public string PinCode { get; set; }
    }
}