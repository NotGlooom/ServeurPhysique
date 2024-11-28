using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjetSynthese.Server.Models;
using ProjetSynthese.Server.Models.Question;
using ProjetSynthese.Server.Models.Reponse;

namespace ProjetSynthese.Server
{
    public class MyDbContext : IdentityDbContext<IdentityUser>
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        public DbSet<Etudiant> Etudiants { get; set; }
        public DbSet<QuestionBase> Questions { get; set; }
        public DbSet<Exercice> Exercices { get; set; }
        public DbSet<Variable> Variables { get; set; }
        public DbSet<ReponseBase> Reponses { get; set; }
        public DbSet<Enseignant> Enseignants { get; set; }
        public virtual DbSet<Activite> Activities { get; set; }
        public DbSet<GroupeCours> LsGroupeCours { get; set; }
        public DbSet<RappelActivite> RappelsActivite { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<ExerciceInstance> ExercicesInstance { get; set; }
        public DbSet<ReponseUtilisateur> ReponsesUtilisateur { get; set; }
        public DbSet<VariableInstance> VariablesInstance { get; set; }
        public DbSet<Indice> Indices { get; set; }
        public DbSet<ExcludeRange> ExcludeRanges { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relation Activite -> Exercice
            modelBuilder.Entity<Activite>()
                .HasMany(e => e.Exercices)
                .WithOne(e => e.Activite)
                .HasForeignKey(e => e.ActiviteId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<Enseignant>()
                .HasMany(e => e.ListeActiviteAuteur)
                .WithOne(e => e.Auteur)
                .HasForeignKey(e => e.AuteurUtilisateurId)
                .HasPrincipalKey(e => e.UtilisateurId)  // Use UtilisateurId as the principal key
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Exercice>()
                .HasMany(e => e.Variables)
                .WithOne(e => e.Exercice)
                .HasForeignKey(e => e.ExerciceId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<QuestionBase>()
                .HasMany(q => q.Reponses)
                .WithOne(r => r.Question)
                .HasForeignKey(r => r.QuestionId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<ReponseBase>()
                .HasDiscriminator<string>("ReponseType")
                .HasValue<ReponseNumerique>("Numerique")
                .HasValue<ReponseChoix>("Choix")
                .HasValue<ReponseTroue>("Troue")
                .HasValue<ReponseDeroulante>("Deroulant");

            modelBuilder.Entity<QuestionBase>()
                .HasDiscriminator<string>("QuestionType")
                .HasValue<QuestionNumerique>("Numerique")
                .HasValue<QuestionChoix>("Choix")
                .HasValue<QuestionTroue>("Troue")
                .HasValue<QuestionDeroulante>("Deroulant");

            // Exercice et Questions
            modelBuilder.Entity<Exercice>()
                .HasMany(e => e.Questions)
                .WithOne(e => e.Exercice)
                .HasForeignKey(e => e.ExerciceId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            // Exercice et Enseignant/Auteur
            modelBuilder.Entity<Exercice>()
                .HasOne(e => e.Auteur)
                .WithMany()
                .HasForeignKey(e => e.AuteurUtilisateurId)
                .HasPrincipalKey(e => e.UtilisateurId)  // Use UtilisateurId as the principal key
                .OnDelete(DeleteBehavior.Restrict);

            // Joindre les activites du groupe cours
            modelBuilder.Entity<GroupeCours>()
                .HasMany(g => g.Activities)
                .WithOne(a => a.GroupeCours)
                .HasForeignKey(a => a.GroupeCoursId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Enseignant>()
                .HasMany(e => e.GroupeCours)
                .WithOne(e => e.Enseignant)
                .HasForeignKey(e => e.EnseignantId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<Enseignant>()
                .HasMany(e => e.ListeActiviteAuteur)
                .WithOne(e => e.Auteur)
                .HasForeignKey(e => e.AuteurUtilisateurId);

            // Table de jointure avec DeleteBehavior.Restrict pour éviter les cascades multiples
            modelBuilder.Entity<GroupeCours>()
                .HasMany(e => e.Etudiants)
                .WithMany(e => e.Cours)
                .UsingEntity("GroupeCoursEtudiant",
                l => l.HasOne(typeof(Etudiant)).WithMany().HasForeignKey("EtudiantId").HasPrincipalKey(nameof(Etudiant.Id)).OnDelete(DeleteBehavior.Restrict),
                r => r.HasOne(typeof(GroupeCours)).WithMany().HasForeignKey("GroupeCoursId").HasPrincipalKey(nameof(GroupeCours.Id)).OnDelete(DeleteBehavior.Restrict),
                j => j.HasKey("GroupeCoursId", "EtudiantId"));

            // Rapel activite
            modelBuilder.Entity<RappelActivite>()
                .HasOne(r => r.Activite)
                .WithMany(e => e.RappelsActivite)
                .HasForeignKey(r => r.ActiviteId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RappelActivite>()
                .HasOne(r => r.Utilisateur)
                .WithMany(e => e.RappelsActivite)
                .HasForeignKey(r => r.UtilisateurId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Activite)
                .WithMany()
                .HasForeignKey(n => n.ActiviteId)
                .OnDelete(DeleteBehavior.SetNull);

            // Relation Exercice -> ExerciceInstance
            modelBuilder.Entity<ExerciceInstance>()
                .HasOne(e => e.Exercice)
                .WithMany(e => e.ExercicesInstance)
                .HasForeignKey(e => e.ExerciceId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ExerciceInstance>()
                .HasOne(e => e.Exercice)
                .WithMany(e => e.ExercicesInstance)
                .HasForeignKey(e => e.ExerciceId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ExerciceInstance>()
                .HasOne(r => r.Utilisateur)
                .WithMany()
                .HasForeignKey(r => r.UtilisateurId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<QuestionBase>()
                .HasMany(q => q.Indices)
                .WithOne(i => i.Question)
                .HasForeignKey(i => i.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Variable instance
            modelBuilder.Entity<VariableInstance>()
                .HasOne(r => r.Variable)
                .WithMany(e => e.VariablesInstance)
                .HasForeignKey(r => r.VariableId)
                .OnDelete(DeleteBehavior.Restrict);

            // Reponse utilisateur
            modelBuilder.Entity<ReponseUtilisateur>()
                .HasOne(r => r.ExerciceInstance)
                .WithMany(e => e.ReponsesUtilisateur)
                .HasForeignKey(r => r.ExerciceInstanceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ReponseUtilisateur>()
                .HasOne(r => r.Question)
                .WithMany(e => e.ReponsesUtilisateur)
                .HasForeignKey(r => r.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relation one-to-one entre Enseignant et IdentityUser
            modelBuilder.Entity<Enseignant>()
                .HasOne(e => e.utilisateur)
                .WithOne()
                .HasForeignKey<Enseignant>(e => e.UtilisateurId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relation one-to-one entre Etudiant et IdentityUser
            modelBuilder.Entity<Etudiant>()
                .HasOne(e => e.utilisateur)
                .WithOne()
                .HasForeignKey<Etudiant>(e => e.UtilisateurId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}