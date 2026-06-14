using Wpf_App_Pattoon_Animalerie.Modele;
using Wpf_App_Pattoon_Animalerie.Service;
using Npgsql;

namespace Wpf_App_Pattoon_Animalerie.AccessDB
{
    public class DB_AnimalCompatibilité
    {
        public static Dictionary<string, AnimalCompatibilité> All_From_Db()
        {
            Dictionary<string, AnimalCompatibilité> retval = new Dictionary<string, AnimalCompatibilité>();

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select id_anim_cmp, date_creation, id_compatibilite, id_animal, remarque, valeur " +
                                                           $"from t_animal_compatibilite " +
                                                           $"order by id_anim_cmp ",
                                                           AccessDB.SqlConn);

            try
            {
                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                while (reader.Read())
                {

                    string id = DB_Convertisseur.String(reader, "id_anim_cmp");
                    DateTime? dateCreation = DB_Convertisseur.Date(reader, "date_creation");
                    Compatibilite? comp = DB_Convertisseur.Compatibilite(reader, "id_compatibilite");
                    Animal? anim = DB_Convertisseur.Animal(reader, "id_animal");
                    string descr = DB_Convertisseur.String(reader, "remarque");
                    bool valeur = DB_Convertisseur.Bool(reader, "valeur");

                    AnimalCompatibilité compatibilite = AnimalCompatibilité.Creer(anim,comp,valeur,descr);
                    compatibilite.Id = id;
                    compatibilite.DateCreation = (DateTime)dateCreation;
                    retval.Add(compatibilite.Id, compatibilite);

                    if (AnimalCompatibilitéService.Find(compatibilite.Id) == null)
                    {
                        AnimalCompatibilitéService.Add(compatibilite);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }
        public static AnimalCompatibilité? UnCompatibiliteById(string id)
        {
            AnimalCompatibilité? retval = null;

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select id_anim_cmp, date_creation, id_compatibilite, id_animal, remarque, valeur " +
                                                           $"from t_animal_compatibilite " +
                                                           $"where id_anim_cmp = @id ",
                                                           AccessDB.SqlConn);

            try
            {
                sqlcmd.Parameters.Add(new NpgsqlParameter("@id", NpgsqlTypes.NpgsqlDbType.Varchar));

                sqlcmd.Prepare();

                sqlcmd.Parameters["@id"].Value = id;

                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                if (reader.Read())
                {
                    string idty = DB_Convertisseur.String(reader, "id_anim_cmp");
                    DateTime? dateCreation = DB_Convertisseur.Date(reader, "date_creation");
                    Compatibilite? comp = DB_Convertisseur.Compatibilite(reader, "id_compatibilite");
                    Animal? anim = DB_Convertisseur.Animal(reader, "id_animal");
                    string descr = DB_Convertisseur.String(reader, "remarque");
                    bool valeur = DB_Convertisseur.Bool(reader, "valeur");

                    retval = AnimalCompatibilité.Creer(anim, comp, valeur, descr);
                    retval.Id = idty;
                    retval.DateCreation = (DateTime)dateCreation;

                }
            }
            catch (Exception ex)
            {
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }
        public static Dictionary<string, AnimalCompatibilité> UnCompatibiliteByNom(Animal animal)
        {
            Dictionary<string, AnimalCompatibilité> retval = new Dictionary<string, AnimalCompatibilité>();

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select id_anim_cmp, date_creation, id_compatibilite, id_animal, remarque, valeur " +
                                                           $"from t_animal_compatibilite " +
                                                           $"where id_animal = @id ",
                                                           AccessDB.SqlConn);

            try
            {
                sqlcmd.Parameters.Add(new NpgsqlParameter("@id", NpgsqlTypes.NpgsqlDbType.Varchar));

                sqlcmd.Prepare();

                sqlcmd.Parameters["@id"].Value = animal.Id;

                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                while (reader.Read())
                {
                    string id = DB_Convertisseur.String(reader, "id_anim_cmp");
                    DateTime? dateCreation = DB_Convertisseur.Date(reader, "date_creation");
                    Compatibilite? comp = DB_Convertisseur.Compatibilite(reader, "id_compatibilite");
                    Animal? anim = DB_Convertisseur.Animal(reader, "id_animal");
                    string descr = DB_Convertisseur.String(reader, "remarque");
                    bool valeur = DB_Convertisseur.Bool(reader, "valeur");

                    AnimalCompatibilité compatibilite = AnimalCompatibilité.Creer(anim, comp, valeur, descr);
                    compatibilite.Id = id;
                    compatibilite.DateCreation = (DateTime)dateCreation;
                    retval.Add(compatibilite.Id, compatibilite);

                }
            }
            catch (Exception ex)
            {
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }

        public static int Add(AnimalCompatibilité compatibilite)
        {
            string cmdtext = "insert into t_animal_compatibilite(id_anim_cmp, date_creation, id_compatibilite, id_animal, remarque, valeur  ) " +
                                                                "values (@id, @date, @idcomp, @idanim, @rem, @valeur) ";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar , compatibilite.Id) },
                { "@date",(NpgsqlTypes.NpgsqlDbType.Date, compatibilite.DateCreation) },
                { "@idcomp",(NpgsqlTypes.NpgsqlDbType.Varchar, compatibilite.Compatibilite.Id) },
                { "@idanim",(NpgsqlTypes.NpgsqlDbType.Varchar , compatibilite.Animal.Id) },
                { "@rem",(NpgsqlTypes.NpgsqlDbType.Varchar , compatibilite.Remaque) },
                { "@valeur",(NpgsqlTypes.NpgsqlDbType.Boolean , compatibilite.Compatible) },
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);

        }
        public static int Update(AnimalCompatibilité compatibilite)
        {

            string cmdtext = "update t_animal_compatibilite set id_compatibilite = @idcomp, id_animal = @idanim, remarque = @rem ,valeur = @valeur  where id_anim_cmp = @id ";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar , compatibilite.Id) },
                { "@date",(NpgsqlTypes.NpgsqlDbType.Date, compatibilite.DateCreation) },
                { "@idcomp",(NpgsqlTypes.NpgsqlDbType.Varchar, compatibilite.Compatibilite.Id) },
                { "@idanim",(NpgsqlTypes.NpgsqlDbType.Varchar , compatibilite.Animal.Id) },
                { "@rem",(NpgsqlTypes.NpgsqlDbType.Varchar , compatibilite.Remaque) },
                { "@valeur",(NpgsqlTypes.NpgsqlDbType.Boolean , compatibilite.Compatible) },
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);

        }
        public static int Delete(string id)
        {
            string cmdtext = "delete from t_animal_compatibilite where id_anim_cmp = @id";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar, id) },
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);
        }
    }
}
