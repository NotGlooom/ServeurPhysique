using ProjetSynthese.Server.Models;
using Microsoft.AspNetCore.Identity;
using ProjetSynthese.Server.Models.Question;
using ProjetSynthese.Server.Models.Reponse;
using Microsoft.EntityFrameworkCore;

namespace ProjetSynthese.Server
{
    public static class InitialiseurBD
    {

        /// <summary>
        /// Liste des utilisateurs seedés
        /// </summary>
        public static readonly List<IdentityUser> users = new List<IdentityUser>
        {
            new IdentityUser { UserName = "11442", Email = "11442@cegepoutaouais.qc.ca", NormalizedEmail = "11442@CEGEPOUTAOUAIS.QC.CA" },
            new IdentityUser { UserName = "12342", Email = "12342@cegepoutaouais.qc.ca", NormalizedEmail = "12342@CEGEPOUTAOUAIS.QC.CA" },
            new IdentityUser { UserName = "12353", Email = "12353@cegepoutaouais.qc.ca", NormalizedEmail = "12353@CEGEPOUTAOUAIS.QC.CA" },
            new IdentityUser { UserName = "39458", Email = "39458@cegepoutaouais.qc.ca", NormalizedEmail = "39458@CEGEPOUTAOUAIS.QC.CA" },
            new IdentityUser { UserName = "82748", Email = "82748@cegepoutaouais.qc.ca", NormalizedEmail = "82748@CEGEPOUTAOUAIS.QC.CA" },
            new IdentityUser { UserName = "83742", Email = "83742@cegepoutaouais.qc.ca", NormalizedEmail = "83742@CEGEPOUTAOUAIS.QC.CA" },
            new IdentityUser { UserName = "19273", Email = "19273@cegepoutaouais.qc.ca", NormalizedEmail = "19273@CEGEPOUTAOUAIS.QC.CA" },
            new IdentityUser { UserName = "10831", Email = "10831@cegepoutaouais.qc.ca", NormalizedEmail = "10831@CEGEPOUTAOUAIS.QC.CA" },
            new IdentityUser { UserName = "12938", Email = "12938@cegepoutaouais.qc.ca", NormalizedEmail = "12938@CEGEPOUTAOUAIS.QC.CA" },
            new IdentityUser { UserName = "12922", Email = "12922@cegepoutaouais.qc.ca", NormalizedEmail = "12922@CEGEPOUTAOUAIS.QC.CA" },
            new IdentityUser { UserName = "83749", Email = "83749@cegepoutaouais.qc.ca", NormalizedEmail = "83749@CEGEPOUTAOUAIS.QC.CA" },
            new IdentityUser { UserName = "19283", Email = "19283@cegepoutaouais.qc.ca", NormalizedEmail = "19283@CEGEPOUTAOUAIS.QC.CA" },
            new IdentityUser { UserName = "59293", Email = "59293@cegepoutaouais.qc.ca", NormalizedEmail = "59293@CEGEPOUTAOUAIS.QC.CA" },
            new IdentityUser { UserName = "71934", Email = "71934@cegepoutaouais.qc.ca", NormalizedEmail = "71934@CEGEPOUTAOUAIS.QC.CA" },
            new IdentityUser { UserName = "27272", Email = "27272@cegepoutaouais.qc.ca", NormalizedEmail = "27272@CEGEPOUTAOUAIS.QC.CA" },
            new IdentityUser { UserName = "2345678", Email = "2345678@cegepoutaouais.qc.ca", NormalizedEmail = "2345678@CEGEPOUTAOUAIS.QC.CA" },
            new IdentityUser { UserName = "3456789", Email = "3456789@cegepoutaouais.qc.ca", NormalizedEmail = "3456789@CEGEPOUTAOUAIS.QC.CA" },
            new IdentityUser { UserName = "4567890", Email = "4567890@cegepoutaouais.qc.ca", NormalizedEmail = "4567890@CEGEPOUTAOUAIS.QC.CA" },
            new IdentityUser { UserName = "5678901", Email = "5678901@cegepoutaouais.qc.ca", NormalizedEmail = "5678901@CEGEPOUTAOUAIS.QC.CA" },
            new IdentityUser { UserName = "6789012", Email = "6789012@cegepoutaouais.qc.ca", NormalizedEmail = "6789012@CEGEPOUTAOUAIS.QC.CA" },
            new IdentityUser { UserName = "7890123", Email = "7890123@cegepoutaouais.qc.ca", NormalizedEmail = "7890123@CEGEPOUTAOUAIS.QC.CA" },
            new IdentityUser { UserName = "8901234", Email = "8901234@cegepoutaouais.qc.ca", NormalizedEmail = "8901234@CEGEPOUTAOUAIS.QC.CA" },
            new IdentityUser { UserName = "9012345", Email = "9012345@cegepoutaouais.qc.ca", NormalizedEmail = "9012345@CEGEPOUTAOUAIS.QC.CA" },
            new IdentityUser { UserName = "0123456", Email = "0123456@cegepoutaouais.qc.ca", NormalizedEmail = "0123456@CEGEPOUTAOUAIS.QC.CA" },
            new IdentityUser { UserName = "1234500", Email = "1234500@cegepoutaouais.qc.ca", NormalizedEmail = "1234500@CEGEPOUTAOUAIS.QC.CA" },
            new IdentityUser { UserName = "2345600", Email = "2345600@cegepoutaouais.qc.ca", NormalizedEmail = "2345600@CEGEPOUTAOUAIS.QC.CA" },
            new IdentityUser { UserName = "3456700", Email = "3456700@cegepoutaouais.qc.ca", NormalizedEmail = "3456700@CEGEPOUTAOUAIS.QC.CA" },
            new IdentityUser { UserName = "4567800", Email = "4567800@cegepoutaouais.qc.ca", NormalizedEmail = "4567800@CEGEPOUTAOUAIS.QC.CA" },
            new IdentityUser { UserName = "5678900", Email = "5678900@cegepoutaouais.qc.ca", NormalizedEmail = "5678900@CEGEPOUTAOUAIS.QC.CA" },
            new IdentityUser { UserName = "5126987", Email = "5126987@cegepoutaouais.qc.ca", NormalizedEmail = "5126987@CEGEPOUTAOUAIS.QC.CA" }
        };

        private static Dictionary<string, IdentityUser> _usersDict;
        public static Dictionary<string, IdentityUser> UserDict
        {
            get
            {
                if (_usersDict == null)
                {
                    _usersDict = new Dictionary<string, IdentityUser>();
                    foreach (IdentityUser user in users)
                    {
                        _usersDict.Add(user.Email, user);
                    }
                }
                return _usersDict;
            }
        }

        public static List<Enseignant> _enseignants = new List<Enseignant>
        {
            new Enseignant { utilisateur = UserDict["11442@cegepoutaouais.qc.ca"], NoEnseignant = "11442", Nom = "Martin", Prenom = "Alice" },
            new Enseignant { utilisateur = UserDict["12342@cegepoutaouais.qc.ca"], NoEnseignant = "12342", Nom = "Johnson", Prenom = "Bob" },
            new Enseignant { utilisateur = UserDict["12353@cegepoutaouais.qc.ca"], NoEnseignant = "12353", Nom = "Dupont", Prenom = "Pierre" },
            new Enseignant { utilisateur = UserDict["39458@cegepoutaouais.qc.ca"], NoEnseignant = "39458", Nom = "Martin", Prenom = "Jean" },
            new Enseignant { utilisateur = UserDict["82748@cegepoutaouais.qc.ca"], NoEnseignant = "82748", Nom = "Bernard", Prenom = "Sophie" },
            new Enseignant { utilisateur = UserDict["83742@cegepoutaouais.qc.ca"], NoEnseignant = "83742", Nom = "Dubois", Prenom = "Alain" },
            new Enseignant { utilisateur = UserDict["19273@cegepoutaouais.qc.ca"], NoEnseignant = "19273", Nom = "Laurent", Prenom = "William" },
            new Enseignant { utilisateur = UserDict["10831@cegepoutaouais.qc.ca"], NoEnseignant = "10831", Nom = "Fontaine", Prenom = "Chloé" },
            new Enseignant { utilisateur = UserDict["12938@cegepoutaouais.qc.ca"], NoEnseignant = "12938", Nom = "Ternette", Prenom = "Alain" },
            new Enseignant { utilisateur = UserDict["12922@cegepoutaouais.qc.ca"], NoEnseignant = "12922", Nom = "Pelpu", Prenom = "Sara" },
            new Enseignant { utilisateur = UserDict["83749@cegepoutaouais.qc.ca"], NoEnseignant = "83749", Nom = "Roy", Prenom = "Nathan" },
            new Enseignant { utilisateur = UserDict["19283@cegepoutaouais.qc.ca"], NoEnseignant = "19283", Nom = "Scott", Prenom = "Lucas" },
            new Enseignant { utilisateur = UserDict["59293@cegepoutaouais.qc.ca"], NoEnseignant = "59293", Nom = "Parker", Prenom = "Peter" },
            new Enseignant { utilisateur = UserDict["71934@cegepoutaouais.qc.ca"], NoEnseignant = "71934", Nom = "Strange", Prenom = "Steven" },
            new Enseignant { utilisateur = UserDict["27272@cegepoutaouais.qc.ca"], NoEnseignant = "27272", Nom = "Stark", Prenom = "Tony" }
        };

        public static List<Etudiant> _etudiants = new List<Etudiant>
        {
            new Etudiant { utilisateur = UserDict["2345678@cegepoutaouais.qc.ca"], Nom = "Martin", Prenom = "Marie", numeroEtudiant = "2345678" },
            new Etudiant { utilisateur = UserDict["3456789@cegepoutaouais.qc.ca"], Nom = "Durand", Prenom = "Pierre", numeroEtudiant = "3456789" },
            new Etudiant { utilisateur = UserDict["4567890@cegepoutaouais.qc.ca"], Nom = "Leroy", Prenom = "Sophie", numeroEtudiant = "4567890" },
            new Etudiant { utilisateur = UserDict["5678901@cegepoutaouais.qc.ca"], Nom = "Moreau", Prenom = "Lucas", numeroEtudiant = "5678901" },
            new Etudiant { utilisateur = UserDict["6789012@cegepoutaouais.qc.ca"], Nom = "Simon", Prenom = "Camille", numeroEtudiant = "6789012" },
            new Etudiant { utilisateur = UserDict["7890123@cegepoutaouais.qc.ca"], Nom = "Fournier", Prenom = "Antoine", numeroEtudiant = "7890123" },
            new Etudiant { utilisateur = UserDict["8901234@cegepoutaouais.qc.ca"], Nom = "Rousseau", Prenom = "Julie", numeroEtudiant = "8901234" },
            new Etudiant { utilisateur = UserDict["9012345@cegepoutaouais.qc.ca"], Nom = "Blanc", Prenom = "Maxime", numeroEtudiant = "9012345" },
            new Etudiant { utilisateur = UserDict["0123456@cegepoutaouais.qc.ca"], Nom = "Garcia", Prenom = "Laura", numeroEtudiant = "0123456" },
            new Etudiant { utilisateur = UserDict["1234500@cegepoutaouais.qc.ca"], Nom = "Bernard", Prenom = "Théo", numeroEtudiant = "1234500" },
            new Etudiant { utilisateur = UserDict["2345600@cegepoutaouais.qc.ca"], Nom = "Thomas", Prenom = "Emma", numeroEtudiant = "2345600" },
            new Etudiant { utilisateur = UserDict["3456700@cegepoutaouais.qc.ca"], Nom = "Petit", Prenom = "Alexandre", numeroEtudiant = "3456700" },
            new Etudiant { utilisateur = UserDict["4567800@cegepoutaouais.qc.ca"], Nom = "Robert", Prenom = "Léa", numeroEtudiant = "4567800" },
            new Etudiant { utilisateur = UserDict["5678900@cegepoutaouais.qc.ca"], Nom = "Richard", Prenom = "Nicolas", numeroEtudiant = "5678900" },
            new Etudiant { utilisateur = UserDict["5126987@cegepoutaouais.qc.ca"], Nom = "Guy", Prenom = "Paul", numeroEtudiant = "5126987" }
        };

        public static List<Notification> _notifications = new List<Notification>
        {
            new Notification { Message = "Activité à remettre dans 2 jours", Date = DateTime.Now.AddDays(2) },
            new Notification { Message = "Activité à remettre dans 4 jours", Date = DateTime.Now.AddDays(4) },
            new Notification { Message = "Activité à remettre dans 1 semaine", Date = DateTime.Now.AddDays(10) },
            new Notification { Message = "Nouvelle activité ajoutée" },
            new Notification { Message = "Nouvelle activité ajoutée" },
            new Notification { Message = "Nouvelle rétroaction" },
            new Notification { Message = "Nouvelle activité ajoutée" },
            new Notification { Message = "Vous n'avez pas remis cette activité" },
            new Notification { Message = "Activité à remettre dans 1 mois", Date = DateTime.Now.AddMonths(1) },
            new Notification { Message = "Activité à remettre dans 1 mois", Date = DateTime.Now.AddMonths(1) }
        };

        public static List<RappelActivite> _rappelActivites = new List<RappelActivite>
        {
            new RappelActivite { Time = 1, TimeFrame = TimeFrame.Minute },
            new RappelActivite { Time = 20, TimeFrame = TimeFrame.Heure },
            new RappelActivite { Time = 100, TimeFrame = TimeFrame.Minute },
            new RappelActivite { Time = 1, TimeFrame = TimeFrame.Heure },
            new RappelActivite { Time = 2, TimeFrame = TimeFrame.Jour },
            new RappelActivite { Time = 10, TimeFrame = TimeFrame.Heure }
        };

        public static List<Activite> _activites = new List<Activite>
        {
            new Activite
            {
                Nom = "Les Lois de la Physique en Action",
                Description = "Mettez en action les lois de la physique.",
                NumeroActivite = 1,
                DateEcheance = DateTime.Now.AddHours(-2),
                AuteurUtilisateurId = UserDict["11442@cegepoutaouais.qc.ca"].Id,
                IsPublique = true,
                DatePublication = DateTime.Now.AddDays(-2),
            },
            new Activite
            {
                Nom = "Les Énergies et leurs Transformations",
                Description = "Explorez les différents types d'énergie et leurs transformations.",
                NumeroActivite = 2,
                DateEcheance = DateTime.Now.AddDays(3),
                AuteurUtilisateurId = UserDict["12938@cegepoutaouais.qc.ca"].Id,
                IsPublique = true,
                DatePublication = DateTime.Now.AddDays(-1),
            },
            new Activite
            {
                Nom = "Les Lois de la Thermodynamique",
                Description = "Comprenez les principes de la thermodynamique.",
                NumeroActivite = 3,
                DateEcheance = DateTime.Now.AddDays(4),
                AuteurUtilisateurId = UserDict["11442@cegepoutaouais.qc.ca"].Id,
                IsPublique = true,
                DatePublication = DateTime.Now.AddDays(-1),
            },
            new Activite 
            {
                Nom = "Les vecteurs",
                Description = "Les vecteurs c'est fou et dure.",
                NumeroActivite = 4,
                DateEcheance = DateTime.Now.AddDays(2),
                AuteurUtilisateurId = UserDict["11442@cegepoutaouais.qc.ca"].Id,
                IsPublique = false,
                DatePublication = DateTime.Now.AddDays(-2),
            },
        };

        private static Dictionary<string, Activite> _NomActiviteDict = new Dictionary<string, Activite>();
        public static Dictionary<string, Activite> NomActiviteDict
        {
            get
            {
                _NomActiviteDict = new Dictionary<string, Activite>();
                foreach (Activite activite in _activites)
                {
                    _NomActiviteDict.Add(activite.Nom, activite);
                }

                return _NomActiviteDict;
            }
        }

        public static List<GroupeCours> _groupeCours = new List<GroupeCours>
        {
            new GroupeCours { Nom = "Physique 1", Numgroupe = 1, Campus = "Gabrielle-Roy", Code= "420-5A4-HU", Lien = "354642", SN = 1, Activities = new List<Activite>()
            {
                _activites[0],
                _activites[1],
                _activites[2],
            } },
            new GroupeCours { Nom = "Physique 2", Numgroupe = 2, Campus = "Félix LeClerc", Code= "420-5A4-HU", Lien = "674921", SN = 1, Activities = new List<Activite>()
            {
               _activites[3]
            } },
            new GroupeCours { Nom = "Physique 3", Numgroupe = 1, Campus = "Gabrielle-Roy", Code= "420-5A4-HU", Lien = "982347", SN = 2, Activities = new List<Activite>()
            {
                
            } },
            new GroupeCours { Nom = "Physique 4", Numgroupe = 2, Campus = "Félix LeClerc", Code = "420-5A4-HU", Lien = "565221", SN = 2, Activities = new List<Activite>()
            {
                
            } },
            new GroupeCours { Nom = "Physique 5", Numgroupe = 1, Campus = "Gabrielle-Roy", Code = "420-5A4-HU", Lien = "156561", SN = 3, Activities = new List<Activite>()
            {
                
            } },
            new GroupeCours { Nom = "Physique 6", Numgroupe = 2, Campus = "Félix LeClerc", Code = "420-5A4-HU", Lien = "136895", SN = 3, Activities = new List<Activite>()
            {
                
            } }
        };

        public static List<Exercice> _exercices = new List<Exercice>()
        {
            // Activité 1
            new Exercice
            {
                Titre = "La Chute Libre",
                Enonce = "Un objet avec une masse de {m} est lâché d'une hauteur de {h}. Calcule le temps qu'il mettra pour toucher le sol. Utilise la formule de la chute libre. Où d est la distance, g est l'accélération due à la gravité (approximativement {g}), et t est le temps en secondes.",
                NumeroExercice = 1,
                DemarcheDisponible = false,
                Activite = NomActiviteDict["Les Lois de la Physique en Action"],
                AuteurUtilisateurId = UserDict["11442@cegepoutaouais.qc.ca"].Id,
                IsPublique = true
            },
            new Exercice
            {
                Titre = "Le Mouvement d’un Projectile",
                Enonce = "Un projectile est lancé avec une vitesse initiale de {v} à un angle de {θ}. g est de {g}. Calcule la portée du projectile.",
                NumeroExercice = 2,
                DemarcheDisponible = false,
                Activite = NomActiviteDict["Les Lois de la Physique en Action"],
                AuteurUtilisateurId = UserDict["12938@cegepoutaouais.qc.ca"].Id,
                IsPublique = true
            },
            new Exercice
            {
                Titre = "La Loi de Hooke",
                Enonce = "Un ressort est comprimé de {x} avec une force de {F}. Calcule la constante de raideur du ressort.",
                NumeroExercice = 3,
                DemarcheDisponible = false,
                Activite = NomActiviteDict["Les Lois de la Physique en Action"],
                AuteurUtilisateurId = UserDict["12922@cegepoutaouais.qc.ca"].Id,
                IsPublique = true
            },
            new Exercice
            {
                Titre = "Le Mouvement Circulaire",
                Enonce = "Un objet effectue un mouvement circulaire avec un rayon de {r} à une vitesse de {v}. Calcule l'accélération centripète.",
                NumeroExercice = 4,
                DemarcheDisponible = false,
                Activite = NomActiviteDict["Les Lois de la Physique en Action"],
                AuteurUtilisateurId = UserDict["11442@cegepoutaouais.qc.ca"].Id,
                IsPublique = true
            },
            // Activité 2
            new Exercice
            {
                Titre = "Conversion d'Énergie",
                Enonce = "Une machine convertit {J} d'énergie potentielle en énergie cinétique. Calcule la vitesse de l'objet.",
                NumeroExercice = 1,
                DemarcheDisponible = false,
                Activite = NomActiviteDict["Les Énergies et leurs Transformations"],
                AuteurUtilisateurId = UserDict["12938@cegepoutaouais.qc.ca"].Id,
                IsPublique = true
            },
            // Activité 3
            new Exercice
            {
                Titre = "Le Transfert Thermodynamique",
                Enonce = "Un objet est chauffé à {t} et transféré à un milieu à {t2}. Calcule la quantité de chaleur perdue si sa masse est de {m} kg et sa capacité thermique est de {c}.",
                NumeroExercice = 1,
                DemarcheDisponible = false,
                Activite = NomActiviteDict["Les Lois de la Thermodynamique"],
                AuteurUtilisateurId = UserDict["12938@cegepoutaouais.qc.ca"].Id,
                IsPublique = true
            },
            // Activité 4
            new Exercice {
                Titre="Le mouvement",
                Enonce = "Timi lance un objet A une vitesse de {v}. Cet objet a une masse de {m}. Répondez aux questions suivantes.",
                NumeroExercice = 1,
                DemarcheDisponible = false,
                Activite=NomActiviteDict["Les vecteurs"],
                AuteurUtilisateurId=UserDict["11442@cegepoutaouais.qc.ca"].Id,
                IsPublique=true
            },
        };

        private static Dictionary<string, Exercice> _TitreExerciceDict = new Dictionary<string, Exercice>();
        public static Dictionary<string, Exercice> TitreExerciceDict
        {
            get
            {
                _TitreExerciceDict = new Dictionary<string, Exercice>();
                foreach (Exercice exercice in _exercices)
                {
                    _TitreExerciceDict.Add(exercice.Titre, exercice);
                }

                return _TitreExerciceDict;
            }
        }

        public static List<QuestionChoix> _questionChoix = new List<QuestionChoix>()
        {
            // Exercice 1
            new QuestionChoix
            {
                Enonce = "Quelle est la valeur approximative de g?",
                Exercice = TitreExerciceDict["La Chute Libre"],
                Indices = new List<Indice> {
                    new Indice { IndiceText = "Utiliser un chiffre après la virgule.", Essaies = 2 },
                    new Indice { IndiceText = "Rappelez-vous que g est environ 9.8 m/s².", Essaies = 3 },
                    new Indice { IndiceText = "Le nombre est entre 9 et 10.", Essaies = 4 }
                }
            },
            // Exercice 2
            new QuestionChoix
            {
                Enonce = "Quel angle maximise la portée d'un projectile?",
                Exercice = TitreExerciceDict["Le Mouvement d’un Projectile"],
                Indices = new List<Indice>
                {
                    new Indice { IndiceText = "Pensez à l'équilibre entre la hauteur et la portée.", Essaies = 2 },
                    new Indice { IndiceText = "Cet angle est un multiple de 15.", Essaies = 3 },
                    new Indice { IndiceText = "C'est un angle symétrique dans un tir en parabole.", Essaies = 4 }
                }
            },
            // Exercice 3
            new QuestionChoix
            {
                Enonce = "Quelle est la formule de l'accélération centripète?",
                Exercice = TitreExerciceDict["Le Mouvement Circulaire"],
                Indices = new List<Indice>
                {
                    new Indice { IndiceText = "Elle dépend de la vitesse et du rayon.", Essaies = 2 },
                    new Indice { IndiceText = "La formule contient v² et r.", Essaies = 3 },
                    new Indice { IndiceText = "C'est a = v² / r.", Essaies = 4 }
                }
            },
            // Exercice 4
            new QuestionChoix
            {
                Enonce = "Quelle forme d'énergie est convertie lors de la chute libre?",
                Exercice = TitreExerciceDict["Conversion d'Énergie"],
                Indices = new List<Indice> {
                    new Indice { IndiceText = "L'énergie potentielle est impliquée.", Essaies = 2 },
                    new Indice { IndiceText = "Pensez à l'énergie cinétique à l'impact.", Essaies = 3 },
                    new Indice { IndiceText = "L'énergie potentielle se transforme en énergie cinétique.", Essaies = 5 }
                },

            },
            new QuestionChoix
            {
                Enonce = "Quel est le principe de conservation de l'énergie?",
                Exercice = TitreExerciceDict["Conversion d'Énergie"],
                Indices = new List<Indice>
                {
                    new Indice { IndiceText = "L'énergie totale reste constante.", Essaies = 2 },
                    new Indice { IndiceText = "Elle peut se transformer mais pas disparaître.", Essaies = 3 },
                    new Indice { IndiceText = "Pensez aux échanges entre différentes formes d'énergie.", Essaies = 4 }
                }
            },
            // Exercice 5
            new QuestionChoix {
                Enonce = "Comment additionne-t-on deux vecteurs?",
                ExerciceId = 1,
                Exercice=TitreExerciceDict["Le mouvement"],
                Indices = new List < Indice > {
                    new Indice { IndiceText = "Utilisez la règle du parallélogramme.", Essaies = 2 },
                    new Indice { IndiceText = "Additionnez les composants horizontaux et verticaux.", Essaies = 3 },
                    new Indice { IndiceText = "Le vecteur résultant dépend de la somme de chaque composant.", Essaies = 4 }
                }
            }
        };

        public static List<QuestionDeroulante> _questionsDeroulante = new List<QuestionDeroulante>()
        {
            // Exercice 1
            new QuestionDeroulante
            {
                Enonce = "Quel est le principe de la conservation de la quantité de mouvement?",
                Exercice = TitreExerciceDict["La Chute Libre"],
                ReponseTexte = "Le principe de la conservation de la quantité du movement {0}",
                Indices = new List<Indice>
                {
                    new Indice { IndiceText = "Pensez à la collision entre deux objets.", Essaies = 2 },
                    new Indice { IndiceText = "La quantité de mouvement est un vecteur.", Essaies = 2 }
                }
            },
            // Exercice 2
            new QuestionDeroulante
            {
                Enonce = "Comment appelle-t-on la force centripète?",
                Exercice = TitreExerciceDict["Le Mouvement Circulaire"],
                ReponseTexte = "La force centripète s'appel {0}",
                Indices = new List<Indice>
                {
                    new Indice { IndiceText = "Elle dirige un objet vers le centre de la trajectoire.", Essaies = 2 },
                    new Indice { IndiceText = "C'est la force qui maintient un objet en mouvement circulaire.", Essaies = 2 }
                }
            },
            // Exercice 3
            new QuestionDeroulante
            {
                Enonce = "Quelle est la première loi de la thermodynamique?",
                Exercice = TitreExerciceDict["Le Transfert Thermodynamique"],
                ReponseTexte = "La premiere loi de la thermodynamique est {0}",
                Indices = new List<Indice>
                {
                    new Indice { IndiceText = "Elle concerne la conservation de l'énergie.", Essaies = 2 },
                    new Indice { IndiceText = "Elle mentionne les échanges de chaleur.", Essaies = 2 }
                }
            },
            new QuestionDeroulante
            {
                Enonce = "Qu'est-ce que l'entropie?",
                Exercice = TitreExerciceDict["Le Transfert Thermodynamique"],
                ReponseTexte = "L'entropie est {0} et est {1}",
                Indices = new List<Indice>
                {
                    new Indice { IndiceText = "Elle est liée au désordre dans un système.", Essaies = 2 },
                    new Indice { IndiceText = "Elle augmente dans un système isolé.", Essaies = 2 }
                }
            },
            // Exercice 4
            new QuestionDeroulante
            {
                Enonce = "Qu'est-ce que le mouvement rectiligne uniforme?",
                ReponseTexte = "Le mouvement rectiligne uniforme est {0}",
                Exercice=TitreExerciceDict["Le mouvement"]
            },
        };

        public static List<QuestionNumerique> _questionsNumerique = new List<QuestionNumerique>()
        {
            // Exercice 1
            new QuestionNumerique
            {
                Enonce = "Calcule la vitesse finale v en m/s à l'impact, en utilisant la formule: v = g*t",
                Exercice = TitreExerciceDict["La Chute Libre"],
                Indices = new List<Indice>
                {
                    new Indice { IndiceText = "N'oublie pas de prendre en compte la valeur de g.", Essaies = 2 },
                    new Indice { IndiceText = "Utilise le temps que l'objet met à tomber.", Essaies = 2 }
                }
            },
            new QuestionNumerique
            {
                Enonce = "Calcule l'énergie potentielle initiale en joules de l'objet, avec la formule: Ep = mgh",
                Exercice = TitreExerciceDict["La Chute Libre"],
                Indices = new List<Indice>
                {
                    new Indice { IndiceText = "Assure-toi de connaître la masse et la hauteur.", Essaies = 2 },
                    new Indice { IndiceText = "L'énergie potentielle dépend de la gravité.", Essaies = 2 }
                }
            },
            // Exercice 2
            new QuestionNumerique
            {
                Enonce = "Calcule la portée du projectile avec la formule: R = (v² * sin(2θ)) / g",
                Exercice = TitreExerciceDict["Le Mouvement d’un Projectile"],
                Indices = new List<Indice>
                {
                    new Indice { IndiceText = "Rappelle-toi que sin(2θ) est important dans le calcul.", Essaies = 2 },
                    new Indice { IndiceText = "Vérifie les unités de v et g.", Essaies = 2 }
                }
            },
            // Exercice 3
            new QuestionNumerique
            {
                Enonce = "Calcule l'énergie emmagasinée dans le ressort: E = (1/2) * k * x²",
                Exercice = TitreExerciceDict["La Loi de Hooke"],
                Indices = new List<Indice>
                {
                    new Indice { IndiceText = "Assure-toi de connaître la constante k du ressort.", Essaies = 2 },
                    new Indice { IndiceText = "N'oublie pas que x est la compression ou l'étirement.", Essaies = 2 }
                }
            },
            // Exercice 4
            new QuestionNumerique
            {
                Enonce = "Calcule l'accélération centripète avec la formule: a_c = v² / r",
                Exercice = TitreExerciceDict["Le Mouvement Circulaire"],
                Indices = new List<Indice>
                {
                    new Indice { IndiceText = "Vérifie que la vitesse est au carré.", Essaies = 2 },
                    new Indice { IndiceText = "R rappelle que c'est le rayon de la trajectoire.", Essaies = 2 }
                }
            },
            // Exercice 5
            new QuestionNumerique
            {
                Enonce = "Calcule la vitesse d'un objet avec une énergie cinétique de 200 Joules et une masse de 5 kg.",
                Exercice = TitreExerciceDict["Conversion d'Énergie"],
                Indices = new List<Indice>
                {
                    new Indice { IndiceText = "Utilise la formule de l'énergie cinétique: Ec = (1/2) * m * v².", Essaies = 2 },
                    new Indice { IndiceText = "Résous pour v en isolant cette variable.", Essaies = 2 }
                }
            },
            // Exercice 6
            new QuestionNumerique
            {
                Enonce = "Calcule la quantité de chaleur perdue avec une masse de 2 kg et une capacité thermique de 500 J/kg°C.",
                Exercice = TitreExerciceDict["Le Transfert Thermodynamique"],
                Indices = new List<Indice>
                {
                    new Indice { IndiceText = "Rappelle-toi de la formule Q = mcΔT.", Essaies = 2 },
                    new Indice { IndiceText = "Vérifie la valeur de ΔT pour le calcul final.", Essaies = 2 }
                }
            },
            // Exercice 7
            new QuestionNumerique
            {
                Enonce = "Quel est la valeur de u dans l'énoncé?",
                ExerciceId = 1,
                Exercice=TitreExerciceDict["Le mouvement"]
            },
        };

        public static List<QuestionTroue> _questionsTroue = new List<QuestionTroue>()
        {
            // Exercice 1
            new QuestionTroue
            {
                Enonce = "Quel est le temps de chute d'un objet lâché depuis 10 mètres?",
                Exercice = TitreExerciceDict["La Chute Libre"],
                ReponseTexte = "Le temps de chute est de {0} secondes.",
                Indices = new List<Indice>
                {
                    new Indice { IndiceText = "Utilise la formule de chute libre pour le temps.", Essaies = 2 },
                    new Indice { IndiceText = "N'oublie pas d'appliquer g dans tes calculs.", Essaies = 2 }
                }
            },
            // Exercice 2
            new QuestionTroue
            {
                Enonce = "Quel est le rayon d'un cercle dont le diamètre est de 6 mètres?",
                Exercice = TitreExerciceDict["Le Mouvement Circulaire"],
                ReponseTexte = "Le rayon est de {0} mètres.",
                Indices = new List<Indice>
                {
                    new Indice { IndiceText = "Le rayon est la moitié du diamètre.", Essaies = 2 },
                    new Indice { IndiceText = "Assure-toi de bien diviser le diamètre par 2.", Essaies = 2 }
                }
            },
            // Exercice 3
            new QuestionTroue
            {
                Enonce = "La formule de l'énergie potentielle est Ep = mgh. Donnez la formule.",
                Exercice = TitreExerciceDict["La Chute Libre"],
                ReponseTexte = "Ep = {0} * g * h",
                Indices = new List<Indice>
                {
                    new Indice { IndiceText = "Identifie les variables dans la formule.", Essaies = 2 },
                    new Indice { IndiceText = "N'oublie pas que m, g et h sont essentiels.", Essaies = 2 }
                }
            },
            // Exercice 4
            new QuestionTroue
            {
                Enonce = "La formule de l'énergie cinétique est Ec = 1/2 mv². Donnez la formule.",
                Exercice = TitreExerciceDict["Conversion d'Énergie"],
                ReponseTexte = "Ec = {0} * m * v²",
                Indices = new List<Indice>
                {
                    new Indice { IndiceText = "Rappelle-toi que 1/2 est une constante dans la formule.", Essaies = 2 },
                    new Indice { IndiceText = "Assure-toi de bien comprendre les variables m et v.", Essaies = 2 }
                }
            },
            // Exercice 5
            new QuestionTroue
            {
                Enonce = "La première loi de la thermodynamique peut être exprimée comme ΔU = ... - ... Donnez la formule.",
                Exercice = TitreExerciceDict["Le Transfert Thermodynamique"],
                ReponseTexte = "ΔU = {0} - {1}",
                Indices = new List<Indice>
                {
                    new Indice { IndiceText = "ΔU représente le changement d'énergie interne.", Essaies = 2 },
                    new Indice { IndiceText = "Assure-toi de bien comprendre Q et W dans la formule.", Essaies = 2 }
                }
            },
            // Exercice 6
            new QuestionTroue
            {
                Enonce = "Donne moi la formule de vitesse.",
                ExerciceId = 1,
                Exercice=TitreExerciceDict["Le mouvement"],
                ReponseTexte="{0} = {1} / {2}"
            }
        };

        private static Dictionary<string, QuestionBase> _EnonceQuestionDict = new Dictionary<string, QuestionBase>();

        public static Dictionary<string, QuestionBase> EnonceQuestionDict
        {
            get
            {
                _EnonceQuestionDict = new Dictionary<string, QuestionBase>();
                foreach (QuestionBase question in _questionChoix)
                {
                    _EnonceQuestionDict.Add(question.Enonce, question);
                }

                foreach (QuestionBase question in _questionsDeroulante)
                {
                    _EnonceQuestionDict.Add(question.Enonce, question);
                }

                foreach (QuestionBase question in _questionsNumerique)
                {
                    _EnonceQuestionDict.Add(question.Enonce, question);
                }

                foreach (QuestionBase question in _questionsTroue)
                {
                    _EnonceQuestionDict.Add(question.Enonce, question);
                }

                return _EnonceQuestionDict;
            }
        }

        public static List<ReponseChoix> _reponsesChoix = new List<ReponseChoix>()
        {
            new ReponseChoix { Question = EnonceQuestionDict["Quelle est la valeur approximative de g?"], Valeur = "9.81 m/s²", IsAnswer = true },
            new ReponseChoix { Question = EnonceQuestionDict["Quelle est la valeur approximative de g?"], Valeur = "10 m/s²", IsAnswer = false },
            new ReponseChoix { Question = EnonceQuestionDict["Quelle est la valeur approximative de g?"], Valeur = "9.8 m/s²", IsAnswer = false },
            new ReponseChoix { Question = EnonceQuestionDict["Quelle est la valeur approximative de g?"], Valeur = "9 m/s²", IsAnswer = false },
            new ReponseChoix { Question = EnonceQuestionDict["Quelle est la valeur approximative de g?"], Valeur = "11 m/s²", IsAnswer = false },
            new ReponseChoix { Question = EnonceQuestionDict["Quel angle maximise la portée d'un projectile?"], Valeur = "45°", IsAnswer = true },
            new ReponseChoix { Question = EnonceQuestionDict["Quel angle maximise la portée d'un projectile?"], Valeur = "30°", IsAnswer = false },
            new ReponseChoix { Question = EnonceQuestionDict["Quel angle maximise la portée d'un projectile?"], Valeur = "60°", IsAnswer = false },
            new ReponseChoix { Question = EnonceQuestionDict["Quel angle maximise la portée d'un projectile?"], Valeur = "90°", IsAnswer = false },
            new ReponseChoix { Question = EnonceQuestionDict["Quelle est la formule de l'accélération centripète?"], Valeur = "a_c = v² / r", IsAnswer = true },
            new ReponseChoix { Question = EnonceQuestionDict["Quelle est la formule de l'accélération centripète?"], Valeur = "a_c = v² + r", IsAnswer = false },
            new ReponseChoix { Question = EnonceQuestionDict["Quelle est la formule de l'accélération centripète?"], Valeur = "a_c = v² - r", IsAnswer = false },
            new ReponseChoix { Question = EnonceQuestionDict["Quelle est la formule de l'accélération centripète?"], Valeur = "a_c = r / v²", IsAnswer = false },
            new ReponseChoix { Question = EnonceQuestionDict["Quelle est la formule de l'accélération centripète?"], Valeur = "a_c = v * r", IsAnswer = false },
            new ReponseChoix { Question = EnonceQuestionDict["Quelle forme d'énergie est convertie lors de la chute libre?"], Valeur = "Énergie cinétique", IsAnswer = false },
            new ReponseChoix { Question = EnonceQuestionDict["Quelle forme d'énergie est convertie lors de la chute libre?"], Valeur = "Énergie potentielle", IsAnswer = true },
            new ReponseChoix { Question = EnonceQuestionDict["Quelle forme d'énergie est convertie lors de la chute libre?"], Valeur = "Énergie nucléaire", IsAnswer = false },
            new ReponseChoix { Question = EnonceQuestionDict["Quelle forme d'énergie est convertie lors de la chute libre?"], Valeur = "Énergie solaire", IsAnswer = false },
            new ReponseChoix { Question = EnonceQuestionDict["Quel est le principe de conservation de l'énergie?"], Valeur = "L'énergie ne peut pas être créée ni détruite", IsAnswer = true },
            new ReponseChoix { Question = EnonceQuestionDict["Quel est le principe de conservation de l'énergie?"], Valeur = "L'énergie peut être créée", IsAnswer = false },
            new ReponseChoix { Question = EnonceQuestionDict["Quel est le principe de conservation de l'énergie?"], Valeur = "L'énergie est infinie", IsAnswer = false },
            new ReponseChoix { Question = EnonceQuestionDict["Comment additionne-t-on deux vecteurs?"], Valeur = "u + v = (u1 + v1, u2 + v2, u3 + v3)", IsAnswer = true },
            new ReponseChoix { Question = EnonceQuestionDict["Comment additionne-t-on deux vecteurs?"], Valeur = "u + v = (u1 - v1, u2 - v2, u3 - v3)", IsAnswer = false },
            new ReponseChoix { Question = EnonceQuestionDict["Comment additionne-t-on deux vecteurs?"], Valeur = "u + v = (u1 x v1, u2 x v2, u3 x v3)", IsAnswer = false },
            new ReponseChoix { Question = EnonceQuestionDict["Comment additionne-t-on deux vecteurs?"], Valeur = "u + v = (u1 / v1, u2 / v2, u3 / v3)", IsAnswer = false },
        };

        public static List<ReponseDeroulante> _reponsesDeroulant = new List<ReponseDeroulante>()
        {
            new ReponseDeroulante { Question = EnonceQuestionDict["Comment appelle-t-on la force centripète?"], Valeur = "Force vers le centre", IsAnswer = true, PositionTexte=0 },
            new ReponseDeroulante { Question = EnonceQuestionDict["Comment appelle-t-on la force centripète?"], Valeur = "Force attractive", IsAnswer = false, PositionTexte=0 },
            new ReponseDeroulante { Question = EnonceQuestionDict["Comment appelle-t-on la force centripète?"], Valeur = "Force intergalactique", IsAnswer = false, PositionTexte=0 },
            new ReponseDeroulante { Question = EnonceQuestionDict["Quelle est la première loi de la thermodynamique?"], Valeur = "L'énergie totale d'un système isolé reste constante", IsAnswer = true, PositionTexte=0 },
            new ReponseDeroulante { Question = EnonceQuestionDict["Quelle est la première loi de la thermodynamique?"], Valeur = "L'énergie peut être créée", IsAnswer = false, PositionTexte=0 },
            new ReponseDeroulante { Question = EnonceQuestionDict["Quelle est la première loi de la thermodynamique?"], Valeur = "L'énergie disparaît", IsAnswer = false, PositionTexte=0 },
            new ReponseDeroulante { Question = EnonceQuestionDict["Qu'est-ce que l'entropie?"], Valeur = "Une mesure de l'énergie dans un système", IsAnswer = false, PositionTexte=0},
            new ReponseDeroulante { Question = EnonceQuestionDict["Qu'est-ce que l'entropie?"], Valeur = "Une mesure du désordre dans un système", IsAnswer = true, PositionTexte=0 },
            new ReponseDeroulante { Question = EnonceQuestionDict["Qu'est-ce que l'entropie?"], Valeur = "Une mesure de distance", IsAnswer = false, PositionTexte=0 },
            new ReponseDeroulante { Question = EnonceQuestionDict["Qu'est-ce que l'entropie?"], Valeur = "Calculable", IsAnswer = true, PositionTexte=1 },
            new ReponseDeroulante { Question = EnonceQuestionDict["Qu'est-ce que l'entropie?"], Valeur = "Pas calculable", IsAnswer = false, PositionTexte=1 },
            new ReponseDeroulante { Question = EnonceQuestionDict["Quel est le principe de la conservation de la quantité de mouvement?"], Valeur = "L'addition vectorielle", IsAnswer = true, PositionTexte=0 },
            new ReponseDeroulante { Question = EnonceQuestionDict["Quel est le principe de la conservation de la quantité de mouvement?"], Valeur = "Aller dans l'espace", IsAnswer = false, PositionTexte=0 },
            new ReponseDeroulante { Question = EnonceQuestionDict["Quel est le principe de la conservation de la quantité de mouvement?"], Valeur = "Observer les nuages", IsAnswer = false, PositionTexte=0 },
            new ReponseDeroulante { Question = EnonceQuestionDict["Qu'est-ce que le mouvement rectiligne uniforme?"], Valeur = "Mouvement avec une vitesse constante", IsAnswer = true, PositionTexte=0 },
            new ReponseDeroulante { Question = EnonceQuestionDict["Qu'est-ce que le mouvement rectiligne uniforme?"], Valeur = "Mouvement en zigzag", IsAnswer = false, PositionTexte=0 },
            new ReponseDeroulante { Question = EnonceQuestionDict["Qu'est-ce que le mouvement rectiligne uniforme?"], Valeur = "Mouvement circulaire", IsAnswer = false, PositionTexte=0 },
            new ReponseDeroulante { Question = EnonceQuestionDict["Qu'est-ce que le mouvement rectiligne uniforme?"], Valeur = "Cela n'existe pas", IsAnswer = false, PositionTexte=0 },
        };

        public static List<ReponseNumerique> _reponsesNumerique = new List<ReponseNumerique>()
        {
            new ReponseNumerique { Question = EnonceQuestionDict["Calcule la vitesse finale v en m/s à l'impact, en utilisant la formule: v = g*t"], Valeur = "\\sqrt{\\frac{h^2}{g}}", IsCalculated = true, ChiffreApresVirgule = 1, MargeErreur = 10 },
            new ReponseNumerique { Question = EnonceQuestionDict["Calcule l'énergie potentielle initiale en joules de l'objet, avec la formule: Ep = mgh"], Valeur = "456", IsCalculated = false },
            new ReponseNumerique { Question = EnonceQuestionDict["Calcule la portée du projectile avec la formule: R = (v² * sin(2θ)) / g"], Valeur = "789", IsCalculated = false },
            new ReponseNumerique { Question = EnonceQuestionDict["Calcule l'énergie emmagasinée dans le ressort: E = (1/2) * k * x²"], Valeur = "321", IsCalculated = false },
            new ReponseNumerique { Question = EnonceQuestionDict["Calcule l'accélération centripète avec la formule: a_c = v² / r"], Valeur = "654", IsCalculated = false },
            new ReponseNumerique { Question = EnonceQuestionDict["Calcule la vitesse d'un objet avec une énergie cinétique de 200 Joules et une masse de 5 kg."], Valeur = "10", IsCalculated = false },
            new ReponseNumerique { Question = EnonceQuestionDict["Calcule la quantité de chaleur perdue avec une masse de 2 kg et une capacité thermique de 500 J/kg°C."], Valeur = "1000", IsCalculated = false },
            new ReponseNumerique { Question = EnonceQuestionDict["Quel est la valeur de u dans l'énoncé?"], Valeur = "1", IsCalculated=false },
        };

        public static List<ReponseTroue> _reponsesTroue = new List<ReponseTroue>()
        {
            new ReponseTroue { Question = EnonceQuestionDict["Quel est le temps de chute d'un objet lâché depuis 10 mètres?"], Valeur = "4.5", PositionTexte = 0 },
            new ReponseTroue { Question = EnonceQuestionDict["Quel est le rayon d'un cercle dont le diamètre est de 6 mètres?"], Valeur = "3", PositionTexte = 0 },
            new ReponseTroue { Question = EnonceQuestionDict["La formule de l'énergie potentielle est Ep = mgh. Donnez la formule."], Valeur = "m", PositionTexte = 0 },
            new ReponseTroue { Question = EnonceQuestionDict["La formule de l'énergie cinétique est Ec = 1/2 mv². Donnez la formule."], Valeur = "1/2", PositionTexte = 0 },
            new ReponseTroue { Question = EnonceQuestionDict["La première loi de la thermodynamique peut être exprimée comme ΔU = ... - ... Donnez la formule."], Valeur = "Q", PositionTexte = 0 },
            new ReponseTroue { Question = EnonceQuestionDict["La première loi de la thermodynamique peut être exprimée comme ΔU = ... - ... Donnez la formule."], Valeur = "W", PositionTexte = 1 },
            new ReponseTroue { Question = EnonceQuestionDict["Donne moi la formule de vitesse."], Valeur = "Vitesse", PositionTexte = 0 },
            new ReponseTroue { Question = EnonceQuestionDict["Donne moi la formule de vitesse."], Valeur = "Distance", PositionTexte = 1 },
            new ReponseTroue { Question = EnonceQuestionDict["Donne moi la formule de vitesse."], Valeur = "Temps", PositionTexte = 2 },
        };

        public static List<Variable> _variables = new List<Variable>()
        {
            new Variable { Exercice = TitreExerciceDict["La Chute Libre"], Nom = "h", Max = 50f, Min = 10f, Increment = 0.1f, Unite = "m"},
            new Variable { Exercice = TitreExerciceDict["La Chute Libre"], Nom = "g", Max = 9.8f, Min = 9.8f, Increment = 0.1f, Unite = "m/s²" },
            new Variable { Exercice = TitreExerciceDict["La Chute Libre"], Nom = "m", Max = 1f, Min = 20f, Increment = 1f, Unite = "kg" },
            new Variable { Exercice = TitreExerciceDict["Le Mouvement d’un Projectile"], Nom = "v", Max = 50f, Min = 20f, Increment = 0.1f, Unite = "m/s" },
            new Variable { Exercice = TitreExerciceDict["Le Mouvement d’un Projectile"], Nom = "θ", Max = 60f, Min = 30f, Increment = 0.1f, Unite = "°" },
            new Variable { Exercice = TitreExerciceDict["Le Mouvement d’un Projectile"], Nom = "g", Max = 3.00f, Min = 9.81f, Increment = 0.01f, Unite = "m/s" },
            new Variable { Exercice = TitreExerciceDict["La Loi de Hooke"], Nom = "F", Max = 100f, Min = 10f, Increment = 0.1f, Unite = "N" },
            new Variable { Exercice = TitreExerciceDict["La Loi de Hooke"], Nom = "x", Max = 0.5f, Min = 0.1f, Increment = 0.01f, Unite = "m" },
            new Variable { Exercice = TitreExerciceDict["Le Mouvement Circulaire"], Nom = "r", Max = 10f, Min = 1f, Increment = 0.1f, Unite = "m" },
            new Variable { Exercice = TitreExerciceDict["Le Mouvement Circulaire"], Nom = "v", Max = 8f, Min = 15f, Increment = 0.1f, Unite = "m/s" },
            new Variable { Exercice = TitreExerciceDict["Conversion d'Énergie"], Nom = "J", Max = 200f, Min = 200f, Increment = 0.1f, Unite = "J" },
            new Variable { Exercice = TitreExerciceDict["Le Transfert Thermodynamique"], Nom = "t", Max = 100f, Min = 100f, Increment = 0.1f, Unite = "°C" },
            new Variable { Exercice = TitreExerciceDict["Le Transfert Thermodynamique"], Nom = "t2", Max = 20f, Min = 20f, Increment = 0.1f, Unite = "°C" },
            new Variable { Exercice = TitreExerciceDict["Le Transfert Thermodynamique"], Nom = "m", Max = 10f, Min = 100f, Increment = 0.1f, Unite = "kg" },
            new Variable { Exercice = TitreExerciceDict["Le Transfert Thermodynamique"], Nom = "c", Max = 4200f, Min = 5000f, Increment = 0.1f, Unite = "J/kg°C" },
            new Variable { Exercice = TitreExerciceDict["Le mouvement"], Nom = "v", Max=20f, Min=0.1f, Increment = 0.1f, Unite = "m/s" },
            new Variable { Exercice = TitreExerciceDict["Le mouvement"], Nom = "m", Max=100f, Min=1f, Increment = 0.1f, Unite = "kg" },
        };

        public static async void Seed(IApplicationBuilder applicationBuilder)
        {
            UserManager<IdentityUser> userManager = applicationBuilder.ApplicationServices.CreateScope()
               .ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            RoleManager<IdentityRole> roleManager = applicationBuilder.ApplicationServices.CreateScope()
                .ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            using (var scope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var myDbContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();

                if (!myDbContext.Users.Any())
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                    await roleManager.CreateAsync(new IdentityRole("Etudiant"));
                    await roleManager.CreateAsync(new IdentityRole("Enseignant"));

                    foreach (var us in users)
                    {
                        us.EmailConfirmed = true;
                        await userManager.CreateAsync(us, "Password1!"); // Mot de passe par defaut des utilisateurs

                        //Assigne le rôle de l'utilisateur
                        if (us.UserName.Length == 7)
                        {
                            await userManager.AddToRoleAsync(us, "Etudiant");
                        }
                        else if (us.UserName.Length == 5)
                        {
                            await userManager.AddToRoleAsync(us, "Enseignant");
                        }
                    }

                    //Assigne le rôle admin a 1 enseignant
                    IdentityUser user = await userManager.FindByEmailAsync("82748@CEGEPOUTAOUAIS.QC.CA");
                    await userManager.AddToRoleAsync(user, "Admin");
                }


                // Seed Étudiants
                if (!myDbContext.Etudiants.Any())
                {
                    foreach (var etudiant in _etudiants)
                    {
                        myDbContext.Attach(etudiant.utilisateur); // Attacher l'utilisateur existant pour empecher la récréation de l'utilisateur
                        myDbContext.Etudiants.Add(etudiant);
                    }
                    myDbContext.SaveChanges();
                }

                // Seed Enseignants
                if (!myDbContext.Enseignants.Any())
                {
                    foreach (var enseignant in _enseignants)
                    {
                        myDbContext.Attach(enseignant.utilisateur); // Attacher l'utilisateur existant pour empecher la récréation de l'utilisateur
                        myDbContext.Enseignants.Add(enseignant);
                    }
                    myDbContext.SaveChanges();
                }



                // Seed GroupeCours et ses relation avec enseignant et etudiant
                if (!myDbContext.LsGroupeCours.Any())
                {
                    for (int i = 0; i < _groupeCours.Count; i++)
                    {
                        // Assigne un enseignant
                        _groupeCours[i].Enseignant = _enseignants[i % _enseignants.Count];

                        // Assigne un etudiant au cour
                        // Nombre d'étudiant a ajouter
                        int etudiantCount = 5;
                        _groupeCours[i].Etudiants = new List<Etudiant>();

                        for (int j = 0; j < etudiantCount; j++)
                        {
                            int etudiantIndex = (i * etudiantCount + j) % _etudiants.Count;
                            _groupeCours[i].Etudiants?.Add(_etudiants[etudiantIndex]);
                        }
                    }
                    myDbContext.LsGroupeCours.AddRange(_groupeCours);
                    myDbContext.SaveChanges();
                }

                // Seed Activites
                if (!myDbContext.Activities.Any())
                {
                    // Assigne un Groupe Cour
                    // TODO améliorer ceci
                    for (int i = 0; i < _activites.Count; i++)
                    {
                        _activites[i].GroupeCours = _groupeCours[i % _groupeCours.Count];
                    }

                    myDbContext.Activities.AddRange(NomActiviteDict.Values);
                    myDbContext.SaveChanges();
                }

                // Seed Notifications
                if (!myDbContext.Notifications.Any())
                {
                    // Assigne une activite
                    for (int i = 0; i < _notifications.Count; i++)
                    {
                        _notifications[i].Activite = _activites[i % _activites.Count];
                    }
                    myDbContext.Notifications.AddRange(_notifications);
                    myDbContext.SaveChanges();
                }

                // Seed RappelActivites
                if (!myDbContext.RappelsActivite.Any())
                {
                    // Assigne une activite et un utilisateur
                    for (int i = 0; i < _rappelActivites.Count; i++)
                    {
                        _rappelActivites[i].Activite = _activites[i % _activites.Count];
                        _rappelActivites[i].Utilisateur = _etudiants[i % _etudiants.Count];
                    }
                    myDbContext.RappelsActivite.AddRange(_rappelActivites);
                    myDbContext.SaveChanges();
                }

                if (!myDbContext.Exercices.Any())
                {
                    myDbContext.Exercices.AddRange(TitreExerciceDict.Values);
                    myDbContext.SaveChanges();
                }

                if (!myDbContext.Questions.Any())
                {
                    myDbContext.Questions.AddRange(EnonceQuestionDict.Values);
                    myDbContext.SaveChanges();
                }

                if (!myDbContext.Reponses.Any())
                {
                    myDbContext.Reponses.AddRange(_reponsesChoix);
                    myDbContext.Reponses.AddRange(_reponsesDeroulant);
                    myDbContext.Reponses.AddRange(_reponsesNumerique);
                    myDbContext.Reponses.AddRange(_reponsesTroue);
                    myDbContext.SaveChanges();
                }

                if (!myDbContext.Variables.Any())
                {
                    myDbContext.Variables.AddRange(_variables);
                    myDbContext.SaveChanges();
                }
            }
        }
    }
}
