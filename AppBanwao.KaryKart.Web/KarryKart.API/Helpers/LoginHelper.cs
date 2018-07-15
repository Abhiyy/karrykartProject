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
                userInfo.Error = "Invalid username/password. Please try again.";
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
                    SmsHelper.SendRegisterMessage(user.Mobile);
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

        public bool VerifyOtp(UserSignUpModel model)
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
                return SendOtpVerificationToUser(user);
            }

            return false;

        }

        bool SendOtpVerificationToUser(User user)
        {
            if (!(string.IsNullOrEmpty(user.Mobile)))
            {
                SmsHelper.SendVerificationMessage(user.Mobile);
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
    }
}