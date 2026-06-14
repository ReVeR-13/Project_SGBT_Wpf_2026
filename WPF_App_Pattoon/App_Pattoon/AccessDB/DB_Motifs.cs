using Wpf_App_Pattoon_Animalerie.Modele;
using Wpf_App_Pattoon_Animalerie.Service;
using Npgsql;

namespace Wpf_App_Pattoon_Animalerie.AccessDB
{
    public class DB_MotifEntree
    {
        public static Dictionary<string, MotifEntree> All_From_Db()
        {
            Dictionary<string, MotifEntree> retval = new Dictionary<string, MotifEntree>();

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select * " +
                                                           $"from t_motif_entree " +
                                                           $"order by id_motif ",
                                                           AccessDB.SqlConn);

            try
            {
                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                while (reader.Read())
                {

                    string id = DB_Convertisseur.String(reader, "id_motif");
                    DateTime? dateCreation = DB_Convertisseur.Date(reader, "date_creation");
                    string nom = DB_Convertisseur.String(reader, "libele");
                    string descr = DB_Convertisseur.String(reader, "description");

                    MotifEntree tpe = MotifEntree.Creer(nom, descr);
                    tpe.Id = id;
                    tpe.DateCreation = (DateTime)dateCreation;
                    retval.Add(tpe.Id, tpe);

                    if (AllMotifsEntrees.FindById(tpe.Id) == null)
                    {
                        AllMotifsEntrees.Add(tpe);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }
        public static MotifEntree? UnMotifById(MotifEntree motif)
        {
            MotifEntree? retval = null;

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select *" +
                                                           $"from t_motif_entree " +
                                                           $"where id_motif = @id ",
                                                           AccessDB.SqlConn);

            try
            {
                sqlcmd.Parameters.Add(new NpgsqlParameter("@id", NpgsqlTypes.NpgsqlDbType.Varchar));

                sqlcmd.Prepare();

                sqlcmd.Parameters["@id"].Value = motif.Id;

                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                if (reader.Read())
                {
                    string id = DB_Convertisseur.String(reader, "id_motif");
                    DateTime? dateCreation = DB_Convertisseur.Date(reader, "date_creation");
                    string nom = DB_Convertisseur.String(reader, "libele");
                    string descr = DB_Convertisseur.String(reader, "description");

                    retval = MotifEntree.Creer(nom, descr);
                    retval.Id = id;
                    retval.DateCreation = (DateTime)dateCreation;

                }
            }
            catch (Exception ex)
            {
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }
        public static MotifEntree? UnMotifByNom(MotifEntree motif)
        {
            MotifEntree? retval = null;

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select * from t_motif_entree " +
                                                           $"where libele = @nom",
                                                           AccessDB.SqlConn);

            try
            {
                sqlcmd.Parameters.Add(new NpgsqlParameter("@nom", NpgsqlTypes.NpgsqlDbType.Varchar));

                sqlcmd.Prepare();

                sqlcmd.Parameters["@nom"].Value = motif.Libele;

                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                if (reader.Read())
                {
                    string id = DB_Convertisseur.String(reader, "id_motif");
                    DateTime? dateCreation = DB_Convertisseur.Date(reader, "date_creation");
                    string nom = DB_Convertisseur.String(reader, "libele");
                    string descr = DB_Convertisseur.String(reader, "description");

                    retval = MotifEntree.Creer(nom, descr);
                    retval.Id = id;
                    retval.DateCreation = (DateTime)dateCreation;

                }
            }
            catch (Exception ex)
            {
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }

        public static int Add(MotifEntree motif)
        {
            string cmdtext = "insert into t_motif_entree(id_motif, date_creation, libele, description ) values (@id, @date, @nom, @descr) ";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@date",(NpgsqlTypes.NpgsqlDbType.Date, motif.DateCreation) },
                { "@nom",(NpgsqlTypes.NpgsqlDbType.Varchar, motif.Libele) },
                { "@descr",(NpgsqlTypes.NpgsqlDbType.Varchar, motif.Details) },
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar , motif.Id) }
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);

        }
        public static int Update(MotifEntree motif)
        {

            string cmdtext = "update t_motif_entree set libele = @nom, description = @descr where id_motif = @id ";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@nom",(NpgsqlTypes.NpgsqlDbType.Varchar, motif.Libele) },
                { "@descr",(NpgsqlTypes.NpgsqlDbType.Varchar, motif.Details) },
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar , motif.Id) }
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);

        }
        public static int Delete(MotifEntree motif)
        {
            string cmdtext = "delete from t_motif_entree where id_motif = @id";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar, motif.Id) },
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);
        }
    }
    public class DB_MotifSortie
    {
        public static Dictionary<string, MotifSortie> All_From_Db()
        {
            Dictionary<string, MotifSortie> retval = new Dictionary<string, MotifSortie>();

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select * " +
                                                           $"from t_motif_sortie " +
                                                           $"order by id_motif ",
                                                           AccessDB.SqlConn);

            try
            {
                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                while (reader.Read())
                {

                    string id = DB_Convertisseur.String(reader, "id_motif");
                    DateTime? dateCreation = DB_Convertisseur.Date(reader, "date_creation");
                    string nom = DB_Convertisseur.String(reader, "libele");
                    string descr = DB_Convertisseur.String(reader, "description");

                    MotifSortie tpe = MotifSortie.Creer(nom, descr);
                    tpe.Id = id;
                    tpe.DateCreation = (DateTime)dateCreation;
                    retval.Add(tpe.Id, tpe);

                    if (AllMotifsSortie.FindById(tpe.Id) == null)
                    {
                        AllMotifsSortie.Add(tpe);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }
        public static MotifSortie? UnMotifById(MotifSortie motif)
        {
            MotifSortie? retval = null;

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select *" +
                                                           $"from t_motif_sortie " +
                                                           $"where id_motif = @id ",
                                                           AccessDB.SqlConn);

            try
            {
                sqlcmd.Parameters.Add(new NpgsqlParameter("@id", NpgsqlTypes.NpgsqlDbType.Varchar));

                sqlcmd.Prepare();

                sqlcmd.Parameters["@id"].Value = motif.Id;

                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                if (reader.Read())
                {
                    string id = DB_Convertisseur.String(reader, "id_motif");
                    DateTime? dateCreation = DB_Convertisseur.Date(reader, "date_creation");
                    string nom = DB_Convertisseur.String(reader, "libele");
                    string descr = DB_Convertisseur.String(reader, "description");

                    retval = MotifSortie.Creer(nom, descr);
                    retval.Id = id;
                    retval.DateCreation = (DateTime)dateCreation;

                }
            }
            catch (Exception ex)
            {
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }
        public static MotifSortie? UnMotifByNom(MotifSortie motif)
        {
            MotifSortie? retval = null;

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select * from t_motif_sortie " +
                                                           $"where libele = @nom",
                                                           AccessDB.SqlConn);

            try
            {
                sqlcmd.Parameters.Add(new NpgsqlParameter("@nom", NpgsqlTypes.NpgsqlDbType.Varchar));

                sqlcmd.Prepare();

                sqlcmd.Parameters["@nom"].Value = motif.Libele;

                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                if (reader.Read())
                {
                    string id = DB_Convertisseur.String(reader, "id_motif");
                    DateTime? dateCreation = DB_Convertisseur.Date(reader, "date_creation");
                    string nom = DB_Convertisseur.String(reader, "libele");
                    string descr = DB_Convertisseur.String(reader, "description");

                    retval = MotifSortie.Creer(nom, descr);
                    retval.Id = id;
                    retval.DateCreation = (DateTime)dateCreation;

                }
            }
            catch (Exception ex)
            {
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }

        public static int Add(MotifSortie motif)
        {
            string cmdtext = "insert into t_motif_sortie(id_motif, date_creation, libele, description ) values (@id, @date, @nom, @descr) ";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@date",(NpgsqlTypes.NpgsqlDbType.Date, motif.DateCreation) },
                { "@nom",(NpgsqlTypes.NpgsqlDbType.Varchar, motif.Libele) },
                { "@descr",(NpgsqlTypes.NpgsqlDbType.Varchar, motif.Details) },
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar , motif.Id) }
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);

        }
        public static int Update(MotifSortie motif)
        {

            string cmdtext = "update t_motif_sortie set libele = @nom, description = @descr where id_motif = @id ";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@nom",(NpgsqlTypes.NpgsqlDbType.Varchar, motif.Libele) },
                { "@descr",(NpgsqlTypes.NpgsqlDbType.Varchar, motif.Details) },
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar , motif.Id) }
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);

        }
        public static int Delete(MotifSortie motif)
        {
            string cmdtext = "delete from t_motif_sortie where id_motif = @id";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar, motif.Id) },
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);
        }
    }
}
