-- 1. Create the new faixas_pontuacao table
CREATE TABLE IF NOT EXISTS faixas_pontuacao (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    regra_pontuacao_id UUID NOT NULL REFERENCES regras_pontuacao(id) ON DELETE CASCADE,
    tipo VARCHAR(10) NOT NULL DEFAULT 'parceiro',  -- 'parceiro' or 'cliente' (future-proof)
    valor_vendas DECIMAL(12,2) NOT NULL,
    pontos INT NOT NULL,
    criado_em TIMESTAMPTZ NOT NULL DEFAULT now()
);

CREATE INDEX ix_faixas_pontuacao_regra ON faixas_pontuacao (regra_pontuacao_id, tipo);

-- 2. Drop the legacy partner scoring columns (replaced by faixas_pontuacao)
ALTER TABLE regras_pontuacao DROP COLUMN IF EXISTS valor_compra_minimo_parceiro;
ALTER TABLE regras_pontuacao DROP COLUMN IF EXISTS pontos_por_valor_parceiro;
