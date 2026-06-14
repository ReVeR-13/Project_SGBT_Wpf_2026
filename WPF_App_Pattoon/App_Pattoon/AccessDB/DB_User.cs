using Wpf_App_Pattoon_Animalerie.Modele;
using Wpf_App_Pattoon_Animalerie.Service;
using Npgsql;

namespace Wpf_App_Pattoon_Animalerie.AccessDB
{
    public static class DB_User
    {
        public static Dictionary<string, User> All_From_Db()
        {
            Dictionary<string, User> retval = [];

            using NpgsqlCommand sqlcmd = new($"select * from f_All_user()",
                                              AccessDB.SqlConn);

            try
            {
                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                while (reader.Read())
                {

                    string id = DB_Convertisseur.String(reader, "id_");
                    DateTime? dateCreation = DB_Convertisseur.Date(reader, "date_");
                    string nom = DB_Convertisseur.String(reader, "nom_");
                    string pass = DB_Convertisseur.String(reader, "pass_");
                    string mail = DB_Convertisseur.String(reader, "mail_");
                    bool admin = DB_Convertisseur.Bool(reader, "adm_");
                    bool active = DB_Convertisseur.Bool(reader, "active_");
                    DateTime? updated = DB_Convertisseur.Date(reader, "update_");
                    string desc = DB_Convertisseur.String(reader, "desc_");

                    User tpe = User.Create(nom, pass, mail, desc);
                    tpe.Id = id;
                    tpe.DateCreation = (DateTime)dateCreation;
                    tpe.Admin = admin;
                    tpe.Active = active;
                    tpe.Updated = (DateTime)updated;
                    retval.Add(tpe.Id, tpe);

                    if (UserService.Find(id) == null)
                    {
                        UserService.Add(tpe);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }
        public static User? UnUserById(string id)
        {
            User? retval = null;

            using NpgsqlCommand sqlcmd = new($"select * from f_One_user(@id)",
                                                           AccessDB.SqlConn);

            try
            {
                sqlcmd.Parameters.Add(new NpgsqlParameter("@id", NpgsqlTypes.NpgsqlDbType.Varchar));

                sqlcmd.Prepare();

                sqlcmd.Parameters["@id"].Value = id;

                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                if (reader.Read())
                {
                    string id_ = DB_Convertisseur.String(reader, "id_");
                    DateTime? dateCreation = DB_Convertisseur.Date(reader, "date_");
                    string nom = DB_Convertisseur.String(reader, "nom_");
                    string pass = DB_Convertisseur.String(reader, "pass_");
                    string mail = DB_Convertisseur.String(reader, "mail_");
                    bool admin = DB_Convertisseur.Bool(reader, "adm_");
                    bool active = DB_Convertisseur.Bool(reader, "active_");
                    DateTime? updated = DB_Convertisseur.Date(reader, "update_");
                    string desc = DB_Convertisseur.String(reader, "desc_");

                    retval = User.Create(nom, pass, mail, desc);
                    retval.Id = id_;
                    retval.DateCreation = (DateTime)dateCreation;
                    retval.Admin = admin;
                    retval.Active = active;
                    retval.Updated = (DateTime)updated;

                    return retval;

                }
            }
            catch (Exception ex)
            {
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }
        public static User? UnUserByEmail(string email)
        {
            User? retval = null;

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select * from f_One_userByEmail(@mail)" ,
                                                           AccessDB.SqlConn);

            try
            {
                sqlcmd.Parameters.Add(new NpgsqlParameter("@mail", NpgsqlTypes.NpgsqlDbType.Varchar));

                sqlcmd.Prepare();

                sqlcmd.Parameters["@mail"].Value = email;

                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                if (reader.Read())
                {
                    string id_ = DB_Convertisseur.String(reader, "id_");
                    DateTime? dateCreation = DB_Convertisseur.Date(reader, "date_");
                    string nom = DB_Convertisseur.String(reader, "nom_");
                    string pass = DB_Convertisseur.String(reader, "pass_");
                    string mail = DB_Convertisseur.String(reader, "mail_");
                    bool admin = DB_Convertisseur.Bool(reader, "adm_");
                    bool active = DB_Convertisseur.Bool(reader, "active_");
                    DateTime? updated = DB_Convertisseur.Date(reader, "update_");
                    string desc = DB_Convertisseur.String(reader, "desc_");

                    retval = User.Create(nom, pass, mail, desc);
                    retval.Id = id_;
                    retval.DateCreation = (DateTime)dateCreation;
                    retval.Admin = admin;
                    retval.Active = active;
                    retval.Updated = (DateTime)updated;

                    return retval;

                }
            }
            catch (Exception ex)
            {
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }
        
        public static int Add(User user)
        {
            string cmdtext = "select * from f_create_user(@username, @password,@email,@description) ";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@username",(NpgsqlTypes.NpgsqlDbType.Varchar, user.Nom) },
                { "@password",(NpgsqlTypes.NpgsqlDbType.Varchar, user.Password) },
                { "@email",(NpgsqlTypes.NpgsqlDbType.Varchar, user.Email) },
                { "@description",(NpgsqlTypes.NpgsqlDbType.Varchar, user.Description) }
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);

        }
        public static int Update(User user)
        {

            string cmdtext = "select * from f_update_user( @id, @username, @password , @email, @description)" ;

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar, user.Id) },
                { "@username",(NpgsqlTypes.NpgsqlDbType.Varchar, user.Nom) },
                { "@password",(NpgsqlTypes.NpgsqlDbType.Varchar, user.Password) },
                { "@email",(NpgsqlTypes.NpgsqlDbType.Varchar, user.Email) },
                { "@description",(NpgsqlTypes.NpgsqlDbType.Varchar, user.Description) }
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);

        }
        public static int UpdateAdmin(User user)
        {

            string cmdtext = "select * from f_update_AdminUser( @id, @admin)";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar, user.Id) },
                { "@admin",(NpgsqlTypes.NpgsqlDbType.Boolean, user.Admin) }
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);

        }
        public static int UpdateActive(User user)
        {

            string cmdtext = "select * from f_update_ActiveUser( @id, @active)";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar, user.Id) },
                { "@active",(NpgsqlTypes.NpgsqlDbType.Boolean, user.Active) }
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);

        }
        public static int Delete(string id)
        {
            string cmdtext = "select * from f_delete_user(@id)";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar, id) },
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);
        }
    }
}
