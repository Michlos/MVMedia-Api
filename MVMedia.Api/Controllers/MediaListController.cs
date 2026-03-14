using Microsoft.AspNetCore.Mvc;
using MVMedia.Api.DTOs;
using MVMedia.Api.Models;
using MVMedia.Api.Services.Interfaces;

namespace MVMedia.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MediaListController : ControllerBase
{
    private readonly IMediaListService _mediaListService;
    private readonly IMediaFileService _mediaFileService;
    private readonly IConfiguration _configuration;

    public MediaListController(
        IMediaListService mediaListService,
        IMediaFileService mediaFileService,
        IConfiguration configuration)
    {
        _mediaListService = mediaListService;
        _mediaFileService = mediaFileService;
        _configuration = configuration;
    }

    [HttpPost("AddMediaList")]
    public async Task<ActionResult<MediaList>> AddMediaList([FromForm] MediaList mediaList)
    {
        ICollection<MediaFile> mediaListFiles = await _mediaFileService.GetAllMediaFiles();

        //mediaList.Name = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
        mediaList.Name = "index";

        if (mediaList == null || string.IsNullOrWhiteSpace(mediaList.Name))
            return BadRequest("Invalid media list data.");

        var mediaListExists = await _mediaListService.GetMediaList();
        if (mediaListExists != null)
            mediaListExists.IsActive = false;

        var mediaListName = mediaList.Name;

        var medialist = new MediaList
        {
            Name = mediaListName,
            CreateDate = DateTime.UtcNow,
            IsActive = true
        };

        // Salva no banco
        var mediaListAdded = await _mediaListService.AddMediaList(mediaList);
        if (mediaListAdded == null)
            return BadRequest("Failed to add media list.");

        // Obtém o caminho da pasta de vídeos do appsettings.json
        var videosFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Videos");
        //var videosFolder = _configuration["FilePath"];
        if (string.IsNullOrWhiteSpace(videosFolder))
            return BadRequest("Caminho da pasta de vídeos não configurado.");

        if (!Directory.Exists(videosFolder))
            Directory.CreateDirectory(videosFolder);

        // Filtra apenas arquivos .mp4
        var mp4FileNames = mediaListFiles
            .Where(f => f.FileName != null && f.FileName.EndsWith(".mp4", StringComparison.OrdinalIgnoreCase))
            .Select(f => f.FileName)
            .ToList();

        // Cria o conteúdo do arquivo .m3u
        var m3uContent = "#EXTM3U\n" + string.Join("\n", mp4FileNames);

        // Caminho completo do arquivo .m3u
        var m3uFilePath = Path.Combine(videosFolder, $"{mediaListName}.m3u");

        // Salva o arquivo .m3u
        await System.IO.File.WriteAllTextAsync(m3uFilePath, m3uContent);

        return Ok(mediaListAdded);
    }

    [HttpGet("GetActiveMediaList")]
    public async Task<ActionResult<MediaListDTO>> GetActiveMediaList()
    {
        var mediaList = await _mediaListService.GetMediaList();
        MediaListDTO mediaListDTO = null;
        mediaListDTO = new MediaListDTO
        {
            Id = mediaList.Id,
            Name = mediaList.Name,
            CreateDate = mediaList.CreateDate,
            IsActive = mediaList.IsActive,
            URI = $"{Request.Scheme}://{Request.Host}/Videos/{mediaList.Name}.m3u"
        };
        if (mediaList == null)
            return NotFound("No active media list found.");
        return Ok(mediaListDTO);
    }
}
