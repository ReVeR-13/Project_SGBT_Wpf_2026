using Wpf_App_Pattoon_Animalerie.Modele;
using Wpf_App_Pattoon_Animalerie.Service;
using Npgsql;

namespace Wpf_App_Pattoon_Animalerie.AccessDB
{
    public static class DB_Vaccin
    {
        public static Dictionary<string, Vaccin> All_From_Db()
        {
            Dictionary<string, Vaccin> retval = new Dictionary<string, Vaccin>();

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select * from f_All_vaccin() ",
                                                           AccessDB.SqlConn);

            try
            {
                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                while (reader.Read())
                {

                    string id = DB_Convertisseur.String(reader, "id");
                    DateTime? dateCreation = DB_Convertisseur.Date(reader, "date");
                    string nom = DB_Convertisseur.String(reader, "vaccin");
                    string descr = DB_Convertisseur.String(reader, "descri");

                    Vaccin vaccin = Vaccin.Creer(nom,descr);
                    vaccin.Id = id;
                    vaccin.DateCreation = (DateTime)dateCreation;
                    retval.Add(vaccin.Id, vaccin);

                    if (AllVaccin.Find(vaccin.Id) == null)
                    {
                        AllVaccin.Add(vaccin);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }
        public static Vaccin? UnVaccinById(string id)
        {
            Vaccin? retval = null;

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select * from f_One_vaccin('@id') ",
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
                    DateTime dateCreation = (DateTime)DB_Convertisseur.Date(reader, "date");
                    string nom = DB_Convertisseur.String(reader, "nom");
                    string descr = DB_Convertisseur.String(reader, "descri");

                    retval = Vaccin.Creer(nom, descr);
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
        public static Vaccin? UnVaccinNom(string nom)
        {
            Vaccin? retval = null;

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select * from f_One_vaccin_by_nom(@nom) ",
                                                           AccessDB.SqlConn);

            try
            {
                sqlcmd.Parameters.Add(new NpgsqlParameter("@nom", NpgsqlTypes.NpgsqlDbType.Varchar));

                sqlcmd.Prepare();

                sqlcmd.Parameters["@nom"].Value = nom;

                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                if (reader.Read())
                {
                    string idty = DB_Convertisseur.String(reader, "id");
                    DateTime dateCreation = (DateTime)DB_Convertisseur.Date(reader, "date");
                    string fnom = DB_Convertisseur.String(reader, "nom");
                    string descr = DB_Convertisseur.String(reader, "descri");

                    retval = Vaccin.Creer(fnom, descr);
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

        public static int Add(Vaccin vaccin)
        {
            string cmdtext = "select * from f_create_vaccin(@nom ,@descr) ";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@nom",(NpgsqlTypes.NpgsqlDbType.Varchar, vaccin.Nom) },
                { "@descr",(NpgsqlTypes.NpgsqlDbType.Varchar , vaccin.Description) }
            };

            return Requets.ResultCommande(cmdtext, parametres);

        }
        public static int Update(Vaccin vaccin)
        {

            string cmdtext = "select * from f_update_vaccin(@id , @nom , @descr)";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@nom",(NpgsqlTypes.NpgsqlDbType.Varchar, vaccin.Nom) },
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar , vaccin.Id) },
                { "@descr",(NpgsqlTypes.NpgsqlDbType.Varchar , vaccin.Description) }
            };

            return Requets.ResultCommande(cmdtext, parametres);

        }
        public static int Delete(string id)
        {
            try
            {
                string cmdtext = "select * from f_delete_vaccin(@id)";

                var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
                {
                    { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar, id) },
                };

                return Requets.ResultCommande(cmdtext, parametres);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.Read();
                return -1;
            }
        }
    }
}
