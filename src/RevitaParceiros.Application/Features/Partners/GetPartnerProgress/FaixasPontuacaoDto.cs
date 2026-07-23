using System;
using System.Collections.Generic;
using System.Text;

namespace RevitaParceiros.Application.Features.Partners.GetPartnerProgress
{
    public sealed record FaixasPontuacaoDto(
        decimal ValorVendas,
        int Pontos
    );
}
