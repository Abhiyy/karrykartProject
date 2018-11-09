using AppBanwao.KarryKart.Model;
using AppBanwao.KaryKart.Web.Helpers;
using DA.EncryptionManager;
using KarryKart.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KarryKart.API.Helpers
{
    public class LoginHelper
    {
        karrykartEntities _context = null;
        APIEmailHelper _emailHelper = null;

        public UserInformation LoginUser(UserModel model)
        {
            var userInfo = new UserInformation();
            if (ValidateUser(model))
            {
                using (_context = new karrykartEntities())
                {
                    var user = _context.Users.Where(x => x.EmailAddress == model.user).FirstOrDefault();
                    if (user.UserLogins.Count==0)
                    {
                        var userLogin = new UserLogin()
                        {
                            LoginTime = DateTime.Now,
                            Token = Guid.NewGuid(),
                            TokenExpiry = DateTime.Now.AddDays(15),
                            UserID = user.UserID
                        };
                        _context.UserLogins.Add(userLogin);
                        _context.SaveChanges();

                        userInfo.ExpiryDateTime = userLogin.TokenExpiry.Value;
                        userInfo.Token = userLogin.Token.Value;
                        userInfo.UserID = userLogin.UserID.Value;
                        userInfo.Name = model.user;//(user.UserDetails != null) ? user.UserDetails.FirstOrDefault().FirstName : model.user;

                    }
                }
            }
            else {
                userInfo.Error = "invalid_user";
            }

            return userInfo;
        }

        public bool ValidateUser(UserModel model)
        {
            using (_context = new karrykartEntities())
            {
                var p=EncryptionManager.ConvertToUnSecureString(EncryptionManager.EncryptData(model.pwd));
                return _context.Users.Any(x => x.EmailAddress == model.user && x.Password == p);
            }
        }

        public UserInformation CheckLogin(UserInformation user)
        {
            using (_context = new karrykartEntities()) {
                
                var userLogin = _context.UserLogins.Where(ul => ul.UserID == user.UserID && ul.Token == user.Token).FirstOrDefault();
                if (userLogin != null)
                {
                    if ((user.ExpiryDateTime - userLogin.TokenExpiry.Value).Days <= 15)
                    {
                    
                        userLogin.TokenExpiry = DateTime.Now.AddDays(15);
                        _context.Entry(userLogin).State = System.Data.Entity.EntityState.Modified;
                        _context.SaveChanges();
                        user.ExpiryDateTime = userLogin.TokenExpiry.Value;
                    }
                    else {
                        user.Error = "token_expired";
                    }
                }
            }
            return user;
        }

        

        public bool Logout(Guid UserID, Guid token)
        {
            using (_context = new karrykartEntities())
            {
                var userLoginEntry = _context.UserLogins.Where(l => l.Token == token && l.UserID == UserID).FirstOrDefault();
                if (userLoginEntry != null)
                {
                    _context.Entry(userLoginEntry).State = System.Data.Entity.EntityState.Deleted;
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public User SignUpUser(UserSignUpModel user)
        {
            _context = new karrykartEntities();
            if (!(_context.Users.Where(x => x.EmailAddress == user.user ).Count() > 0))
            {
                var userToCreate = new User()
                {
                    DateCreated = DateTime.Now,
                    EmailAddress = user.user,
                   // Mobile = CommonHelper.IsMobile(model.UserIdentifier) ? model.UserIdentifier : string.Empty,
                    Password = EncryptionManager.ConvertToUnSecureString(EncryptionManager.EncryptData(user.pwd)),
                    UserID = Guid.NewGuid(),
                    LastUpdated = DateTime.Now,
                    RoleID = CommonHelper.CustomerType.Customer,
                    Active = false,
                    ProfileComplete = false
                };
                var userDet = new UserDetail()
                {
                    UserID = userToCreate.UserID,
                    FirstName = user.Name.Split(' ')[0],
                    LastName = user.Name.Split(' ').Length>1?user.Name.Split(' ')[1]:null
                };

                var userAddress = new UserAddressDetail()
                {
                    UserID = userToCreate.UserID
                };


                _context.Users.Add(userToCreate);
                _context.UserDetails.Add(userDet);
                _context.UserAddressDetails.Add(userAddress);
                _context.SaveChanges();
                return userToCreate;
            }

            _context = null;

            return null;
        }

        public string SendRegisterUserConfirmation(User user)
        {
            string userRegisterWith = null;
            if (user != null)
            {
                if (!(string.IsNullOrEmpty(user.Mobile)))
                {
                //    SmsHelper.SendRegisterMessage(user.Mobile);
                    userRegisterWith = ApplicationMessages.UserRegisterationType.MOBILE;
                }
                if (!(string.IsNullOrEmpty(user.EmailAddress)))
                {
                    _emailHelper = new APIEmailHelper();

                    if (!_emailHelper.SendRegisterEmail(user.EmailAddress,"Hunger's Click"))
                        userRegisterWith = ApplicationMessages.UserRegisterationType.EMAILWITHERROR;
                    else
                        userRegisterWith = ApplicationMessages.UserRegisterationType.EMAIL;

                    _emailHelper = null;
                }
            }
            else
            {
                userRegisterWith = ApplicationMessages.UserRegisterationType.USEREXIST;
            }
            return userRegisterWith;
        }

        public bool VerifyOtp(UserSignUpModel model,bool NeedToSendEmail=true)
        {
            _context = new karrykartEntities();

            var otp = _context.OTPHolders.Where(x => x.OTPAssignedTo == model.user && x.OTPValue == model.Otp).FirstOrDefault();

            if (otp != null)
            {
                var user = _context.Users.Where(u => u.EmailAddress == model.user ).FirstOrDefault();
                user.LastUpdated = DateTime.Now;
                user.Active = true;
                _context.Entry(user).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();
                CommonHelper.RemoveOTP(model.user);
                if(NeedToSendEmail)
                return SendOtpVerificationToUser(user);
            }

            return false;

        }

        public bool VerfiyOtpForgotPassword(UserSignUpModel model) {
            _context = new karrykartEntities();

            var otp = _context.OTPHolders.Where(x => x.OTPAssignedTo == model.user && x.OTPValue == model.Otp).FirstOrDefault();
            if(otp!=null)
            {
                CommonHelper.RemoveOTP(model.user);
                _context = null;
                return true;
            }

            return false;
        }

        bool SendOtpVerificationToUser(User user)
        {
            if (!(string.IsNullOrEmpty(user.Mobile)))
            {
              //  SmsHelper.SendVerificationMessage(user.Mobile);
                return true;
            }
            if (!(string.IsNullOrEmpty(user.EmailAddress)))
            {
                _emailHelper = new APIEmailHelper();

                if (_emailHelper.SendVerificationEmail(user.EmailAddress,"Hunger's Click"))
                {
                    _emailHelper = null;
                    return true;
                }
                else
                {
                    _emailHelper = null;
                    return false;
                }

            }
            return false;
        }

        public bool ForgotPassword(string Email)
        {
            using (_context = new karrykartEntities()) { 
                var usr =_context.Users.Where(x=>x.EmailAddress==Email).FirstOrDefault();
                if (usr != null) { 
                _emailHelper  = new APIEmailHelper();
                string otp = Email.Substring(0,4)+CommonHelper.GenerateOTP();
                CommonHelper.SaveOTP(otp,Email);
                if(_emailHelper.SendOtpEmail(Email,otp,"Hunger's Click")){
                return true;
                }

                }
            }

            return false;
        }

        public bool ChangePassword(UserSignUpModel userModel) {
            using (_context = new karrykartEntities()) {
                var user = _context.Users.Where(u => u.EmailAddress == userModel.user).FirstOrDefault();

                if (user != null) {
                    user.Password = EncryptionManager.ConvertToUnSecureString(EncryptionManager.EncryptData(userModel.pwd));
                    user.LastUpdated = DateTime.Now;
                    _context.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public UserDetails GetUser(Guid Id,int AddressID=-1)
        {
            using (_context = new karrykartEntities()) {

                var usr = _context.Users.Find(Id);
                if (usr != null) {
                    return new UserDetails(usr,_context,AddressID);
                }
            }
            return null;

        }

        public UserDetails AddUserAddress(AddUserAddressModel user)
        {
            using (_context = new karrykartEntities()) {
                if (!user.GuestCheckout)
                {
                    var usr = _context.Users.Find(user.UserID);

                    if (usr != null)
                    {

                        if (string.IsNullOrEmpty(usr.EmailAddress))
                        {
                            usr.EmailAddress = user.Email;
                        }

                        usr.Mobile = user.Phone;
                        usr.LastUpdated = DateTime.UtcNow;
                        usr.ProfileComplete = true;
                        _context.Entry(usr).State = System.Data.Entity.EntityState.Modified;
                    }

                    var userDetails = _context.UserDetails.Where(x => x.UserID == user.UserID).FirstOrDefault();
                    if (userDetails != null)
                    {
                        if (string.IsNullOrEmpty(userDetails.FirstName))
                        {
                            userDetails.FirstName = user.FirstName;
                            userDetails.LastName = user.LastName;
                            _context.Entry(userDetails).State = System.Data.Entity.EntityState.Modified;
                            _context.SaveChanges();

                        }
                    }

                    var userAddress = _context.UserAddressDetails.Where(x => x.UserID == user.UserID).FirstOrDefault();

                    if (userAddress != null)
                    {
                        if (string.IsNullOrEmpty(userAddress.AddressLine1))
                        {
                            userAddress.AddressLine1 = user.AddressLine1;
                            userAddress.AddressLine2 = user.AddressLine2;
                            userAddress.CityID = user.CityID;
                            userAddress.StateID = user.StateID;
                            userAddress.CountryID = user.CountryID;
                            userAddress.Landmark = user.LandMark;
                            userAddress.Pincode = user.PinCode;
                            _context.Entry(userAddress).State = System.Data.Entity.EntityState.Modified;
                            _context.SaveChanges();
                        }
                        else
                        {
                            _context.UserAddressDetails.Add(new UserAddressDetail()
                            {
                                AddressLine1 = user.AddressLine1,
                                AddressLine2 = user.AddressLine2,
                                CityID = user.CityID,
                                CountryID = user.CountryID,
                                Landmark = user.LandMark,
                                Pincode = user.PinCode,
                                StateID = user.StateID,
                                UserID = user.UserID

                            });
                            _context.SaveChanges();
                        }

                    }
                    else
                    {
                        _context.UserAddressDetails.Add(new UserAddressDetail()
                        {
                            AddressLine1 = user.AddressLine1,
                            AddressLine2 = user.AddressLine2,
                            CityID = user.CityID,
                            CountryID = user.CountryID,
                            Landmark = user.LandMark,
                            Pincode = user.PinCode,
                            StateID = user.StateID,
                            UserID = user.UserID

                        });
                        _context.SaveChanges();
                    }
                }
                else {
                    var guestUser = new GuestUserDetail() { 
                                    ID = Guid.NewGuid(),
                                    AddressLine1 = user.AddressLine1,
                                    AddressLine2 = user.AddressLine2,
                                    CityID = user.CityID,
                                    CountryID = user.CountryID,
                                    EmailAddress = user.Email,
                                    FirstName = user.FirstName,
                                    LandMark = user.LandMark,
                                    LastName = user.LastName,
                                    Phone = user.Phone,
                                    Pincode = user.PinCode,
                                    StateID = user.StateID
                    };
                    _context.GuestUserDetails.Add(guestUser);
                    _context.SaveChanges();

                    return GetGuestUserDetails(guestUser.ID);

                }
            }

            return GetUser(user.UserID);
        }

        public UserDetails GetGuestUserDetails(Guid GuestUserID) {
            using (_context = new karrykartEntities())
            {
                var guestUser = _context.GuestUserDetails.Find(GuestUserID);
                if (guestUser != null)
                {
                    var userDetail = new UserDetails()
                    {
                        Email = guestUser.EmailAddress,
                        FirstName = guestUser.FirstName,
                        LastName = guestUser.LastName,
                        Phone = guestUser.Phone,
                        UserID = guestUser.ID
                    };

                    userDetail.AddressList = new List<UserAddress>();
                    userDetail.AddressList.Add(new UserAddress()
                    {
                        AddressLine1 = guestUser.AddressLine1,
                        AddressLine2 = guestUser.AddressLine2,
                        City = _context.refCities.Find(guestUser.CityID.Value).Name,
                        CityID = guestUser.CityID.Value,
                        State = _context.refStates.Find(guestUser.StateID.Value).Name,
                        StateID = guestUser.StateID.Value,
                        Country = _context.Countries.Find(guestUser.CountryID.Value).CountryName,
                        CountryID = guestUser.CountryID.Value,
                        LandMark = guestUser.LandMark,
                        PinCode = guestUser.Pincode
                    });

                    return userDetail;
                }
                else
                {
                    return null;
                }
            }
        }

        public UserDetails EditUserAddress(AddUserAddressModel user)
        {
            using (_context = new karrykartEntities())
            {
                if (!user.GuestCheckout)
                {
                    var usr = _context.Users.Find(user.UserID);

                    if (usr != null)
                    {

                        if (string.IsNullOrEmpty(usr.EmailAddress))
                        {
                            usr.EmailAddress = user.Email;
                        }

                        usr.Mobile = user.Phone;
                        usr.LastUpdated = DateTime.UtcNow;
                        usr.ProfileComplete = true;
                        _context.Entry(usr).State = System.Data.Entity.EntityState.Modified;
                    }

                    var userDetails = _context.UserDetails.Where(x => x.UserID == user.UserID).FirstOrDefault();
                    if (userDetails != null)
                    {
                        if (string.IsNullOrEmpty(userDetails.FirstName))
                        {
                            userDetails.FirstName = user.FirstName;
                            userDetails.LastName = user.LastName;
                            _context.Entry(userDetails).State = System.Data.Entity.EntityState.Modified;
                            _context.SaveChanges();

                        }
                    }

                    var userAddress = _context.UserAddressDetails.Where(x => x.UserID == user.UserID && x.AddressID == user.AddressID).FirstOrDefault();

                    if (userAddress != null)
                    {

                        userAddress.AddressLine1 = user.AddressLine1;
                        userAddress.AddressLine2 = user.AddressLine2;
                        userAddress.CityID = user.CityID;
                        userAddress.StateID = user.StateID;
                        userAddress.CountryID = user.CountryID;
                        userAddress.Landmark = user.LandMark;
                        userAddress.Pincode = user.PinCode;
                        _context.Entry(userAddress).State = System.Data.Entity.EntityState.Modified;
                        _context.SaveChanges();


                    }

                }
                else {
                    var guestUser = _context.GuestUserDetails.Find(user.UserID);
                    if (guestUser != null) {
                        guestUser.AddressLine1 = user.AddressLine1;
                        guestUser.AddressLine2 = user.AddressLine2;
                        guestUser.CityID = user.CityID;
                        guestUser.CountryID = user.CountryID;
                        guestUser.EmailAddress = user.Email;
                        guestUser.FirstName = user.FirstName;
                        guestUser.LandMark = user.LandMark;
                        guestUser.LastName = user.LastName;
                        guestUser.Phone = user.Phone;
                        guestUser.Pincode = user.PinCode;
                        guestUser.StateID = user.StateID;
                        _context.Entry(guestUser).State = System.Data.Entity.EntityState.Modified;
                        _context.SaveChanges();
                        return GetGuestUserDetails(guestUser.ID);
                    }
                }
            }

            return GetUser(user.UserID);
        }

        public UserDetails RemoveUserAddress(Guid Id, int AddressID = -1)
        {
            using (_context = new karrykartEntities())
            {
                var userAddress = _context.UserAddressDetails.Where(x => x.UserID == Id && x.AddressID==AddressID).FirstOrDefault();

                if (userAddress != null)
                {
                    _context.Entry(userAddress).State = System.Data.Entity.EntityState.Deleted;
                    _context.SaveChanges();
                   

                }
            }

            return GetUser(Id);
        }
    }
}