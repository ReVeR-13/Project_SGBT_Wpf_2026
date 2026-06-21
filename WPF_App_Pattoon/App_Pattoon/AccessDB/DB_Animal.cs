using Wpf_App_Pattoon_Animalerie.Modele;
using Wpf_App_Pattoon_Animalerie.Service;
using Npgsql;


namespace Wpf_App_Pattoon_Animalerie.AccessDB
{
    public static class DB_Animal
    {
        public static Dictionary<string, Animal> Db_listeAnimaux()
        {
            Dictionary<string, Animal> retval = [];

            using NpgsqlCommand sqlcmd = new(DB_Convertisseur.SelectFrom("f_All_animal()"),
                AccessDB.SqlConn);

            try
            {
                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                while (reader.Read())
                {

                    string? id                  = DB_Convertisseur.String(reader, "id");
                    DateTime? dateCreation      = DB_Convertisseur.Date( reader, "_date");
                    string? nom                 = DB_Convertisseur.String(reader, "_nom");
                    string? sexe                = DB_Convertisseur.String(reader, "_sexe");
                    Couleur? couleur            = DB_Convertisseur.Couleur(reader, "_couleur");
                    DateTime? dateNaissance     = DB_Convertisseur.Date(reader, "_date_naiss");
                    DateTime? dateDeces         = DB_Convertisseur.Date(reader, "_dte_deces");
                    bool Sterile                = DB_Convertisseur.Bool(reader, "_sterile");
                    DateTime? DateSterilisation = DB_Convertisseur.Date(reader, "_dte_sterile");
                    string? Particularite       = DB_Convertisseur.String(reader, "_particularite");
                    string? Description         = DB_Convertisseur.String(reader, "_description");

                    string? idtype              = DB_Convertisseur.String(reader, "_type");
                    EStatutAnimal statut        = (EStatutAnimal)DB_Convertisseur.StatutAnimal(reader, "_statut");
                    string? idabri              = DB_Convertisseur.String(reader, "_id_abri"); 

                    

                    TypeAnimal? typeAnimal = AllTypeAnimal.FindTypebyId(idtype);
                    if (typeAnimal != null )
                    {
                        Animal? animal = Animal.Creer(nom, typeAnimal.Nom, dateNaissance, sexe, couleur, Sterile, DateSterilisation, Description, Particularite);
                        animal.Id = id;
                        animal.Statut = statut;
                        if (idabri != null)
                        {
                            animal.Abri = AllAbri.Find(idabri);
                        }
                        animal.DateCreation = (DateTime)dateCreation;

                        retval.Add(animal.Id, animal);

                        if (AllAnimal.Rechercher(animal.Id) == null)
                        {
                            AllAnimal.Add(animal);
                        }
                        
                    }

                    
                }
                
            }catch (Exception ex)
            {
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }
        public static Animal? UnAnimalById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                ExceptionLauncher.New("DB_Animal UnAnimalById", $"l'ID Animale ne peut etre vide : {nameof(id)}");
            }

            Animal? retval = null;

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select * " +
                                                           $"from t_animal " +
                                                           $"where id_animal = @id ",
                                                           AccessDB.SqlConn);

            try
            {
                sqlcmd.Parameters.Add(new NpgsqlParameter("@id", NpgsqlTypes.NpgsqlDbType.Varchar));

                sqlcmd.Prepare();

                sqlcmd.Parameters["@id"].Value = id;

                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                if (reader.Read())
                {
                    string? fid = DB_Convertisseur.String(reader, "id_animal");
                    DateTime? dateCreation = DB_Convertisseur.Date(reader, "date_creation");
                    string? nom = DB_Convertisseur.String(reader, "nom_animal");
                    string? sexe = DB_Convertisseur.String(reader, "sexe");
                    Couleur? couleur = DB_Convertisseur.Couleur(reader, "couleur");
                    DateTime? dateNaissance = DB_Convertisseur.Date(reader, "date_naissance");
                    DateTime? dateDeces = DB_Convertisseur.Date(reader, "date_deces");
                    bool Sterile = DB_Convertisseur.Bool(reader, "sterile");
                    DateTime? DateSterilisation = DB_Convertisseur.Date(reader, "date_sterile");
                    string? Particularite = DB_Convertisseur.String(reader, "particularité");
                    string? Description = DB_Convertisseur.String(reader, "description");

                    string? idtype = DB_Convertisseur.String(reader, "id_type_animal");
                    EStatutAnimal statut = (EStatutAnimal)DB_Convertisseur.StatutAnimal(reader, "statut");
                    string? idabri = DB_Convertisseur.String(reader, "id_abri");

                    TypeAnimal? typeAnimal = AllTypeAnimal.FindTypebyId(idtype);
                    if (typeAnimal != null)
                    {
                        retval = Animal.Creer(nom, typeAnimal.Nom, dateNaissance, sexe, couleur, Sterile, DateSterilisation, Description, Particularite);
                        retval.Id = fid;
                        retval.DateCreation = dateCreation ?? DateTime.Now;
                        retval.Statut = statut;
                        if (idabri != null)
                        {
                            retval.Abri = AllAbri.Find(idabri);
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

        public static int Add(Animal animal)
        {
            string cmdtext = "insert into t_animal(id_animal ,nom_animal, date_creation, sexe, couleur, date_naissance, " +
                                                  " date_deces, sterile, date_sterile, particularité, description , id_type_animal, statut) " +
                                          " values(@id, @nom, @date,  @sexe::e_sexe, @couleur , @date_naissance, @date_deces, @sterile , @date_sterile ," +
                                                  "@particularité , @description , @id_type_animal, @statut::e_statut_animal ) ";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@date",(NpgsqlTypes.NpgsqlDbType.Date, animal.DateCreation) },
                { "@nom",(NpgsqlTypes.NpgsqlDbType.Varchar, animal.Nom) },
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar , animal.Id) },
                { "@sexe",(NpgsqlTypes.NpgsqlDbType.Varchar , animal.Sexe.ToString()) },
                { "@couleur",(NpgsqlTypes.NpgsqlDbType.Varchar , animal.Couleur.Id) },
                { "@date_naissance",(NpgsqlTypes.NpgsqlDbType.Date , animal.DateNaissance) },
                { "@date_deces",(NpgsqlTypes.NpgsqlDbType.Date , animal.DateDeces) },
                { "@sterile",(NpgsqlTypes.NpgsqlDbType.Boolean , animal.Sterile) },
                { "@date_sterile",(NpgsqlTypes.NpgsqlDbType.Date , animal.DateSterilisation) },
                { "@particularité",(NpgsqlTypes.NpgsqlDbType.Varchar , animal.Particularite) },
                { "@description",(NpgsqlTypes.NpgsqlDbType.Varchar , animal.Description) },
                { "@id_type_animal",(NpgsqlTypes.NpgsqlDbType.Varchar , animal.Type.Id) },
                { "@statut",(NpgsqlTypes.NpgsqlDbType.Varchar , animal.Statut.ToString()) }
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);

        }
        public static int Update(Animal animal)
        {

            string cmdtext = "update t_animal set nom_animal = @nom, " +
                                                 "sexe = @sexe::e_sexe, " +
                                                 "couleur = @couleur, " +
                                                 "date_naissance = @date_naissance, " +
                                                 "date_deces = @date_deces, " +
                                                 "sterile = @sterile, " +
                                                 "date_sterile = @date_sterile, " +
                                                 "particularité = @particularité, " +
                                                 "description = @description, " +
                                                 "id_type_animal = @id_type_animal, " +
                                                 "statut = @statut::e_statut_animal, " +
                                                 "id_abri = @id_abri " +
                             " where id_animal = @id ";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@date",(NpgsqlTypes.NpgsqlDbType.Date, animal.DateCreation) },
                { "@nom",(NpgsqlTypes.NpgsqlDbType.Varchar, animal.Nom) },
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar , animal.Id) },
                { "@sexe",(NpgsqlTypes.NpgsqlDbType.Varchar , animal.Sexe.ToString()) },
                { "@couleur",(NpgsqlTypes.NpgsqlDbType.Varchar , animal.Couleur.Id) },
                { "@date_naissance",(NpgsqlTypes.NpgsqlDbType.Date , animal.DateNaissance) },
                { "@date_deces",(NpgsqlTypes.NpgsqlDbType.Date , animal.DateDeces) },
                { "@sterile",(NpgsqlTypes.NpgsqlDbType.Boolean , animal.Sterile) },
                { "@date_sterile",(NpgsqlTypes.NpgsqlDbType.Date , animal.DateSterilisation) },
                { "@particularité",(NpgsqlTypes.NpgsqlDbType.Varchar , animal.Particularite) },
                { "@description",(NpgsqlTypes.NpgsqlDbType.Varchar , animal.Description) },
                { "@id_type_animal",(NpgsqlTypes.NpgsqlDbType.Varchar , animal.Type.Id) },
                { "@statut",(NpgsqlTypes.NpgsqlDbType.Varchar , animal.Statut.ToString()) },
                { "@id_abri",(NpgsqlTypes.NpgsqlDbType.Varchar , animal.Abri?.Id) }
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);

        }
        public static int UpdateStatut(Animal animal)
        {
            int ret = 0;
            if (animal != null)
            {
                string cmdtext = "update t_animal set statut = @statut::e_statut_animal " +
                              " where id_animal = @id ";

                var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
                {
                    { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar , animal.Id) },
                    { "@statut",(NpgsqlTypes.NpgsqlDbType.Varchar , animal.Statut.ToString()) },
                };
                ret = Requets.ExecuteNonQuery(cmdtext, parametres);
            }
            
            return ret;

        }
        public static int UpdateAbri(Animal animal)
        {
            int ret = 0;
            if (animal !=null && animal.Abri != null)
            {
                string cmdtext = "update t_animal set id_abri = @idabri " +
                              " where id_animal = @id ";

                var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
                {
                    { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar , animal.Id) },
                    { "@idabri",(NpgsqlTypes.NpgsqlDbType.Varchar , animal.Abri.Id) },
                };

                ret = Requets.ExecuteNonQuery(cmdtext, parametres);
            }
            

            return ret;

        }
        public static int Delete(string id)
        {
            string cmdtext = "delete from t_animal where id_animal = @id";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar, id) },
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);
        }
    }


}
