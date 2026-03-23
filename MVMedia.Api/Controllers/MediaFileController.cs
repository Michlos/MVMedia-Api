using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using MVMedia.Api.DTOs;
using MVMedia.Api.Identity;
using MVMedia.Api.Models;
using MVMedia.Api.Services.Interfaces;
using System.Security.Claims;

namespace MVMedia.Api.Controllers;


//TODO: fazer o App.js logar na API

[ApiController]
[Route("api/[controller]")]
public class MediaFileController : ControllerBase
{
    private readonly IMediaFileService _mediaFileService;
    private readonly IUserService _userService;
    private readonly IClientService _clientService;

    public MediaFileController(IMediaFileService mediaFileService, IUserService userService, IClientService clientService)
    {
        _mediaFileService = mediaFileService;
        _userService = userService;
        _clientService = clientService;
    }

    [HttpPost("AddMediaFile")]
    public async Task<ActionResult<MediaFile>> AddMediaFile([FromForm] MediaFileUploadDTO dto)
    {
        if (dto.File == null || dto.File.Length == 0)
            return BadRequest("Nenhum arquivo enviado.");

        var fileName = $"{Guid.NewGuid()}_{dto.File.FileName}";
        var mediaFile = new MediaFile
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Description = dto.Description,
            FileName = fileName,
            FileSize = dto.File.Length,
            UploadedAt = DateTime.UtcNow,
            IsPublic = dto.IsPublic,
            IsActive = true,
            ClientId = dto.ClientId,
            CompanyId = dto.CompanyId
        };

        // Salva no banco
        var mediaFileAdded = await _mediaFileService.AddMediaFile(mediaFile);
        if (mediaFileAdded == null)
            return BadRequest("Falha ao adicionar arquivo de mídia.");

        var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Videos");

        //COPILOT - UGESTÕES DE TESTES PARA VERIFICAR SE O ARQUIVO FOI GRAVADO
        //COPILOT - SUG. 1
        Console.WriteLine($"[AddMediaFile] uploadPath: {uploadPath}");
        
        Directory.CreateDirectory(uploadPath);
        var filePath = Path.Combine(uploadPath, fileName);

        //COPILOT - SUG. 2 INTRU TRY CATCH
        try
        {
            Console.WriteLine($"[AddMediaFile] Gravando arquivo em: {filePath}");

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.File.CopyToAsync(stream);
            }

            Console.WriteLine($"[AddMediaFile] Arquivo salvo com sucesso: {filePath}");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AddMediaFile] ERRO ao salvar arquivo: {ex}");
            return StatusCode(500, "Erro ao salvar arquivo em disco");
        }

        return Ok(mediaFileAdded);
        
        //using (var stream = new FileStream(filePath, FileMode.Create))
        //{
        //    await dto.File.CopyToAsync(stream);
        //}

        //return Ok(mediaFileAdded);
    }

    [HttpGet("ListActiveMediaFiles")]
    public async Task<ActionResult<IEnumerable<MediaFile>>> ListActiveMediaFiles()
    {

        #region AUTHENTICATING

        //// AUTENTICANDO O USUÁRIO PARA RETORNAR APENAS OS ARQUIVOS DE MÍDIA DA EMPRESA DO USUÁRIO AUTENTICADO
        //if (!User.Identity.IsAuthenticated)
        //{
        //    return Unauthorized("Usuário não autenticado.");
        //}
        //else
        //{
        //    var userId = User.GetUserId();
        //    var user = await _userService.GetUser(userId);

        //    var allMediaFiles = await _mediaFileService.GetAllMediaFiles();
        //    var activeMediaFiles = allMediaFiles.Where(m => m.IsActive).ToList();

        //    //LISTA TODAS AS EMPRESAS SE FOR AFIM
        //    //SE NÃO FOR LISTA SÓ AS COMPANIES DO USUÁRIO
        //    if (user.IsAdmin)
        //    {
        //        return Ok(activeMediaFiles);
        //    }
        //    else
        //    {
        //        var filteredMediaFiles = activeMediaFiles.Where(m => m.CompanyId == user.CompanyId).ToList();
        //        return Ok(filteredMediaFiles);

        //    }
        //}
        #endregion

        ///SEM AUTENTICAÇÃO, RETORNANDO TODOS OS ARQUIVOS DE MÍDIA ATIVOS
        var allMediaFiles = await _mediaFileService.GetAllMediaFiles();
        var activeMediaFiles = allMediaFiles.Where(m => m.IsActive).ToList();
        return Ok(activeMediaFiles);



    }

    [HttpGet("GetMediaFileByClientId/{clientId}")]
    public async Task<ActionResult<ClientWithMediaFileDTO>> GetMediaFileByClientId(int clientId)
    {
        var userId = User.GetUserId();
        var user = await _userService.GetUser(userId);
        var clientCompanyId = (await _clientService.GetClientById(clientId))?.CompanyId;

        var clientMediaFiles = await _mediaFileService.GetAllMediaByClientId(clientId);
        if (clientMediaFiles == null)
            return NotFound("No Media linked to this client.");

        if (!User.Identity.IsAuthenticated)
        {
            return Unauthorized("Usuário não autenticado");
        }
        else if (user.IsAdmin)
        {
            return Ok(clientMediaFiles);
        }
        else if (user.CompanyId == clientCompanyId)
        {
            return Ok(clientMediaFiles);
        }
        else
        {
            return Unauthorized("This média file is not a Client in your portfolio");
        }
    }

    [HttpGet("ListMediaUris")]
    public async Task<ActionResult<IEnumerable<string>>> ListMediaUris()
    {
        var allMediaFiles = await _mediaFileService.GetAllMediaFiles();
        var activeMediaFiles = allMediaFiles.Where(m => m.IsActive).ToList();
        var baseUrl = $"{Request.Scheme}://{Request.Host}/Videos/";
        var uris = activeMediaFiles.Select(m => baseUrl + m.FileName).ToList();
        return Ok(uris);
    }

    [HttpPut("UpdateMediaFile/{id}")]
    public async Task<ActionResult<MediaFile>> UpdateMediaFile(Guid id, [FromForm] MediaFileUploadDTO dto)
    {
        var userId = User.GetUserId();
        var user = await _userService.GetUser(userId);
        var userCompanyId = await _userService.GetCompanyId(userId);

        if (userCompanyId != dto.CompanyId)
            return BadRequest("This media is not a Client in your portfolio");

        var existingMediaFile = await _mediaFileService.GetMediaFileById(id);
        var oldFileName = existingMediaFile?.FileName;
        if (existingMediaFile == null)
            return NotFound("Arquivo de mídia não encontrado.");

        // Atualiza os campos do modelo
        existingMediaFile.Title = dto.Title;
        existingMediaFile.Description = dto.Description;
        existingMediaFile.IsPublic = dto.IsPublic;
        existingMediaFile.UpdatedAt = DateTime.UtcNow;
        existingMediaFile.ClientId = dto.ClientId;

        // Se um novo arquivo foi enviado, atualiza o arquivo físico e o nome
        if (dto.File != null && dto.File.Length > 0)
        {
            var newFileName = $"{Guid.NewGuid()}_{dto.File.FileName}";
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Videos");
            Directory.CreateDirectory(uploadPath);
            var newFilePath = Path.Combine(uploadPath, newFileName);

            using (var stream = new FileStream(newFilePath, FileMode.Create))
            {
                await dto.File.CopyToAsync(stream);
            }

            existingMediaFile.FileName = newFileName;
            existingMediaFile.FileSize = dto.File.Length;
        }

        var updatedMediaFile = await _mediaFileService.UpdateMediaFile(existingMediaFile, oldFileName);
        return Ok(updatedMediaFile);
    }

    [HttpDelete("DeleteMediaFile/{id}")]
    public async Task<ActionResult> DeleteMediaFile(Guid id)
    {
        //User isAutentecated
        //Identity = IsAuthenticated = false
        if (User.Identity.IsAuthenticated)
        {
            var userId = User.GetUserId();
            var userCompanyId = await _userService.GetCompanyId(userId);
            var mediaToDelete = await _mediaFileService.GetMediaFileById(id);
            var mediaCompanyId = mediaToDelete.CompanyId;

            if (userCompanyId != mediaCompanyId)
                return BadRequest("This media file is not a Client in your portfolio");

            var success = await _mediaFileService.DeleteMediaFile(id);
            if (!success)
                return NotFound("Arquivo de mídia não encontrado ou falha ao deletar.");
            else return Ok("File Media Deleted");

        }
        else
        {
            return BadRequest("User not Authenticaded.");
        }
    }

    [HttpGet("GetMediaFileById/{id}")]
    public async Task<ActionResult<MediaFile>> GetMediaFileById(Guid id)
    {
        var userId = User.GetUserId();
        var user = await _userService.GetUser(userId);
        var mediaFile = await _mediaFileService.GetMediaFileById(id);
        var clientCompanyId = (mediaFile != null) ? (await _clientService.GetClientById(mediaFile.ClientId))?.CompanyId : null;


        if (mediaFile == null)
            return NotFound("No Media linked to this client.");

        if (!User.Identity.IsAuthenticated)
        {
            return Unauthorized("Usuário não autenticado");
        }
        else if (user.IsAdmin)
        {
            return Ok(mediaFile);
        }
        else if (user.CompanyId == clientCompanyId)
        {
            return Ok(mediaFile);
        }
        else
        {
            return Unauthorized("This média file is not a Client in your portfolio");
        }
    }

    [HttpGet("GetToPlay/{id}")]
    public async Task<IActionResult> GetToPlay([FromRoute] Guid id)
    {

        //Bucar metadata no banco
        var mediaFile = await _mediaFileService.GetMediaFileById(id);

        if (mediaFile == null)
            return NotFound("Mídia não encontrada.");

        //Montar caminho físico do arquivo

        var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Videos");
        var filePath = Path.Combine(uploadPath, mediaFile.FileName);

        Console.WriteLine($"[GetToPlay] Tentando acessar arquivo em: {filePath}");

        if (!System.IO.File.Exists(filePath))
            return NotFound("Arquivo de vídeo não encontrado em disco");

        //retornando o arquivo como vídeo
        const string contentType = "video/mp4";
        return PhysicalFile(filePath, contentType);
    }


    [HttpGet("DebugDownload")]
    [AllowAnonymous] // ou com [Authorize] se quiser exigir token
    public IActionResult DebugDownload([FromQuery] string fileName)
    {
        var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Videos");
        var filePath = Path.Combine(uploadPath, fileName);

        Console.WriteLine($"[DebugDownload] Tentando acessar arquivo em: {filePath}");

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound($"Arquivo não encontrado em disco: {filePath}");
        }

        const string contentType = "video/mp4";
        return PhysicalFile(filePath, contentType);
    }
``
}
