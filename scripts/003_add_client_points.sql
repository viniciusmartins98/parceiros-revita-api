-- =============================================================================
-- Script 003: Adicionar suporte a pontos para Clientes
-- =============================================================================
-- 1. Tabela clientes: adicionar saldo de pontos
ALTER TABLE clientes
    ADD COLUMN IF NOT EXISTS total_pontos INTEGER NOT NULL DEFAULT 0,
    ADD CONSTRAINT ck_clientes_total_pontos CHECK (total_pontos >= 0);

COMMENT ON COLUMN clientes.total_pontos IS 'Saldo atual de pontos do cliente (ganhos - resgates).';

-- 2. Tabela extrato_pontos: permitir cliente_id e tornar parceiro_id opcional
ALTER TABLE extrato_pontos 
    ALTER COLUMN parceiro_id DROP NOT NULL,
    ADD COLUMN IF NOT EXISTS cliente_id UUID NULL;

ALTER TABLE extrato_pontos
    ADD CONSTRAINT fk_extrato_cliente
    FOREIGN KEY (cliente_id) REFERENCES clientes (id) ON DELETE RESTRICT;

ALTER TABLE extrato_pontos
    ADD CONSTRAINT ck_extrato_owner 
    CHECK ((parceiro_id IS NOT NULL AND cliente_id IS NULL) OR (parceiro_id IS NULL AND cliente_id IS NOT NULL));

CREATE INDEX IF NOT EXISTS ix_extrato_cliente ON extrato_pontos (cliente_id) WHERE cliente_id IS NOT NULL;

-- 3. Tabela compras: separar pontos do parceiro e do cliente
ALTER TABLE compras RENAME COLUMN pontos_gerados TO pontos_gerados_parceiro;

ALTER TABLE compras 
    ADD COLUMN IF NOT EXISTS pontos_gerados_cliente INTEGER NOT NULL DEFAULT 0,
    ADD CONSTRAINT ck_compras_pontos_gerados_cliente CHECK (pontos_gerados_cliente >= 0);

COMMENT ON COLUMN compras.pontos_gerados_parceiro IS 'Pontos gerados para o parceiro nesta compra.';
COMMENT ON COLUMN compras.pontos_gerados_cliente  IS 'Pontos gerados para o cliente nesta compra.';

-- 4. Tabela regras_pontuacao: separar regras de parceiro e cliente
ALTER TABLE regras_pontuacao RENAME COLUMN valor_compra_minimo TO valor_compra_minimo_parceiro;
ALTER TABLE regras_pontuacao RENAME COLUMN pontos_por_valor TO pontos_por_valor_parceiro;
ALTER TABLE regras_pontuacao RENAME COLUMN valor_monetario_por_pontos TO valor_monetario_por_pontos_parceiro;
ALTER TABLE regras_pontuacao RENAME COLUMN pontos_para_conversao_monetaria TO pontos_para_conversao_monetaria_parceiro;

ALTER TABLE regras_pontuacao 
    ADD COLUMN IF NOT EXISTS valor_compra_minimo_cliente NUMERIC(12,2) NOT NULL DEFAULT 1000.00,
    ADD COLUMN IF NOT EXISTS pontos_por_valor_cliente INTEGER NOT NULL DEFAULT 100,
    ADD COLUMN IF NOT EXISTS valor_monetario_por_pontos_cliente NUMERIC(12,2) NOT NULL DEFAULT 50.00,
    ADD COLUMN IF NOT EXISTS pontos_para_conversao_monetaria_cliente INTEGER NOT NULL DEFAULT 100;

ALTER TABLE regras_pontuacao
    ADD CONSTRAINT ck_regras_pontuacao_valor_minimo_cliente  CHECK (valor_compra_minimo_cliente > 0),
    ADD CONSTRAINT ck_regras_pontuacao_pontos_cliente        CHECK (pontos_por_valor_cliente > 0),
    ADD CONSTRAINT ck_regras_pontuacao_valor_cliente         CHECK (valor_monetario_por_pontos_cliente > 0),
    ADD CONSTRAINT ck_regras_pontuacao_conversao_cliente     CHECK (pontos_para_conversao_monetaria_cliente > 0);

COMMENT ON COLUMN regras_pontuacao.valor_compra_minimo_cliente IS 'Valor em R$ de compras necessário para gerar pontos para o cliente (ex: R$ 1.000,00).';
COMMENT ON COLUMN regras_pontuacao.pontos_por_valor_cliente IS 'Quantidade de pontos gerados ao atingir o valor mínimo para o cliente.';
COMMENT ON COLUMN regras_pontuacao.valor_monetario_por_pontos_cliente IS 'Valor em R$ que o cliente recebe ao resgatar pontos.';
COMMENT ON COLUMN regras_pontuacao.pontos_para_conversao_monetaria_cliente IS 'Quantidade de pontos necessários para conversão monetária do cliente.';
