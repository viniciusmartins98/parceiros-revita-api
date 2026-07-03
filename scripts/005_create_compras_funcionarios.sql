-- =============================================================================
-- REVITA PARCEIROS — Adiciona Tabela de Compras de Funcionários (Controle Interno)
-- =============================================================================

CREATE TABLE IF NOT EXISTS compras_funcionarios (
    id                UUID            PRIMARY KEY DEFAULT uuid_generate_v4(),
    funcionario_id    UUID            NOT NULL,
    registrado_por    UUID            NOT NULL,
    valor             NUMERIC(12,2)   NOT NULL,
    descricao         VARCHAR(500)    NULL,
    data_compra       TIMESTAMPTZ     NOT NULL DEFAULT NOW(),
    criado_em         TIMESTAMPTZ     NOT NULL DEFAULT NOW(),
    atualizado_em     TIMESTAMPTZ     NULL,

    -- Constraints
    CONSTRAINT fk_compras_func_funcionario 
        FOREIGN KEY (funcionario_id) REFERENCES funcionarios (id) ON DELETE RESTRICT,

    CONSTRAINT fk_compras_func_registrado_por 
        FOREIGN KEY (registrado_por) REFERENCES usuarios (id) ON DELETE RESTRICT,

    CONSTRAINT ck_compras_func_valor CHECK (valor > 0)
);

COMMENT ON TABLE  compras_funcionarios                  IS 'Registro de compras internas realizadas por funcionários (apenas para controle, sem gerar pontos).';
COMMENT ON COLUMN compras_funcionarios.registrado_por   IS 'Usuário (Admin ou Funcionario) que registrou a compra.';
COMMENT ON COLUMN compras_funcionarios.valor            IS 'Valor total da compra.';
COMMENT ON COLUMN compras_funcionarios.descricao        IS 'Descrição opcional dos itens comprados.';

-- Índices
CREATE INDEX IF NOT EXISTS ix_compras_func_funcionario  ON compras_funcionarios (funcionario_id);
CREATE INDEX IF NOT EXISTS ix_compras_func_registrado   ON compras_funcionarios (registrado_por);
CREATE INDEX IF NOT EXISTS ix_compras_func_data         ON compras_funcionarios (data_compra DESC);

-- Trigger para atualizado_em
DROP TRIGGER IF EXISTS trg_compras_funcionarios_atualizado_em ON compras_funcionarios;
CREATE TRIGGER trg_compras_funcionarios_atualizado_em
    BEFORE UPDATE ON compras_funcionarios
    FOR EACH ROW
    EXECUTE FUNCTION fn_set_atualizado_em();

-- =============================================================================
-- FIM DO SCRIPT
-- =============================================================================
