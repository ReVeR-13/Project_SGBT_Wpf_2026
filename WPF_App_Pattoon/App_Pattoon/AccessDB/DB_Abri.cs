
using Npgsql;
using Wpf_App_Pattoon_Animalerie.Modele;
using Wpf_App_Pattoon_Animalerie.Service;


namespace Wpf_App_Pattoon_Animalerie.AccessDB
{
    public static class DB_Abri
    {
        public static Dictionary<string, Abri> All_From_db()
        {
            Dictionary<string, Abri> retval = new Dictionary<string, Abri>();

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select id_abri, date_creation, nom_abri, description, statut " +
                                                           $"from t_abri " +
                                                           $"order by id_abri ",
                                                           AccessDB.SqlConn);

            try
            {
                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                while (reader.Read())
                {

                    string id = DB_Convertisseur.String(reader, "id_abri");
                    DateTime? dateCreation = DB_Convertisseur.Date(reader, "date_creation");
                    string libelle = DB_Convertisseur.String(reader, "nom_abri");
                    string descr = DB_Convertisseur.String(reader, "description");
                    EStatutAbri statut = (EStatutAbri)DB_Convertisseur.StatutAbri(reader, "statut");

                    Abri abri = Abri.Creer(libelle,descr);
                    abri.Id = id;
                    abri.DateCreation = (DateTime)dateCreation;
                    abri.Statut = statut;
                    retval.Add(abri.Id, abri);

                    if(AllAbri.Find(abri.Id) == null)
                    {
                        AllAbri.Add(abri);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }
        public static Abri? UnAbriId_db(string id)
        {
            Abri? retval = null;

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select id_abri, date_creation, nom_abri, description, statut " +
                                                           $"from t_abri " +
                                                           $"where id_abri = @id ",
                                                           AccessDB.SqlConn);

            try
            {
                sqlcmd.Parameters.Add(new NpgsqlParameter("@id", NpgsqlTypes.NpgsqlDbType.Varchar));

                sqlcmd.Prepare();

                sqlcmd.Parameters["@id"].Value = id;

                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                if (reader.Read())
                {
                    string idty = DB_Convertisseur.String(reader, "id_abri");
                    DateTime dateCreation = (DateTime)DB_Convertisseur.Date(reader, "date_creation");
                    string nom = DB_Convertisseur.String(reader, "nom_abri");
                    string descr = DB_Convertisseur.String(reader, "description");
                    EStatutAbri statut = (EStatutAbri)DB_Convertisseur.StatutAbri(reader, "statut");

                    retval = Abri.Creer(nom,descr);
                    retval.Id = idty;
                    retval.Statut = statut;
                    retval.DateCreation = dateCreation;


                }
            }
            catch (Exception ex)
            {
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }
        public static Abri? UnAbriByNom_db(string nom)
        {
            Abri? retval = null;

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select id_abri, date_creation, nom_abri, description, statut " +
                                                           $"from t_abri " +
                                                           $"where nom_abri = @nom ",
                                                           AccessDB.SqlConn);

            try
            {
                sqlcmd.Parameters.Add(new NpgsqlParameter("@nom", NpgsqlTypes.NpgsqlDbType.Varchar));

                sqlcmd.Prepare();

                sqlcmd.Parameters["@nom"].Value = nom;

                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                if (reader.Read())
                {
                    string idty = DB_Convertisseur.String(reader, "id_abri");
                    DateTime dateCreation = (DateTime)DB_Convertisseur.Date(reader, "date_creation");
                    string fnom = DB_Convertisseur.String(reader, "nom_abri");
                    string descr = DB_Convertisseur.String(reader, "description");
                    EStatutAbri statut = (EStatutAbri)DB_Convertisseur.StatutAbri(reader, "statut");

                    retval = Abri.Creer(fnom, descr);
                    retval.Id = idty;
                    retval.Statut = statut;
                    retval.DateCreation = dateCreation;

                }
            }
            catch (Exception ex)
            {
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }

        public static int Add(Abri abri)
        {
            string cmdtext = "insert into t_abri (id_abri, date_creation, nom_abri, description, statut ) values (@id, @date, @nom, @descr,@statut::e_statut_abri ) ";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@date",(NpgsqlTypes.NpgsqlDbType.Date, abri.DateCreation) },
                { "@nom",(NpgsqlTypes.NpgsqlDbType.Varchar, abri.Libelle) },
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar , abri.Id) },
                { "@descr",(NpgsqlTypes.NpgsqlDbType.Varchar , abri.Description) },
                { "@statut",(NpgsqlTypes.NpgsqlDbType.Varchar , abri.Statut.ToString() )}
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);

        }
        public static int Update(Abri abri)
        {

            string cmdtext = "update t_abri set nom_abri = @nom, description =@descr, statut = @statut::e_statut_abri   where id_abri = @id ";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@nom",(NpgsqlTypes.NpgsqlDbType.Varchar, abri.Libelle) },
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar , abri.Id) },
                { "@descr",(NpgsqlTypes.NpgsqlDbType.Varchar , abri.Description) },
                { "@statut" , (NpgsqlTypes.NpgsqlDbType.Varchar, abri.Statut.ToString()) }
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);

        }
        public static int Delete(string id)
        {
            string cmdtext = "delete from t_abri where id_abri = @id";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar, id) },
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);
        }
    }
}
