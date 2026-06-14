
using Npgsql;
using Wpf_App_Pattoon_Animalerie.Modele;
using Wpf_App_Pattoon_Animalerie.Service;


namespace Wpf_App_Pattoon_Animalerie.AccessDB
{
    public class DB_Accueil
    {
        public static Dictionary<string, Accueil> All_From_Db()
        {
            Dictionary<string, Accueil> retval = new();

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select * " +
                                                           $"from t_accueil " +
                                                           $"order by id_accueil ",
                                                           AccessDB.SqlConn);

            try
            {
                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                while (reader.Read())
                {

                    string id = DB_Convertisseur.String(reader, "id_accueil");
                    DateTime? dateCreation = DB_Convertisseur.Date(reader, "date_creation");
                    string detail = DB_Convertisseur.String(reader, "details");
                    EStatutValidation? statut = DB_Convertisseur.StatutValidation(reader, "statut");
                    Demande? demande = DB_Convertisseur.Demande(reader, "id_demande");
                    string? refus = DB_Convertisseur.String(reader, "raison_refus");
                    DateTime? dteD = DB_Convertisseur.Date(reader, "date_debut");
                    DateTime? dteF = DB_Convertisseur.Date(reader, "date_fin");

                    if (demande != null)
                    {
                        Accueil tpe = Accueil.Creer(demande, detail);
                        tpe.Id = id;
                        tpe.DateCreation = (DateTime)dateCreation;
                        tpe.Statut = (EStatutValidation)statut;
                        tpe.DateDebut = dteD;
                        tpe.DateFin = dteF;
                        tpe.RaisonAnullation = refus;

                        retval.Add(tpe.Id, tpe);

                        if (AllAccueil.Find(tpe.Id) == null)
                        {
                            AllAccueil.Add(tpe);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }
        public static Accueil? UnAccueilById(Accueil accueil)
        {
            Accueil? retval = null;

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select * " +
                                                           $"from t_accueil " +
                                                           $"where id_accueil = @id ",
                                                           AccessDB.SqlConn);

            try
            {
                sqlcmd.Parameters.Add(new NpgsqlParameter("@id", NpgsqlTypes.NpgsqlDbType.Varchar));

                sqlcmd.Prepare();

                sqlcmd.Parameters["@id"].Value = accueil.Id;

                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                if (reader.Read())
                {
                    string id = DB_Convertisseur.String(reader, "id_accueil");
                    DateTime? dateCreation = DB_Convertisseur.Date(reader, "date_creation");
                    string detail = DB_Convertisseur.String(reader, "details");
                    EStatutValidation? statut = DB_Convertisseur.StatutValidation(reader, "statut");
                    Demande? demande = DB_Convertisseur.Demande(reader, "id_demande");
                    string? refus = DB_Convertisseur.String(reader, "raison_refus");
                    DateTime? dteD = DB_Convertisseur.Date(reader, "date_debut");
                    DateTime? dteF = DB_Convertisseur.Date(reader, "date_fin");

                    if (demande != null)
                    {
                        retval = Accueil.Creer(demande, detail);
                        retval.Id = id;
                        retval.DateCreation = (DateTime)dateCreation;
                        retval.Statut = (EStatutValidation)statut;
                        retval.DateDebut = dteD;
                        retval.DateFin = dteF;
                        retval.RaisonAnullation = refus;
                    }

                }
            }
            catch (Exception ex)
            {
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }
        public static Accueil? UnAccueilByDemande(Demande demande)
        {
            Accueil? retval = null;

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select * from t_accueil " +
                                                           $"where id_accueil = @iddm",
                                                           AccessDB.SqlConn);

            try
            {
                sqlcmd.Parameters.Add(new NpgsqlParameter("@iddm", NpgsqlTypes.NpgsqlDbType.Varchar));

                sqlcmd.Prepare();

                sqlcmd.Parameters["@iddm"].Value = demande.Id;

                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                if (reader.Read())
                {
                    string id = DB_Convertisseur.String(reader, "id_accueil");
                    DateTime? dateCreation = DB_Convertisseur.Date(reader, "date_creation");
                    string detail = DB_Convertisseur.String(reader, "details");
                    EStatutValidation? statut = DB_Convertisseur.StatutValidation(reader, "statut");
                    Demande? fdemande = DB_Convertisseur.Demande(reader, "id_demande");
                    string? refus = DB_Convertisseur.String(reader, "raison_refus"); 
                    DateTime? dteD = DB_Convertisseur.Date(reader, "date_debut");
                    DateTime? dteF = DB_Convertisseur.Date(reader, "date_fin");

                    if (demande != null)
                    {
                        retval = Accueil.Creer(fdemande, detail);
                        retval.Id = id;
                        retval.DateCreation = (DateTime)dateCreation;
                        retval.Statut = (EStatutValidation)statut;
                        retval.DateDebut = dteD;
                        retval.DateFin = dteF;
                        retval.RaisonAnullation = refus;
                    }

                }
            }
            catch (Exception ex)
            {
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }

        public static int Add(Accueil accueil)
        {
            string cmdtext = "insert into t_accueil(id_accueil, date_creation, details, statut, id_demande, date_debut, date_fin, raison_refus  ) " +
                                           "values (@id, @date, @detail, @statut::e_statut_accueil, @id_demande ,@date_debut , @date_fin ,@refus) ";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@date",(NpgsqlTypes.NpgsqlDbType.Date, accueil.DateCreation) },
                { "@detail",(NpgsqlTypes.NpgsqlDbType.Varchar, accueil.Info) },
                { "@statut",(NpgsqlTypes.NpgsqlDbType.Varchar, accueil.Statut.ToString()) },
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar , accueil.Id) },
                { "@id_demande",(NpgsqlTypes.NpgsqlDbType.Varchar , accueil.Demande.Id) },
                { "@refus",(NpgsqlTypes.NpgsqlDbType.Varchar , accueil.RaisonAnullation) },
                { "@date_debut",(NpgsqlTypes.NpgsqlDbType.Date , accueil.DateDebut) },
                { "@date_fin",(NpgsqlTypes.NpgsqlDbType.Date , accueil.DateFin) }
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);

        }
        public static int Update(Accueil accueil)
        {

            string cmdtext = "update t_accueil set " +
                "details = @details, statut = @statut::e_statut_accueil, id_demande = @id_demande, " +
                "date_debut = @date_debut, date_fin = @date_fin, raison_refus = @raison_refus " +
                "where id_accueil = @id ";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@details",(NpgsqlTypes.NpgsqlDbType.Varchar, accueil.Info) },
                { "@statut",(NpgsqlTypes.NpgsqlDbType.Varchar, accueil.Statut.ToString()) },
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar , accueil.Id) },
                { "@id_demande",(NpgsqlTypes.NpgsqlDbType.Varchar , accueil.Demande.Id) },
                { "@raison_refus",(NpgsqlTypes.NpgsqlDbType.Varchar , accueil.RaisonAnullation) },
                { "@date_debut",(NpgsqlTypes.NpgsqlDbType.Date , accueil.DateDebut) },
                { "@date_fin",(NpgsqlTypes.NpgsqlDbType.Date , accueil.DateFin) }
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);

        }
        public static int Delete(Accueil accueil)
        {
            string cmdtext = "delete from t_accueil where id_accueil = @id";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar, accueil.Id) },
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);
        }
    }
}
