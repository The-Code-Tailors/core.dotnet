using com.fabioscagliola.Core.Data;
using com.fabioscagliola.Core.DataAccess.Social;
using System;
using System.ComponentModel;
using Microsoft.Data.SqlClient;
using System.Xml.Serialization;

namespace com.fabioscagliola.Core.DataAccess
{
    public abstract class DataAccessEntity : Entity<long>, IEditable, IEditableObject, IValidateableObject
    {
        protected abstract DataAccessFunction DeleteDataAccessFunction { get; }

        protected abstract DataAccessFunction UpdateDataAccessFunction { get; }

        [Browsable(false)]
        public string DataType { get; set; }
        [Browsable(false)]
        public Guid Guid { get; set; }
        [Browsable(false)]
        public DateTime? InsertDate { get; set; }
        [Browsable(false)]
        [XmlIgnore]
        public DateTime? InsertDateLocal { get { return InsertDate?.ToLocalTime() ?? null; } }
        [Browsable(false)]
        public long? InsertUserId { get; set; }
        [Browsable(false)]
        public DateTime? UpdateDate { get; set; }
        [Browsable(false)]
        [XmlIgnore]
        public DateTime? UpdateDateLocal { get { return UpdateDate?.ToLocalTime() ?? null; } }
        [Browsable(false)]
        public virtual long? UpdateUserId { get; set; }
        [Browsable(false)]
        public DateTime? DeleteDate { get; set; }
        [Browsable(false)]
        public long? DeleteUserId { get; set; }
        [Browsable(false)]
        public KeyValuePairList<string, object> Attributes { get; set; }

        public DataAccessEntity()
        {
            Attributes = new KeyValuePairList<string, object>();

            editableObjectHelper = new EditableObjectHelper(this);
            validationHelper = new ValidateableObjectHelper();
        }

        /// <summary>
        /// A Boolean value indicating if the entity already exists 
        /// </summary>
        [XmlIgnore]
        [Browsable(false)]
        public bool Exists
        {
            get { return (Id != 0); }
        }

        /// <summary>
        /// Blob count 
        /// </summary>
        [XmlIgnore]
        [Browsable(false)]
        public int SocialCount1 { get; set; }

        /// <summary>
        /// Event count 
        /// </summary>
        [XmlIgnore]
        [Browsable(false)]
        public int SocialCount2 { get; set; }

        /// <summary>
        /// Remark count 
        /// </summary>
        [XmlIgnore]
        [Browsable(false)]
        public int SocialCount3 { get; set; }

        /// <summary>
        /// Task count 
        /// </summary>
        [XmlIgnore]
        [Browsable(false)]
        public int SocialCount4 { get; set; }

        /// <summary>
        /// Use Delete(Milieu milieu) instead!
        /// </summary>
        public sealed override void Delete()
        {
            throw new DataAccessException("Use Delete(Milieu milieu) instead!");
        }

        /// <summary>
        /// Use Update(Milieu milieu) instead!
        /// </summary>
        public sealed override void Update()
        {
            throw new DataAccessException("Use Update(Milieu milieu) instead!");
        }

        public void Delete(Milieu milieu)  // Cannot override 
        {
            Delete(milieu, null, false);
        }

        public void Delete(Milieu milieu, bool permanently)  // Cannot override 
        {
            Delete(milieu, null, permanently);
        }

        public void Delete(Milieu milieu, SqlTransaction transaction)  // Cannot override 
        {
            Delete(milieu, transaction, false);
        }

        public virtual void Delete(Milieu milieu, SqlTransaction transaction, bool permanently)
        {
            if (!IsUserAuthorizedToDelete(milieu))
            {
                throw new AccessDeniedException(DeleteDataAccessFunction);
            }

            DeleteDate = DateTime.UtcNow;
            DeleteUserId = milieu.UserId;

            SqlController controller = (SqlController)GetDefaultController();

            if (transaction == null)
            {
                controller.Delete(permanently);
            }
            else
            {
                controller.Delete(transaction, permanently);
            }
        }

        public void Update(Milieu milieu)  // Cannot override 
        {
            Update(milieu, null);
        }

        public virtual void Update(Milieu milieu, SqlTransaction transaction)
        {
            if (!IsUserAuthorizedToUpdate(milieu))
            {
                throw new AccessDeniedException(UpdateDataAccessFunction);
            }

            if (Id == 0)
            {
                // Insert 
                DataType = DoDataType();
                Guid = System.Guid.NewGuid();
                InsertDate = DateTime.UtcNow;
                InsertUserId = milieu.UserId;
            }
            else
            {
                // Update 
                UpdateDate = DateTime.UtcNow;
                UpdateUserId = milieu.UserId;
            }

            SqlController controller = (SqlController)GetDefaultController();

            if (transaction == null)
            {
                controller.Update();
            }
            else
            {
                controller.Update(transaction);
            }
        }

        /// <summary>
        /// Ensures that a user is authorized to access a function in a domain 
        /// </summary>
        protected static void EnsureAuthorized(Milieu milieu, DataAccessFunction function)
        {
            if (milieu.UserId == Milieu.SystemMilieu.UserId)
            {
                // Do nothing 
            }
            else
            {
                //User user = User.Select(Milieu.SystemMilieu, milieu.UserId);
                //if (!user.IsAuthorized(milieu.DomainId, function))
                if (!milieu.FunctionList.Contains(function))
                {
                    throw new AccessDeniedException(function);
                }
            }
        }

        /// <summary>
        /// Returns a Boolean value indicating whether everyone can create new entity instances 
        /// </summary>
        protected virtual bool IsEveryoneAuthorizedToInsert { get { return false; } }

        /// <summary>
        /// Returns true if Id = 0 and IsEveryoneAuthorizedToInsert is true. 
        /// Returns true if Id != 0 and the user is the creator of the entity instance. 
        /// Otherwise, ensures that a user is authorized to access a function in a domain 
        /// </summary>
        protected virtual bool IsUserAuthorized(Milieu milieu, DataAccessFunction function)
        {
            if (Id == 0 /* Insert */ && IsEveryoneAuthorizedToInsert)
            {
                return true;
            }
            else
            {
                if (milieu.UserId == InsertUserId)
                {
                    return true;
                }
                else
                {
                    try
                    {
                        EnsureAuthorized(milieu, function);
                        return true;
                    }
                    catch (AccessDeniedException)
                    {
                        return false;
                    }
                }
            }
        }

        public bool IsUserAuthorizedToDelete(Milieu milieu)
        {
            return IsUserAuthorized(milieu, DeleteDataAccessFunction);
        }

        public bool IsUserAuthorizedToUpdate(Milieu milieu)
        {
            return IsUserAuthorized(milieu, UpdateDataAccessFunction);
        }

        protected User insertUser;

        [Browsable(false)]
        public virtual User InsertUser
        {
            get
            {
                if (insertUser == null && InsertUserId.HasValue)
                {
                    insertUser = User.Select(Milieu.SystemMilieu, InsertUserId.Value);
                }
                return insertUser;
            }
        }

        protected User updateUser;

        [Browsable(false)]
        public User UpdateUser
        {
            get
            {
                if (updateUser == null && UpdateUserId.HasValue)
                {
                    updateUser = User.Select(Milieu.SystemMilieu, UpdateUserId.Value);
                }
                return updateUser;
            }
        }

        public virtual void AssignFollower(Milieu milieu, long userId)
        {
            // First, ensure that the user is not already following this entity 
            Follower follower = Follower.Select(milieu, this.Guid, userId);
            if (follower.Id == 0)
            {
                follower = DoAssignFollower(milieu, userId);
            }
        }

        protected Follower DoAssignFollower(Milieu milieu, long userId)
        {
            Follower follower = new Follower();
            follower.MasterEntity = this.GetType().FullName;
            follower.MasterGuid = this.Guid;
            follower.MasterId = this.Id;
            follower.UserId = userId;
            follower.Update(milieu);
            return follower;
        }

        public virtual void RemoveFollower(Milieu milieu, long userId)
        {
            // First, ensure that the user is actually following this entity 
            Follower follower = Follower.Select(milieu, this.Guid, userId);
            if (follower.Id != 0)
            {
                follower.Delete(milieu, true);
            }
        }


        public void EnsureTransaction(Milieu milieu, SqlTransaction transaction, bool permanently, Action<Milieu, SqlTransaction, bool, bool> deleteAction)
        {
            if (transaction == null)
            {
                SqlController controller = (SqlController)GetDefaultController();
                SqlConnection connection = controller.GetConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                try
                {
                    deleteAction(milieu, transaction, permanently, true);
                }
                finally
                {
                    transaction.Dispose();
                    connection.Close();
                    connection.Dispose();
                }
            }
            else
            {
                deleteAction(milieu, transaction, permanently, false);
            }
        }

        public void EnsureTransaction(Milieu milieu, SqlTransaction transaction, Action<Milieu, SqlTransaction, bool> updateAction)
        {
            if (transaction == null)
            {
                SqlController controller = (SqlController)GetDefaultController();
                SqlConnection connection = controller.GetConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                try
                {
                    updateAction(milieu, transaction, true);
                }
                finally
                {
                    transaction.Dispose();
                    connection.Close();
                    connection.Dispose();
                }
            }
            else
            {
                updateAction(milieu, transaction, false);
            }
        }

        public string DoDataType()
        {
            return string.Format("{0}, {1}", GetType().FullName, GetType().Assembly.GetName().Name);
        }


        #region IEditableObject

        protected EditableObjectHelper editableObjectHelper;

        public void BeginEdit()
        {
            editableObjectHelper.BeginEdit();
        }

        public void CancelEdit()
        {
            editableObjectHelper.CancelEdit();
        }

        public void EndEdit()
        {
            editableObjectHelper.EndEdit();
        }

        #endregion

        #region IValidateable

        protected ValidateableObjectHelper validationHelper;

        /// <summary>
        /// Adds a validation error 
        /// </summary>
        /// <param name="error">A unique string identifying the property whose value is invalid (typically the name of the property)</param>
        protected void AddError(string error)
        {
            validationHelper.AddError(error);
        }

        /// <summary>
        /// Removes a validation error 
        /// </summary>
        /// <param name="error">A unique string identifying the property whose value is valid (typically the name of the property)</param>
        protected void RemError(string error)
        {
            validationHelper.RemError(error);
        }

        /// <summary>
        /// A Boolean value indicating if the value of at least one property is invalid 
        /// </summary>
        [Browsable(false)]
        public bool HasError
        {
            get
            {
                return validationHelper.HasError;
            }
        }

        #endregion

    }
}

