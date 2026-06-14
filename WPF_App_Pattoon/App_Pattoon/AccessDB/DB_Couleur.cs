using Wpf_App_Pattoon_Animalerie.Modele;
using Wpf_App_Pattoon_Animalerie.Service;
using Npgsql;

namespace Wpf_App_Pattoon_Animalerie.AccessDB
{
    public class DB_Couleur
    {
        public static Dictionary<string, Couleur> All_From_db()
        {
            Dictionary<string, Couleur> retval = new();

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select id, date_creation, valeur " +
                                                           $"from t_couleur " +
                                                           $"order by id ",
                                                           AccessDB.SqlConn);

            try
            {
                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                while (reader.Read())
                {

                    string id = DB_Convertisseur.String(reader, "id");
                    DateTime? dateCreation = DB_Convertisseur.Date(reader, "date_creation");
                    string libelle = DB_Convertisseur.String(reader, "valeur");

                    Couleur c = Couleur.Creer(libelle);
                    c.Id = id;
                    c.DateCreation = (DateTime)dateCreation;
                    retval.Add(c.Id,c);

                    if (AllCouleur.Find(c.Id) == null)
                    {
                        AllCouleur.Add(c);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }
        public static Couleur? UnCouleurById(string id)
        {
            Couleur? retval = null;

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select id, date_creation, valeur " +
                                                           $"from t_couleur " +
                                                           $"where id = @id ",
                                                           AccessDB.SqlConn);

            try
            {
                sqlcmd.Parameters.Add(new NpgsqlParameter("@id", NpgsqlTypes.NpgsqlDbType.Varchar));

                sqlcmd.Prepare();

                sqlcmd.Parameters["@id"].Value = id;

                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                if (reader.Read())
                {
                    string idty = DB_Convertisseur.String(reader, "id");
                    DateTime dateCreation = (DateTime)DB_Convertisseur.Date(reader, "date_creation");
                    string nom = DB_Convertisseur.String(reader, "valeur");

                    retval = Couleur.Creer(nom);
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
        public static Couleur? UnCouleurByNom(string nom)
        {
            Couleur? retval = null;

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select id, date_creation, valeur " +
                                                           $"from t_couleur " +
                                                           $"where valeur = @nom ",
                                                           AccessDB.SqlConn);

            try
            {
                sqlcmd.Parameters.Add(new NpgsqlParameter("@nom", NpgsqlTypes.NpgsqlDbType.Varchar));

                sqlcmd.Prepare();

                sqlcmd.Parameters["@nom"].Value = Forma.TrimUpper(nom);

                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                if (reader.Read())
                {
                    string idty = DB_Convertisseur.String(reader, "id");
                    DateTime dateCreation = (DateTime)DB_Convertisseur.Date(reader, "date_creation");
                    string fnom = DB_Convertisseur.String(reader, "valeur");

                    retval = Couleur.Creer(fnom);
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

        public static int Add(Couleur c)
        {
            string cmdtext = "insert into t_couleur (id, date_creation, valeur ) values (@id, @date, @nom ) ";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@date",(NpgsqlTypes.NpgsqlDbType.Date, c.DateCreation) },
                { "@nom",(NpgsqlTypes.NpgsqlDbType.Varchar, c.Nom) },
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar , c.Id) }
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);

        }
        public static int Update(Couleur c)
        {

            string cmdtext = "update t_couleur set valeur = @nom  where id = @id ";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@nom",(NpgsqlTypes.NpgsqlDbType.Varchar, c.Nom) },
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar , c.Id) }
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);

        }
        public static int Delete(string id)
        {
            string cmdtext = "delete from t_couleur where id = @id";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar, id) },
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);
        }
    }
}
