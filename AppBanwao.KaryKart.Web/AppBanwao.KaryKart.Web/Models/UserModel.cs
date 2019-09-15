using AppBanwao.KarryKart.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace AppBanwao.KaryKart.Web.Models
{
    public class UserModel
    {
        public Guid UserID { get; set; }

        public string EmailAddress { get; set; }

        public string Mobile { get; set; }

        public DateTime Datecreated { get; set; }

        public DateTime LastUpdated { get; set; }

        public int RoleID { get; set; }

        public bool Active { get; set; }

        public bool ProfileComplete { get; set;}

        public int UserDetailsID { get; set; }
        
        public string Salutation { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int AddressID { get; set; }
        
        public string AddressLine1 { get; set; }
        
        public string AddressLine2 { get; set; }
        
        public Nullable<int> CityID { get; set; }
        public Nullable<int> StateID { get; set; }
        public Nullable<int> CountryID { get; set; }
        public string Pincode { get; set; }
        public string Landmark { get; set; }

        public List<Country> countries { get; set; }

        public List<refSaluation> salutations { get; set; }

        public List<refCity> cities { get; set; }

        public List<refState> states { get; set; }

    }
    public class UserInformation
    {
        public string Name { get; set; }
        public Guid UserID { get; set; }
        public Guid Token { get; set; }
        public DateTime ExpiryDateTime { get; set; }
        public string Error { get; set; }
    }

    public class UserSignUpModel : UserModel
    {
        public string Name { get; set; }
        public Guid UserID { get; set; }
        public string Message { get; set; }
        public string Otp { get; set; }
    }

    public class UserAddress
    {

        public int AddressID { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public int CityID { get; set; }
        public int StateID { get; set; }
        public int CountryID { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string LandMark { get; set; }
        public string PinCode { get; set; }

        public UserAddress() { }
        public UserAddress(UserAddressDetail address, karrykartEntities context)
        {
            this.AddressID = address.AddressID;
            this.AddressLine1 = address.AddressLine1;
            this.AddressLine2 = address.AddressLine2;
            this.City = context.refCities.Find(address.CityID).Name;
            this.Country = context.Countries.Find(address.CountryID).CountryName;
            this.LandMark = address.Landmark;
            this.PinCode = address.Pincode;
            this.State = context.refStates.Find(address.StateID).Name;
            this.StateID = address.StateID.Value;
            this.CountryID = address.CountryID.Value;
            this.CityID = address.CityID.Value;
        }
    }

    public class UserDetails
    {

        public Guid UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }


        public List<UserAddress> AddressList { get; set; }

        public UserDetails() { }

        public UserDetails(User user, karrykartEntities context, int AddressID = -1)
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
                    if (AddressID != -1)
                    {
                        if (userAddress.AddressID == AddressID)
                            if (!string.IsNullOrEmpty(userAddress.AddressLine1))
                                this.AddressList.Add(new UserAddress(userAddress, context));
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(userAddress.AddressLine1))
                            this.AddressList.Add(new UserAddress(userAddress, context));
                    }
                }
            }
        }

    }

    public class AddUserAddressModel
    {
        public Guid UserID { get; set; }
        public int AddressID { get; set; }
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
        public bool GuestCheckout { get; set; }
    }
}