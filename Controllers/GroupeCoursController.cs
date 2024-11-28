using System.Runtime.InteropServices.Marshalling;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using Microsoft.VisualBasic;
using ProjetSynthese.Server.Models;

namespace ProjetSynthese.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupeCoursController : Controller
    {
        private readonly MyDbContext _context;

        public GroupeCoursController(MyDbContext context)
        {
            _context = context;
        }

        /**
         * Permet de récupérer une liste de tous les groupes de cours.
         * 
         * Cette méthode retourne une liste de groupes de cours, incluant leurs étudiants associés
         * et leurs activités respectives. Les données sont récupérées à partir de la base de données,
         * 
         * @return Une réponse HTTP 200 contenant la liste des groupes de cours avec leurs étudiants et activités.
         */
        [HttpGet]
        public async Task<IActionResult> GetAllGroupeCoursAsync()
        {
            try
            {
                // Récupération des groupes de cours avec les étudiants et activités associées
                var groupeCoursList = await _context.LsGroupeCours
                    .Include(gc => gc.Etudiants) // Inclusion des étudiants associés
                    .Include(gc => gc.Activities) // Inclusion des activités associées
                    .ThenInclude(a => a.Exercices) // Inclure les exercices dans les activités
                    .ThenInclude(e => e.Questions) // Inclure les questions dans les exercices
                    .ToListAsync();

                if (!groupeCoursList.Any())
                {
                    return NotFound("Aucun groupe de cours trouvé.");
                }

                // Retourne la liste brute des groupes de cours en réponse HTTP 200
                return Ok(groupeCoursList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Une erreur est survenue lors de l'accès aux groupes de cours. Détails: " + ex.Message);
            }
        }

        /**
         * Permet de récupérer un groupe de cours spécifique par son identifiant.
         * 
         * Cette méthode recherche un groupe de cours dans la base de données,
         * incluant les étudiants et activités associés. Si le groupe de cours n'est pas trouvé,
         * elle retourne une réponse 404 NotFound. 
         * 
         * @param id L'identifiant du groupe de cours à récupérer.
         * @return Une réponse HTTP 200 contenant le groupe de cours demandé
         */
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGroupeCoursAsync(int id)
        {
            try
            {
                // Récupération du groupe de cours par ID avec les étudiants et activités associées
                var groupeCours = await _context.LsGroupeCours
                    .Include(gc => gc.Etudiants) // Inclusion des étudiants associés
                    .Include(gc => gc.Activities) // Inclusion des activités associées
                    .ThenInclude(a => a.Exercices) // Inclure les exercices dans les activités
                    .ThenInclude(e => e.Questions) // Inclure les questions dans les exercices
                    .FirstOrDefaultAsync(gc => gc.Id == id); // Récupérer le groupe de cours par ID

                // Vérification si le groupe de cours a été trouvé
                if (groupeCours == null)
                {
                    return NotFound("Groupe de cours non trouvé.");
                }

                // Retourne le groupe de cours brut en réponse HTTP 200
                return Ok(groupeCours);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Une erreur est survenue lors de l'accès au groupe de cours. Détails: " + ex.Message);
            }
        }

        /** Permet d'ajouter un élève au groupe de cours */
        [HttpPut("rejoindre/{code}")]
        public async Task<IActionResult> AddEtudiantAsync(string code, int id_etudiant)
        {
            try
            {
                var groupeCours = await _context.LsGroupeCours.FirstOrDefaultAsync(g => g.Lien == code && g.Archiver == false);
                var etudiant = await _context.Etudiants.FirstOrDefaultAsync(e => e.Id == id_etudiant);

                //ceci c'est si le groupe n'existe pas
                if (groupeCours == null)
                {
                    return NotFound();
                }

                //ceci c'est si l'etudiant est deja dans le groupe
                if (groupeCours.Etudiants.Any(e => e.Id == id_etudiant))
                {
                    //envoyer le bon message si l'etudiant est bloquer ou pas
                    if (!groupeCours.EtudiantIdBloquer.Contains(id_etudiant))
                    {
                        return Conflict("L'étudiant est déjà dans le groupe.");
                    }
                    else {
                        return BadRequest("L'etudiant a ete bloquer par l'enseignant. IL ne peut pas rejoindre le groupe.");
                    }
                }

                groupeCours.Etudiants.Add(etudiant);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Une erreur est survenue lors de l'ajout du groupe cour.");
            }

        }

        /** Permet d'enlever un groupe de cours */
        [HttpDelete("{id}")]
        public async Task<IActionResult> SupprimerGroupe(int id)
        { //TODO: verifier que c'est un eneignant qui fait une suppression ou un admin
            try
            {
                var groupeCours = await _context.LsGroupeCours.FindAsync(id);
                if (groupeCours == null)
                {
                    return NotFound();
                }

                _context.LsGroupeCours.Remove(groupeCours);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Une erreur est survenue lors de la suppression du groupe cour.");
            }
        }

        /** Permet de récupérer la liste des groupes d'un élève */
        [HttpGet("etudiant/{id}")]
        public async Task<IActionResult> GetGroupesDeEtudiantAsync(int id)
        {
            try
            {
                var etudiant = await _context.Etudiants.Include(e => e.Cours).FirstOrDefaultAsync(e => e.Id == id);

                if (etudiant != null)
                {
                    var groupes = await _context.LsGroupeCours
                        .Where(groupe => groupe.Etudiants.Any(e => e.Id == id)) //trouver les groupes
                        .Where(groupe => groupe.EtudiantIdBloquer.Contains(etudiant.Id) == false) //l'etudiant n'est pas bloquer
                        .Include(groupe => groupe.Activities) // Inclure les activités pour chaque groupe
                        .ThenInclude(activite => activite.Exercices) // Inclure les exercices dans les activités
                        .ThenInclude(exercice => exercice.Questions) // Inclure les questions dans les exercices
                        .ToListAsync();

                    return Ok(groupes);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Une erreur est survenue lors de l'accèss au groupe cours de l'étudiant.");
            }
        }

        /** Genere un nouveau code en verifiant que le code est valide */
        [HttpGet("newCode")]
        public async Task<IActionResult> GenererCodeInvitation() {
            //chercher le code generer
            string code = CreationCode();

            //Le code est enfin pret
            return Ok(code);
        }

        /** Ajoute un groupe dans la base de donnee */
        [HttpPost]
        public async Task<IActionResult> CreerGroupe(GroupeCours groupeCours) {
            //validation
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            
            try
            {
                groupeCours.Id = 0; //On ne dois pas gerer les ID, c'est la base de donnee qui sens occupe

                _context.LsGroupeCours.Add(groupeCours);
                await _context.SaveChangesAsync();

                //recuperer l'id du dernier groupe creer
                var lastGroup = _context.LsGroupeCours.OrderByDescending(gc => gc.Id).First();

                return Ok(lastGroup);
            }
            catch (Exception ex) {
                return StatusCode(500, "Une erreur est survenue lors de l'ajout du groupe dans la base de donnée: " + ex);
            }
        }

        /** Modifie un groupe dans la base de donnee */
        [HttpPut("{id}")]
        public async Task<IActionResult> ModifierGroupe(int id, GroupeCours groupeCours) {
            //validation
            if (!ModelState.IsValid) {
                BadRequest(ModelState);
            }
            
            try
            {
                //s'assurer qu'on modifie le bon groupe
                if (id != groupeCours.Id) { return StatusCode(500, "Une erreur dans les donnees à été détecté."); } //Comme je ne peut pas savoir qu'elle id est le bon, je ne vais pas prendre de chance. 
                                                                                                                    //Je ne fait pas confiance au client.
                                                                                                                    //retrouver le groupe dans la base de donnee et le modifier
                var groupeToUpdate = _context.LsGroupeCours.FirstOrDefault(gc => gc.Id == id);
                groupeToUpdate.Nom = groupeCours.Nom;
                groupeToUpdate.Lien = groupeCours.Lien;
                groupeToUpdate.SN = groupeCours.SN;
                groupeToUpdate.Campus = groupeCours.Campus;
                groupeToUpdate.Numgroupe = groupeCours.Numgroupe;
                await _context.SaveChangesAsync();

                //return
                return Ok(groupeToUpdate);
            }
            catch (Exception ex) {
                return StatusCode(500, "Une erreur est survenue lors de la modification du groupe dans la base de donnée: " + ex);
            }
        }

        /** Archive ou desarchive un groupe */
        [HttpPatch("archivage/{id}")]
        public async Task<IActionResult> ArchivageGroupe(int id, AnyType anyType) {
            try
            {
                //trouver le groupe
                var groupeCours = await _context.LsGroupeCours.FirstOrDefaultAsync(gc => gc.Id == id);

                //aucun groupe trouver
                if (groupeCours == null) {
                    return BadRequest();
                }

                //si le groupe est pas archiver, il faut l'archiver
                if (!groupeCours.Archiver)
                {
                    groupeCours.Archiver = true;

                    //final
                    await _context.SaveChangesAsync();
                    return Ok();
                }

                //si le groupe est archiver, il faut le sortir de sont archivage
                else
                {
                    //pour eviter les problemes on dois refaire un lien 
                    string code = CreationCode();

                    //update du groupe
                    groupeCours.Lien = code;
                    groupeCours.Archiver = false;

                    //final
                    await _context.SaveChangesAsync();
                    return Ok(groupeCours);
                }
            }
            catch {
                return StatusCode(500, "Une erreur est survenue lors de l'archivage ou le desarchivage du groupe");
            }
        }

        [HttpPatch("bloquage/{id_groupe}/{id_etudiant}")]
        public async Task<IActionResult> BlocageEtudiant(int id_groupe, int id_etudiant) {
            try {
                //trouver les elements
                var groupeCours = await _context.LsGroupeCours.FirstOrDefaultAsync(gc => gc.Id == id_groupe);
                var etudiant = await _context.Etudiants.FirstOrDefaultAsync(e => e.Id == id_etudiant);

                //verifier que tout est correction
                if (groupeCours == null) { return BadRequest("Le groupe est inexitant"); }
                if (etudiant == null) { return BadRequest("L'etudiant est inexistant"); }

                //tout est bon donc ajouter ou retirer l'etudiant de la liste des etudiants bloquer
                List<int>? listId = groupeCours.EtudiantIdBloquer;

                //si la liste est vide on s'embete pas je creer la liste avec l'etudiant
                if (listId == null) {
                    groupeCours.EtudiantIdBloquer = new List<int> { id_etudiant };
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                //l'etudiant doit etre bloquer
                if (!listId.Contains(id_etudiant)) {
                    groupeCours.EtudiantIdBloquer.Add(id_etudiant);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                //l'etudiant doit etre debloquer
                else {
                    groupeCours.EtudiantIdBloquer.Remove(id_etudiant);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
            }
            catch
            {
                return StatusCode(500, "Une erreur est survenue lors de l'archivage ou le desarchivage du groupe");
            }
        }

        //METHODE PRIVER
        /** generation d'un code */
        private string CreationCode() {
            //generer une premiere fois un code
            Random random = new Random();
            string code = string.Empty;
            for (int i = 0; i < 6; i++)
            {
                code += random.Next(0, 10).ToString();
            }

            //boucle de verification avec la database
            var listGroupeCours = _context.LsGroupeCours.ToList();
            var CodeIdentique = false;
            while (true)
            {
                if (listGroupeCours != null)
                {
                    //parcourir la liste des groupes
                    foreach (GroupeCours groupe in listGroupeCours)
                    {
                        //faire la comparaison
                        if (groupe.Lien == code && groupe.Archiver == false) { CodeIdentique = true; }
                    }

                    //regenerer un nouveau code si necessaire
                    if (CodeIdentique)
                    {
                        CodeIdentique = false;
                        code = string.Empty;
                        for (int i = 0; i < 6; i++)
                        {
                            code += random.Next(0, 10).ToString();
                        }
                    }
                    else
                    { //le code est bon donc on arrete la boucle
                        break;
                    }
                }
                else { break; }
                
            }

            //return
            return code;
        }
    }
}
