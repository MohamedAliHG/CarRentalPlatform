using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgenceLocationVoiture.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Adresse",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CodePostal",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateInscription",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateNaissance",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateObtentionPermis",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EstActif",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EstVerifiee",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "HeureFermeture",
                table: "AspNetUsers",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "HeureOuverture",
                table: "AspNetUsers",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogoUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nom",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NomAgence",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumeroPermis",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumeroSiret",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PermisVerifie",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoPermisUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Prenom",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SiteWeb",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TypeUtilisateur",
                table: "AspNetUsers",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Ville",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Avis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Note = table.Column<int>(type: "int", nullable: false),
                    Commentaire = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateAvis = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstVerifie = table.Column<bool>(type: "bit", nullable: false),
                    EstVisible = table.Column<bool>(type: "bit", nullable: false),
                    NoteQualiteVehicule = table.Column<int>(type: "int", nullable: true),
                    NoteServiceClient = table.Column<int>(type: "int", nullable: true),
                    NoteRapportQualitePrix = table.Column<int>(type: "int", nullable: true),
                    NoteProprete = table.Column<int>(type: "int", nullable: true),
                    ReponseAgence = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateReponse = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AgenceId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Avis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Avis_AspNetUsers_AgenceId",
                        column: x => x.AgenceId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Avis_AspNetUsers_ClientId",
                        column: x => x.ClientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FichesTechniques",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MoteurType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Puissance = table.Column<int>(type: "int", nullable: false),
                    Consommation = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    EmissionCO2 = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false),
                    CapaciteReservoir = table.Column<int>(type: "int", nullable: false),
                    VolumeCodeffre = table.Column<int>(type: "int", nullable: false),
                    Longueur = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Largeur = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Hauteur = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PoidsVide = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Equipements = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OptionsSecurite = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FichesTechniques", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Voitures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Marque = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Modele = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Annee = table.Column<int>(type: "int", nullable: false),
                    Immatriculation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Categorie = table.Column<int>(type: "int", nullable: false),
                    TypeCarburant = table.Column<int>(type: "int", nullable: false),
                    Transmission = table.Column<int>(type: "int", nullable: false),
                    NombrePlaces = table.Column<int>(type: "int", nullable: false),
                    NombrePortes = table.Column<int>(type: "int", nullable: false),
                    Kilometrage = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Couleur = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstDisponible = table.Column<bool>(type: "bit", nullable: false),
                    PhotoPrincipaleUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhotosUrls = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AgenceId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FicheTechniqueId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voitures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Voitures_AspNetUsers_AgenceId",
                        column: x => x.AgenceId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Voitures_FichesTechniques_FicheTechniqueId",
                        column: x => x.FicheTechniqueId,
                        principalTable: "FichesTechniques",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OffresLocation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrixParJour = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    PrixParSemaine = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    CautionMontant = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    KilometrageInclus = table.Column<int>(type: "int", nullable: false),
                    FraisKmSupplementaire = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AgeMinimumConducteur = table.Column<int>(type: "int", nullable: false),
                    AnciennetePermisMinimum = table.Column<int>(type: "int", nullable: false),
                    AssuranceIncluse = table.Column<bool>(type: "bit", nullable: false),
                    LivraisonPossible = table.Column<bool>(type: "bit", nullable: false),
                    FraisLivraison = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DateDebut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstActive = table.Column<bool>(type: "bit", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VoitureId = table.Column<int>(type: "int", nullable: false),
                    AgenceId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OffresLocation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OffresLocation_AspNetUsers_AgenceId",
                        column: x => x.AgenceId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OffresLocation_Voitures_VoitureId",
                        column: x => x.VoitureId,
                        principalTable: "Voitures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DemandesLocation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateDebut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LieuPriseEnCharge = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LieuRetour = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LivraisonDemandee = table.Column<bool>(type: "bit", nullable: false),
                    AdresseLivraison = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Statut = table.Column<int>(type: "int", nullable: false),
                    DateDemande = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MessageClient = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReponseAgence = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateReponse = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MontantTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ClientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OffreLocId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemandesLocation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DemandesLocation_AspNetUsers_ClientId",
                        column: x => x.ClientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DemandesLocation_OffresLocation_OffreLocId",
                        column: x => x.OffreLocId,
                        principalTable: "OffresLocation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_NumeroSiret",
                table: "AspNetUsers",
                column: "NumeroSiret",
                unique: true,
                filter: "[NumeroSiret] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Avis_AgenceId",
                table: "Avis",
                column: "AgenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Avis_ClientId",
                table: "Avis",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_DemandesLocation_ClientId",
                table: "DemandesLocation",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_DemandesLocation_OffreLocId",
                table: "DemandesLocation",
                column: "OffreLocId");

            migrationBuilder.CreateIndex(
                name: "IX_DemandesLocation_Statut",
                table: "DemandesLocation",
                column: "Statut");

            migrationBuilder.CreateIndex(
                name: "IX_OffresLocation_AgenceId",
                table: "OffresLocation",
                column: "AgenceId");

            migrationBuilder.CreateIndex(
                name: "IX_OffresLocation_EstActive",
                table: "OffresLocation",
                column: "EstActive");

            migrationBuilder.CreateIndex(
                name: "IX_OffresLocation_VoitureId",
                table: "OffresLocation",
                column: "VoitureId");

            migrationBuilder.CreateIndex(
                name: "IX_Voitures_AgenceId",
                table: "Voitures",
                column: "AgenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Voitures_EstDisponible",
                table: "Voitures",
                column: "EstDisponible");

            migrationBuilder.CreateIndex(
                name: "IX_Voitures_FicheTechniqueId",
                table: "Voitures",
                column: "FicheTechniqueId",
                unique: true,
                filter: "[FicheTechniqueId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Voitures_Marque",
                table: "Voitures",
                column: "Marque");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Avis");

            migrationBuilder.DropTable(
                name: "DemandesLocation");

            migrationBuilder.DropTable(
                name: "OffresLocation");

            migrationBuilder.DropTable(
                name: "Voitures");

            migrationBuilder.DropTable(
                name: "FichesTechniques");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_NumeroSiret",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Adresse",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CodePostal",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DateInscription",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DateNaissance",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DateObtentionPermis",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EstActif",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EstVerifiee",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "HeureFermeture",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "HeureOuverture",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LogoUrl",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Nom",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NomAgence",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NumeroPermis",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NumeroSiret",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PermisVerifie",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PhotoPermisUrl",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Prenom",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SiteWeb",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TypeUtilisateur",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Ville",
                table: "AspNetUsers");
        }
    }
}
