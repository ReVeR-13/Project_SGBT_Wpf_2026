using Wpf_App_Pattoon_Animalerie.Modele;
using Wpf_App_Pattoon_Animalerie.Service;
using Npgsql;


namespace Wpf_App_Pattoon_Animalerie.AccessDB
{
    public class DB_AnimalCouleur
    {
        public static Dictionary<string, AnimalCouleur> All_From_Db()
        {
            Dictionary<string, AnimalCouleur> retval = new();

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select id, date_creation, id_animal, id_couleur " +
                                                           $"from t_couleur_animal " +
                                                           $"order by id ",
                                                           AccessDB.SqlConn);

            try
            {
                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                while (reader.Read())
                {

                    string id = DB_Convertisseur.String(reader, "id");
                    DateTime? dateCreation = DB_Convertisseur.Date(reader, "date_creation");
                    Couleur? comp = DB_Convertisseur.Couleur(reader, "id_couleur");
                    Animal? anim = DB_Convertisseur.Animal(reader, "id_animal");

                    AnimalCouleur compatibilite = AnimalCouleur.Creer(anim, comp);
                    compatibilite.Id = id;
                    compatibilite.DateCreation = (DateTime)dateCreation;
                    retval.Add(compatibilite.Id, compatibilite);

                    if (AllAnimalCouleur.Find(compatibilite.Id) == null)
                    {
                        AllAnimalCouleur.Add(compatibilite);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }
        public static AnimalCouleur? UneColorationById(string id)
        {
            AnimalCouleur? retval = null;

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select id, date_creation, id_couleur, id_animal " +
                                                           $"from t_couleur_animal " +
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
                    DateTime? dateCreation = DB_Convertisseur.Date(reader, "date_creation");
                    Couleur? comp = DB_Convertisseur.Couleur(reader, "id_couleur");
                    Animal? anim = DB_Convertisseur.Animal(reader, "id_animal");


                    retval = AnimalCouleur.Creer(anim, comp);
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
        public static Dictionary<string, AnimalCouleur> ColorationByAnimal(Animal animal)
        {
            Dictionary<string, AnimalCouleur> retval = new();

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select id, date_creation, id_couleur, id_animal " +
                                                           $"from t_couleur_animal " +
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
                    string id = DB_Convertisseur.String(reader, "id");
                    DateTime? dateCreation = DB_Convertisseur.Date(reader, "date_creation");
                    Couleur? comp = DB_Convertisseur.Couleur(reader, "id_couleur");
                    Animal? anim = DB_Convertisseur.Animal(reader, "id_animal");

                    AnimalCouleur compatibilite = AnimalCouleur.Creer(anim, comp);
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

        public static int Add(AnimalCouleur compatibilite)
        {
            string cmdtext = "insert into t_couleur_animal(id, date_creation, id_couleur, id_animal) " +
                                                                "values (@id, @date, @idcomp, @idanim) ";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar , compatibilite.Id) },
                { "@date",(NpgsqlTypes.NpgsqlDbType.Date, compatibilite.DateCreation) },
                { "@idcomp",(NpgsqlTypes.NpgsqlDbType.Varchar, compatibilite.Couleur.Id) },
                { "@idanim",(NpgsqlTypes.NpgsqlDbType.Varchar , compatibilite.Animal.Id) }
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);

        }
        public static int Delete(string id)
        {
            string cmdtext = "delete from t_couleur_animal where id = @id";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar, id) },
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);
        }
    }
}
