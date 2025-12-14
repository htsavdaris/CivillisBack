using Dapper;
using Dapper.Database;
using Dapper.Database.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Security.Claims;
using System.Security.Cryptography;



namespace CivilisDB.Models
{


    [Table("tbluser")]
    public partial class tblUser :BaseEntity
    {
        [Column, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long flduserid { get; set; } = 0;

        public string fldlastname { get; set; } = "";

        public string fldfirstname { get; set; } = "";

        public string fldlogin { get; set; } = "";

        public string fldpassword { get; set; } = "";

        public string fldemail { get; set; } = "";

        public string fldmobile { get; set; } = "";

        public long fldpersonid { get; set; } = 0;


    }


    public class tblUserDac : DapperBaseRepository<tblUser>
    {

        private const string tableName = "tbluser";
        private const string pkeyName = "flduserid";

        public tblUserDac(string ConnectionString, ILogger logger) : base(tableName, pkeyName, ConnectionString, logger)
        {

        }

        public tblUserDac(IDbConnection connection, ILogger logger) : base(tableName, pkeyName, connection, logger)
        {

        }

        public tblUser GetByEmail(string email)
        {
            try
            {
                var obj = Connection.QueryFirst<tblUser>(SqlSelectCommand + " WHERE email=@email", new { email = email });
                return obj;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(1, "GetByEmail: Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
                return null;
            }
        }

        public tblUser GetByLogin(string fldlogin)
        {
            try
            {
                var obj = Connection.QueryFirstOrDefault<tblUser>(SqlSelectCommand + " WHERE fldlogin=@fldlogin", new { fldlogin = fldlogin });
                if (obj != null)
                    return obj;
                else
                {
                    return null;
                }
            }
            catch (Npgsql.NpgsqlException ex)
            {
                _logger.LogError(1, "GetByLogin: Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
                return null;
            }

        }

        public tblUser Authenticate(string login, string providedpassword)
        {
            var user = GetByLogin(login);

            if (user != null)
            {
                var providedpasswordHashed = PasswordHasher.ComputeHash(providedpassword, login, "ThisIsaNic3P3pp3r");
                if (user.fldpassword == providedpasswordHashed)
                {
                    return user;
                }
                return null;
            }
            else
            {
                return null;
            }
        }
        public bool Register(tblUser oUser)
        {
            if (oUser != null)
            {
                PasswordHasher<string> pw = new PasswordHasher<string>();
                string hashedpass = pw.HashPassword(oUser.fldlogin, oUser.fldpassword);
                oUser.fldpassword = hashedpass;
                try
                {
                    var identity = Connection.Insert<tblUser>(oUser);
                    return true;
                }
                catch (NpgsqlException ex)
                {
                    _logger.LogError(1, "Register: Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
                    return false;
                }
            }
            else
                throw new Exception("User in Registration is null");
        }

        public bool ChangePassword(string login, string oldpass, string newpass)
        {
            PasswordHasher<string> pw = new PasswordHasher<string>();
            tblUser user = Authenticate(login, oldpass);
            if (user != null)
            {
                string hashedpass = PasswordHasher.ComputeHash(newpass, user.fldlogin, "ThisIsaNic3P3pp3r");
                user.fldpassword = hashedpass;
                bool suc  = Connection.Update<tblUser>(user);
                return (suc );
            }
            else
            {
                return false;
            }
        }

        public bool ChangePasswordByAdmin(long userid, string newpass)
        {
                       
            PasswordHasher<string> pw = new PasswordHasher<string>();
            tblUser user = Get(userid);
            if (user != null)
            {
                string hashedpass = PasswordHasher.ComputeHash(newpass, user.fldlogin, "ThisIsaNic3P3pp3r");
                user.fldpassword = hashedpass;
                bool suc = Connection.Update<tblUser>(user);
                return (suc);
            }
            else
            {
                return false;
            }
        }

        ~tblUserDac()
        {
            if (Connection != null)
                Connection.Close();
        }

    }

}
