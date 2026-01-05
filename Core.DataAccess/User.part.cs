using com.fabioscagliola.Core.Data;
using com.fabioscagliola.Core.DataAccess.Social;
using com.fabioscagliola.Core.DataAccess.Social.Notifications;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace com.fabioscagliola.Core.DataAccess
{
    public partial class User : IDataErrorInfo
    {
        protected List<Domain> domainList = null;
        protected List<Role> roleList = null;

        public bool Disabled { get; set; }

        [RefreshProperties(RefreshProperties.All)]
        protected string password;

        [XmlIgnore]
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;

                // If the password fields are blank, then the password is not to be updated; 
                // the validation rules DO NOT require that the password fields are set for EXISTING users anyway, 
                // and, on the other hand, they DO require that the password fields are set for NON-EXISTING users 

                if (!string.IsNullOrWhiteSpace(password))
                {
                    PHash = Hash(password);
                }
            }
        }

        [XmlIgnore]
        public string PasswordConfirmation { get; set; }

        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", FName, LName);
            }
        }

        public static string Hash(string s)
        {
            MD5 md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(Encoding.Default.GetBytes(s));
            return hash.ToHexString();
        }

        public static string GeneratePassword(int length)
        {
            const string CHARACTERS = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

            string password = null;
            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                password += CHARACTERS[random.Next(0, CHARACTERS.Length)];
            }
            return password;
        }

        public static User Select(Milieu milieu, string username)
        {
            EnsureAuthorized(milieu, DataAccessFunction.UserSelect);
            UserSqlController controller = (UserSqlController)new User().GetDefaultController();
            return controller.Select(username);
        }

        public static User Select(Milieu milieu, string username, string password)
        {
            EnsureAuthorized(milieu, DataAccessFunction.UserSelect);
            UserSqlController controller = (UserSqlController)new User().GetDefaultController();
            return controller.Select(username, Hash(password));
        }

        /// <summary>
        /// Returns the list of users who have access to the specified function, within the specified domain 
        /// </summary>
        public static List<User> SelectList(Milieu milieu, long domainId, DataAccessFunction function)
        {
            EnsureAuthorized(milieu, DataAccessFunction.UserSelect);
            UserSqlController controller = (UserSqlController)new User().GetDefaultController();
            return controller.SelectList(domainId, function);
        }

        /// <summary>
        /// Returns the list of users whose first name or last name contains the specified term 
        /// </summary>
        public static List<User> Search(Milieu milieu, string term)
        {
            EnsureAuthorized(milieu, DataAccessFunction.UserSelect);
            UserSqlController controller = (UserSqlController)new User().GetDefaultController();
            return controller.Search(term);
        }

        /// <summary>
        /// Returns a Boolean value indicating if the user has access to the specified function, within the specified domain 
        /// </summary>
        public virtual bool IsAuthorized(long domainId, DataAccessFunction function)
        {
            bool result = false;
            if (Id == Milieu.SystemMilieu.UserId)
            {
                result = true;
            }
            else
            {
                Milieu milieu = new Milieu(domainId, Id);
                result = milieu.FunctionList.Contains(function);
            }
            return result;
        }

        #region Domain list management

        public void AssignDomain(Milieu milieu, Domain domain)
        {
            AssignDomain(milieu, domain, null);
        }

        public void AssignDomain(Milieu milieu, Domain domain, SqlTransaction transaction)
        {
            EnsureAuthorized(milieu, DataAccessFunction.UserUpdate);
            UserDomain userDomain = UserDomain.Select(milieu, domain.Id, this.Id);
            if (userDomain.Id == 0)  // Ensure it does not already exist 
            {
                userDomain = new UserDomain() { DomainId = domain.Id, UserId = this.Id, };
                userDomain.Update(milieu, transaction);
                domainList = null;  // Flush cache 
            }
        }

        public void RemoveDomain(Milieu milieu, Domain domain)
        {
            RemoveDomain(milieu, domain, null);
        }

        public void RemoveDomain(Milieu milieu, Domain domain, SqlTransaction transaction)
        {
            EnsureAuthorized(milieu, DataAccessFunction.UserUpdate);
            UserDomain userDomain = UserDomain.Select(milieu, domain.Id, this.Id);
            if (userDomain.Id != 0)  // Ensure it exists 
            {
                userDomain.Delete(milieu, transaction);
                domainList = null;  // Flush cache 
            }
        }

        public List<Domain> SelectDomainList(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.UserSelect);
            if (domainList == null)
            {
                domainList = new List<Domain>();
                List<UserDomain> userDomainList = UserDomain.SelectListByUserId(milieu, this.Id);
                foreach (UserDomain userDomain in userDomainList)
                {
                    Domain domain = Domain.Select(milieu, userDomain.DomainId);
                    domainList.Add(domain);
                }
            }
            return domainList;
        }

        #endregion

        #region Role list management

        public void AssignRole(Milieu milieu, Domain domain, Role role)
        {
            AssignRole(milieu, domain, role, null);
        }

        public void AssignRole(Milieu milieu, Domain domain, Role role, SqlTransaction transaction)
        {
            EnsureAuthorized(milieu, DataAccessFunction.UserUpdate);
            UserRole userRole = UserRole.Select(milieu, domain.Id, this.Id, role.Id);
            if (userRole.Id == 0)  // Ensure it does not already exist 
            {
                userRole = new UserRole() { DomainId = domain.Id, UserId = this.Id, RoleId = role.Id, };
                userRole.Update(milieu, transaction);
                roleList = null;  // Flush cache 
            }
        }

        public void RemoveRole(Milieu milieu, Domain domain, Role role)
        {
            RemoveRole(milieu, domain, role, null);
        }

        public void RemoveRole(Milieu milieu, Domain domain, Role role, SqlTransaction transaction)
        {
            EnsureAuthorized(milieu, DataAccessFunction.UserUpdate);
            UserRole userRole = UserRole.Select(milieu, domain.Id, this.Id, role.Id);
            if (userRole.Id != 0)  // Ensure it exists 
            {
                userRole.Delete(milieu, transaction);
                roleList = null;  // Flush cache 
            }
        }

        public List<Role> SelectRoleList(Milieu milieu, long domainId)  // TODO: Change the type of the last parameter to Domain 
        {
            EnsureAuthorized(milieu, DataAccessFunction.UserSelect);
            if (roleList == null)
            {
                roleList = new List<Role>();
                List<UserRole> userRoleList = UserRole.SelectListByUserId(milieu, domainId, this.Id);
                foreach (UserRole userRole in userRoleList)
                {
                    Role role = Role.Select(milieu, userRole.RoleId);
                    roleList.Add(role);
                }
            }
            return roleList;
        }

        #endregion

        public override void AssignFollower(Milieu milieu, long userId)
        {
            // First, ensure that the user is not already following this entity 
            Follower follower = Follower.Select(milieu, this.Guid, userId);
            if (follower.Id == 0)
            {
                follower = DoAssignFollower(milieu, userId);

                User user = User.Select(Milieu.SystemMilieu, follower.UserId);
                FollowedUserNotification notification = new FollowedUserNotification(user, Id);
                notification.Update(Milieu.SystemMilieu);
            }
        }

        #region ProfilePicture

        public static int ProfilePictureWidth { get { return 128; } }
        public static int ProfilePictureHeight { get { return 128; } }

        protected Blob profilePicture;

        public Guid ProfilePictureGuid { get; set; }

        public Blob ProfilePicture
        {
            get
            {
                if (ProfilePictureGuid != Guid.Empty && profilePicture == null)
                {
                    profilePicture = Blob.Select(Milieu.SystemMilieu, ProfilePictureGuid);
                }
                return profilePicture;
            }
        }

        #endregion

        #region SignaturePicture

        //public static int SignaturePictureWidth { get { return 128; } }
        //public static int SignaturePictureHeight { get { return 128; } }

        protected Blob signaturePicture;

        public Guid SignaturePictureGuid { get; set; }

        public Blob SignaturePicture
        {
            get
            {
                if (SignaturePictureGuid != Guid.Empty && signaturePicture == null)
                {
                    signaturePicture = Blob.Select(Milieu.SystemMilieu, SignaturePictureGuid);
                }
                return signaturePicture;
            }
        }

        #endregion

        public static DataAccessList<User> SelectDelcoNetEntityList(Milieu milieu)
        {
            List<User> list = User.SelectList(milieu);
            return new DataAccessList<User>(list);
        }

        #region IDataErrorInfo

        [Browsable(false)]
        public string Error { get; }

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case nameof(FName):
                        return ValidateFName();
                    case nameof(UName):
                        return ValidateUName();
                    case nameof(Password):
                        return ValidatePassword();
                    case nameof(PasswordConfirmation):
                        return ValidatePasswordConfirmation();
                }
                return null;
            }
        }

        protected string ValidateFName()
        {
            if (string.IsNullOrWhiteSpace(FName))
            {
                AddError(nameof(FName));
                return "You must indicate a first name!";  // TODO: RESX 
            }
            RemError(nameof(FName));
            return null;
        }

        protected string ValidateUName()
        {
            if (string.IsNullOrWhiteSpace(UName))
            {
                AddError(nameof(UName));
                return "You must indicate a username!";  // TODO: RESX 
            }
            RemError(nameof(UName));
            return null;
        }

        protected string ValidatePassword()
        {
            if (Id == 0 && string.IsNullOrWhiteSpace(password))
            {
                AddError(nameof(Password));
                return "You must indicate a password!";  // TODO: RESX 
            }
            RemError(nameof(Password));
            return null;
        }

        protected string ValidatePasswordConfirmation()
        {
            if (Id == 0 && string.IsNullOrWhiteSpace(PasswordConfirmation))
            {
                AddError(nameof(PasswordConfirmation));
                return "You must indicate a password confirmation!";  // TODO: RESX 
            }
            if (PasswordConfirmation != password)
            {
                AddError(nameof(PasswordConfirmation));
                return "The password confirmation does not match the password!";  // TODO: RESX 
            }
            RemError(nameof(PasswordConfirmation));
            return null;
        }

        #endregion

    }
}

