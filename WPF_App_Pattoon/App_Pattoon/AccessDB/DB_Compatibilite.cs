using Wpf_App_Pattoon_Animalerie.Modele;
using Wpf_App_Pattoon_Animalerie.Service;
using Npgsql;

namespace Wpf_App_Pattoon_Animalerie.AccessDB
{
    public class DB_Compatibilite
    {
        public static Dictionary<string, Compatibilite> All_From_Db()
        {
            Dictionary<string, Compatibilite> retval = new Dictionary<string, Compatibilite>();

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select id_compatibilite, date_creation, nom_compatibilite, description " +
                                                           $"from t_compatibilite " +
                                                           $"order by id_compatibilite ",
                                                           AccessDB.SqlConn);

            try
            {
                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                while (reader.Read())
                {

                    string id = DB_Convertisseur.String(reader, "id_compatibilite");
                    DateTime? dateCreation = DB_Convertisseur.Date(reader, "date_creation");
                    string nom = DB_Convertisseur.String(reader, "nom_compatibilite");
                    string descr = DB_Convertisseur.String(reader, "description");

                    Compatibilite compatibilite = Compatibilite.Creer(nom, descr);
                    compatibilite.Id = id;
                    compatibilite.DateCreation = (DateTime)dateCreation;
                    retval.Add(compatibilite.Id, compatibilite);

                    if (AllCompatibilite.Find(compatibilite.Id) == null)
                    {
                        AllCompatibilite.Add(compatibilite);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }
        public static Compatibilite? UnCompatibiliteById(string id)
        {
            Compatibilite? retval = null;

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select id_compatibilite, date_creation, nom_compatibilite, description " +
                                                           $"from t_compatibilite " +
                                                           $"where id_compatibilite = @id ",
                                                           AccessDB.SqlConn);

            try
            {
                sqlcmd.Parameters.Add(new NpgsqlParameter("@id", NpgsqlTypes.NpgsqlDbType.Varchar));

                sqlcmd.Prepare();

                sqlcmd.Parameters["@id"].Value = id;

                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                if (reader.Read())
                {
                    string idty = DB_Convertisseur.String(reader, "id_compatibilite");
                    DateTime dateCreation = (DateTime)DB_Convertisseur.Date(reader, "date_creation");
                    string nom = DB_Convertisseur.String(reader, "nom_compatibilite");
                    string descr = DB_Convertisseur.String(reader, "description");

                    retval = Compatibilite.Creer(nom, descr);
                    retval.Id = idty;
                    retval.DateCreation = dateCreation;

                }
            }
            catch (Exception ex)
            {
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }
        public static Compatibilite? UnCompatibiliteNom(string nom)
        {
            Compatibilite? retval = null;

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select id_compatibilite, date_creation, nom_compatibilite, description " +
                                                           $"from t_compatibilite " +
                                                           $"where nom_compatibilite = @nom ",
                                                           AccessDB.SqlConn);

            try
            {
                sqlcmd.Parameters.Add(new NpgsqlParameter("@nom", NpgsqlTypes.NpgsqlDbType.Varchar));

                sqlcmd.Prepare();

                sqlcmd.Parameters["@nom"].Value = nom;

                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                if (reader.Read())
                {
                    string idty = DB_Convertisseur.String(reader, "id_compatibilite");
                    DateTime dateCreation = (DateTime)DB_Convertisseur.Date(reader, "date_creation");
                    string fnom = DB_Convertisseur.String(reader, "nom_compatibilite");
                    string descr = DB_Convertisseur.String(reader, "description");

                    retval = Compatibilite.Creer(fnom, descr);
                    retval.Id = idty;
                    retval.DateCreation = dateCreation;

                }
            }
            catch (Exception ex)
            {
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }

        public static int Add(Compatibilite Compatibilite)
        {
            string cmdtext = "insert into t_compatibilite(id_compatibilite, date_creation, nom_compatibilite, description ) values (@id, @date, @nom, @descr) ";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@date",(NpgsqlTypes.NpgsqlDbType.Date, Compatibilite.DateCreation) },
                { "@nom",(NpgsqlTypes.NpgsqlDbType.Varchar, Compatibilite.Nom) },
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar , Compatibilite.Id) },
                { "@descr",(NpgsqlTypes.NpgsqlDbType.Varchar , Compatibilite.Details) }
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);

        }
        public static int Update(Compatibilite compatibilite)
        {

            string cmdtext = "update t_compatibilite set nom_compatibilite = @nom, description = @descr where id_compatibilite = @id ";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@nom",(NpgsqlTypes.NpgsqlDbType.Varchar, compatibilite.Nom) },
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar , compatibilite.Id) },
                { "@descr",(NpgsqlTypes.NpgsqlDbType.Varchar , compatibilite.Details) }
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);

        }
        public static int Delete(string id)
        {
            string cmdtext = "delete from t_compatibilite where id_compatibilite = @id";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar, id) },
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);
        }
    }
}
