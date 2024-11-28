using System.Text.RegularExpressions;

public interface IImageExtractionService
{
    Task<string> ExtractImages(string htmlContent);
}

public class ImageExtractionService : IImageExtractionService
{
    private readonly IWebHostEnvironment _hostingEnvironment;

    public ImageExtractionService(IWebHostEnvironment hostingEnvironment)
    {
        _hostingEnvironment = hostingEnvironment;
    }

    /**
     * Stocke toutes les images sur le serveur
     */
    public async Task<string> ExtractImages(string htmlContent)
    {
        // Trouve toutes les images
        var imgTags = GetImgTags(htmlContent);

        // Itère à travers les images
        foreach (var imgTag in imgTags)
        {
            // Extrait la source
            string srcValue = ExtractSrcAttribute(imgTag);

            if (string.IsNullOrEmpty(srcValue))
                continue;

            // Traite les images
            var newImgTag = await ProcessImageSource(srcValue);

            // Contient le nouveau tag, et si l'image est déjà upload ne contient rien
            if (newImgTag != null)
            {
                // Remplace l'ancienne image avec le nouveau chemin
                htmlContent = htmlContent.Replace(imgTag, newImgTag);
            }
        }
        return htmlContent;
    }

    /**
     * Retourne la source de l'image
     */
    private string ExtractSrcAttribute(string imgTag)
    {
        var srcMatch = Regex.Match(imgTag, @"src=""([^""]+)""", RegexOptions.IgnoreCase);
        return srcMatch.Success ? srcMatch.Groups[1].Value : string.Empty;
    }

    /**
     * Gère si l'image doit être stocker
     */
    private async Task<string?> ProcessImageSource(string src)
    {
        try
        {
            // Vérifie si l'image contient des données
            if (src.StartsWith("data:image"))
            {
                // Sauvegarde l'image
                var imagePath = await SaveBase64Image(src);

                var imgTag = $"<img src={imagePath} class=\"imagespop\" alt=\"Cliquez pour voir l'image\" />";
                
                return imgTag;
            }

            // Ne contient pas de données
            // L'image est déjà sauvegardé
            return null;
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }

    /**
     * Sauvegarde une image en base64
     * @return son chemin
     */
    private async Task<string> SaveBase64Image(string base64String)
    {
        // Extrait les informations et vérifie que l'image est en base 64
        var match = Regex.Match(base64String, @"data:image/(?<type>.+?);base64,(?<data>.+)");

        if (!match.Success)
            throw new FormatException("Le format base64 de l'image est invalide.");

        string imageType = match.Groups["type"].Value;
        string base64Data = match.Groups["data"].Value;

        // Liste des types d'images autorisés
        string[] allowedTypes = { "jpeg", "jpg", "png", "bmp", "webp", "svg" };
        if (!allowedTypes.Contains(imageType))
            throw new ArgumentException($"Type d'image non autorisé : {imageType}. Types autorisés : {string.Join(", ", allowedTypes)}");

        // Convertit les données en byte
        byte[] imageBytes;
        try
        {
            imageBytes = Convert.FromBase64String(base64Data);
        }
        catch (FormatException)
        {
            throw new FormatException("Les données base64 ne sont pas valides.");
        }

        // Vérifie la taille de l'image
        const long maxSizeInBytes = 5 * 1024 * 1024; // 5 MB
        if (imageBytes.Length > maxSizeInBytes)
            throw new ArgumentException($"La taille de l'image dépasse la limite autorisée de {maxSizeInBytes / 1024 / 1024} MB.");

        // Génère le nom de fichier unique
        string fileName = @$"{Guid.NewGuid()}.{imageType}";

        // Utilise le chemin vers wwwRoot/images/uploads
        string imageFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, "images", "uploads");

        string fullPath = Path.Combine(imageFolderPath, fileName);

        // Sauvegarde l'image
        await File.WriteAllBytesAsync(fullPath, imageBytes);

        // Retourne le chemin publique de l'image
        return $"images/uploads/{fileName}";
    }

    /**
     * Retourne une liste de tous les tags img d'un texte
     */
    public static List<string> GetImgTags(string htmlContent)
    {
        var imgTags = new List<string>();
        string pattern = @"<img[^>]*>";
        Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

        // Applique le regex
        MatchCollection matches = regex.Matches(htmlContent);

        // Ajoute les images à la liste de retour
        foreach (Match match in matches)
        {
            imgTags.Add(match.Value);
        }

        return imgTags;
    }
}
