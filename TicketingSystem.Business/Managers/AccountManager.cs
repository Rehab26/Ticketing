using System;
using System.Linq;
using TicketingSystem.Business.Encryption;
using TicketingSystem.Common.Enums;
using TicketingSystem.Common.Models.UserModels;
using TicketingSystem.Data;
using TicketingSystem.Data.Entites;
using TicketingSystem.Logging;
using TicketingSystem.AvailoService;
using System.Configuration;

namespace TicketingSystem.Business.Managers
{
    public class AccountManager
    {
        public TicketingStystemContext db;
        public AccountManager()
        {
            db = new TicketingStystemContext();
        }
        AttachmentType attachmentType;

        // Employee Register
        public Boolean CreateEmployee(ViewUser user)
        {
            var newUser = new User()
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = user.DateOfBirth,
                CreatedDate = DateTime.Now,
                Type = UserType.Employee,
                LoginType = user.LoginType
            };
            try
            {

                db.Users.Add(newUser);
                db.SaveChanges();


                if (user.LoginType == LoginApproach.Face)
                {
                    attachmentType = AttachmentType.UserRegisterImage;
                }
                else if (user.LoginType == LoginApproach.Voice)
                {
                    attachmentType = AttachmentType.UserRegisterVoice;
                }
                if (user.File != null)
                {
                    db.FileStorages.Add(new FileStorage()
                    {
                        Path = user.File,
                        Reference = newUser.Id,
                        Type = attachmentType,
                        CreateDate = DateTime.Now
                    });
                    db.SaveChanges();

                }
                return true;

            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return false;
            }
        }

        public LoginApproach? CheckLoginApproach(string phoneNumber)
        {
            var user = db.Users.Where(u => u.PhoneNumber == phoneNumber).FirstOrDefault();
            if (user == null) return null;
            return user.LoginType;
        }
        // Employee Login
        public ViewUser LoginEmployee(ViewUser user)
        {
            AvailoAuthManager availoAuth = new AvailoAuthManager();
            bool useBiometric = Boolean.Parse(ConfigurationManager.AppSettings["UseBiometric"]);
            try
            {
                var entity = db.Users.Where(a => a.PhoneNumber == user.PhoneNumber && a.LoginType == user.LoginType).FirstOrDefault();
                    var registerFile = db.FileStorages.Where(s => s.Reference == entity.Id && s.Type == AttachmentType.UserRegisterImage || s.Reference == entity.Id && s.Type == AttachmentType.UserRegisterVoice).FirstOrDefault();
                    if (entity == null)
                        return null;

                    else if (entity.UserStatus == UserStatus.Inactive || entity.Type != UserType.Employee)
                        return null;// access denied

                    else if (useBiometric)
                    {
                        if (availoAuth.AuthenticateUser(registerFile.Path, user.File))
                        {
                            var LoggedUser = new ViewUser()
                                             {
                                                 Email = entity.Email,
                                                 PhoneNumber = entity.PhoneNumber,
                                                 Id = entity.Id,
                                                 FirstName = entity.FirstName,
                                                 LastName = entity.LastName,
                                                 File = registerFile.Path,
                                                 Type = entity.Type,
                                                 Token = user.Token
                                             };
                            // save the login file
                            if (user.File != null)
                            {
                                if (user.LoginType == LoginApproach.Face && registerFile.Type == AttachmentType.UserRegisterImage)
                                {
                                    db.FileStorages.Add(new FileStorage()
                                    {
                                        Path = user.File,
                                        Reference = entity.Id,
                                        Type = AttachmentType.UserLoginImage,
                                        CreateDate = DateTime.Now
                                    });
                                    db.SaveChanges();
                                }
                                else if (user.LoginType == LoginApproach.Voice && registerFile.Type == AttachmentType.UserRegisterVoice)
                                {
                                    db.FileStorages.Add(new FileStorage()
                                    {
                                        Path = user.File,
                                        Reference = entity.Id,
                                        Type = AttachmentType.UserLoginVoice,
                                        CreateDate = DateTime.Now
                                    });
                                    db.SaveChanges();
                                }
                                else
                                {
                                    return null;
                                }

                            }

                            return LoggedUser;

                        }
                    }
                    else if (!useBiometric)
                    {
                            var LoggedUser = new ViewUser()
                                             {
                                                 Email = entity.Email,
                                                 PhoneNumber = entity.PhoneNumber,
                                                 Id = entity.Id,
                                                 FirstName = entity.FirstName,
                                                 LastName = entity.LastName,
                                                 File = registerFile.Path,
                                                 Type = entity.Type,
                                                 Token = user.Token
                                             };
                            // save the login file
                            if (user.File != null)
                            {
                                if (user.LoginType == LoginApproach.Face && registerFile.Type == AttachmentType.UserRegisterImage)
                                {
                                    db.FileStorages.Add(new FileStorage()
                                    {
                                        Path = user.File,
                                        Reference = entity.Id,
                                        Type = AttachmentType.UserLoginImage,
                                        CreateDate = DateTime.Now
                                    });
                                    db.SaveChanges();
                                }
                                else if (user.LoginType == LoginApproach.Voice && registerFile.Type == AttachmentType.UserRegisterVoice)
                                {
                                    db.FileStorages.Add(new FileStorage()
                                    {
                                        Path = user.File,
                                        Reference = entity.Id,
                                        Type = AttachmentType.UserLoginVoice,
                                        CreateDate = DateTime.Now
                                    });
                                    db.SaveChanges();
                                }
                                else
                                {
                                    return null;
                                }

                            return LoggedUser;

                        }
                    }

                    return null;
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return null;
            }
        }
        // Check if user is registered before in the system
        public string isValid(string phoneNo, string Email)
        {
            try
            {
                var user = db.Users.Where(a => a.PhoneNumber == phoneNo).FirstOrDefault();
                var user2 = db.Users.Where(a => a.Email == Email).FirstOrDefault();
                if (user == null && user2 == null) return "Valid";
                else if (user != null) return "Phone Number is Already exist";
                else if (user2 != null) return "Email entered is Already exist";
                return "Phone Number and Email entered are Already exist";
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return null;
            }
        }

        // Client Register
        public ClientView CreateClient(ViewUser user)
        {
            try
            {
                var client = new User()
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    DateOfBirth = user.DateOfBirth,
                    Password = user.Password.Hash(),
                    CreatedDate = DateTime.Now,
                    LoginType = LoginApproach.Password,
                    Type = UserType.Client,
                    UserStatus = UserStatus.Active,
                    Country = user.Country,
                };
                db.Users.Add(client);
                db.SaveChanges();
                return new ClientView
                {
                    Id = client.Id,
                    Country = client.Country,
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                    Type = client.Type,
                    DateOfBirth = client.DateOfBirth.Value.ToShortDateString(),
                    Email = client.Email,
                    PhoneNumber = client.PhoneNumber
                };
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return null;
            }
        }
        public ViewUser LoginManager(ViewUser login)
        {
            try
            {
                login.Password = login.Password.Hash();
                var entity = db.Users.FirstOrDefault(a => a.PhoneNumber.Equals(login.PhoneNumber) && login.Password == a.Password);

                if (entity == null)
                    return null;

            else if (entity.UserStatus == UserStatus.Inactive || entity.Type != UserType.Manager)
                return null;// access denied

                else
                {
                    var LoggedUser = new ViewUser()
                                     {
                                         Email = entity.Email,
                                         PhoneNumber = entity.PhoneNumber,
                                         Id = entity.Id,
                                         FirstName = entity.FirstName,
                                         LastName = entity.LastName,
                                         Token = login.Token,//added for token
                                         Type = entity.Type,

                                     };
                    return LoggedUser;
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return null;
            }

        }

        // Client login 
        public ClientView LoginClient(ClientLogin login)
        {
            try
            {
                // Hash password
                login.Password = login.Password.Hash();
                var entity = db.Users.FirstOrDefault(a => a.PhoneNumber.Equals(login.PhoneNumber) && login.Password == a.Password);

                if (entity == null)
                    return null;

                else if (entity.UserStatus == UserStatus.Inactive || entity.Type != UserType.Client)
                    return null;// access denied

                else
                    return new ClientView
                    {
                        Email = entity.Email,
                        PhoneNumber = entity.PhoneNumber,
                        Id = entity.Id,
                        FirstName = entity.FirstName,
                        LastName = entity.LastName,
                        Token = login.Token //added for token
                    };
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return null;
            }
        }

        // Check if email ask for change password is exists in database
        public string IsUserExists(PasswordForget changePassword)
        {
            try
            {
                var entity = db.Users.FirstOrDefault(a => a.Email.Equals(changePassword.Email));

                if (entity == null)
                    return null;

                else
                {
                    var ResetCode = changePassword.Link.Split('/').Last();
                    entity.ResetPasswordCode = ResetCode;
                    db.SaveChanges();
                    return "Valid";

                }
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return null;
            }
        }

        // Change to new password
        public ClientView ResetPassword(PasswordReset newPassword)
        {
            try
            {
                var ResetCode = newPassword.Link.Split('/').Last();
                var entity = db.Users.FirstOrDefault(a => a.ResetPasswordCode.Equals(ResetCode));

                if (entity == null)
                    return null;

                else
                {
                    // If user is valid, change old password with new one + hash new password
                    entity.Password = newPassword.Password.Hash();
                    entity.ResetPasswordCode = "";
                    db.SaveChanges();
                    return new ClientView();
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return null;
            }
        }

        public string SendEmail(PasswordForget changing)
        {
            try
            {
                // Send email to client to change his/her password
                EmailManager emailManager = new EmailManager();
                emailManager.ForgetPsswordEmail(changing);
                return "Sent!";
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return null;
            }
        }

    }

}
