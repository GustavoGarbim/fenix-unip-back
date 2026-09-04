namespace fenix_unip_back.DTOs;

public class CarteirinhaDigitalResponseDto
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public string? UsuarioNome { get; set; }
    public string NumeroCarteirinha { get; set; } = string.Empty;
    public DateTime DataEmissao { get; set; }
    public DateTime DataValidade { get; set; }
    public string Status { get; set; } = string.Empty;
}
