using Wpf_App_Pattoon_Animalerie.Modele;
using Wpf_App_Pattoon_Animalerie.Service;
using Npgsql;


namespace Wpf_App_Pattoon_Animalerie.AccessDB
{
    public class DB_Vaccination
    {
        public static Dictionary<string, Vaccination> All_From_Db()
        {
            Dictionary<string, Vaccination> retval = new Dictionary<string, Vaccination>();

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select id_anim_vaccin, date_creation, id_vaccin, id_animal, remarque " +
                                                           $"from t_animal_vaccin " +
                                                           $"order by id_anim_vaccin ",
                                                           AccessDB.SqlConn);

            try
            {
                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                while (reader.Read())
                {

                    string id = DB_Convertisseur.String(reader, "id_anim_vaccin");
                    DateTime? dateCreation = DB_Convertisseur.Date(reader, "date_creation");
                    Vaccin? vaccin = DB_Convertisseur.Vaccin(reader, "id_vaccin");
                    Animal? animal = DB_Convertisseur.Animal(reader, "id_animal");
                    string remarque = DB_Convertisseur.String(reader, "remarque");

                    if (vaccin != null && animal != null)
                    {
                        Vaccination vaccination = Vaccination.Creer(animal,vaccin,remarque);
                        vaccination.Id = id;
                        vaccination.DateCreation = (DateTime)dateCreation;
                        retval.Add(vaccination.Id, vaccination);

                        if (AllVaccination.Find(vaccination.Id) == null)
                        {
                            AllVaccination.Add(vaccination);
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
        public static Vaccination? UnVaccinationById(string id)
        {
            Vaccination? retval = null;

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select id_anim_vaccin, date_creation, id_vaccin, id_animal, remarque " +
                                                           $"from t_animal_vaccin " +
                                                           $"where id_anim_vaccin = @id ",
                                                           AccessDB.SqlConn);

            try
            {
                sqlcmd.Parameters.Add(new NpgsqlParameter("@id", NpgsqlTypes.NpgsqlDbType.Varchar));

                sqlcmd.Prepare();

                sqlcmd.Parameters["@id"].Value = id;

                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                if (reader.Read())
                {
                    string fid = DB_Convertisseur.String(reader, "id_anim_vaccin");
                    DateTime? dateCreation = DB_Convertisseur.Date(reader, "date_creation");
                    Vaccin? vaccin = DB_Convertisseur.Vaccin(reader, "id_vaccin");
                    Animal? animal = DB_Convertisseur.Animal(reader, "id_animal");
                    string remarque = DB_Convertisseur.String(reader, "remarque");

                    if (vaccin != null && animal != null)
                    {
                        retval = Vaccination.Creer(animal, vaccin, remarque);
                        retval.Id = fid;
                        retval.DateCreation = (DateTime)dateCreation;

                    }

                }
            }
            catch (Exception ex)
            {
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }
        public static Dictionary<string, Vaccination> LesVaccinsByAnimal(Animal animal)
        {
            Dictionary<string, Vaccination> retval = [];

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select id_anim_vaccin, date_creation, id_vaccin, id_animal, remarque " +
                                                           $"from t_animal_vaccin " +
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
                    string fid = DB_Convertisseur.String(reader, "id_anim_vaccin");
                    DateTime? dateCreation = DB_Convertisseur.Date(reader, "date_creation");
                    Vaccin? vaccin = DB_Convertisseur.Vaccin(reader, "id_vaccin");
                    string remarque = DB_Convertisseur.String(reader, "remarque");

                    if (vaccin != null && animal != null)
                    {
                        Vaccination vaccination = Vaccination.Creer(animal, vaccin, remarque);
                        vaccination.Id = fid;
                        vaccination.DateCreation = (DateTime)dateCreation;
                        retval.Add(vaccination.Id, vaccination);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }

        public static int Add(Vaccination vaccination)
        {
            string cmdtext = "insert into t_animal_vaccin(id_anim_vaccin, date_creation, id_vaccin, id_animal, remarque ) values (@id, @date, @idvac, @idani, @rem) ";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@date",(NpgsqlTypes.NpgsqlDbType.Date, vaccination.DateCreation) },
                { "@idvac",(NpgsqlTypes.NpgsqlDbType.Varchar, vaccination.Vaccin.Id) },
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar , vaccination.Id) },
                { "@idani",(NpgsqlTypes.NpgsqlDbType.Varchar , vaccination.Animal.Id) },
                { "@rem",(NpgsqlTypes.NpgsqlDbType.Varchar , vaccination.Remaque) }
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);

        }
        public static int Update(Vaccination vaccination)
        {

            string cmdtext = "update t_animal_vaccin set id_vaccin = @idvac, id_animal = @idani, remarque = @rem  where id_anim_vaccin = @id ";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@date",(NpgsqlTypes.NpgsqlDbType.Date, vaccination.DateCreation) },
                { "@idvac",(NpgsqlTypes.NpgsqlDbType.Varchar, vaccination.Vaccin.Id) },
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar , vaccination.Id) },
                { "@idani",(NpgsqlTypes.NpgsqlDbType.Varchar , vaccination.Animal.Id) },
                { "@rem",(NpgsqlTypes.NpgsqlDbType.Varchar , vaccination.Remaque) }
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);

        }
        public static int Delete(string id)
        {
            string cmdtext = "delete from t_animal_vaccin where id_anim_vaccin = @id";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar , id) }
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);
        }
    }


}
